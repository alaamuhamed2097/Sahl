/**
 * Deferred Script Loader and Service Worker Registration
 * Handles non-critical resource loading after Blazor initialization
 */

(function() {
    'use strict';

    // Load non-critical resources after Blazor starts
 function loadDeferredResources() {
        window.addEventListener('blazor-started', function () {
      setTimeout(() => {
    const deferredScripts = [
       'assets/plugins/sweetalert/js/sweetalert.min.js',
       'assets/js/pages/ac-alert.min.js'
         ];

         deferredScripts.forEach((src, index) => {
  setTimeout(() => {
         const script = document.createElement('script');
            script.src = src;
  script.async = true;
  document.head.appendChild(script);
}, index * 50);
   });
     }, 500);
        });
 }

    // Service worker registration - Non-blocking
  function registerServiceWorker() {
     if ('serviceWorker' in navigator) {
      window.addEventListener('load', function () {
   navigator.serviceWorker.register('service-worker.js').catch(function (error) {
             console.log('Service worker registration failed:', error);
    });
        });
   }
    }

    // Initialize all deferred features
    function initializeDeferredFeatures() {
 loadDeferredResources();
        registerServiceWorker();
    }

    // Auto-initialize when DOM is ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initializeDeferredFeatures);
    } else {
   initializeDeferredFeatures();
    }

})();