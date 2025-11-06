/**
 * Blazor WebAssembly Initialization and Core Dependencies
 * Handles Blazor startup, jQuery, localization, and critical features
 */

(function() {
    'use strict';

    // Performance monitoring for LCP optimization
    window.performanceMetrics = {
      startTime: performance.now(),
    lcpCandidate: null,

        // Track LCP candidates
        observeLCP: function () {
       if ('PerformanceObserver' in window) {
            const observer = new PerformanceObserver((list) => {
         const entries = list.getEntries();
   const lastEntry = entries[entries.length - 1];
          this.lcpCandidate = lastEntry;
             });
       observer.observe({ entryTypes: ['largest-contentful-paint'] });
     }
        },

        // Progress tracking for better UX
        updateProgress: function (percentage) {
   const progressBar = document.getElementById('progress-bar');
 if (progressBar) {
        progressBar.style.width = percentage + '%';
            }
   }
    };

    // Start LCP observation immediately
    window.performanceMetrics.observeLCP();

// Simulate loading progress for better UX
    let progress = 0;
    const progressInterval = setInterval(() => {
        progress += Math.random() * 10;
        if (progress > 90) progress = 90;
      window.performanceMetrics.updateProgress(progress);
    }, 100);

    // Clear interval when Blazor starts
    window.addEventListener('blazor-started', () => {
        clearInterval(progressInterval);
  window.performanceMetrics.updateProgress(100);
    });

    // Debug timeout handler to prevent hanging in mono_wasm_fire_debugger_agent_message
    if (window.location.hostname === 'localhost' || window.location.hostname === '127.0.0.1') {
        // Override console.assert to completely disable debugger invocation
        const originalAssert = console.assert;

        console.assert = function (...args) {
            // Simply log the assertion without calling debugger
            // This prevents mono_wasm_fire_debugger_agent_message from pausing execution
            if (args.length > 0 && !args[0]) {
                // Only log if assertion fails
                console.log('[Debug Disabled] Assertion:', ...args.slice(1));
            }
            // DO NOT call originalAssert to avoid debugger; statement
            // originalAssert.apply(console, args);
        };
    }

    // jQuery dependency tracker
    window.jQueryReady = false;

    // Localization dependency tracker
    window.localizationReady = false;

    // Check if jQuery is loaded
    function checkjQueryLoaded() {
      return typeof $ !== 'undefined' && typeof jQuery !== 'undefined';
  }

    // Check if localization is loaded
    function checkLocalizationLoaded() {
     return typeof localization !== 'undefined' && typeof localization.getCurrentLanguage === 'function';
    }

    // Wait for jQuery to be available
    function waitForjQuery(callback) {
        if (checkjQueryLoaded()) {
    window.jQueryReady = true;
            callback();
        } else {
    setTimeout(() => waitForjQuery(callback), 50);
  }
    }

    // Wait for localization to be available
    function waitForLocalization(callback) {
        if (checkLocalizationLoaded()) {
 window.localizationReady = true;
            callback();
} else {
    setTimeout(() => waitForLocalization(callback), 50);
        }
    }

    // Wait for all dependencies
    function waitForDependencies(callback) {
        const checkAndProceed = () => {
  if (checkjQueryLoaded() && checkLocalizationLoaded()) {
        window.jQueryReady = true;
   window.localizationReady = true;
      callback();
            } else {
 setTimeout(checkAndProceed, 50);
            }
        };
        checkAndProceed();
    }

    // Export functions for global access
    window.checkjQueryLoaded = checkjQueryLoaded;
    window.checkLocalizationLoaded = checkLocalizationLoaded;
 window.waitForjQuery = waitForjQuery;
    window.waitForLocalization = waitForLocalization;
    window.waitForDependencies = waitForDependencies;

    // Blazor startup configuration
 window.startBlazor = function() {
      Blazor.start({
    loadBootResource: function (type, name, defaultUri, integrity) {
  // Optimize resource loading
      if (type === 'dotnetjs') {
     return defaultUri + '?v=' + Date.now();
     }
       return defaultUri;
            },
            // Add debug configuration
       circuit: {
                reconnectionOptions: {
      maxRetries: 3,
 retryIntervalMilliseconds: 2000
      }
            }
        }).then(() => {
// Blazor has started - hide loading screen
          document.body.classList.add('loaded');
      document.body.classList.remove('loading');

       // Dispatch custom event for other scripts
         window.dispatchEvent(new CustomEvent('blazor-started'));

            // Wait for all dependencies before initializing critical features
        waitForDependencies(() => {
       if (typeof initializeCriticalFeatures === 'function') {
          initializeCriticalFeatures();
                }
      });
      });
    };

})();