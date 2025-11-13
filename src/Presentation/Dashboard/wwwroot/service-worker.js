// Service Worker for Dashboard - Enhanced Cache Management with Dynamic Versioning
// Version-aware caching with automatic updates

let CACHE_VERSION = 'v1.0.8'; // Default fallback
let CACHE_NAME = `dashboard-cache-${CACHE_VERSION}`;
let RUNTIME_CACHE = `runtime-${CACHE_VERSION}`;

// Fetch version from server on service worker activation
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
            
            if (newVersion !== CACHE_VERSION) {
                console.log('📦 Version changed:', CACHE_VERSION, '->', newVersion);
                CACHE_VERSION = newVersion;
                CACHE_NAME = `dashboard-cache-${CACHE_VERSION}`;
                RUNTIME_CACHE = `runtime-${CACHE_VERSION}`;
                return true; // Version changed
            }
        }
    } catch (error) {
        console.warn('⚠️ Failed to fetch version:', error);
    }
    return false; // No version change
}

// Files to cache immediately (critical assets)
const PRECACHE_URLS = [
    '/',
    '/index.html',
    '/css/app.min.css',
    '/css/loader.min.css',
    '/assets/images/logo-icon.svg',
    '/assets/images/favicon.svg'
];

// Install event - cache critical assets
self.addEventListener('install', event => {
    console.log('📦 Service Worker installing...', CACHE_VERSION);
    
    event.waitUntil(
        caches.open(CACHE_NAME)
            .then(cache => {
                console.log('📥 Caching critical assets');
                return cache.addAll(PRECACHE_URLS).catch(err => {
                    console.warn('⚠️ Failed to cache some assets:', err);
                });
            })
            .then(() => {
                console.log('✅ Service Worker installed, skipping waiting...');
                return self.skipWaiting(); // Activate immediately
            })
    );
});

// Activate event - clean old caches and check version
self.addEventListener('activate', event => {
    console.log('🚀 Service Worker activating...', CACHE_VERSION);
    
    event.waitUntil(
        (async () => {
            // Check for version update
            const versionChanged = await fetchLatestVersion();
            
            if (versionChanged) {
                console.log('🔄 New version detected, clearing all caches...');
                // Clear ALL caches when version changes
                const cacheNames = await caches.keys();
                await Promise.all(
                    cacheNames.map(cacheName => {
                        console.log('🗑️ Deleting cache:', cacheName);
                        return caches.delete(cacheName);
                    })
                );
                
                // Notify all clients about the update
                const clients = await self.clients.matchAll({ type: 'window' });
                clients.forEach(client => {
                    client.postMessage({
                        type: 'VERSION_UPDATE',
                        version: CACHE_VERSION
                    });
                });
            } else {
                // Only clean old caches with different versions
                const cacheNames = await caches.keys();
                await Promise.all(
                    cacheNames.map(cacheName => {
                        if (cacheName !== CACHE_NAME && cacheName !== RUNTIME_CACHE) {
                            console.log('🗑️ Deleting old cache:', cacheName);
                            return caches.delete(cacheName);
                        }
                    })
                );
            }
            
            // Take control of all clients immediately
            await self.clients.claim();
            console.log('✅ Service Worker activated and claimed all clients');
        })()
    );
});

// Fetch event - network first strategy for dynamic content
self.addEventListener('fetch', event => {
    const { request } = event;
    const url = new URL(request.url);

    // Skip cross-origin requests
    if (url.origin !== location.origin) {
        return;
    }

    // ✅ CRITICAL: Never cache _framework files (Blazor WebAssembly dependencies)
    if (url.pathname.includes('/_framework/')) {
        event.respondWith(
            fetch(request, { 
                cache: 'no-store',
                headers: {
                    'Cache-Control': 'no-cache, no-store, must-revalidate',
                    'Pragma': 'no-cache'
                }
            })
        );
        return;
    }

    // Skip version.json from cache (always fetch fresh)
    if (url.pathname.includes('/version.json')) {
        event.respondWith(
            fetch(request, { 
                cache: 'no-store',
                headers: {
                    'Cache-Control': 'no-cache, no-store, must-revalidate',
                    'Pragma': 'no-cache'
                }
            })
        );
        return;
    }

    // Never cache the service worker itself
    if (url.pathname.includes('service-worker')) {
        event.respondWith(
            fetch(request, { cache: 'no-store' })
        );
        return;
    }

    // Network-first strategy for API calls
    if (url.pathname.includes('/api/')) {
        event.respondWith(networkFirst(request));
        return;
    }

    // Cache-first strategy for static assets (but not _framework)
    if (isStaticAsset(url.pathname)) {
        event.respondWith(cacheFirst(request));
        return;
    }

    // Network-first for everything else
    event.respondWith(networkFirst(request));
});

