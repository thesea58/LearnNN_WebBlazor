/**
 * LearnNN - Study Activities & Gamification JS Helpers
 * Provides Web Speech API Text-to-Speech (TTS) and UI audio feedback.
 */

window.studyInterop = {
    /**
     * Speaks the specified English text aloud using the browser's built-in Web Speech API.
     * @param {string} text - The word or sentence to pronounce.
     * @param {string} [lang='en-US'] - The BCP 47 language tag.
     */
    speakWord: function (text, lang) {
        if (!('speechSynthesis' in window)) {
            console.warn('Speech synthesis not supported in this browser.');
            return;
        }

        // Cancel any pending speech
        window.speechSynthesis.cancel();

        const utterance = new SpeechSynthesisUtterance(text);
        utterance.lang = lang || 'en-US';
        utterance.rate = 0.9; // Slightly slower for clear English pronunciation
        utterance.pitch = 1.0;

        window.speechSynthesis.speak(utterance);
    },

    /**
     * Plays a pleasant synthesized audio beep feedback using Web Audio API.
     * @param {string} type - 'success', 'error', 'complete', or 'flip'
     */
    playSound: function (type) {
        try {
            const ctx = new (window.AudioContext || window.webkitAudioContext)();
            const osc = ctx.createOscillator();
            const gain = ctx.createGain();

            osc.connect(gain);
            gain.connect(ctx.destination);

            const now = ctx.currentTime;

            if (type === 'success') {
                osc.type = 'sine';
                osc.frequency.setValueAtTime(523.25, now); // C5
                osc.frequency.setValueAtTime(659.25, now + 0.1); // E5
                gain.gain.setValueAtTime(0.15, now);
                gain.gain.exponentialRampToValueAtTime(0.001, now + 0.3);
                osc.start(now);
                osc.stop(now + 0.3);
            } else if (type === 'error') {
                osc.type = 'triangle';
                osc.frequency.setValueAtTime(220, now); // A3
                osc.frequency.setValueAtTime(180, now + 0.1);
                gain.gain.setValueAtTime(0.18, now);
                gain.gain.exponentialRampToValueAtTime(0.001, now + 0.3);
                osc.start(now);
                osc.stop(now + 0.3);
            } else if (type === 'complete') {
                osc.type = 'triangle';
                osc.frequency.setValueAtTime(440, now); // A4
                osc.frequency.setValueAtTime(554.37, now + 0.15); // C#5
                osc.frequency.setValueAtTime(659.25, now + 0.3); // E5
                gain.gain.setValueAtTime(0.15, now);
                gain.gain.exponentialRampToValueAtTime(0.001, now + 0.6);
                osc.start(now);
                osc.stop(now + 0.6);
            }
        } catch (e) {
            // Audio context might fail on un-interacted pages, ignore safely
        }
    }
};

/**
 * Helper to manage view mode preference (table vs card) and detect mobile devices.
 */
window.viewModeHelper = {
    /**
     * Retrieves the stored view mode or defaults based on viewport width (< 768px -> card, >= 768px -> table).
     * @param {string} storageKey - localStorage key.
     * @returns {string} 'table' or 'card'.
     */
    getViewMode: function (storageKey) {
        try {
            const saved = localStorage.getItem(storageKey);
            if (saved === 'card' || saved === 'table') {
                return saved;
            }
        } catch (e) {
            // Fall through if localStorage is inaccessible
        }

        // Auto-detect mobile phones & tablets (Android / iPhone / iPad portrait < 768px)
        return window.innerWidth < 768 ? 'card' : 'table';
    },

    /**
     * Persists the selected view mode in localStorage.
     * @param {string} storageKey - localStorage key.
     * @param {string} mode - 'table' or 'card'.
     */
    setViewMode: function (storageKey, mode) {
        try {
            localStorage.setItem(storageKey, mode);
        } catch (e) {
            // Ignore quota or private browsing exceptions
        }
    }
};
