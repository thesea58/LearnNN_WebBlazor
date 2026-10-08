/**
 * AI Bridge JS Interop helpers for clipboard copying and browser window opening.
 */
window.aiBridge = {
    /**
     * Copies plain text to the system clipboard using the modern Navigator Clipboard API with fallback.
     * @param {string} text 
     * @returns {Promise<boolean>}
     */
    copyText: async function (text) {
        if (!text) return false;
        try {
            if (navigator.clipboard && window.isSecureContext) {
                await navigator.clipboard.writeText(text);
                return true;
            } else {
                // Fallback for non-secure contexts or older browsers
                const textArea = document.createElement("textarea");
                textArea.value = text;
                textArea.style.position = "fixed";
                textArea.style.left = "-999999px";
                textArea.style.top = "-999999px";
                document.body.appendChild(textArea);
                textArea.focus();
                textArea.select();
                const successful = document.execCommand('copy');
                document.body.removeChild(textArea);
                return successful;
            }
        } catch (err) {
            console.error("AI Bridge: Failed to copy text to clipboard", err);
            return false;
        }
    },

    /**
     * Opens a web chatbot URL in a new browser tab.
     * @param {string} url 
     */
    openChatbot: function (url) {
        if (!url) return;
        window.open(url, '_blank', 'noopener,noreferrer');
    }
};