// Network-first strategy
async function networkFirst(request) {
    try {
        const response = await fetch(request);
        
        // Cache successful responses
        if (response && response.status === 200) {
            const cache = await caches.open(RUNTIME_CACHE);
            cache.put(request, response.clone());
        }
        
        return response;
    } catch (error) {
        // Fallback to cache if network fails
        const cachedResponse = await caches.match(request);
        if (cachedResponse) {
            console.log('📦 Serving from cache (offline):', request.url);
            return cachedResponse;
        }
        
        // Return offline page if available
        return caches.match('/offline.html') || new Response('Offline');
    }
}

// Cache-first strategy
async function cacheFirst(request) {
    const cachedResponse = await caches.match(request);
    
    if (cachedResponse) {
        // Update cache in background
        fetch(request).then(response => {
            if (response && response.status === 200) {
                caches.open(CACHE_NAME).then(cache => {
                    cache.put(request, response);
                });
            }
        }).catch(() => {});
        
        return cachedResponse;
    }
    
    try {
        const response = await fetch(request);
        
        if (response && response.status === 200) {
            const cache = await caches.open(CACHE_NAME);
            cache.put(request, response.clone());
        }
        
        return response;
    } catch (error) {
        console.error('❌ Failed to fetch:', request.url);
        return new Response('Resource not available', { status: 503 });
    }
}

// Check if URL is a static asset
function isStaticAsset(pathname) {
    const staticExtensions = ['.css', '.js', '.png', '.jpg', '.jpeg', '.svg', '.gif', '.woff', '.woff2', '.ttf', '.eot'];
    return staticExtensions.some(ext => pathname.endsWith(ext));
}

// Listen for messages from the client
self.addEventListener('message', event => {
    if (event.data && event.data.type === 'SKIP_WAITING') {
        console.log('⏭️ Skipping waiting...');
        self.skipWaiting();
    }
    
    if (event.data && event.data.type === 'CLEAR_CACHE') {
        console.log('🗑️ Clearing all caches...');
        event.waitUntil(
            caches.keys().then(cacheNames => {
                return Promise.all(
                    cacheNames.map(cacheName => caches.delete(cacheName))
                );
            }).then(() => {
                // Notify client that cache is cleared
                event.source.postMessage({ type: 'CACHE_CLEARED' });
            })
        );
    }
    
    if (event.data && event.data.type === 'CHECK_VERSION') {
        console.log('🔍 Checking version...');
        event.waitUntil(
            fetchLatestVersion().then(versionChanged => {
                if (versionChanged) {
                    // Clear all caches
                    return caches.keys().then(cacheNames => {
                        return Promise.all(
                            cacheNames.map(cacheName => caches.delete(cacheName))
                        );
                    }).then(() => {
                        event.source.postMessage({
                            type: 'VERSION_UPDATE',
                            version: CACHE_VERSION
                        });
                    });
                } else {
                    event.source.postMessage({
                        type: 'VERSION_CURRENT',
                        version: CACHE_VERSION
                    });
                }
            })
        );
    }
});

// Periodic version check (every 5 minutes)
setInterval(async () => {
    const versionChanged = await fetchLatestVersion();
    if (versionChanged) {
        console.log('🔄 Version changed during periodic check, notifying clients...');
        const clients = await self.clients.matchAll({ type: 'window' });
        clients.forEach(client => {
            client.postMessage({
                type: 'VERSION_UPDATE',
                version: CACHE_VERSION
            });
        });
    }
}, 300000); // 5 minutes

