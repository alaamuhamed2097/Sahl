// clipboardCopy.js - Blazor-compatible version
window.clipboardCopy = {
    // Main function that Blazor will call - returns just boolean for simplicity
    copyText: async function (text) {
        console.log('Attempting to copy:', text);

        // Try modern Clipboard API first
        if (navigator.clipboard && window.isSecureContext) {
            try {
                await navigator.clipboard.writeText(text);
                console.log('Copied using Clipboard API');
                return true;  // Return simple boolean
            } catch (err) {
                console.warn('Clipboard API failed:', err.message);
                // Fall through to fallback method
            }
        }

        // Fallback method using execCommand
        try {
            const textarea = document.createElement("textarea");
            textarea.value = text;
            textarea.style.position = 'fixed';
            textarea.style.left = '-999999px';
            textarea.style.top = '-999999px';
            textarea.style.opacity = '0';
            textarea.style.pointerEvents = 'none';

            document.body.appendChild(textarea);
            textarea.focus();
            textarea.select();
            textarea.setSelectionRange(0, 99999); // For mobile devices

            const successful = document.execCommand('copy');
            document.body.removeChild(textarea);

            if (successful) {
                console.log('Copied using execCommand');
                return true;  // Return simple boolean
            } else {
                throw new Error('execCommand copy failed');
            }
        } catch (err) {
            console.error('Fallback copy method failed:', err);
            return false;  // Return simple boolean
        }
    },

    // Alternative method that shows a manual copy dialog if automatic copy fails
    copyTextWithFallback: async function (text) {
        const result = await this.copyText(text);

        if (!result.success) {
            // Show a prompt with the text selected for manual copying
            const userChoice = confirm(
                `Automatic copy failed. The text "${text}" is ready to be copied manually. ` +
                `Click OK to see it in a popup where you can select and copy it manually.`
            );

            if (userChoice) {
                // Create a modal-like div with the text selected
                const modal = document.createElement('div');
                modal.style.cssText = `
                    position: fixed;
                    top: 50%;
                    left: 50%;
                    transform: translate(-50%, -50%);
                    background: white;
                    border: 2px solid #007bff;
                    border-radius: 8px;
                    padding: 20px;
                    z-index: 10000;
                    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
                    max-width: 500px;
                    width: 90%;
                `;

                modal.innerHTML = `
                    <h4 style="margin-top: 0; color: #007bff;">Copy Link Manually</h4>
                    <p>Select the text below and copy it (Ctrl+C or Cmd+C):</p>
                    <textarea readonly style="width: 100%; height: 60px; padding: 10px; border: 1px solid #ddd; border-radius: 4px;">${text}</textarea>
                    <div style="margin-top: 15px; text-align: right;">
                        <button onclick="this.parentElement.parentElement.remove()" style="background: #007bff; color: white; border: none; padding: 8px 16px; border-radius: 4px; cursor: pointer;">Close</button>
                    </div>
                `;

                document.body.appendChild(modal);

                // Select the text in the textarea
                const textarea = modal.querySelector('textarea');
                textarea.focus();
                textarea.select();

                return { success: true, method: 'manual-fallback' };
            }
        }

        return result;
    }
};