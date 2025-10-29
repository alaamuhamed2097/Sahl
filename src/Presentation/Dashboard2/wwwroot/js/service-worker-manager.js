// Service Worker Update Manager for Subdirectory Setup
// This script helps detect and handle service worker updates

class ServiceWorkerManager {
    constructor() {
        this.currentVersion = null;
        this.updateAvailable = false;
        this.registration = null;
        this.isSubdirectorySetup = true; // Flag for subdirectory setup
    }

    async initialize() {
        if ('serviceWorker' in navigator) {
            try {
                // For subdirectory setup, we need to be careful about the scope and URL
                const swUrl = `/service-worker.js?v=${Date.now()}`;
                
                console.log('Initializing Service Worker Manager...');
                console.log('Service Worker URL:', swUrl);
                
                // First, unregister any existing service workers to ensure clean state
                await this.cleanupExistingServiceWorkers();

                // Register service worker with proper scope for subdirectory setup
                this.registration = await navigator.serviceWorker.register(swUrl, {
                    scope: '/',
                    updateViaCache: 'none' // Always check for updates
                });

                console.log('Service Worker registered successfully:', this.registration);

                // Check for updates immediately
                await this.checkForUpdates();

                // Listen for service worker updates
                this.registration.addEventListener('updatefound', () => {
                    console.log('Service Worker update found!');
                    this.handleUpdateFound();
                });

                // Listen for messages from service worker
                navigator.serviceWorker.addEventListener('message', (event) => {
                    this.handleServiceWorkerMessage(event.data);
                });

                // Check for updates every 30 seconds
                setInterval(() => this.checkForUpdates(), 30000);

                // Get current version
                await this.getCurrentVersion();

                // Handle page visibility changes to check for updates when page becomes visible
                document.addEventListener('visibilitychange', () => {
                    if (!document.hidden) {
                        setTimeout(() => this.checkForUpdates(), 1000);
                    }
                });

            } catch (error) {
                console.error('Service Worker registration failed:', error);
                this.showUpdateNotification(
                    'Service Worker registration failed. Some features may not work offline. Error: ' + error.message, 
                    'error'
                );
            }
        } else {
            console.warn('Service Workers are not supported in this browser');
        }
    }

    async cleanupExistingServiceWorkers() {
        try {
            const registrations = await navigator.serviceWorker.getRegistrations();
            
            if (registrations.length > 0) {
                console.log(`Found ${registrations.length} existing service worker registrations`);
                
                const unregisterPromises = registrations.map(registration => {
                    console.log('Unregistering existing service worker:', registration.scope);
                    return registration.unregister();
                });
                
                await Promise.all(unregisterPromises);
                console.log('All existing service workers unregistered');
                
                // Clear all caches as well
                const cacheNames = await caches.keys();
                if (cacheNames.length > 0) {
                    console.log(`Clearing ${cacheNames.length} existing caches`);
                    await Promise.all(
                        cacheNames.map(cacheName => {
                            console.log('Deleting cache:', cacheName);
                            return caches.delete(cacheName);
                        })
                    );
                    console.log('All existing caches cleared');
                }
            }
        } catch (error) {
            console.warn('Error during cleanup:', error);
        }
    }

    async checkForUpdates() {
        if (this.registration) {
            try {
                console.log('Checking for service worker updates...');
                await this.registration.update();
            } catch (error) {
                console.error('Failed to check for service worker updates:', error);
            }
        }
    }

    async getCurrentVersion() {
        if ('serviceWorker' in navigator && navigator.serviceWorker.controller) {
            const messageChannel = new MessageChannel();
            
            return new Promise((resolve) => {
                const timeout = setTimeout(() => {
                    console.warn('Timeout waiting for service worker version response');
                    resolve({ version: 'unknown', timeout: true });
                }, 5000);
                
                messageChannel.port1.onmessage = (event) => {
                    clearTimeout(timeout);
                    this.currentVersion = event.data.version;
                    console.log('Current Service Worker version:', this.currentVersion);
                    resolve(event.data);
                };

                navigator.serviceWorker.controller.postMessage({
                    type: 'GET_VERSION'
                }, [messageChannel.port2]);
            });
        }
    }

    handleUpdateFound() {
        const installingWorker = this.registration.installing;
        
        installingWorker.addEventListener('statechange', () => {
            console.log('Service worker state changed to:', installingWorker.state);
            
            if (installingWorker.state === 'installed') {
                if (navigator.serviceWorker.controller) {
                    // New update available
                    this.updateAvailable = true;
                    console.log('New service worker installed and update available');
                    this.showUpdateNotification(
                        'A new version is available. Click to refresh and get the latest features.',
                        'update'
                    );
                } else {
                    // First time install
                    console.log('Service Worker installed for the first time');
                    this.showUpdateNotification(
                        'Application is now ready for offline use!',
                        'success'
                    );
                }
            } else if (installingWorker.state === 'activated') {
                console.log('New service worker activated');
            }
        });
    }

    handleServiceWorkerMessage(data) {
        if (data && data.type === 'SW_UPDATED') {
            console.log('Service Worker updated to version:', data.version);
            
            if (this.currentVersion && this.currentVersion !== data.version) {
                this.showUpdateNotification(
                    `Application updated to version ${data.version}. Cached content has been refreshed.`,
                    'success'
                );
            }
            
            this.currentVersion = data.version;
        }
    }

