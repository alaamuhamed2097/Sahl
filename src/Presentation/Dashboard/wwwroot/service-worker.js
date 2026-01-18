// Service Worker for Dashboard - Version Management Only (Minimal Version)
// No caching, no fetch interception - only version updates

let CACHE_VERSION = 'v1.0.8';

// Fetch version from server
async function fetchLatestVersion() {
    try {
        const response = await fetch('/version.json?t=' + Date.now(), {
            cache: 'no-store'
        });

        if (response.ok) {
            const versionData = await response.json();
            const newVersion = 'v' + versionData.version;

            if (newVersion !== CACHE_VERSION) {
                CACHE_VERSION = newVersion;
                return true;
            }
        }
    } catch (error) {
        console.warn('⚠️ Failed to fetch version:', error);
    }
    return false;
}

// Install event
self.addEventListener('install', event => {
    self.skipWaiting();
});

// Activate event
self.addEventListener('activate', event => {
    event.waitUntil(
        (async () => {
            const versionChanged = await fetchLatestVersion();

            if (versionChanged) {
                const clients = await self.clients.matchAll({ type: 'window' });
                clients.forEach(client => {
                    client.postMessage({
                        type: 'VERSION_UPDATE',
                        version: CACHE_VERSION
                    });
                });
            }

            await self.clients.claim();
        })()
    );
});

// Message handler
self.addEventListener('message', event => {
    if (event.data?.type === 'SKIP_WAITING') {
        self.skipWaiting();
    }

    if (event.data?.type === 'CHECK_VERSION') {
        event.waitUntil(
            fetchLatestVersion().then(versionChanged => {
                event.source.postMessage({
                    type: versionChanged ? 'VERSION_UPDATE' : 'VERSION_CURRENT',
                    version: CACHE_VERSION
                });
            })
        );
    }

    if (event.data?.type === 'GET_VERSION') {
        event.source.postMessage({
            type: 'CURRENT_VERSION',
            version: CACHE_VERSION
        });
    }
});

// Periodic version check (every 5 minutes)
setInterval(async () => {
    const versionChanged = await fetchLatestVersion();
    if (versionChanged) {
        const clients = await self.clients.matchAll({ type: 'window' });
        clients.forEach(client => {
            client.postMessage({
                type: 'VERSION_UPDATE',
                version: CACHE_VERSION
            });
        });
    }
}, 300000);