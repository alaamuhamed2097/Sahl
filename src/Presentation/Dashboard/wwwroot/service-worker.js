// Service Worker for Dashboard - Enhanced Cache Management
// Version-aware caching with automatic updates

const CACHE_VERSION = 'v1.0.1';
const CACHE_NAME = `dashboard-cache-${CACHE_VERSION}`;
const RUNTIME_CACHE = `runtime-${CACHE_VERSION}`;

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
    console.log('?? Service Worker installing...', CACHE_VERSION);
    
    event.waitUntil(
        caches.open(CACHE_NAME)
            .then(cache => {
                console.log('?? Caching critical assets');
                return cache.addAll(PRECACHE_URLS).catch(err => {
                    console.warn('?? Failed to cache some assets:', err);
                });
            })
            .then(() => self.skipWaiting())
    );
});

// Activate event - clean old caches
self.addEventListener('activate', event => {
    console.log('? Service Worker activating...', CACHE_VERSION);
    
    event.waitUntil(
        caches.keys()
            .then(cacheNames => {
                return Promise.all(
                    cacheNames.map(cacheName => {
                        if (cacheName !== CACHE_NAME && cacheName !== RUNTIME_CACHE) {
                            console.log('??? Deleting old cache:', cacheName);
                            return caches.delete(cacheName);
                        }
                    })
                );
            })
            .then(() => self.clients.claim())
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

    // Skip version.json from cache (always fetch fresh)
    if (url.pathname.includes('/version.json')) {
        event.respondWith(fetch(request));
        return;
    }

    // Network-first strategy for API calls
    if (url.pathname.includes('/api/')) {
        event.respondWith(networkFirst(request));
        return;
    }

    // Cache-first strategy for static assets
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
            console.log('?? Serving from cache (offline):', request.url);
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
        console.error('? Failed to fetch:', request.url);
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
        console.log('?? Skipping waiting...');
        self.skipWaiting();
    }
    
    if (event.data && event.data.type === 'CLEAR_CACHE') {
        console.log('?? Clearing all caches...');
        event.waitUntil(
            caches.keys().then(cacheNames => {
                return Promise.all(
                    cacheNames.map(cacheName => caches.delete(cacheName))
                );
            })
        );
    }
});
