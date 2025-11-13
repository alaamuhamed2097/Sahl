// Cache Buster - Manual cache clearing utility
// Can be called from browser console or triggered by admin

(function () {
    'use strict';

    window.CacheBuster = {
        // Clear all application caches and reload
        clearAllAndReload: async function () {
            console.log('??? Cache Buster: Clearing all caches...');

            try {
                // 1. Clear browser caches
                if ('caches' in window) {
                    const cacheNames = await caches.keys();
                    console.log('?? Found caches:', cacheNames);
                    
                    await Promise.all(
                        cacheNames.map(async cacheName => {
                            await caches.delete(cacheName);
                            console.log('? Deleted cache:', cacheName);
                        })
                    );
                }

                // 2. Unregister all service workers
                if ('serviceWorker' in navigator) {
                    const registrations = await navigator.serviceWorker.getRegistrations();
                    console.log('?? Found service workers:', registrations.length);
                    
                    for (const registration of registrations) {
                        await registration.unregister();
                        console.log('? Unregistered service worker');
                    }
                }

                // 3. Clear localStorage (except authentication)
                this.clearLocalStorage();

                // 4. Clear sessionStorage
                sessionStorage.clear();
                console.log('? SessionStorage cleared');

                // 5. Clear cookies (optional - be careful with authentication)
                // this.clearCookies();

                console.log('? All caches cleared!');
                console.log('?? Reloading in 1 second...');

                // 6. Reload with cache bypass
                setTimeout(() => {
                    window.location.href = window.location.origin + '?cacheBust=' + Date.now();
                }, 1000);

            } catch (error) {
                console.error('? Error clearing caches:', error);
                alert('Failed to clear cache: ' + error.message);
            }
        },

        // Clear localStorage but keep authentication
        clearLocalStorage: function () {
            const authKeys = [
                'auth_token',
                'refresh_token',
                'authToken',
                'isAuthenticated'
            ];

            // Save auth values
            const savedAuth = {};
            authKeys.forEach(key => {
                const value = localStorage.getItem(key);
                if (value !== null) {
                    savedAuth[key] = value;
                }
            });

            // Clear all
            localStorage.clear();
            console.log('? LocalStorage cleared');

            // Restore auth
            Object.keys(savedAuth).forEach(key => {
                localStorage.setItem(key, savedAuth[key]);
            });

            if (Object.keys(savedAuth).length > 0) {
                console.log('? Authentication preserved');
            }
        },

        // Clear cookies (use with caution)
        clearCookies: function () {
            const cookies = document.cookie.split(";");
            
            for (let i = 0; i < cookies.length; i++) {
                const cookie = cookies[i];
                const eqPos = cookie.indexOf("=");
                const name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
                
                // Don't clear authentication cookies
                if (!name.toLowerCase().includes('auth')) {
                    document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT;path=/";
                }
            }
            
            console.log('? Non-auth cookies cleared');
        },

        // Check current cache status
        checkCacheStatus: async function () {
            console.log('?? Cache Status Check:');
            console.log('?'.repeat(50));

            // Check caches
            if ('caches' in window) {
                const cacheNames = await caches.keys();
                console.log('?? Active Caches:', cacheNames.length);
                
                for (const cacheName of cacheNames) {
                    const cache = await caches.open(cacheName);
                    const keys = await cache.keys();
                    console.log(`   - ${cacheName}: ${keys.length} items`);
                }
            } else {
                console.log('? Cache API not supported');
            }

            // Check service workers
            if ('serviceWorker' in navigator) {
                const registrations = await navigator.serviceWorker.getRegistrations();
                console.log('?? Service Workers:', registrations.length);
                
                registrations.forEach((reg, index) => {
                    console.log(`   - SW ${index + 1}:`, reg.scope);
                    console.log('     Installing:', reg.installing);
                    console.log('     Waiting:', reg.waiting);
                    console.log('     Active:', reg.active);
                });
            } else {
                console.log('? Service Workers not supported');
            }

            // Check storage
            console.log('?? LocalStorage Items:', localStorage.length);
            console.log('?? SessionStorage Items:', sessionStorage.length);

            // Check current version
            const storedVersion = localStorage.getItem('app_version');
            console.log('?? Stored Version:', storedVersion || 'Not set');

            try {
                const response = await fetch('/version.json?t=' + Date.now(), {
                    cache: 'no-cache'
                });
                if (response.ok) {
                    const versionData = await response.json();
                    console.log('?? Server Version:', versionData.version);
                    console.log('?? Force Update:', versionData.forceUpdate);
                }
            } catch (error) {
                console.error('? Failed to fetch version:', error);
            }

            console.log('?'.repeat(50));
        },

        // Force update check
        forceUpdateCheck: async function () {
            console.log('?? Forcing update check...');
            
            if (window.VersionManager) {
                await window.VersionManager.manualCheck();
            } else {
                console.error('? VersionManager not available');
            }
        },

        // Quick reset (for users having issues)
        quickReset: function () {
            console.log('? Quick Reset initiated...');
            console.log('This will clear cache and reload the page.');
            console.log('Your login session will be preserved.');
            
            if (confirm('Clear cache and reload? (Your login will be preserved)')) {
                this.clearAllAndReload();
            }
        }
    };

    // Expose methods to console for easy access
    console.log('?'.repeat(60));
    console.log('???  Cache Buster Utility Loaded');
    console.log('?'.repeat(60));
    console.log('Available commands:');
    console.log('  CacheBuster.clearAllAndReload() - Clear all caches and reload');
    console.log('  CacheBuster.checkCacheStatus()  - Check current cache status');
    console.log('  CacheBuster.forceUpdateCheck()  - Force version update check');
    console.log('  CacheBuster.quickReset()        - Quick cache reset (with confirm)');
    console.log('?'.repeat(60));

})();
