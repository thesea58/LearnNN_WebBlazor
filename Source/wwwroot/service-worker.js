// ==========================================================================
// LearnNN Service Worker (PWA Cache & Offline Shell) - Version 1.0.0
// ==========================================================================

const CACHE_NAME = 'learnnn-cache-v1';

// Pre-cached core static assets
const PRECACHE_ASSETS = [
    '/',
    '/offline.html',
    '/manifest.webmanifest',
    '/favicon.png',
    '/app.css',
    '/study.css',
    '/study.js',
    '/ai-bridge.js',
    '/icons/icon-192.png',
    '/icons/icon-512.png',
    '/icons/icon-maskable-192.png',
    '/icons/icon-maskable-512.png',
    '/icons/apple-touch-icon.png'
];

// Install Event: pre-cache static assets
self.addEventListener('install', (event) => {
    event.waitUntil(
        caches.open(CACHE_NAME).then((cache) => {
            return cache.addAll(PRECACHE_ASSETS).catch((err) => {
                console.warn('Some assets failed to precache:', err);
            });
        }).then(() => self.skipWaiting())
    );
});

// Activate Event: clear legacy caches
self.addEventListener('activate', (event) => {
    event.waitUntil(
        caches.keys().then((cacheNames) => {
            return Promise.all(
                cacheNames.map((cacheName) => {
                    if (cacheName !== CACHE_NAME) {
                        return caches.delete(cacheName);
                    }
                })
            );
        }).then(() => self.clients.claim())
    );
});

// Fetch Event: handle requests with appropriate caching strategies
self.addEventListener('fetch', (event) => {
    const url = new URL(event.request.url);

    // 1. Bypass non-GET requests and Blazor SignalR / WebSocket circuits
    if (event.request.method !== 'GET') return;
    if (url.pathname.startsWith('/_blazor') ||
        url.pathname.startsWith('/_framework') ||
        url.pathname.includes('/negotiate')) {
        return;
    }

    // 2. Navigation / HTML Document request: Network-first, fallback to cache
    if (event.request.mode === 'navigate') {
        event.respondWith(
            fetch(event.request)
                .then((networkResponse) => {
                    // Update cache with latest page if valid
                    if (networkResponse && networkResponse.status === 200) {
                        const responseClone = networkResponse.clone();
                        caches.open(CACHE_NAME).then((cache) => {
                            cache.put(event.request, responseClone);
                        });
                    }
                    return networkResponse;
                })
                .catch(() => {
                    // Offline fallback: return cached request, or dedicated offline page, or root
                    return caches.match(event.request).then((cachedResponse) => {
                        return cachedResponse || caches.match('/offline.html') || caches.match('/');
                    });
                })
        );
        return;
    }

    // 3. Static assets (CSS, JS, Fonts, Images): Cache-first, fallback to network
    if (url.pathname.match(/\.(css|js|png|jpg|jpeg|svg|woff2?|webmanifest)$/i)) {
        event.respondWith(
            caches.match(event.request).then((cachedResponse) => {
                if (cachedResponse) {
                    return cachedResponse;
                }
                return fetch(event.request).then((networkResponse) => {
                    if (networkResponse && networkResponse.status === 200) {
                        const responseClone = networkResponse.clone();
                        caches.open(CACHE_NAME).then((cache) => {
                            cache.put(event.request, responseClone);
                        });
                    }
                    return networkResponse;
                }).catch(() => {
                    // If network fails and no cache, return empty response or fallback
                    return new Response('', { status: 408, statusText: 'Request timed out' });
                });
            })
        );
    }
});
