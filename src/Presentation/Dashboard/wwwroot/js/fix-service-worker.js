/**
 * Fix Service Worker Issues
 * Run this in console to unregister problematic service workers
 */

(async function fixServiceWorker() {
    console.log('?? Fixing Service Worker...');

    try {
        // 1. Unregister all service workers
        if ('serviceWorker' in navigator) {
            const registrations = await navigator.serviceWorker.getRegistrations();
            
            for (const registration of registrations) {
                console.log('??? Unregistering service worker:', registration.scope);
                await registration.unregister();
            }
            
            console.log(`? Unregistered ${registrations.length} service worker(s)`);
        }

        // 2. Clear all caches
        const cacheNames = await caches.keys();
        for (const cacheName of cacheNames) {
            console.log('??? Deleting cache:', cacheName);
            await caches.delete(cacheName);
        }
        console.log(`? Deleted ${cacheNames.length} cache(s)`);

        // 3. Clear storage
        localStorage.clear();
        sessionStorage.clear();
        console.log('? Cleared localStorage and sessionStorage');

        // 4. Reload page
        console.log('?? Reloading page in 2 seconds...');
        setTimeout(() => {
            window.location.reload(true);
        }, 2000);

    } catch (error) {
        console.error('? Error fixing service worker:', error);
    }
})();
