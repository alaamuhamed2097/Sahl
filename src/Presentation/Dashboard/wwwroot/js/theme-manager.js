/**
 * Theme Manager and Component Script Loader for Sahl Dashboard
 * Handles dynamic CSS loading and component-specific scripts
 */

(function() {
    'use strict';

    // Enhanced CSS Theme Loader for Blazor WebAssembly
    window.ThemeManager = {
        loadTheme: function (themeName) {
      const themeFiles = {
    'layout-3': 'assets/css/layouts/layout-3.min.css',
        'layout-4': 'assets/css/layouts/layout-4.min.css',
       'layout-5': 'assets/css/layouts/layout-5.min.css',
          'layout-9': 'assets/css/layouts/layout-9.min.css',
         'layout-12': 'assets/css/layouts/layout-12.min.css',
 'dark': 'assets/css/layouts/dark.min.css',
       'rtl': 'assets/css/layouts/rtl.min.css',
     'landing': 'assets/css/layouts/landing.min.css'
            };

            if (themeFiles[themeName]) {
     this.loadCSS(themeFiles[themeName], 'theme-' + themeName);
       }
        },

        loadPageStyle: function (pageName) {
    const pageFiles = {
    'pages': 'assets/css/pages/pages.min.css',
         'gallery': 'assets/css/pages/gallery.min.css',
                'team-overview': 'css/team-overview.min.css'
   };

         if (pageFiles[pageName]) {
  this.loadCSS(pageFiles[pageName], 'page-' + pageName);
         }
        },

     loadCSS: function (href, id) {
            if (id && document.getElementById(id)) {
                document.getElementById(id).remove();
            }

 const link = document.createElement('link');
     link.rel = 'stylesheet';
    link.href = href;
          if (id) link.id = id;

    // Use requestIdleCallback for non-critical CSS
            if ('requestIdleCallback' in window) {
     requestIdleCallback(() => {
        document.head.appendChild(link);
 });
        } else {
      setTimeout(() => {
            document.head.appendChild(link);
     }, 0);
      }
   },

        unloadTheme: function (themeName) {
          const themeElement = document.getElementById('theme-' + themeName);
            if (themeElement) {
        themeElement.remove();
       }
        }
    };

    // Enhanced Script Loader for Blazor Components
    window.ComponentScriptLoader = {
        loadedScripts: new Set(),

        loadComponentScript: function (scriptName) {
  if (this.loadedScripts.has(scriptName)) {
        return Promise.resolve();
            }

            const scriptFiles = {
          'charts': 'js/statistics-charts.min.js',
  'Vendor-charts': 'js/Vendor-statistics-charts.min.js',
         'text-editor': 'js/text-editor.min.js',
       'rich-text-editor': 'assets/js/rich-text-editor-helper.js',
         'data-tables': 'assets/js/pages/data-basic-custom.min.js',
           'data-source': 'assets/js/pages/data-source-custom.min.js',
     'data-plugin': 'assets/js/pages/data-plugin-custom.min.js',
    'statistics': 'assets/js/pages/statistic.min.js',
      'notifications': 'assets/js/pages/ac-notification.min.js',
      'widget-data': 'assets/js/pages/widget-data.min.js',
                'team-overview': 'scripts/team-overview.min.js',
         'grid-animation': 'assets/js/pages/grid-animation/main.min.js',
   'daterangepicker': 'assets/js/plugins/daterangepicker.min.js',
       'trumbowyg': 'assets/js/plugins/trumbowyg/trumbowyg.min.js',
          'error-page': 'assets/js/pages/error.min.js'
   };

     if (!scriptFiles[scriptName]) {
             return Promise.reject(new Error('Script not found: ' + scriptName));
         }

          return this.loadScript(scriptFiles[scriptName])
       .then(() => {
        this.loadedScripts.add(scriptName);
             console.log('Loaded component script:', scriptName);
          });
        },

   loadScript: function (src) {
  return new Promise((resolve, reject) => {
      if (document.querySelector(`script[src="${src}"]`)) {
         resolve();
               return;
  }

            const script = document.createElement('script');
                script.src = src;
       script.async = true;
        script.onload = resolve;
         script.onerror = reject;
                document.head.appendChild(script);
    });
        }
    };

})();