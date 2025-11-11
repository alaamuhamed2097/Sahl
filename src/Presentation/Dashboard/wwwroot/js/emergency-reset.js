// Clear Version System - Emergency Reset Script
// ?????? ??? ???????? ??? ????? ????? ?? ??????? ????????

(function() {
    console.log('?? Starting emergency cleanup...');
    
    // 1. Clear localStorage
    console.log('??? Clearing localStorage...');
    localStorage.clear();
    
    // 2. Clear sessionStorage
    console.log('??? Clearing sessionStorage...');
    sessionStorage.clear();
    
    // 3. Clear all caches
    if ('caches' in window) {
        caches.keys().then(function(cacheNames) {
            console.log('??? Clearing caches...');
            return Promise.all(
                cacheNames.map(function(cacheName) {
                    console.log('   - Deleting:', cacheName);
                    return caches.delete(cacheName);
                })
            );
        }).then(function() {
            console.log('? All caches cleared');
        });
    }
    
    // 4. Unregister all service workers
    if ('serviceWorker' in navigator) {
        navigator.serviceWorker.getRegistrations().then(function(registrations) {
            console.log('?? Unregistering service workers...');
            registrations.forEach(function(registration) {
                console.log('   - Unregistering:', registration.scope);
                registration.unregister();
            });
            console.log('? All service workers unregistered');
        });
    }
    
    console.log('? Emergency cleanup complete!');
    console.log('?? Reloading in 2 seconds...');
    
    setTimeout(function() {
        window.location.reload(true);
    }, 2000);
    
})();
