// Version Manager - Automatic Cache Update System
(function () {
    'use strict';

    const VERSION_CHECK_INTERVAL = 300000; // 5 minutes
    const VERSION_STORAGE_KEY = 'app_version';
    const LAST_CHECK_KEY = 'version_last_check';
    const RELOAD_LOCK_KEY = 'version_reload_lock';
    const RELOAD_LOCK_DURATION = 10000; // 10 seconds

    const VersionManager = {
        currentVersion: null,
        checkTimer: null,

        // Initialize version manager
        init: function () {
            console.log('? Version Manager: Initializing...');
            
            // Check if we just reloaded
            const reloadLock = localStorage.getItem(RELOAD_LOCK_KEY);
            if (reloadLock) {
                const lockTime = parseInt(reloadLock);
                const timeSinceLock = Date.now() - lockTime;
                
                if (timeSinceLock < RELOAD_LOCK_DURATION) {
                    console.log('?? Recently reloaded, skipping check for', (RELOAD_LOCK_DURATION - timeSinceLock) / 1000, 'seconds');
                    setTimeout(() => {
                        localStorage.removeItem(RELOAD_LOCK_KEY);
                        this.checkForUpdates(true);
                    }, RELOAD_LOCK_DURATION - timeSinceLock);
                    this.startPeriodicCheck();
                    this.listenToVisibilityChange();
                    return;
                }
            }
            
            this.checkForUpdates(true);
            this.startPeriodicCheck();
            this.listenToVisibilityChange();
        },

        // Check for updates
        checkForUpdates: async function (isInitial = false) {
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

                if (serverVersion !== storedVersion) {
                    console.log('?? New version detected! Updating...');
                    await this.performUpdate(serverVersion, forceUpdate);
                } else if (forceUpdate) {
                    console.log('?? Force update requested!');
                    await this.performUpdate(serverVersion, true);
                } else if (!isInitial) {
                    console.log('? App is up to date');
                }

                localStorage.setItem(LAST_CHECK_KEY, Date.now().toString());

            } catch (error) {
                console.error('? Version check error:', error);
            }
        },

        // Perform update
        performUpdate: async function (newVersion, forceUpdate) {
            try {
                console.log('?? Clearing cache...');

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
                // Force reload anyway
                this.reloadApp();
            }
        },

        // Clear non-essential storage
        clearNonEssentialStorage: function (newVersion) {
            const keysToKeep = [
                VERSION_STORAGE_KEY,
                LAST_CHECK_KEY,
                RELOAD_LOCK_KEY,
                'auth_token',
                'refresh_token',
                'user_preferences',
                'selected_language'
            ];

            // Save values
            const savedValues = {};
            keysToKeep.forEach(key => {
                const value = localStorage.getItem(key);
                if (value !== null) {
                    savedValues[key] = value;
                }
            });

            // Clear all
            localStorage.clear();
            sessionStorage.clear();

            // Restore saved values
            Object.keys(savedValues).forEach(key => {
                localStorage.setItem(key, savedValues[key]);
            });

            // Store new version
            localStorage.setItem(VERSION_STORAGE_KEY, newVersion);

            console.log('? Storage cleaned (keeping essential data)');
        },

        // Show update notification
        showUpdateNotification: function (version, forceUpdate) {
            const message = forceUpdate 
                ? `????? ??? ????? ???? (${version}). ???? ????? ??????? ?????...`
                : `????? ???? ???? (${version}). ???? ??????? ????????...`;

            console.log('??', message);

            // Show SweetAlert if available
            if (typeof Swal !== 'undefined') {
                Swal.fire({
                    title: '????? ?????',
                    text: message,
                    icon: 'info',
                    showConfirmButton: false,
                    timer: forceUpdate ? 2000 : 3000,
                    allowOutsideClick: false,
                    allowEscapeKey: false
                }).then(() => {
                    this.reloadApp();
                });
            } else if (typeof swal === 'function') {
                swal({
                    title: '????? ?????',
                    text: message,
                    icon: 'info',
                    buttons: false,
                    timer: forceUpdate ? 2000 : 3000,
                    closeOnClickOutside: false,
                    closeOnEsc: false
                }).then(() => {
                    this.reloadApp();
                });
            } else {
                // Fallback notification
                this.showFallbackNotification(message);
                setTimeout(() => this.reloadApp(), forceUpdate ? 2000 : 3000);
            }
        },

        // Fallback notification (if SweetAlert not available)
        showFallbackNotification: function (message) {
            const notification = document.createElement('div');
            notification.style.cssText = `
                position: fixed;
                top: 20px;
                right: 20px;
                background: linear-gradient(135deg, #0EA5E9 0%, #0369A1 100%);
                color: white;
                padding: 20px 30px;
                border-radius: 12px;
                box-shadow: 0 4px 20px rgba(0, 0, 0, 0.3);
                z-index: 999999;
                font-family: 'Cairo', 'Tajawal', sans-serif;
                font-size: 16px;
                max-width: 400px;
                animation: slideIn 0.3s ease-out;
            `;
            notification.innerHTML = `
                <div style="display: flex; align-items: center; gap: 15px;">
                    <div style="font-size: 32px;">??</div>
                    <div>${message}</div>
                </div>
            `;
            document.body.appendChild(notification);
        },

        // Reload app with hard refresh
        reloadApp: function () {
            console.log('?? Reloading application...');
            
            // Set reload lock to prevent infinite loop
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
