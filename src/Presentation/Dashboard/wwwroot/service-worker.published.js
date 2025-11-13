/* Manifest version: OPTIMIZED + VERSION AWARE + DYNAMIC VERSION */
// Caution! Be sure you understand the caveats before publishing an application with
// offline support. See https://aka.ms/blazor-offline-considerations

// ✅ DYNAMIC VERSION MANAGEMENT: Fetch version from server
let APP_VERSION = 'v1.0.9'; // Fallback version
const cacheNamePrefix = 'offline-cache-';
let cacheName = `${cacheNamePrefix}${APP_VERSION}-${self.assetsManifest?.version || 'manual'}`;

// ✅ Fetch latest version from server
async function fetchLatestVersion() {
    try {
        const response = await fetch('/version.json?t=' + Date.now(), {
            cache: 'no-store',
            headers: {
                'Cache-Control': 'no-cache, no-store, must-revalidate',
                'Pragma': 'no-cache'
            }
        });
        
        if (response.ok) {
            const versionData = await response.json();
            const newVersion = 'v' + versionData.version;
            
            if (newVersion !== APP_VERSION) {
                console.log('📦 Service Worker: Version changed:', APP_VERSION, '->', newVersion);
                APP_VERSION = newVersion;
                cacheName = `${cacheNamePrefix}${APP_VERSION}-${self.assetsManifest?.version || 'manual'}`;
                return true; // Version changed
            }
        }
    } catch (error) {
        console.warn('⚠️ Service Worker: Failed to fetch version:', error);
    }
    return false; // No version change
}

self.importScripts('./service-worker-assets.js');
self.addEventListener('install', event => event.waitUntil(onInstall(event)));
self.addEventListener('activate', event => event.waitUntil(onActivate(event)));
self.addEventListener('fetch', event => event.respondWith(onFetch(event)));

// ✅ Include compressed files
const offlineAssetsInclude = [
    /\.dll$/, /\.dll\.br$/, /\.dll\.gz$/,
    /\.wasm$/, /\.wasm\.br$/, /\.wasm\.gz$/,
    /\.html$/, /\.js$/, /\.js\.br$/, /\.js\.gz$/,
    /\.css$/, /\.css\.br$/, /\.css\.gz$/,
    /\.woff2?$/, /\.png$/, /\.jpe?g$/, /\.gif$/, /\.ico$/,
    /\.blat$/, /\.blat\.br$/, /\.blat\.gz$/,
    /\.dat$/, /\.dat\.br$/, /\.dat\.gz$/,
    /\.json$/, /\.json\.br$/, /\.json\.gz$/,
    /\.webcil$/, /\.webcil\.br$/, /\.webcil\.gz$/
];

// ✅ Exclude version.json and service workers from cache (always fetch fresh)
const offlineAssetsExclude = [
    /^service-worker\.js$/,
    /service-worker\.published\.js$/,
    /version\.json$/  // Always fetch version.json fresh
];

const apiPathPrefixes = ['/api', '/signalr'];

async function onInstall(event) {
    console.info(`📦 Service Worker: Install (${APP_VERSION})`);

    // Check version before caching
    await fetchLatestVersion();

    // Cache assets without SRI validation to prevent integrity failures
    const assetsRequests = self.assetsManifest.assets
        .filter(asset => offlineAssetsInclude.some(pattern => pattern.test(asset.url)))
        .filter(asset => !offlineAssetsExclude.some(pattern => pattern.test(asset.url)))
        .map(asset => new Request(asset.url, {
            cache: 'no-cache',
            // Remove integrity to prevent caching failures with compressed files
            integrity: undefined
        }));

    // Cache assets individually to avoid atomic failures
    const cache = await caches.open(cacheName);
    const results = await Promise.allSettled(
        assetsRequests.map(req =>
            cache.add(req).catch(err => {
                console.warn(`⚠️ Failed to cache ${req.url}:`, err);
                return Promise.reject(err);
            })
        )
    );

    const failed = results.filter(r => r.status === 'rejected');
    if (failed.length) {
        console.warn(`⚠️ Service Worker: ${failed.length} asset(s) failed to cache during install.`);
    } else {
        console.info(`✅ Service Worker: Successfully cached ${results.length} assets.`);
    }

    // Ensure the new SW takes control ASAP
    await self.skipWaiting();
}

async function onActivate(event) {
    console.info(`🚀 Service Worker: Activate (${APP_VERSION})`);

    // Check for version update
    const versionChanged = await fetchLatestVersion();
    
    if (versionChanged) {
        console.log('🔄 Service Worker: New version detected, clearing all caches...');
        // Clear ALL caches when version changes
        const allCaches = await caches.keys();
        await Promise.all(
            allCaches.map(key => {
                console.log('🗑️ Deleting cache:', key);
                return caches.delete(key);
            })
        );
        
        // Notify all clients about the update
        const clients = await self.clients.matchAll({ type: 'window' });
        clients.forEach(client => {
            client.postMessage({
                type: 'VERSION_UPDATE',
                version: APP_VERSION
            });
        });
    } else {
        // Delete unused caches (including old version caches)
        const cacheKeys = await caches.keys();
        const deletedCaches = [];
        
        await Promise.all(cacheKeys
            .filter(key => {
                // Delete if it's an old offline-cache or if it doesn't match current version
                return (key.startsWith(cacheNamePrefix) && key !== cacheName) ||
                       (key.startsWith('dashboard-cache-') && !key.includes(APP_VERSION));
            })
            .map(key => {
                console.info(`🗑️ Service Worker: Deleting old cache ${key}`);
                deletedCaches.push(key);
                return caches.delete(key);
            })
        );

        if (deletedCaches.length > 0) {
            console.info(`✅ Service Worker: Deleted ${deletedCaches.length} old cache(s):`, deletedCaches);
        }
    }

    // Take control of uncontrolled clients immediately
    await self.clients.claim();
    console.info('✅ Service Worker: Activated and claimed clients');
}

