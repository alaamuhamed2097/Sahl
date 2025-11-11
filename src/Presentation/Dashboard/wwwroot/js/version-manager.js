// Version Manager - Automatic Cache Update System
(function () {
    'use strict';

    const VERSION_CHECK_INTERVAL = 300000; // 5 minutes
    const VERSION_STORAGE_KEY = 'app_version';
    const LAST_CHECK_KEY = 'version_last_check';
    const RELOAD_LOCK_KEY = 'version_reload_lock';
    const RELOAD_LOCK_DURATION = 30000; // 30 seconds
    const INITIALIZED_KEY = 'version_manager_initialized'; // ? NEW: Track initialization

    const VersionManager = {
        currentVersion: null,
        checkTimer: null,
        isUpdating: false,
        _hasInitialized: false, // ? NEW: Local flag

        // Initialize version manager
        init: function () {
            // ? FIX: ???? ?? ??????? ???? ????? init ??? ????
            if (this._hasInitialized) {
                console.log('? Version Manager: Already initialized, skipping...');
                return;
            }

            // ? FIX: ???? ?? sessionStorage ???? init ?? ?? ????
            const sessionInitialized = sessionStorage.getItem(INITIALIZED_KEY);
            if (sessionInitialized) {
                console.log('? Version Manager: Already initialized in this session, skipping...');
                this._hasInitialized = true;
                return;
            }

            console.log('? Version Manager: Initializing...');
            
            // ? Mark as initialized
            this._hasInitialized = true;
            sessionStorage.setItem(INITIALIZED_KEY, 'true');
            
            // Check if we just reloaded
            const reloadLock = localStorage.getItem(RELOAD_LOCK_KEY);
            if (reloadLock) {
                const lockTime = parseInt(reloadLock);
                const timeSinceLock = Date.now() - lockTime;
                
                if (timeSinceLock < RELOAD_LOCK_DURATION) {
                    console.log('?? Recently reloaded, skipping check for', (RELOAD_LOCK_DURATION - timeSinceLock) / 1000, 'seconds');
                    
                    // ? FIX: ???? ??? lock ??? ?????? ????? ??? ?? ?????? checkForUpdates
                    setTimeout(() => {
                        localStorage.removeItem(RELOAD_LOCK_KEY);
                        console.log('?? Reload lock cleared');
                        // ? ?? ?????? checkForUpdates ???!
                    }, RELOAD_LOCK_DURATION - timeSinceLock);
                    
                    this.startPeriodicCheck();
                    this.listenToVisibilityChange();
                    return;
                }
                
                // ? FIX: ??? ????? ??? ??? lock? ????? ?????? ??????
                localStorage.removeItem(RELOAD_LOCK_KEY);
            }
            
            this.checkForUpdates(true);
            this.startPeriodicCheck();
            this.listenToVisibilityChange();
        },

        // Check for updates
        checkForUpdates: async function (isInitial = false) {
            // ? FIX: ???? ?? isUpdating ???? checks ??????
            if (this.isUpdating) {
                console.log('?? Update already in progress, skipping check...');
                return;
            }

            try {
                const response = await fetch('/version.json?t=' + Date.now(), {
                    cache: 'no-cache',
                    headers: {
                        'Cache-Control': 'no-cache',
                        'Pragma': 'no-cache'
                    }
                });

                if (!response.ok) {
                    console.warn('?? Version check failed:', response.status);
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
                    // First time - store current version
                    localStorage.setItem(VERSION_STORAGE_KEY, serverVersion);
                    console.log('?? Version stored:', serverVersion);
                    return;
                }

                // ? FIX: ???? ?? RELOAD_LOCK ??? ???????
                const reloadLock = localStorage.getItem(RELOAD_LOCK_KEY);
                if (reloadLock) {
                    const lockTime = parseInt(reloadLock);
                    const timeSinceLock = Date.now() - lockTime;
                    
                    if (timeSinceLock < RELOAD_LOCK_DURATION) {
                        console.log('?? Update already in progress, skipping...');
                        return;
                    }
                    
                    // ? FIX: ???? ??? lock ??? ????? ????
                    localStorage.removeItem(RELOAD_LOCK_KEY);
                }

                if (serverVersion !== storedVersion) {
                    console.log('?? New version detected! Updating...');
                    this.isUpdating = true; // ? Mark as updating
                    await this.performUpdate(serverVersion, forceUpdate);
                } else if (forceUpdate) {
                    console.log('?? Force update requested!');
                    this.isUpdating = true; // ? Mark as updating
                    await this.performUpdate(serverVersion, true);
                } else if (!isInitial) {
                    console.log('? App is up to date');
                }

                localStorage.setItem(LAST_CHECK_KEY, Date.now().toString());

            } catch (error) {
                console.error('? Version check error:', error);
                this.isUpdating = false; // ? Reset on error
            }
        },

        // Perform update
        performUpdate: async function (newVersion, forceUpdate) {
            try {
                console.log('?? Clearing cache...');

                // ? FIX: ????? ??????? ??? ??? reload
                localStorage.setItem(VERSION_STORAGE_KEY, newVersion);
                console.log('?? Version updated in storage:', newVersion);

                // Clear browser cache
                if ('caches' in window) {
                    const cacheNames = await caches.keys();
                    await Promise.all(
                        cacheNames.map(cacheName => {
                            console.log('??? Deleting cache:', cacheName);
                            return caches.delete(cacheName);
                        })
                    );
                }

                // Unregister service workers
                if ('serviceWorker' in navigator) {
                    const registrations = await navigator.serviceWorker.getRegistrations();
                    for (const registration of registrations) {
                        console.log('?? Unregistering service worker...');
                        await registration.unregister();
                    }
                }

                // Clear local storage except important data
                this.clearNonEssentialStorage(newVersion);

                // Show update notification
                this.showUpdateNotification(newVersion, forceUpdate);

            } catch (error) {
                console.error('? Update failed:', error);
                // ? FIX: ??? ?? ???? ?????? ???? ??????? ??????
                localStorage.setItem(VERSION_STORAGE_KEY, newVersion);
                // Force reload anyway
                this.reloadApp();
            }
        },

        // Clear non-essential storage
        clearNonEssentialStorage: function (newVersion) {
            // ? FIX: Keep ALL authentication and user-related keys
            const keysToKeep = [
                VERSION_STORAGE_KEY,
                LAST_CHECK_KEY,
                RELOAD_LOCK_KEY,
                // ? Authentication keys
                'auth_token',
                'refresh_token',
                'authToken',
                'isAuthenticated',
                // ? User preferences
                'user_preferences',
                'autoLoadPreference',
                // ? Language keys
                'selected_language',
                'lang',
                'current_language',
                // ? Application settings
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

            // ? NEW: Also save ALL keys that start with specific prefixes
            const prefixesToKeep = ['auth', 'user', 'selected_', 'version_', 'reload_', 'lang', 'pref_'];
            for (let i = 0; i < localStorage.length; i++) {
                const key = localStorage.key(i);
                if (key && prefixesToKeep.some(prefix => key.toLowerCase().startsWith(prefix))) {
                    const value = localStorage.getItem(key);
                    if (value !== null && !savedValues[key]) {
                        savedValues[key] = value;
                        console.log('?? Keeping prefixed key:', key);
                    }
                }
            }

            // Clear all localStorage
            localStorage.clear();
            
            // ? CRITICAL FIX: DON'T clear sessionStorage - it contains Blazor state and language data
            // sessionStorage.clear(); // ? REMOVED - This was causing logout and language reset!

            // Restore saved values
            Object.keys(savedValues).forEach(key => {
                localStorage.setItem(key, savedValues[key]);
            });

            // ? Ensure version is saved
            localStorage.setItem(VERSION_STORAGE_KEY, newVersion);

            console.log('? Storage cleaned (keeping authentication & user data)');
            console.log('?? Kept keys:', Object.keys(savedValues));
        },

        // Show update notification
        showUpdateNotification: function (version, forceUpdate) {
            // ? SILENT UPDATE: Skip notifications, just reload directly
            console.log('?? Silent update to version:', version);
            this.reloadApp();
        },

        // Fallback notification (NOT USED - Silent update)
        showFallbackNotification: function (message) {
            // ? REMOVED: No visual notifications
            console.log('?? Update notification (silent):', message);
        },

        // Reload app with hard refresh
        reloadApp: function () {
            console.log('?? Reloading application...');
            
            // ? Clear sessionStorage flag before reload
            sessionStorage.removeItem(INITIALIZED_KEY);
            
            // ? FIX: Set reload lock AFTER updating version
            localStorage.setItem(RELOAD_LOCK_KEY, Date.now().toString());
            
            // Hard reload
            setTimeout(() => {
                window.location.reload(true);
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

        // Listen to visibility change (check when user returns to tab)
        listenToVisibilityChange: function () {
            document.addEventListener('visibilitychange', () => {
                if (!document.hidden) {
                    const lastCheck = parseInt(localStorage.getItem(LAST_CHECK_KEY) || '0');
                    const timeSinceLastCheck = Date.now() - lastCheck;
                    
                    // Check if more than 5 minutes since last check
                    if (timeSinceLastCheck > VERSION_CHECK_INTERVAL) {
                        console.log('?? Tab became visible - checking for updates...');
                        this.checkForUpdates(false);
                    }
                }
            });
        },

        // Manual version check (can be called from UI)
        manualCheck: async function () {
            console.log('?? Manual version check triggered');
            await this.checkForUpdates(false);
        }
    };

    // ? FIX: ????? init() ??? ??? ????? ?????? ??????
    // Auto-initialize when DOM is ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', () => {
            VersionManager.init();
        });
    } else {
        VersionManager.init();
    }

    // Expose to window for manual checks
    window.VersionManager = VersionManager;

})();
