// ===== PAYMENT COMPLETION LISTENER =====
// Main function to listen for payment completion from iframe and handle redirects
window.listenForPaymentCompletion = (dotNetRef) => {
    let paymentProcessed = false; // Flag to prevent duplicate processing

    // Process payment completion (called only once)
    const processPayment = (isSuccess, transactionId) => {
        if (paymentProcessed) return;

        paymentProcessed = true;
        console.log(`Payment processed: Success=${isSuccess}, TransactionID=${transactionId}`);

        // Call Blazor component method
        dotNetRef.invokeMethodAsync('OnPaymentCompleted', isSuccess, transactionId);

        // Clean up listeners
        cleanup();
    };

    // Handler for messages from payment iframe
    const messageHandler = (event) => {
        if (paymentProcessed) return;

        // Only process messages from trusted payment gateway domains
        const trustedDomains = [
            'https://accept.paymob.com',
            'https://accept.paymobsolutions.com',
            'https://portal.weaccept.co'
        ];

        // Check if the message origin is from a trusted domain
        const isTrustedDomain = trustedDomains.some(domain => event.origin.startsWith(domain));

        if (!isTrustedDomain) {
            return; // Ignore messages from untrusted domains
        }

        console.log('Payment message received from iframe:', event.data);

        try {
            let paymentData;

            // Handle different message formats (JSON string or object)
            if (typeof event.data === 'string') {
                try {
                    paymentData = JSON.parse(event.data);
                } catch (e) {
                    // If it's not JSON, treat as plain text
                    paymentData = { message: event.data };
                }
            } else {
                paymentData = event.data;
            }

            // Check for payment completion indicators
            const isSuccess = checkPaymentSuccess(paymentData);
            const isFailure = checkPaymentFailure(paymentData);

            if (isSuccess || isFailure) {
                // Extract transaction ID if available
                const transactionId = extractTransactionId(paymentData);
                processPayment(isSuccess, transactionId);
            }
        } catch (error) {
            console.error('Error processing payment message:', error);
        }
    };

    // Handler for URL changes (redirects from payment gateway)
    const checkURL = () => {
        if (paymentProcessed) return;

        const currentUrl = window.location.href;
        const urlParams = new URLSearchParams(window.location.search);

        // Get payment result parameters from URL
        const success = urlParams.get('success');
        const pending = urlParams.get('pending');
        const errorOccurred = urlParams.get('error_occured') || urlParams.get('error_occurred');

        console.log('Checking URL for payment result:', { success, pending, errorOccurred });

        // Process payment if we have result parameters in URL
        if (success !== null) {
            const isSuccess = (success === 'true' && pending === 'false' && errorOccurred !== 'true');
            const transactionId = urlParams.get('id') || urlParams.get('transaction_id');
            processPayment(isSuccess, transactionId);
        }

        // Also check for specific success/failure routes
        else if (currentUrl.includes('/order-success') ||
            currentUrl.includes('/event-payment-success')) {
            processPayment(true, null);
        }
        else if (currentUrl.includes('/event-payment-failed')) {
            processPayment(false, null);
        }
    };

    // Clean up all event listeners
    const cleanup = () => {
        console.log('Cleaning up payment listeners');

        if (window.paymentMessageHandler) {
            window.removeEventListener('message', window.paymentMessageHandler);
            window.paymentMessageHandler = null;
        }

        if (window.paymentPopStateHandler) {
            window.removeEventListener('popstate', window.paymentPopStateHandler);
            window.paymentPopStateHandler = null;
        }

        if (window.paymentUrlCheckTimeout) {
            clearTimeout(window.paymentUrlCheckTimeout);
            window.paymentUrlCheckTimeout = null;
        }

        if (window.paymentDotNetRef) {
            window.paymentDotNetRef = null;
        }
    };

    // Store handlers for cleanup
    window.paymentMessageHandler = messageHandler;
    window.paymentPopStateHandler = checkURL;
    window.paymentDotNetRef = dotNetRef;

    // Set up event listeners
    window.addEventListener('message', messageHandler);
    window.addEventListener('popstate', checkURL); // For SPA navigation

    // Check URL once on initial load (after short delay)
    window.paymentUrlCheckTimeout = setTimeout(checkURL, 300);

    console.log('Payment completion listener activated');
};

// ===== PAYMENT STATUS DETECTION HELPERS =====

// Check if payment was successful based on common success indicators
function checkPaymentSuccess(data) {
    if (!data) return false;

    // Common success indicators
    const successKeywords = ['success', 'completed', 'approved', 'paid', 'processed'];
    const successValues = [true, 'true', 'success', 'completed', 'approved', 'paid'];

    // Properties that might contain success status
    const checkProperties = [
        'success', 'status', 'state', 'result',
        'payment_status', 'transaction_status', 'type'
    ];

    // Check each property for success indicators
    for (const prop of checkProperties) {
        if (data.hasOwnProperty(prop)) {
            const value = data[prop];

            // Check boolean/string values
            if (successValues.includes(value) ||
                (typeof value === 'string' &&
                    successKeywords.some(keyword => value.toLowerCase().includes(keyword)))) {
                return true;
            }
        }
    }

    // Check message content for success keywords
    if (data.message && typeof data.message === 'string') {
        const message = data.message.toLowerCase();
        if (successKeywords.some(keyword => message.includes(keyword))) {
            return true;
        }
    }

    // Check for specific gateway responses
    if (data.txn_response_code === 'APPROVED' || data.acq_response_code === '00') {
        return true;
    }

    return false;
}

