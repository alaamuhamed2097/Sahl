// Service Worker for Dashboard
// This service worker intentionally doesn't cache assets for easier development.
// For production, consider implementing proper caching strategies.

self.addEventListener('install', event => {
    console.log('Service Worker installing.');
    self.skipWaiting();
});

self.addEventListener('activate', event => {
    console.log('Service Worker activating.');
    return self.clients.claim();
});