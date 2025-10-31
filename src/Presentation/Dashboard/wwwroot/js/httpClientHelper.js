// HTTP Client Helper for Cookie-based Authentication in Blazor WebAssembly
// This ensures cookies AND Bearer tokens are sent with every request

window.httpClientHelper = {
    // ? Get auth token from localStorage
    getAuthToken: function () {
        try {
            const token = localStorage.getItem('authToken');
            return token;
        } catch (error) {
            console.error('[HttpClientHelper] Error getting auth token:', error);
            return null;
        }
    },

    // Fetch with credentials to ensure cookies are sent
    fetchWithCredentials: async function (url, method, body, headers) {
        try {
            // ? Get token from localStorage
            const token = this.getAuthToken();
            
            const options = {
                method: method || 'GET',
                credentials: 'include', // ? CRITICAL: This ensures cookies are sent with requests
                mode: 'cors', // ? IMPORTANT: Enable CORS mode
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json',
                    ...headers
                }
            };

            // ? Add Bearer token if available
            if (token) {
                options.headers['Authorization'] = `Bearer ${token}`;
            }

            // Add body if provided (for POST, PUT, etc.)
            if (body && method !== 'GET' && method !== 'HEAD') {
                options.body = JSON.stringify(body);
            }

            const response = await fetch(url, options);
            const responseText = await response.text();

            // ? Check if we got set-cookie headers (for debugging - won't work with HttpOnly cookies)
            const setCookieHeader = response.headers.get('set-cookie');

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

    // ? Check if a specific cookie exists
    // Note: HttpOnly cookies cannot be read by JavaScript for security
    // So we check localStorage for auth state flag instead
    hasCookie: function (cookieName) {
        try {
            // ? For HttpOnly cookies, we can't check document.cookie
            // Instead, we check localStorage for auth state flag
            const hasAuthState = localStorage.getItem('isAuthenticated') === 'true';
            
            return hasAuthState;
        } catch (error) {
            console.error('[HttpClientHelper] Error checking cookie:', error);
            return false;
        }
    },

    // Check all cookies (for debugging)
    checkCookies: function () {
        return document.cookie;
    },

    // ? Set a flag in localStorage when user logs in
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

    // ? Check auth state from localStorage
    checkAuthState: function () {
        try {
            const isAuth = localStorage.getItem('isAuthenticated') === 'true';
            return isAuth;
        } catch (error) {
            console.error('[HttpClientHelper] Error checking auth state:', error);
            return false;
        }
    },

    // ? Clear all auth-related data
    clearAuthData: function () {
        try {
            localStorage.removeItem('isAuthenticated');
            localStorage.removeItem('authToken'); // ? Also clear token
        } catch (error) {
            console.error('[HttpClientHelper] Error clearing auth data:', error);
        }
    }
};
