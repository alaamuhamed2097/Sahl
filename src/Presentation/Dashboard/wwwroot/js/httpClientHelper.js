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
            console.log(`[HttpClientHelper] ${method} ${url}`);
            console.log(`[HttpClientHelper] Current cookies:`, document.cookie);

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
                console.log('[HttpClientHelper] ? Bearer token added to request');
            } else {
                console.log('[HttpClientHelper] ? No Bearer token found in localStorage');
            }

            // Add body if provided (for POST, PUT, etc.)
            if (body && method !== 'GET' && method !== 'HEAD') {
                options.body = JSON.stringify(body);
            }

            console.log(`[HttpClientHelper] Fetch options:`, { ...options, body: body ? 'present' : 'none' });

            const response = await fetch(url, options);
            const responseText = await response.text();

            console.log(`[HttpClientHelper] Response: ${response.status} ${response.statusText}`);
            console.log(`[HttpClientHelper] After request cookies:`, document.cookie);

            // ? Check if we got set-cookie headers (for debugging - won't work with HttpOnly cookies)
            const setCookieHeader = response.headers.get('set-cookie');
            if (setCookieHeader) {
                console.log(`[HttpClientHelper] Set-Cookie header found:`, setCookieHeader);
            }

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
            
            console.log(`[HttpClientHelper] Has auth state: ${hasAuthState}`);
            return hasAuthState;
        } catch (error) {
            console.error('[HttpClientHelper] Error checking cookie:', error);
            return false;
        }
    },

    // Check all cookies (for debugging)
    checkCookies: function () {
        console.log('[HttpClientHelper] Current cookies:', document.cookie);
        return document.cookie;
    },

    // ? Set a flag in localStorage when user logs in
    setAuthState: function (isAuthenticated) {
        try {
            if (isAuthenticated) {
                localStorage.setItem('isAuthenticated', 'true');
                console.log('[HttpClientHelper] ? Auth state set to true');
            } else {
                localStorage.removeItem('isAuthenticated');
                console.log('[HttpClientHelper] ? Auth state cleared');
            }
        } catch (error) {
            console.error('[HttpClientHelper] Error setting auth state:', error);
        }
    },

    // ? Check auth state from localStorage
    checkAuthState: function () {
        try {
            const isAuth = localStorage.getItem('isAuthenticated') === 'true';
            console.log('[HttpClientHelper] Auth state:', isAuth);
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
            // Note: We can't clear HttpOnly cookies from JavaScript
            // The logout endpoint on the server must do that
            console.log('[HttpClientHelper] Auth data cleared from localStorage');
        } catch (error) {
            console.error('[HttpClientHelper] Error clearing auth data:', error);
        }
    }
};
