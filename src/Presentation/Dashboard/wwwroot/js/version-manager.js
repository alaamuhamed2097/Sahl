// Version Manager - Automatic Cache Update System
(function () {
    'use strict';

    const VERSION_CHECK_INTERVAL = 300000; // 5 minutes
    const VERSION_STORAGE_KEY = 'app_version';
    const LAST_CHECK_KEY = 'version_last_check';
    const RELOAD_LOCK_KEY = 'version_reload_lock';
    const RELOAD_LOCK_DURATION = 30000; // 30 seconds
    const INITIALIZED_KEY = 'version_manager_initialized';

    const VersionManager = {
        currentVersion: null,
        checkTimer: null,
        isUpdating: false,
        _hasInitialized: false,
        serviceWorkerRegistration: null,

        // Initialize version manager
        init: function () {
            // Prevent multiple initializations
            if (this._hasInitialized) {
                console.log('? Version Manager: Already initialized, skipping...');
                return;
            }

            const sessionInitialized = sessionStorage.getItem(INITIALIZED_KEY);
            if (sessionInitialized) {
                console.log('? Version Manager: Already initialized in this session, skipping...');
                this._hasInitialized = true;
                return;
            }

            console.log('?? Version Manager: Initializing...');
            
            this._hasInitialized = true;
            sessionStorage.setItem(INITIALIZED_KEY, 'true');
            
            // Check if we just reloaded
            const reloadLock = localStorage.getItem(RELOAD_LOCK_KEY);
            if (reloadLock) {
                const lockTime = parseInt(reloadLock);
                const timeSinceLock = Date.now() - lockTime;
                
                if (timeSinceLock < RELOAD_LOCK_DURATION) {
                    console.log('? Recently reloaded, skipping check for', (RELOAD_LOCK_DURATION - timeSinceLock) / 1000, 'seconds');
                    
                    setTimeout(() => {
                        localStorage.removeItem(RELOAD_LOCK_KEY);
                        console.log('? Reload lock cleared');
                    }, RELOAD_LOCK_DURATION - timeSinceLock);
                    
                    this.registerServiceWorker();
                    this.startPeriodicCheck();
                    this.listenToVisibilityChange();
                    return;
                }
                
                localStorage.removeItem(RELOAD_LOCK_KEY);
            }
            
            this.registerServiceWorker();
            this.checkForUpdates(true);
            this.startPeriodicCheck();
            this.listenToVisibilityChange();
        },

        // Register service worker and listen for updates
        registerServiceWorker: async function () {
            if (!('serviceWorker' in navigator)) {
                console.log('?? Service Worker not supported');
                return;
            }

            try {
                // Unregister old service workers first
                const registrations = await navigator.serviceWorker.getRegistrations();
                for (const registration of registrations) {
                    await registration.unregister();
                    console.log('??? Unregistered old service worker');
                }

                // Register new service worker with cache busting
                const registration = await navigator.serviceWorker.register(
                    '/service-worker.js?v=' + Date.now(),
                    { 
                        scope: '/',
                        updateViaCache: 'none' // Never cache the service worker
                    }
                );
                
                this.serviceWorkerRegistration = registration;
                console.log('? Service Worker registered successfully');

                // Force immediate update check
                registration.update();

                // Listen for service worker updates
                registration.addEventListener('updatefound', () => {
                    const newWorker = registration.installing;
                    console.log('?? New Service Worker found, installing...');
                    
                    newWorker.addEventListener('statechange', () => {
                        if (newWorker.state === 'installed' && navigator.serviceWorker.controller) {
                            console.log('? New Service Worker installed, activating...');
                            // Tell the new service worker to skip waiting
                            newWorker.postMessage({ type: 'SKIP_WAITING' });
                        }
                    });
                });

                // Listen for messages from service worker
                navigator.serviceWorker.addEventListener('message', event => {
                    if (event.data && event.data.type === 'VERSION_UPDATE') {
                        console.log('?? Version update detected by service worker:', event.data.version);
                        this.handleServiceWorkerUpdate();
                    }
                });

                // Listen for controller change (new service worker activated)
                navigator.serviceWorker.addEventListener('controllerchange', () => {
                    console.log('?? Service Worker controller changed, reloading...');
                    if (!this.isUpdating) {
                        this.isUpdating = true;
                        this.reloadApp();
                    }
                });

            } catch (error) {
                console.error('? Service Worker registration failed:', error);
            }
        },

        // Handle service worker update
        handleServiceWorkerUpdate: async function () {
            if (this.isUpdating) {
                console.log('?? Update already in progress');
                return;
            }

            console.log('?? Handling service worker update...');
            this.isUpdating = true;

            try {
                // Fetch latest version
                const response = await fetch('/version.json?t=' + Date.now(), {
                    cache: 'no-cache',
                    headers: {
                        'Cache-Control': 'no-cache, no-store, must-revalidate',
                        'Pragma': 'no-cache'
                    }
                });

                if (response.ok) {
                    const versionData = await response.json();
                    const serverVersion = versionData.version;
                    
                    // Update stored version
                    localStorage.setItem(VERSION_STORAGE_KEY, serverVersion);
                    console.log('?? Version updated to:', serverVersion);
                }

                // Clear all caches
                await this.clearAllCaches();

                // Reload app
                this.reloadApp();

            } catch (error) {
                console.error('? Error handling service worker update:', error);
                this.isUpdating = false;
            }
        },

        // Check for updates
        checkForUpdates: async function (isInitial = false) {
            if (this.isUpdating) {
                console.log('? Update already in progress, skipping check...');
                return;
            }

            try {
                const response = await fetch('/version.json?t=' + Date.now(), {
                    cache: 'no-cache',
                    headers: {
                        'Cache-Control': 'no-cache, no-store, must-revalidate',
                        'Pragma': 'no-cache'
                    }
                });

                if (!response.ok) {
                    // ? FIX: Gracefully handle missing version.json
                    if (response.status === 404) {
                        console.warn('?? version.json not found (404) - Version checking disabled');
                        return;
                    }
                    console.warn('?? Version check failed:', response.status, response.statusText);
                    return;
                }

                // ? FIX: Check content type to ensure we got JSON
                const contentType = response.headers.get('content-type');
                if (!contentType || !contentType.includes('application/json')) {
                    console.warn('?? version.json returned unexpected content type:', contentType);
                    const text = await response.text();
                    console.warn('Response preview:', text.substring(0, 200));
                    return;
                }

                const versionData = await response.json();
                const serverVersion = versionData.version;
                const forceUpdate = versionData.forceUpdate;
                const storedVersion = localStorage.getItem(VERSION_STORAGE_KEY);

                console.log('?? Current Version:', storedVersion || 'Unknown');
                console.log('?? Server Version:', serverVersion);
                console.log('?? Force Update:', forceUpdate);

                if (!storedVersion) {
                    localStorage.setItem(VERSION_STORAGE_KEY, serverVersion);
                    console.log('? Version stored:', serverVersion);
                    return;
                }

                const reloadLock = localStorage.getItem(RELOAD_LOCK_KEY);
                if (reloadLock) {
                    const lockTime = parseInt(reloadLock);
                    const timeSinceLock = Date.now() - lockTime;
                    
                    if (timeSinceLock < RELOAD_LOCK_DURATION) {
                        console.log('? Update already in progress, skipping...');
                        return;
                    }
                    
                    localStorage.removeItem(RELOAD_LOCK_KEY);
                }

                if (serverVersion !== storedVersion || forceUpdate) {
                    console.log('?? Version mismatch or force update detected! Updating...');
                    this.isUpdating = true;
                    await this.performUpdate(serverVersion, forceUpdate);
                } else if (!isInitial) {
                    console.log('? App is up to date');
                }

                localStorage.setItem(LAST_CHECK_KEY, Date.now().toString());

            } catch (error) {
                // ? FIX: Better error handling
                console.error('? Version check error:', error.message);
                
                // Check if it's a JSON parse error (likely HTML returned)
                if (error instanceof SyntaxError) {
                    console.error('? Failed to parse version.json - likely received HTML instead of JSON');
                    console.error('? This usually means version.json is missing or the path is incorrect');
                }
                
                this.isUpdating = false;
            }
        },

        // Perform update
        performUpdate: async function (newVersion, forceUpdate) {
            try {
                console.log('?? Performing update to version:', newVersion);

                // Update version in storage first
                localStorage.setItem(VERSION_STORAGE_KEY, newVersion);
                console.log('? Version updated in storage:', newVersion);

                // Clear all caches
                await this.clearAllCaches();

                // Clear non-essential storage
                this.clearNonEssentialStorage(newVersion);

                // Reload app
                this.reloadApp();

            } catch (error) {
                console.error('? Update failed:', error);
                localStorage.setItem(VERSION_STORAGE_KEY, newVersion);
                this.reloadApp();
            }
        },

        // Clear all caches
        clearAllCaches: async function () {
            console.log('??? Clearing all caches...');

            // Clear browser caches
            if ('caches' in window) {
                const cacheNames = await caches.keys();
                await Promise.all(
                    cacheNames.map(cacheName => {
                        console.log('??? Deleting cache:', cacheName);
                        return caches.delete(cacheName);
                    })
                );
            }

            // Tell service worker to clear its caches
            if (this.serviceWorkerRegistration && navigator.serviceWorker.controller) {
                navigator.serviceWorker.controller.postMessage({ type: 'CLEAR_CACHE' });
            }

            // Unregister service workers
            if ('serviceWorker' in navigator) {
                const registrations = await navigator.serviceWorker.getRegistrations();
                for (const registration of registrations) {
                    console.log('??? Unregistering service worker...');
                    await registration.unregister();
                }
            }

            console.log('? All caches cleared');
        },

        // Clear non-essential storage
        clearNonEssentialStorage: function (newVersion) {
            const keysToKeep = [
                VERSION_STORAGE_KEY,
                LAST_CHECK_KEY,
                RELOAD_LOCK_KEY,
                // Authentication keys
                'auth_token',
                'refresh_token',
                'authToken',
                'isAuthenticated',
                // User preferences
                'user_preferences',
                'autoLoadPreference',
                // Language keys
                'selected_language',
                'lang',
                'current_language',
                // Application settings
                'theme',
                'sidebar_collapsed',
                'notifications_enabled'
            ];

            // Save values from localStorage
            const savedValues = {};
            keysToKeep.forEach(key => {
                const value = localStorage.getItem(key);
                if (value !== null) {
                    savedValues[key] = value;
                }
            });

            // Also save ALL keys that start with specific prefixes
            const prefixesToKeep = ['auth', 'user', 'selected_', 'version_', 'reload_', 'lang', 'pref_'];
            for (let i = 0; i < localStorage.length; i++) {
                const key = localStorage.key(i);
                if (key && prefixesToKeep.some(prefix => key.toLowerCase().startsWith(prefix))) {
                    const value = localStorage.getItem(key);
                    if (value !== null && !savedValues[key]) {
                        savedValues[key] = value;
                        console.log('? Keeping prefixed key:', key);
                    }
                }
            }

            // Clear all localStorage
            localStorage.clear();

            // Restore saved values
            Object.keys(savedValues).forEach(key => {
                localStorage.setItem(key, savedValues[key]);
            });

            // Ensure version is saved
            localStorage.setItem(VERSION_STORAGE_KEY, newVersion);

            console.log('? Storage cleaned (keeping authentication & user data)');
            console.log('?? Kept keys:', Object.keys(savedValues));
        },

        // Reload app with hard refresh
        reloadApp: function () {
            console.log('?? Reloading application...');
            
            // Clear sessionStorage flag before reload
            sessionStorage.removeItem(INITIALIZED_KEY);
            
            // Set reload lock
            localStorage.setItem(RELOAD_LOCK_KEY, Date.now().toString());
            
            // Hard reload with cache bypass
            setTimeout(() => {
                // Try multiple reload methods for maximum compatibility
                if (window.location.reload) {
                    window.location.reload(true);
                } else {
                    window.location.href = window.location.href + '?t=' + Date.now();
                }
            }, 500);
        },

        // Start periodic version check
        startPeriodicCheck: function () {
            if (this.checkTimer) {
                clearInterval(this.checkTimer);
            }

            this.checkTimer = setInterval(() => {
                this.checkForUpdates(false);
            }, VERSION_CHECK_INTERVAL);

            console.log(`? Periodic version check started (every ${VERSION_CHECK_INTERVAL / 1000 / 60} minutes)`);
        },

        // Listen to visibility change
        listenToVisibilityChange: function () {
            document.addEventListener('visibilitychange', () => {
                if (!document.hidden) {
                    const lastCheck = parseInt(localStorage.getItem(LAST_CHECK_KEY) || '0');
                    const timeSinceLastCheck = Date.now() - lastCheck;
                    
                    if (timeSinceLastCheck > VERSION_CHECK_INTERVAL) {
                        console.log('?? Tab became visible - checking for updates...');
                        this.checkForUpdates(false);
                    }
                }
            });
        },

        // Manual version check
        manualCheck: async function () {
            console.log('?? Manual version check triggered');
            await this.checkForUpdates(false);
        },

        // Force clear cache (for debugging)
        forceClearCache: async function () {
            console.log('??? Force clearing cache...');
            await this.clearAllCaches();
            console.log('? Cache cleared, reloading...');
            this.reloadApp();
        }
    };

    // Auto-initialize when DOM is ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', () => {
            VersionManager.init();
        });
    } else {
        VersionManager.init();
    }

    // Expose to window for manual checks and debugging
    window.VersionManager = VersionManager;

})();
