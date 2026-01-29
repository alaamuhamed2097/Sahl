// HTTP Client Helper for Token-based Authentication in Blazor WebAssembly
// Simplified version - focuses only on Bearer token authentication

window.httpClientHelper = {
    // Get auth token from localStorage
    getAuthToken: function () {
        try {
            const token = localStorage.getItem('authToken');
            return token;
        } catch (error) {
            console.error('[HttpClientHelper] Error getting auth token:', error);
            return null;
        }
    },

    // Fetch with Bearer token
    fetchWithCredentials: async function (url, method, body, headers) {
        try {
            const token = this.getAuthToken();

            const options = {
                method: method || 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json',
                    ...headers
                }
            };

            // Add Bearer token if available
            if (token) {
                options.headers['Authorization'] = `Bearer ${token}`;
            }

            // Add body if provided (for POST, PUT, etc.)
            if (body && method !== 'GET' && method !== 'HEAD') {
                options.body = JSON.stringify(body);
            }

            const response = await fetch(url, options);
            const responseText = await response.text();

            return {
                ok: response.ok,
                status: response.status,
                statusText: response.statusText,
                body: responseText
            };
        } catch (error) {
            console.error('[HttpClientHelper] Error:', error);
            return {
                ok: false,
                status: 0,
                statusText: error.message || 'Network error',
                body: ''
            };
        }
    },

    // Fetch without Authorization header (for refresh token endpoint - avoids sending expired token)
    fetchWithoutAuth: async function (url, method, body, headers) {
        try {
            const options = {
                method: method || 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json',
                    ...headers
                }
            };

            if (body && method !== 'GET' && method !== 'HEAD') {
                options.body = JSON.stringify(body);
            }

            const response = await fetch(url, options);
            const responseText = await response.text();

            return {
                ok: response.ok,
                status: response.status,
                statusText: response.statusText,
                body: responseText
            };
        } catch (error) {
            console.error('[HttpClientHelper] fetchWithoutAuth Error:', error);
            return {
                ok: false,
                status: 0,
                statusText: error.message || 'Network error',
                body: ''
            };
        }
    },

    // Set authentication state flag in localStorage
    setAuthState: function (isAuthenticated) {
        try {
            if (isAuthenticated) {
                localStorage.setItem('isAuthenticated', 'true');
            } else {
                localStorage.removeItem('isAuthenticated');
            }
        } catch (error) {
            console.error('[HttpClientHelper] Error setting auth state:', error);
        }
    },

    // Check authentication state from localStorage
    checkAuthState: function () {
        try {
            const isAuth = localStorage.getItem('isAuthenticated') === 'true';
            return isAuth;
        } catch (error) {
            console.error('[HttpClientHelper] Error checking auth state:', error);
            return false;
        }
    },

    // Clear all authentication data
    clearAuthData: function () {
        try {
            localStorage.removeItem('isAuthenticated');
            localStorage.removeItem('authToken');
            localStorage.removeItem('refreshToken');
            localStorage.removeItem('userEmail');
        } catch (error) {
            console.error('[HttpClientHelper] Error clearing auth data:', error);
        }
    }
};