    showUpdateNotification(message, type = 'info') {
        // Create notification element
        const notification = document.createElement('div');
        notification.className = `sw-notification sw-notification-${type}`;
        notification.innerHTML = `
            <div class="sw-notification-content">
                <span class="sw-notification-message">${message}</span>
                ${type === 'update' ? '<button class="sw-refresh-btn" onclick="window.serviceWorkerManager.forceUpdate()">Refresh Now</button>' : ''}
                <button class="sw-close-btn" onclick="this.parentElement.parentElement.remove()">×</button>
            </div>
        `;

        // Add styles if not already added
        if (!document.getElementById('sw-notification-styles')) {
            const styles = document.createElement('style');
            styles.id = 'sw-notification-styles';
            styles.textContent = `
                .sw-notification {
                    position: fixed;
                    top: 20px;
                    right: 20px;
                    max-width: 400px;
                    padding: 16px;
                    border-radius: 8px;
                    box-shadow: 0 4px 12px rgba(0,0,0,0.15);
                    z-index: 10000;
                    font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
                    font-size: 14px;
                    color: white;
                    animation: slideInRight 0.3s ease-out;
                }

                .sw-notification-info { background-color: #2196F3; }
                .sw-notification-success { background-color: #4CAF50; }
                .sw-notification-update { background-color: #FF9800; }
                .sw-notification-error { background-color: #f44336; }

                .sw-notification-content {
                    display: flex;
                    align-items: center;
                    gap: 12px;
                }

                .sw-notification-message {
                    flex: 1;
                    line-height: 1.4;
                }

                .sw-refresh-btn, .sw-close-btn {
                    background: rgba(255,255,255,0.2);
                    border: 1px solid rgba(255,255,255,0.3);
                    color: white;
                    padding: 6px 12px;
                    border-radius: 4px;
                    cursor: pointer;
                    font-size: 12px;
                    transition: background-color 0.2s;
                }

                .sw-refresh-btn:hover, .sw-close-btn:hover {
                    background: rgba(255,255,255,0.3);
                }

                .sw-close-btn {
                    padding: 4px 8px;
                    font-weight: bold;
                }

                @keyframes slideInRight {
                    from { transform: translateX(100%); opacity: 0; }
                    to { transform: translateX(0); opacity: 1; }
                }
                
                @media (max-width: 480px) {
                    .sw-notification {
                        right: 10px;
                        left: 10px;
                        max-width: none;
                    }
                    
                    .sw-notification-content {
                        flex-direction: column;
                        align-items: stretch;
                        gap: 8px;
                    }
                    
                    .sw-refresh-btn {
                        align-self: center;
                    }
                }
            `;
            document.head.appendChild(styles);
        }

        document.body.appendChild(notification);

        // Auto-remove after 15 seconds (except update notifications)
        if (type !== 'update') {
            setTimeout(() => {
                if (notification.parentElement) {
                    notification.remove();
                }
            }, 15000);
        }
    }

    // Force update method for manual refresh
    async forceUpdate() {
        try {
            console.log('Force updating service worker...');
            
            if (this.updateAvailable && this.registration && this.registration.waiting) {
                // Tell the waiting service worker to skip waiting
                this.registration.waiting.postMessage({ type: 'SKIP_WAITING' });
                
                // Wait a moment then reload
                setTimeout(() => {
                    window.location.reload();
                }, 500);
            } else {
                // No waiting worker, just check for updates and reload
                await this.checkForUpdates();
                
                setTimeout(() => {
                    window.location.reload();
                }, 1000);
            }
        } catch (error) {
            console.error('Error during force update:', error);
            // Fallback: just reload
            window.location.reload();
        }
    }

    // Clear all caches manually
    async clearAllCaches() {
        try {
            console.log('Clearing all caches...');
            
            const cacheNames = await caches.keys();
            await Promise.all(
                cacheNames.map(cacheName => {
                    console.log('Deleting cache:', cacheName);
                    return caches.delete(cacheName);
                })
            );
            
            console.log('All caches cleared successfully');
            this.showUpdateNotification('All caches cleared successfully!', 'success');
            
            // Reload after clearing caches
            setTimeout(() => {
                window.location.reload();
            }, 2000);
            
        } catch (error) {
            console.error('Error clearing caches:', error);
            this.showUpdateNotification('Failed to clear caches: ' + error.message, 'error');
        }
    }
}

// Initialize service worker manager
const swManager = new ServiceWorkerManager();

// Auto-initialize when DOM is loaded
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => {
        console.log('DOM loaded, initializing Service Worker Manager...');
        swManager.initialize();
    });
} else {
    console.log('DOM already loaded, initializing Service Worker Manager...');
    swManager.initialize();
}

// Expose globally for manual control
window.serviceWorkerManager = swManager;

// Add keyboard shortcut for manual cache clearing (Ctrl+Shift+R for developers)
document.addEventListener('keydown', (event) => {
    if (event.ctrlKey && event.shiftKey && event.key === 'R') {
        event.preventDefault();
        console.log('Manual cache clear triggered by keyboard shortcut');
        swManager.clearAllCaches();
    }
});

console.log('Service Worker Manager script loaded successfully');