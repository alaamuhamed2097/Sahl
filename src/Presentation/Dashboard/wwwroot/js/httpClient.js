// HTTP Client wrapper with credentials support for Blazor WebAssembly
window.httpClientHelper = {
    /**
     * Makes an HTTP request with credentials (cookies)
     * @param {string} url - The URL to fetch
     * @param {string} method - HTTP method (GET, POST, PUT, DELETE)
     * @param {object} body - Request body (optional)
     * @param {object} headers - Additional headers (optional)
     * @returns {Promise<Object>} - The fetch response with ok, status, statusText, and body
     */
    fetchWithCredentials: async function (url, method, body, headers) {
        console.log('?? Fetching with credentials:', { url, method, body });
        
        const options = {
            method: method || 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json',
                ...headers
            },
            credentials: 'include' // ? CRITICAL: Send cookies with every request
        };

        if (body && method !== 'GET' && method !== 'HEAD') {
            options.body = JSON.stringify(body);
        }

        try {
            const response = await fetch(url, options);
            const responseText = await response.text();
            
            console.log('? Fetch response:', {
                ok: response.ok,
                status: response.status,
                statusText: response.statusText,
                bodyLength: responseText.length
            });
            
            // Return object that matches C# FetchResponse class
            return {
                ok: response.ok,
                status: response.status,
                statusText: response.statusText,
                body: responseText
            };
        } catch (error) {
            console.error('? HTTP request failed:', error);
            
            // Return error response that matches C# FetchResponse class
            return {
                ok: false,
                status: 0,
                statusText: error.message || 'Network error',
                body: JSON.stringify({ success: false, message: error.message })
            };
        }
    }
};

// Log that the helper is loaded
console.log('? httpClientHelper loaded successfully');
