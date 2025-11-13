/* Manifest version: OPTIMIZED + VERSION AWARE */
// Caution! Be sure you understand the caveats before publishing an application with
// offline support. See https://aka.ms/blazor-offline-considerations

// ✅ VERSION MANAGEMENT: Dynamic cache versioning
const APP_VERSION = 'v1.0.8'; // Will be updated by update-version.ps1
const cacheNamePrefix = 'offline-cache-';
const cacheName = `${cacheNamePrefix}${APP_VERSION}-${self.assetsManifest?.version || 'manual'}`;

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

// ✅ Exclude version.json from cache (always fetch fresh)
const offlineAssetsExclude = [
    /^service-worker\.js$/,
    /version\.json$/  // Always fetch version.json fresh
];

const apiPathPrefixes = ['/api', '/signalr'];

async function onInstall(event) {
    console.info(`Service worker: Install (${APP_VERSION})`);

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
                console.warn(`Failed to cache ${req.url}:`, err);
                return Promise.reject(err);
            })
        )
    );

    const failed = results.filter(r => r.status === 'rejected');
    if (failed.length) {
        console.warn(`Service worker: ${failed.length} asset(s) failed to cache during install.`);
    } else {
        console.info(`Service worker: Successfully cached ${results.length} assets.`);
    }

    // Ensure the new SW takes control ASAP
    await self.skipWaiting();
}

async function onActivate(event) {
    console.info(`Service worker: Activate (${APP_VERSION})`);

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
            console.info(`Service worker: Deleting old cache ${key}`);
            deletedCaches.push(key);
            return caches.delete(key);
        })
    );

    if (deletedCaches.length > 0) {
        console.info(`Service worker: Deleted ${deletedCaches.length} old cache(s):`, deletedCaches);
    }

    // Take control of uncontrolled clients immediately
    await self.clients.claim();
    console.info('Service worker: Activated and claimed clients');
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
                'Cache-Control': 'no-cache',
                'Pragma': 'no-cache'
            }
        });
    }

    // ✅ Always fetch version.json fresh (no cache)
    if (url.pathname.includes('version.json')) {
        return fetch(event.request, {
            cache: 'no-cache',
            headers: {
                'Cache-Control': 'no-cache',
                'Pragma': 'no-cache'
            }
        });
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
            return cachedResponse;
        }

        // If not in cache, fetch from network
        const networkResponse = await fetch(event.request);

        // Cache successful responses for future use
        if (networkResponse && networkResponse.status === 200) {
            const responseToCache = networkResponse.clone();
            cache.put(cacheRequest, responseToCache).catch(err => {
                console.warn(`Failed to cache ${event.request.url}:`, err);
            });
        }

        return networkResponse;
    } catch (error) {
        console.error(`Fetch failed for ${event.request.url}:`, error);

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
            })
        );
    }
    
    if (event.data && event.data.type === 'GET_VERSION') {
        event.ports[0].postMessage({
            version: APP_VERSION,
            cacheName: cacheName
        });
    }
});

console.info(`📦 Service Worker loaded: ${APP_VERSION}`);