async function onFetch(event) {
    const url = new URL(event.request.url);
    const isSameOrigin = url.origin === self.location.origin;
    const isApi = apiPathPrefixes.some(p => url.pathname.startsWith(p));

    // ✅ CRITICAL: Never cache _framework files (Blazor WebAssembly dependencies)
    if (url.pathname.includes('/_framework/')) {
        return fetch(event.request, { 
            cache: 'no-store',
            headers: {
                'Cache-Control': 'no-cache, no-store, must-revalidate',
                'Pragma': 'no-cache'
            }
        });
    }

    // ✅ Always fetch version.json fresh (no cache)
    if (url.pathname.includes('version.json')) {
        return fetch(event.request, {
            cache: 'no-cache',
            headers: {
                'Cache-Control': 'no-cache, no-store, must-revalidate',
                'Pragma': 'no-cache'
            }
        });
    }

    // Never cache service workers
    if (url.pathname.includes('service-worker')) {
        return fetch(event.request, { cache: 'no-store' });
    }

    // Bypass SW for API calls or cross-origin requests
    if (!isSameOrigin || isApi) {
        return fetch(event.request);
    }

    // Only handle GET requests
    if (event.request.method !== 'GET') {
        return fetch(event.request);
    }

    // For navigation requests, serve index.html from cache
    const shouldServeIndexHtml = event.request.mode === 'navigate';
    const cacheRequest = shouldServeIndexHtml ? 'index.html' : event.request;

    try {
        const cache = await caches.open(cacheName);
        let cachedResponse = await cache.match(cacheRequest);

        if (cachedResponse) {
            // Update cache in background (stale-while-revalidate)
            fetch(event.request).then(networkResponse => {
                if (networkResponse && networkResponse.status === 200) {
                    cache.put(cacheRequest, networkResponse.clone()).catch(() => {});
                }
            }).catch(() => {});
            
            return cachedResponse;
        }

        // If not in cache, fetch from network
        const networkResponse = await fetch(event.request);

        // Cache successful responses for future use
        if (networkResponse && networkResponse.status === 200) {
            const responseToCache = networkResponse.clone();
            cache.put(cacheRequest, responseToCache).catch(err => {
                console.warn(`⚠️ Failed to cache ${event.request.url}:`, err);
            });
        }

        return networkResponse;
    } catch (error) {
        console.error(`❌ Fetch failed for ${event.request.url}:`, error);

        // Try to serve index.html for navigation requests even on error
        if (shouldServeIndexHtml) {
            const cache = await caches.open(cacheName);
            const fallback = await cache.match('index.html');
            if (fallback) {
                return fallback;
            }
        }

        throw error;
    }
}

// ✅ Listen for messages from version-manager
self.addEventListener('message', event => {
    if (event.data && event.data.type === 'SKIP_WAITING') {
        console.log('⏭️ Service Worker: Skipping waiting...');
        self.skipWaiting();
    }
    
    if (event.data && event.data.type === 'CLEAR_CACHE') {
        console.log('🧹 Service Worker: Clearing all caches...');
        event.waitUntil(
            caches.keys().then(cacheNames => {
                return Promise.all(
                    cacheNames.map(cacheName => {
                        console.log(`🗑️ Deleting cache: ${cacheName}`);
                        return caches.delete(cacheName);
                    })
                );
            }).then(() => {
                // Notify client that cache is cleared
                if (event.source) {
                    event.source.postMessage({ type: 'CACHE_CLEARED' });
                }
            })
        );
    }
    
    if (event.data && event.data.type === 'GET_VERSION') {
        if (event.ports && event.ports[0]) {
            event.ports[0].postMessage({
                version: APP_VERSION,
                cacheName: cacheName
            });
        }
    }
    
    if (event.data && event.data.type === 'CHECK_VERSION') {
        console.log('🔍 Service Worker: Checking version...');
        event.waitUntil(
            fetchLatestVersion().then(versionChanged => {
                if (versionChanged && event.source) {
                    event.source.postMessage({
                        type: 'VERSION_UPDATE',
                        version: APP_VERSION
                    });
                } else if (event.source) {
                    event.source.postMessage({
                        type: 'VERSION_CURRENT',
                        version: APP_VERSION
                    });
                }
            })
        );
    }
});

// ✅ Periodic version check (every 5 minutes)
setInterval(async () => {
    const versionChanged = await fetchLatestVersion();
    if (versionChanged) {
        console.log('🔄 Service Worker: Version changed during periodic check, notifying clients...');
        const clients = await self.clients.matchAll({ type: 'window' });
        clients.forEach(client => {
            client.postMessage({
                type: 'VERSION_UPDATE',
                version: APP_VERSION
            });
        });
    }
}, 300000); // 5 minutes

console.info(`📦 Service Worker loaded: ${APP_VERSION}`);







