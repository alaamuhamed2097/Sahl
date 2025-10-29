window.initializePerfectScrollbar = (elementId) => {
    const element = document.getElementById(elementId);
    if (element) {
        // Destroy existing instance if any
        if (element.perfectScrollbarInstance) {
            element.perfectScrollbarInstance.destroy();
        }

        // Create new instance
        element.perfectScrollbarInstance = new PerfectScrollbar(element, {
            wheelSpeed: 0.5,
            wheelPropagation: false,
            minScrollbarLength: 20,
            suppressScrollX: true // Only allow vertical scrolling
        });

        console.log('Perfect Scrollbar initialized for:', elementId);
    } else {
        console.warn('Element not found:', elementId);
    }
};

window.updatePerfectScrollbar = (elementId) => {
    const element = document.getElementById(elementId);
    if (element && element.perfectScrollbarInstance) {
        element.perfectScrollbarInstance.update();
        console.log('Perfect Scrollbar updated for:', elementId);
    }
};

window.destroyPerfectScrollbar = (elementId) => {
    const element = document.getElementById(elementId);
    if (element && element.perfectScrollbarInstance) {
        element.perfectScrollbarInstance.destroy();
        delete element.perfectScrollbarInstance;
        console.log('Perfect Scrollbar destroyed for:', elementId);
    }
};
// Global object to store scroll listeners
window.scrollListeners = window.scrollListeners || {};

window.initializeScrollListener = (containerId, dotNetRef) => {
    const container = document.getElementById(containerId);
    if (!container) {
        console.error(`Container with ID ${containerId} not found`);
        return;
    }

    // Remove existing listener if any
    if (window.scrollListeners[containerId]) {
        container.removeEventListener('scroll', window.scrollListeners[containerId]);
    }

    // Create new scroll handler
    const scrollHandler = debounce(async () => {
        const scrollTop = container.scrollTop;
        const scrollHeight = container.scrollHeight;
        const clientHeight = container.clientHeight;

        // Check if user has scrolled near the bottom (within 100px)
        const threshold = 100;
        const isNearBottom = scrollTop + clientHeight >= scrollHeight - threshold;

        if (isNearBottom) {
            try {
                await dotNetRef.invokeMethodAsync('OnScrollNearEnd');
            } catch (error) {
                console.error('Error invoking OnScrollNearEnd:', error);
            }
        }
    }, 200); // Debounce for 200ms

    // Store the handler for cleanup
    window.scrollListeners[containerId] = scrollHandler;

    // Add event listener
    container.addEventListener('scroll', scrollHandler, { passive: true });

    console.log(`Scroll listener initialized for ${containerId}`);
};

window.removeScrollListener = (containerId) => {
    const container = document.getElementById(containerId);
    if (container && window.scrollListeners[containerId]) {
        container.removeEventListener('scroll', window.scrollListeners[containerId]);
        delete window.scrollListeners[containerId];
        console.log(`Scroll listener removed for ${containerId}`);
    }
};

// Debounce function to prevent excessive API calls
function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

// Clean up function for when the page unloads
window.addEventListener('beforeunload', () => {
    // Clean up all scroll listeners
    Object.keys(window.scrollListeners).forEach(containerId => {
        window.removeScrollListener(containerId);
    });
});