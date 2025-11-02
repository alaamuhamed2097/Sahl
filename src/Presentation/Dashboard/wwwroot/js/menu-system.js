/**
 * Menu System and Navigation for Sahl Dashboard
 * Handles mobile menu, responsive navigation, and active states
 */

(function() {
    'use strict';

    // Enhanced menu toggle for Blazor WebAssembly
 function initializeCriticalFeatures() {
        // Ensure all dependencies are loaded before proceeding
 if (!window.checkjQueryLoaded()) {
            console.warn('jQuery not loaded yet, waiting...');
  window.waitForjQuery(initializeCriticalFeatures);
      return;
}

      if (!window.checkLocalizationLoaded()) {
 console.warn('Localization not loaded yet, waiting...');
      window.waitForLocalization(initializeCriticalFeatures);
     return;
        }

    // Enhanced menu toggle for Blazor WebAssembly
      $(document).off('click.blazor-enhanced').on('click.blazor-enhanced', '#mobile-collapse, #mobile-collapse1, .mobile-menu', function (e) {
   e.preventDefault();
   e.stopPropagation();

            var vw = window.innerWidth || document.documentElement.clientWidth;

       if (vw < 992) {
    var $navbar = $(".pcoded-navbar");
    var isOpen = $navbar.hasClass('mob-open');

      if (isOpen) {
           $navbar.removeClass('mob-open');
 $(this).removeClass('on');
     $(".mobile-menu").removeClass('on');
      $("#mobile-collapse, #mobile-collapse1").removeClass('on');
 } else {
        $navbar.addClass('mob-open');
     $(this).addClass('on');
         $(".mobile-menu").addClass('on');
     $("#mobile-collapse, #mobile-collapse1").addClass('on');
   }
         }
     });

   // Initialize responsive navbar
      var currentVw = window.innerWidth || document.documentElement.clientWidth;
  var $navbar = $(".pcoded-navbar");

     if (currentVw >= 992 && currentVw <= 1200) {
     $navbar.addClass('navbar-collapsed');
        } else if (currentVw >= 1200) {
 $navbar.removeClass('navbar-collapsed');
      }
    }

    // Initialize navigation helpers - with dependency checks
    window.initializePcodedMenu = function () {
 try {
            // Wait for all dependencies if not loaded
   if (!window.checkjQueryLoaded() || !window.checkLocalizationLoaded()) {
          console.warn('Dependencies not available for pcoded menu initialization, waiting...');
      window.waitForDependencies(() => {
    window.initializePcodedMenu();
     });
    return;
    }

   if (typeof $.fn.pcodedmenu === 'function' && $("#pcoded").length > 0) {
    $("#pcoded").pcodedmenu({
   MenuTrigger: 'click',
      SubMenuTrigger: 'click',
         });
         }
        } catch (error) {
  console.error('Error initializing pcoded menu:', error);
    }
    };

    window.clearAllActiveStates = function () {
      if (!window.checkjQueryLoaded()) {
     console.warn('jQuery not available for clearing active states');
  return;
        }
        try {
 $(".pcoded-navbar .pcoded-inner-navbar li").removeClass("active");
            $(".pcoded-navbar .pcoded-inner-navbar .pcoded-hasmenu").removeClass("active pcoded-trigger");
$(".pcoded-navbar .nav-link").blur();
        } catch (error) {
    console.error('Error clearing active states:', error);
    }
    };

    window.setActiveMenuItems = function () {
        if (!window.checkjQueryLoaded()) {
     console.warn('jQuery not available for setting active menu items');
     return;
        }
 try {
      var pageUrl = window.location.href.split(/[?#]/)[0];
         $(".pcoded-navbar .pcoded-inner-navbar a").each(function () {
            if (this.href == pageUrl && $(this).attr('href') != "") {
       $(this).parent('li').addClass("active");
   if (!$('.pcoded-navbar').hasClass('theme-horizontal')) {
           $(this).parent('li').parent().parent('.pcoded-hasmenu').addClass("active").addClass("pcoded-trigger");
  }
      }
          });
      } catch (error) {
 console.error('Error setting active menu items:', error);
        }
    };

    // Initialize Perfect Scrollbar when needed
    window.initializePerfectScrollbar = function (selector) {
     if (typeof PerfectScrollbar === 'undefined') return;
try {
          var element = document.querySelector('.' + selector);
 if (element) {
                new PerfectScrollbar('.' + selector, {
   wheelSpeed: 0.5,
    swipeEasing: 0,
     suppressScrollX: true,
   wheelPropagation: 1,
         minScrollbarLength: 40,
                });
 }
        } catch (error) {
      console.error('Error initializing perfect scrollbar:', error);
        }
  };

    // Navigation scrollbar initialization
    window.initializeNavScrollbar = function () {
        if ('requestIdleCallback' in window) {
    requestIdleCallback(() => {
    if (document.querySelector('.navbar-content') && typeof PerfectScrollbar !== 'undefined') {
      if (window.navScrollbar) {
              window.navScrollbar.destroy();
      }
      window.navScrollbar = new PerfectScrollbar('.navbar-content', {
         wheelSpeed: 0.5,
       swipeEasing: 0,
           suppressScrollX: true,
        });
  }
       });
        }
    };

    // Initialize navbar handler
 window.navbarHandler = {
     init: function () {
   var navbarCollapse = document.getElementById("navbarSupportedContent");
            if (!navbarCollapse) return;

 var navList = navbarCollapse.querySelector("ul.navbar-nav");
     if (!window.matchMedia("(max-width: 991.98px)").matches) {
            navList.style.display = "";
     return;
            }

      var isShown = navbarCollapse.classList.contains("show");
        if (navList) {
      navList.style.display = isShown ? "" : "none";
            }
 }
    };

    // Export the main initialization function
 window.initializeCriticalFeatures = initializeCriticalFeatures;

})();