/* Service Worker - Version Management Only (No Caching, No Logs) */

let APP_VERSION = 'v1.0.9';

async function fetchLatestVersion() {
    try {
        const response = await fetch('/version.json?t=' + Date.now(), {
            cache: 'no-store'
        });

        if (response.ok) {
            const versionData = await response.json();
            const newVersion = 'v' + versionData.version;

            if (newVersion !== APP_VERSION) {
                APP_VERSION = newVersion;
                return true;
            }
        }
    } catch (error) {
        // Silent fail
    }
    return false;
}

self.addEventListener('install', event => {
    event.waitUntil(self.skipWaiting());
});

self.addEventListener('activate', event => {
    event.waitUntil(
        (async () => {
            const versionChanged = await fetchLatestVersion();

            if (versionChanged) {
                const clients = await self.clients.matchAll({ type: 'window' });
                clients.forEach(client => {
                    client.postMessage({
                        type: 'VERSION_UPDATE',
                        version: APP_VERSION
                    });
                });
            }

            await self.clients.claim();
        })()
    );
});

self.addEventListener('message', event => {
    if (event.data?.type === 'SKIP_WAITING') {
        self.skipWaiting();
    }

    if (event.data?.type === 'CHECK_VERSION') {
        event.waitUntil(
            fetchLatestVersion().then(versionChanged => {
                if (event.source) {
                    event.source.postMessage({
                        type: versionChanged ? 'VERSION_UPDATE' : 'VERSION_CURRENT',
                        version: APP_VERSION
                    });
                }
            })
        );
    }

    if (event.data?.type === 'GET_VERSION') {
        if (event.ports?.[0]) {
            event.ports[0].postMessage({
                version: APP_VERSION
            });
        }
    }
});

setInterval(async () => {
    const versionChanged = await fetchLatestVersion();
    if (versionChanged) {
        const clients = await self.clients.matchAll({ type: 'window' });
        clients.forEach(client => {
            client.postMessage({
                type: 'VERSION_UPDATE',
                version: APP_VERSION
            });
        });
    }
}, 300000);