// Check if payment failed based on common failure indicators
function checkPaymentFailure(data) {
    if (!data) return false;

    // Common failure indicators
    const failureKeywords = ['failed', 'error', 'declined', 'cancelled', 'rejected', 'expired'];
    const failureValues = [false, 'false', 'failed', 'error', 'declined', 'cancelled', 'rejected'];

    // Properties that might contain failure status
    const checkProperties = [
        'success', 'status', 'state', 'result',
        'payment_status', 'transaction_status', 'error', 'type'
    ];

    // Check each property for failure indicators
    for (const prop of checkProperties) {
        if (data.hasOwnProperty(prop)) {
            const value = data[prop];

            // Check boolean/string values
            if (failureValues.includes(value) ||
                (typeof value === 'string' &&
                    failureKeywords.some(keyword => value.toLowerCase().includes(keyword)))) {
                return true;
            }
        }
    }

    // Check message content for failure keywords
    if (data.message && typeof data.message === 'string') {
        const message = data.message.toLowerCase();
        if (failureKeywords.some(keyword => message.includes(keyword))) {
            return true;
        }
    }

    // Check for specific gateway error codes
    if (data.txn_response_code === 'DECLINED' || data.error_occured === 'true') {
        return true;
    }

    return false;
}

// Extract transaction ID from various possible property names
function extractTransactionId(data) {
    if (!data) return null;

    // Common transaction ID property names across different payment gateways
    const transactionIdProps = [
        'transaction_id', 'transactionId', 'id', 'order_id', 'orderId',
        'invoice_id', 'invoiceId', 'payment_id', 'paymentId', 'merchant_order_id'
    ];

    // Check each possible property name
    for (const prop of transactionIdProps) {
        if (data.hasOwnProperty(prop) && data[prop]) {
            return data[prop].toString();
        }
    }

    return null;
}

// ===== NAVIGATION PROTECTION =====

// Prevent accidental navigation away from payment page
window.preventAccidentalNavigation = () => {
    const handleBeforeUnload = (event) => {
        // Only show warning if payment is in progress
        if (window.paymentInProgress) {
            event.preventDefault();
            event.returnValue = 'You have a payment in progress. Are you sure you want to leave?';
            return event.returnValue;
        }
    };

    window.addEventListener('beforeunload', handleBeforeUnload);
    window.paymentBeforeUnloadHandler = handleBeforeUnload;

    console.log('Accidental navigation protection activated');
};

// Start payment process with navigation protection
window.startPayment = (dotNetRef) => {
    window.paymentInProgress = true;
    window.preventAccidentalNavigation();
    window.listenForPaymentCompletion(dotNetRef);

    console.log('Payment process started');
};

// Stop payment process and remove protection
window.stopPayment = () => {
    window.paymentInProgress = false;

    // Remove beforeunload listener
    if (window.paymentBeforeUnloadHandler) {
        window.removeEventListener('beforeunload', window.paymentBeforeUnloadHandler);
        window.paymentBeforeUnloadHandler = null;
    }

    console.log('Payment process stopped');
};

// ===== CLEANUP FUNCTION =====

// Clean up all payment-related listeners and references
window.cleanupPaymentListener = () => {
    console.log('Cleaning up all payment listeners');

    // Remove message listener
    if (window.paymentMessageHandler) {
        window.removeEventListener('message', window.paymentMessageHandler);
        window.paymentMessageHandler = null;
    }

    // Remove popstate listener
    if (window.paymentPopStateHandler) {
        window.removeEventListener('popstate', window.paymentPopStateHandler);
        window.paymentPopStateHandler = null;
    }

    // Clear timeout
    if (window.paymentUrlCheckTimeout) {
        clearTimeout(window.paymentUrlCheckTimeout);
        window.paymentUrlCheckTimeout = null;
    }

    // Remove beforeunload listener
    if (window.paymentBeforeUnloadHandler) {
        window.removeEventListener('beforeunload', window.paymentBeforeUnloadHandler);
        window.paymentBeforeUnloadHandler = null;
    }

    // Clear DotNet reference
    if (window.paymentDotNetRef) {
        window.paymentDotNetRef = null;
    }

    // Reset flags
    window.paymentInProgress = false;

    console.log('All payment listeners cleaned up');
};

// ===== INITIALIZATION =====

// Initialize payment system when script loads
console.log('Payment listener script loaded successfully');