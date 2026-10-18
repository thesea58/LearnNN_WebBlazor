// Set up event handlers
const reconnectModal = document.getElementById("components-reconnect-modal");
reconnectModal.addEventListener("components-reconnect-state-changed", handleReconnectStateChanged);

const retryButton = document.getElementById("components-reconnect-button");
retryButton.addEventListener("click", retry);

const resumeButton = document.getElementById("components-resume-button");
resumeButton.addEventListener("click", resume);

let isRetrying = false;

function isModalVisible() {
    return reconnectModal && (reconnectModal.open ||
        reconnectModal.classList.contains("components-reconnect-show") ||
        reconnectModal.classList.contains("components-reconnect-retrying") ||
        reconnectModal.classList.contains("components-reconnect-failed") ||
        reconnectModal.classList.contains("components-reconnect-paused") ||
        reconnectModal.classList.contains("components-reconnect-resume-failed"));
}

// Auto-reconnect listeners for mobile resilience (app switching, screen unlock, network recovery)
window.addEventListener("online", async () => {
    if (isModalVisible()) {
        await retry();
    }
});

document.addEventListener("visibilitychange", async () => {
    if (document.visibilityState === "visible" && isModalVisible()) {
        await retry();
    }
});

window.addEventListener("focus", async () => {
    if (isModalVisible()) {
        await retry();
    }
});

function handleReconnectStateChanged(event) {
    if (event.detail.state === "show") {
        reconnectModal.showModal();
    } else if (event.detail.state === "hide") {
        reconnectModal.close();
    } else if (event.detail.state === "failed") {
        // Circuit failed, auto-retry will be triggered on visibility change or online
    } else if (event.detail.state === "rejected") {
        location.reload();
    }
}

async function retry() {
    if (isRetrying) return;
    isRetrying = true;

    try {
        // Reconnect will asynchronously return:
        // - true to mean success
        // - false to mean we reached the server, but it rejected the connection (e.g., unknown circuit ID)
        // - exception to mean we didn't reach the server (this can be sync or async)
        const successful = await Blazor.reconnect();
        if (!successful) {
            // We have been able to reach the server, but the circuit is no longer available.
            // We'll reload the page so the user can continue using the app as quickly as possible.
            const resumeSuccessful = await Blazor.resumeCircuit();
            if (!resumeSuccessful) {
                location.reload();
            } else {
                reconnectModal.close();
            }
        }
    } catch (err) {
        // Exception: server is currently unavailable
    } finally {
        isRetrying = false;
    }
}

async function resume() {
    try {
        const successful = await Blazor.resumeCircuit();
        if (!successful) {
            location.reload();
        }
    } catch {
        reconnectModal.classList.replace("components-reconnect-paused", "components-reconnect-resume-failed");
    }
}

