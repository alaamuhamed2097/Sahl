// Performance Configuration for LCP Optimization
window.PerformanceConfig = {
    // Resource loading priorities
    CRITICAL_RESOURCES: [
        '/assets/images/logo-icon.svg',
        'css/app.min.css',
        'css/loader.min.css',
        '_framework/blazor.webassembly.js'
    ],
    
    // Lazy loading thresholds
    LAZY_LOAD_THRESHOLD: 100, // pixels
    
    // Performance budgets (in milliseconds)
    PERFORMANCE_BUDGETS: {
        LCP: 2500,  // Target LCP under 2.5 seconds
        FID: 100,   // Target FID under 100ms
        CLS: 0.1    // Target CLS under 0.1
    },
    
    // Resource hints configuration
    RESOURCE_HINTS: {
        DNS_PREFETCH: [
            '//fonts.googleapis.com',
            '//cdnjs.cloudflare.com'
        ],
        PRECONNECT: [
            'https://fonts.googleapis.com'
        ]
    },
    
    // Critical CSS extraction patterns
    CRITICAL_CSS_PATTERNS: [
        '.loading-container',
        '.loading-logo-img',
        '.loading-text',
        '.loading-subtitle',
        '.progress-container',
        '.progress-bar'
    ],
    
    // Monitoring configuration
    MONITORING: {
        ENABLE_LCP_TRACKING: true,
        ENABLE_RESOURCE_TIMING: true,
        REPORT_ENDPOINT: '/api/performance-metrics'
    }
};

// Performance monitoring utilities
window.PerformanceMonitor = {
    metrics: {},
    
    // Track Core Web Vitals
    trackWebVitals: function() {
        // LCP tracking
        if ('PerformanceObserver' in window) {
            const lcpObserver = new PerformanceObserver((list) => {
                const entries = list.getEntries();
                const lastEntry = entries[entries.length - 1];
                this.metrics.lcp = lastEntry.startTime;
                
                if (lastEntry.startTime > window.PerformanceConfig.PERFORMANCE_BUDGETS.LCP) {
                    console.warn(`LCP exceeded budget: ${lastEntry.startTime}ms > ${window.PerformanceConfig.PERFORMANCE_BUDGETS.LCP}ms`);
                }
            });
            lcpObserver.observe({entryTypes: ['largest-contentful-paint']});
            
            // FID tracking
            const fidObserver = new PerformanceObserver((list) => {
                const entries = list.getEntries();
                entries.forEach((entry) => {
                    this.metrics.fid = entry.processingStart - entry.startTime;
                });
            });
            fidObserver.observe({entryTypes: ['first-input']});
            
            // CLS tracking
            let clsValue = 0;
            const clsObserver = new PerformanceObserver((list) => {
                for (const entry of list.getEntries()) {
                    if (!entry.hadRecentInput) {
                        clsValue += entry.value;
                        this.metrics.cls = clsValue;
                    }
                }
            });
            clsObserver.observe({entryTypes: ['layout-shift']});
        }
    },
    
    // Resource timing analysis
    analyzeResourceTiming: function() {
        if ('performance' in window && 'getEntriesByType' in performance) {
            const resources = performance.getEntriesByType('resource');
            const slowResources = resources.filter(resource => 
                resource.duration > 1000 && // Resources taking more than 1 second
                window.PerformanceConfig.CRITICAL_RESOURCES.some(critical => 
                    resource.name.includes(critical)
                )
            );
            
            if (slowResources.length > 0) {
                console.warn('Slow critical resources detected:', slowResources);
            }
            
            return {
                totalResources: resources.length,
                slowResources: slowResources.length,
                criticalResourceTiming: resources.filter(r => 
                    window.PerformanceConfig.CRITICAL_RESOURCES.some(critical => 
                        r.name.includes(critical)
                    )
                )
            };
        }
    },
    
    // Generate performance report
    generateReport: function() {
        const report = {
            timestamp: Date.now(),
            url: window.location.href,
            userAgent: navigator.userAgent,
            metrics: this.metrics,
            resourceTiming: this.analyzeResourceTiming(),
            budgetViolations: []
        };
        
        // Check budget violations
        if (this.metrics.lcp > window.PerformanceConfig.PERFORMANCE_BUDGETS.LCP) {
            report.budgetViolations.push({
                metric: 'LCP',
                actual: this.metrics.lcp,
                budget: window.PerformanceConfig.PERFORMANCE_BUDGETS.LCP
            });
        }
        
        if (this.metrics.fid > window.PerformanceConfig.PERFORMANCE_BUDGETS.FID) {
            report.budgetViolations.push({
                metric: 'FID',
                actual: this.metrics.fid,
                budget: window.PerformanceConfig.PERFORMANCE_BUDGETS.FID
            });
        }
        
        if (this.metrics.cls > window.PerformanceConfig.PERFORMANCE_BUDGETS.CLS) {
            report.budgetViolations.push({
                metric: 'CLS',
                actual: this.metrics.cls,
                budget: window.PerformanceConfig.PERFORMANCE_BUDGETS.CLS
            });
        }
        
        return report;
    },
    
    // Send metrics to backend (if configured)
    sendMetrics: function() {
        if (window.PerformanceConfig.MONITORING.REPORT_ENDPOINT) {
            const report = this.generateReport();
            
            if ('sendBeacon' in navigator) {
                navigator.sendBeacon(
                    window.PerformanceConfig.MONITORING.REPORT_ENDPOINT,
                    JSON.stringify(report)
                );
            } else {
                // Fallback for older browsers
                fetch(window.PerformanceConfig.MONITORING.REPORT_ENDPOINT, {
                    method: 'POST',
                    body: JSON.stringify(report),
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    keepalive: true
                }).catch(console.error);
            }
        }
    }
};

// Initialize performance monitoring
if (window.PerformanceConfig.MONITORING.ENABLE_LCP_TRACKING) {
    window.PerformanceMonitor.trackWebVitals();
}

// Send metrics when page is about to unload
window.addEventListener('beforeunload', function() {
    window.PerformanceMonitor.sendMetrics();
});

// Send metrics after page load
window.addEventListener('load', function() {
    setTimeout(() => {
        window.PerformanceMonitor.sendMetrics();
    }, 5000); // Wait 5 seconds after load to capture final metrics
});