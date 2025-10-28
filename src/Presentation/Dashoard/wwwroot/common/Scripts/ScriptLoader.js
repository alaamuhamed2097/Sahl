// Enhanced scriptLoader.js - Production-ready with fallback handling (Fixed circular dependency)
window.ScriptLoader = {
    // Track loaded scripts
    loadedScripts: new Set(),
    loadingPromises: new Map(),

    // Configuration for production environment
    config: {
        retryAttempts: 3,
        retryDelay: 1000,
        timeout: 30000,
        fallbackToLocal: true
    },

    // Load a single script file with production-ready error handling
    loadScript: function (url, options = {}) {
        const finalOptions = { ...this.config, ...options };
        
        // Return existing promise if script is being loaded
        if (this.loadingPromises.has(url)) {
            return this.loadingPromises.get(url);
        }

        // Return resolved promise if script already loaded
        if (this.loadedScripts.has(url)) {
            console.log(`Script ${url} already loaded`);
            return Promise.resolve();
        }

        const promise = this.loadScriptWithRetry(url, finalOptions);
        this.loadingPromises.set(url, promise);
        return promise;
    },

    // Load script with retry logic
    loadScriptWithRetry: function(url, options, attempt = 1) {
        return new Promise((resolve, reject) => {
            // Check if script already exists in DOM
            if (document.querySelector(`script[src="${url}"]`)) {
                this.loadedScripts.add(url);
                resolve();
                return;
            }

            const script = document.createElement('script');
            script.type = 'text/javascript';
            script.src = url;
            
            // Add timeout handling
            const timeoutId = setTimeout(() => {
                script.onload = script.onerror = null;
                try {
                    document.head.removeChild(script);
                } catch (e) {
                    // Script might have been removed already
                }
                
                if (attempt < options.retryAttempts) {
                    console.warn(`Script load timeout for ${url}, retrying... (attempt ${attempt + 1})`);
                    setTimeout(() => {
                        this.loadScriptWithRetry(url, options, attempt + 1)
                            .then(resolve)
                            .catch(reject);
                    }, options.retryDelay);
                } else {
                    this.loadingPromises.delete(url);
                    reject(new Error(`Script load timeout after ${options.retryAttempts} attempts: ${url}`));
                }
            }, options.timeout);

            script.onload = () => {
                clearTimeout(timeoutId);
                console.log(`Script loaded successfully: ${url}`);
                this.loadedScripts.add(url);
                this.loadingPromises.delete(url);
                resolve();
            };

            script.onerror = (error) => {
                clearTimeout(timeoutId);
                console.error(`Error loading script: ${url}`, error);
                
                if (attempt < options.retryAttempts) {
                    console.warn(`Retrying script load for ${url}... (attempt ${attempt + 1})`);
                    setTimeout(() => {
                        this.loadScriptWithRetry(url, options, attempt + 1)
                            .then(resolve)
                            .catch(reject);
                    }, options.retryDelay);
                } else {
                    this.loadingPromises.delete(url);
                    reject(new Error(`Failed to load script after ${options.retryAttempts} attempts: ${url}`));
                }
            };

            try {
                document.head.appendChild(script);
            } catch (domError) {
                clearTimeout(timeoutId);
                this.loadingPromises.delete(url);
                reject(new Error(`DOM error adding script: ${domError.message}`));
            }
        });
    },

    // Load multiple scripts sequentially
    loadScriptsSequential: function (urlArray) {
        const urls = Array.isArray(urlArray) ? urlArray : [].slice.call(arguments);

        return urls.reduce((previousPromise, scriptUrl) => {
            return previousPromise.then(() => {
                return this.loadScript(scriptUrl);
            });
        }, Promise.resolve());
    },

    // Load multiple scripts in parallel
    loadScriptsParallel: function (urlArray) {
        const urls = Array.isArray(urlArray) ? urlArray : [].slice.call(arguments);
        const promises = urls.map(url => this.loadScript(url));
        return Promise.all(promises);
    },

    // Load Highcharts library on demand with production compatibility
    loadHighcharts: function () {
        // Use different CDN URLs with fallbacks for production
        const highchartsUrls = [
            'https://code.highcharts.com/highcharts.js',
            'https://code.highcharts.com/modules/exporting.js',
            'https://code.highcharts.com/modules/export-data.js',
            'https://code.highcharts.com/modules/accessibility.js'
        ];

        console.log('Loading Highcharts library...');
        
        return this.loadScriptsSequential(highchartsUrls)
            .then(() => {
                console.log('Highcharts library loaded successfully');
                
                // Verify Highcharts is available with timeout
                return this.waitForGlobal('Highcharts', 5000);
            })
            .then(() => {
                console.log('Highcharts verified and ready');
                return true;
            })
            .catch(error => {
                console.error('Failed to load Highcharts:', error);
                
                // Try fallback strategy for production
                return this.tryHighchartsFallback()
                    .then(() => {
                        console.log('Highcharts loaded via fallback');
                        return true;
                    })
                    .catch(fallbackError => {
                        console.error('Highcharts fallback also failed:', fallbackError);
                        throw new Error(`Complete Highcharts loading failure: ${error.message}`);
                    });
            });
    },

    // Fallback strategy for Highcharts in production
    tryHighchartsFallback: function() {
        const fallbackUrls = [
            'https://cdn.jsdelivr.net/npm/highcharts@latest/highcharts.js',
            'https://cdn.jsdelivr.net/npm/highcharts@latest/modules/exporting.js',
            'https://cdn.jsdelivr.net/npm/highcharts@latest/modules/export-data.js',
            'https://cdn.jsdelivr.net/npm/highcharts@latest/modules/accessibility.js'
        ];

        console.log('Trying Highcharts fallback URLs...');
        
        return this.loadScriptsSequential(fallbackUrls)
            .then(() => this.waitForGlobal('Highcharts', 5000));
    },

    // Wait for a global variable to become available
    waitForGlobal: function(globalName, timeout = 5000) {
        return new Promise((resolve, reject) => {
            if (typeof window[globalName] !== 'undefined') {
                resolve(window[globalName]);
                return;
            }

            let elapsed = 0;
            const interval = 100;
            
            const checkInterval = setInterval(() => {
                elapsed += interval;
                
                if (typeof window[globalName] !== 'undefined') {
                    clearInterval(checkInterval);
                    resolve(window[globalName]);
                } else if (elapsed >= timeout) {
                    clearInterval(checkInterval);
                    reject(new Error(`Global variable '${globalName}' not available after ${timeout}ms`));
                }
            }, interval);
        });
    },

    // Load TinyMCE on demand with production compatibility (FIXED - no circular dependency)
    loadTinyMCE: function () {
        console.log('Loading TinyMCE core library...');
        
        return this.loadScript('assets/js/plugins/tinymce/tinymce.min.js')
            .then(() => {
                console.log('TinyMCE core loaded successfully');
                return this.waitForGlobal('tinymce', 5000);
            })
            .then(() => {
                console.log('TinyMCE core verified and ready');
                return true;
            })
            .catch(error => {
                console.error('Failed to load TinyMCE core:', error);
                throw error;
            });
    },

    // Check if a library is loaded
    isLoaded: function (url) {
        return this.loadedScripts.has(url);
    },

    // Check if Highcharts is available
    isHighchartsLoaded: function () {
        return typeof Highcharts !== 'undefined' && 
               (this.loadedScripts.has('https://code.highcharts.com/highcharts.js') ||
                this.loadedScripts.has('https://cdn.jsdelivr.net/npm/highcharts@latest/highcharts.js'));
    },

    // Check if TinyMCE is available
    isTinyMCELoaded: function () {
        return typeof tinymce !== 'undefined' && 
               this.loadedScripts.has('assets/js/plugins/tinymce/tinymce.min.js');
    },

    // Remove a script
    removeScript: function (url) {
        const script = document.querySelector(`script[src="${url}"]`);
        if (script) {
            script.parentNode.removeChild(script);
            this.loadedScripts.delete(url);
            console.log(`Script removed: ${url}`);
            return true;
        }
        return false;
    },

    // Clear all tracking
    clear: function () {
        this.loadedScripts.clear();
        this.loadingPromises.clear();
    },

    // Get environment info
    getEnvironmentInfo: function() {
        return {
            isProduction: !window.location.hostname.includes('localhost'),
            origin: window.location.origin,
            userAgent: navigator.userAgent,
            loadedScripts: Array.from(this.loadedScripts)
        };
    }
};

// Global helper functions for Blazor components
window.loadHighchartsIfNeeded = function () {
    return window.ScriptLoader.loadHighcharts();
};

window.isHighchartsAvailable = function () {
    return window.ScriptLoader.isHighchartsLoaded();
};

window.loadTinyMCEIfNeeded = function () {
    return window.ScriptLoader.loadTinyMCE();
};

window.isTinyMCEAvailable = function () {
    return window.ScriptLoader.isTinyMCELoaded();
};

// Debug function for troubleshooting
window.getScriptLoaderInfo = function() {
    return window.ScriptLoader.getEnvironmentInfo();
};