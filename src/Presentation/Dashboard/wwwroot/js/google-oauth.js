// Google OAuth initialization and sign-in handler
window.googleOAuth = {
    clientId: document.currentScript?.getAttribute('data-client-id') || '',
    authenticationService: null,

    // Initialize Google Sign-In
    initializeGoogleSignIn: function () {
        if (typeof google !== 'undefined' && google.accounts) {
            google.accounts.id.initialize({
                client_id: this.clientId,
                callback: window.googleOAuth.handleCredentialResponse
            });
            console.log('[Google OAuth] Initialized successfully');
        } else {
            console.error('[Google OAuth] Google SDK not loaded');
        }
    },

    // Handle the credential response from Google
    handleCredentialResponse: async function (response) {
        console.log('[Google OAuth] Credential received');

        if (!response.credential) {
            console.error('[Google OAuth] No credential in response');
            return;
        }

        try {
            // Send the token to the backend
            const signInResult = await window.googleOAuth.signInWithGoogle(response.credential);

            if (signInResult && signInResult.success) {
                console.log('[Google OAuth] Sign-in successful');
                // Redirect to home or dashboard
                window.location.href = '/';
            } else {
                console.error('[Google OAuth] Sign-in failed:', signInResult?.message);
                // Show error message
                window.showErrorNotification?.(signInResult?.message || 'Google sign-in failed');
            }
        } catch (error) {
            console.error('[Google OAuth] Error handling credential response:', error);
            window.showErrorNotification?.('Failed to process Google sign-in');
        }
    },

    // Send the ID token to backend
    signInWithGoogle: async function (idToken) {
        try {
            const response = await fetch('/api/v1/auth/google-signin', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'X-Platform': 'website'
                },
                credentials: 'include', // Include cookies
                body: JSON.stringify({
                    idToken: idToken
                })
            });

            if (!response.ok) {
                console.error('[Google OAuth] API error:', response.status);
                return {
                    success: false,
                    message: 'API request failed'
                };
            }

            const result = await response.json();
            console.log('[Google OAuth] API response:', result);

            return result.data ? {
                success: true,
                data: result.data
            } : {
                success: false,
                message: result.message || 'Sign-in failed'
            };
        } catch (error) {
            console.error('[Google OAuth] Network error:', error);
            return {
                success: false,
                message: 'Network error occurred'
            };
        }
    },

    // Programmatic sign-in (for button click)
    startGoogleSignIn: function () {
        console.log('[Google OAuth] Starting Google sign-in');
        if (typeof google !== 'undefined' && google.accounts) {
            google.accounts.id.prompt((notification) => {
                if (notification.isNotDisplayed() || notification.isSkippedMoment()) {
                    // Show custom button instead
                    console.log('[Google OAuth] Showing custom button');
                    google.accounts.id.renderButton(
                        document.getElementById('google-signin-button'),
                        { theme: 'outline', size: 'large', width: '100%' }
                    );
                }
            });
        }
    }
};

// Load Google SDK dynamically
function loadGoogleSignInScript() {
    const script = document.createElement('script');
    script.src = 'https://accounts.google.com/gsi/client';
    script.async = true;
    script.defer = true;
    script.onload = () => {
        console.log('[Google OAuth] SDK loaded');
        window.googleOAuth.initializeGoogleSignIn();
    };
    script.onerror = () => {
        console.error('[Google OAuth] Failed to load SDK');
    };
    document.head.appendChild(script);
}

// Initialize when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', loadGoogleSignInScript);
} else {
    loadGoogleSignInScript();
}
