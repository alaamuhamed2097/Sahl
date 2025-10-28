// Lazy loading utilities for better performance
window.lazyLoadUtils = {
    observers: new Map(),
    
    // Intersection Observer for lazy loading components
    observeLazyComponent: function(element, dotNetRef) {
        if (!element || !window.IntersectionObserver) {
            // Fallback for browsers without IntersectionObserver
            setTimeout(() => {
                dotNetRef.invokeMethodAsync('OnVisible');
            }, 1000);
            return;
        }

        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    dotNetRef.invokeMethodAsync('OnVisible');
                    observer.unobserve(entry.target);
                    this.observers.delete(element);
                }
            });
        }, {
            rootMargin: '50px 0px', // Load 50px before element comes into view
            threshold: 0.1
        });

        observer.observe(element);
        this.observers.set(element, observer);
    },

    // Preload critical resources
    preloadCriticalResources: function() {
        const criticalResources = [
            '/css/marketer-statistics-widget.css',
            '/js/marketer-statistics-charts.min.js'
        ];

        criticalResources.forEach(resource => {
            this.preloadResource(resource);
        });
    },

    // Preload statistics resources
    preloadStatisticsResources: function() {
        const resources = [
            '/js/statistics-charts.min.js'
        ];

        resources.forEach(resource => {
            this.preloadResource(resource);
        });
    },

    // Preload admin resources
    //preloadAdminResources: function() {
    //    const resources = [
    //        //'/js/admin-dashboard.min.js'
    //    ];

    //    resources.forEach(resource => {
    //        this.preloadResource(resource);
    //    });
    //},

    // Generic resource preloader
    preloadResource: function(href) {
        if (!href) return;

        // Check if already preloaded
        if (document.querySelector(`link[href="${href}"], script[src="${href}"]`)) {
            return;
        }

        const isCSS = href.endsWith('.css');
        const element = isCSS ? document.createElement('link') : document.createElement('script');

        if (isCSS) {
            element.rel = 'preload';
            element.as = 'style';
            element.href = href;
            element.onload = function() {
                this.rel = 'stylesheet';
            };
        } else {
            element.src = href;
            element.async = true;
        }

        document.head.appendChild(element);
    },

    // Check auto-load preference
    getAutoLoadPreference: function() {
        try {
            const preference = localStorage.getItem('autoLoadDashboard');
            return preference !== 'false'; // Default to true
        } catch {
            return true;
        }
    },

    // Set auto-load preference
    setAutoLoadPreference: function(value) {
        try {
            localStorage.setItem('autoLoadDashboard', value.toString());
        } catch {
            // Ignore localStorage errors
        }
    },

    // Performance monitoring
    measurePerformance: function(label) {
        if (window.performance && window.performance.mark) {
            window.performance.mark(label);
        }
    },

    // Get performance metrics
    getPerformanceMetrics: function() {
        if (!window.performance) return null;

        const navigation = performance.getEntriesByType('navigation')[0];
        const paintEntries = performance.getEntriesByType('paint');

        return {
            domContentLoaded: navigation ? navigation.domContentLoadedEventEnd - navigation.domContentLoadedEventStart : 0,
            firstPaint: paintEntries.find(entry => entry.name === 'first-paint')?.startTime || 0,
            firstContentfulPaint: paintEntries.find(entry => entry.name === 'first-contentful-paint')?.startTime || 0
        };
    },

    // Cleanup observers
    cleanup: function() {
        this.observers.forEach(observer => {
            observer.disconnect();
        });
        this.observers.clear();
    }
};

// Global functions for backward compatibility
window.observeLazyComponent = window.lazyLoadUtils.observeLazyComponent.bind(window.lazyLoadUtils);
window.preloadCriticalResources = window.lazyLoadUtils.preloadCriticalResources.bind(window.lazyLoadUtils);
window.preloadStatisticsResources = window.lazyLoadUtils.preloadStatisticsResources.bind(window.lazyLoadUtils);
//window.preloadAdminResources = window.lazyLoadUtils.preloadAdminResources.bind(window.lazyLoadUtils);
window.getAutoLoadPreference = window.lazyLoadUtils.getAutoLoadPreference.bind(window.lazyLoadUtils);
window.setAutoLoadPreference = window.lazyLoadUtils.setAutoLoadPreference.bind(window.lazyLoadUtils);

// Enhanced chart loading with better error handling
window.isHighchartsAvailable = function() {
    return typeof window.Highcharts !== 'undefined';
};

window.loadHighchartsIfNeeded = async function() {
    if (window.isHighchartsAvailable()) {
        return Promise.resolve();
    }

    return new Promise((resolve, reject) => {
        const script = document.createElement('script');
        script.src = 'https://code.highcharts.com/highcharts.js';
        script.async = true;
        
        script.onload = () => {
            console.log('Highcharts loaded successfully');
            resolve();
        };
        
        script.onerror = () => {
            console.error('Failed to load Highcharts');
            reject(new Error('Failed to load Highcharts'));
        };
        
        // Add timeout
        setTimeout(() => {
            if (!window.isHighchartsAvailable()) {
                reject(new Error('Highcharts loading timeout'));
            }
        }, 10000);
        
        document.head.appendChild(script);
    });
};

// Cleanup on page unload
window.addEventListener('beforeunload', () => {
    window.lazyLoadUtils.cleanup();
});

// Initialize performance monitoring
document.addEventListener('DOMContentLoaded', () => {
    window.lazyLoadUtils.measurePerformance('DOMContentLoaded');
});

console.log('Lazy loading utilities initialized');