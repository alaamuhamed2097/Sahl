// Facebook OAuth initialization and sign-in handler
window.facebookOAuth = {
    appId: document.currentScript?.getAttribute('data-app-id') || '',

    // Initialize Facebook SDK
    initializeFacebookSDK: function () {
        if (typeof FB !== 'undefined') {
            FB.init({
                appId: this.appId,
                xfbml: true,
                version: 'v18.0'
            });
            console.log('[Facebook OAuth] Initialized successfully');
        } else {
            console.error('[Facebook OAuth] Facebook SDK not loaded');
        }
    },

    // Handle Facebook login
    startFacebookSignIn: function () {
        console.log('[Facebook OAuth] Starting Facebook sign-in');

        if (typeof FB === 'undefined') {
            console.error('[Facebook OAuth] Facebook SDK not available');
            window.showErrorNotification?.('Facebook SDK not loaded');
            return;
        }

        FB.login((response) => {
            if (response.authResponse) {
                console.log('[Facebook OAuth] Login successful');
                window.facebookOAuth.signInWithFacebook(response.authResponse.accessToken);
            } else {
                console.error('[Facebook OAuth] Login failed');
                window.showErrorNotification?.('Facebook login failed');
            }
        }, { scope: 'public_profile,email' });
    },

    // Send the access token to backend
    signInWithFacebook: async function (accessToken) {
        try {
            const response = await fetch('/api/v1/auth/facebook-signin', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'X-Platform': 'website'
                },
                credentials: 'include', // Include cookies
                body: JSON.stringify({
                    accessToken: accessToken
                })
            });

            if (!response.ok) {
                console.error('[Facebook OAuth] API error:', response.status);
                window.showErrorNotification?.('Sign-in failed');
                return;
            }

            const result = await response.json();
            console.log('[Facebook OAuth] API response:', result);

            if (result.data) {
                console.log('[Facebook OAuth] Sign-in successful');
                // Redirect to home or dashboard
                window.location.href = '/';
            } else {
                console.error('[Facebook OAuth] Sign-in failed:', result.message);
                window.showErrorNotification?.(result.message || 'Facebook sign-in failed');
            }
        } catch (error) {
            console.error('[Facebook OAuth] Network error:', error);
            window.showErrorNotification?.('Network error occurred');
        }
    }
};

// Load Facebook SDK dynamically
function loadFacebookSignInScript() {
    window.fbAsyncInit = function () {
        console.log('[Facebook OAuth] SDK initialized');
        window.facebookOAuth.initializeFacebookSDK();
    };

    const script = document.createElement('script');
    script.src = 'https://connect.facebook.net/en_US/sdk.js';
    script.async = true;
    script.defer = true;
    script.crossOrigin = 'anonymous';
    script.onerror = () => {
        console.error('[Facebook OAuth] Failed to load SDK');
    };
    document.head.appendChild(script);
}

// Initialize when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', loadFacebookSignInScript);
} else {
    loadFacebookSignInScript();
}
