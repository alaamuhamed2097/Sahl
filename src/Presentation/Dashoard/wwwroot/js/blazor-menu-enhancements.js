/**
 * Enhanced menu toggle functionality for Blazor WebAssembly
 * Handles both mobile and desktop menu states with proper transitions
 */

(function() {
    'use strict';

    // Configuration
    const CONFIG = {
        MOBILE_BREAKPOINT: 992,
        DESKTOP_COLLAPSED_MAX: 1200,
        NAVBAR_EXPANDED_WIDTH: 270,
        NAVBAR_COLLAPSED_WIDTH: 60,
        INIT_DELAY: 1000,
        RETRY_DELAY: 3000,
        LAYOUT_UPDATE_DELAY: 50
    };

    // State management
    let isInitialized = false;
    let currentViewport = 'unknown';

    /**
     * Get current viewport width
     */
    function getViewportWidth() {
        return window.innerWidth || document.documentElement.clientWidth;
    }

    /**
     * Determine viewport type based on width
     */
    function getViewportType(width) {
        if (width < CONFIG.MOBILE_BREAKPOINT) return 'mobile';
        if (width <= CONFIG.DESKTOP_COLLAPSED_MAX) return 'desktop-medium';
        return 'desktop-large';
    }

    /**
     * Update layout based on navbar state
     */
    function updateLayoutForNavbarState() {
        const vw = getViewportWidth();
        const $navbar = $('.pcoded-navbar');
        const $mainContainer = $('.pcoded-main-container');
        const $header = $('.pcoded-header');

        if (vw >= CONFIG.MOBILE_BREAKPOINT) {
            // Desktop behavior
            const isCollapsed = $navbar.hasClass('navbar-collapsed');
            const leftOffset = isCollapsed ? CONFIG.NAVBAR_COLLAPSED_WIDTH : CONFIG.NAVBAR_EXPANDED_WIDTH;
            
            $mainContainer.css('margin-left', leftOffset + 'px');
            $header.css({
                'left': leftOffset + 'px',
                'width': `calc(100% - ${leftOffset}px)`
            });

            console.log(`Desktop layout updated: navbar ${isCollapsed ? 'collapsed' : 'expanded'} (${leftOffset}px)`);
        } else {
            // Mobile behavior - reset to mobile defaults
            $mainContainer.css('margin-left', '0');
            $header.css({
                'left': '0',
                'width': '100%'
            });

            console.log('Mobile layout updated');
        }
    }

    /**
     * Handle mobile menu toggle
     */
    function toggleMobileMenu($navbar, $toggleButton) {
        const isOpen = $navbar.hasClass('mob-open');

        if (isOpen) {
            // Close mobile menu
            $navbar.removeClass('mob-open');
            $toggleButton.removeClass('on');
            $('.mobile-menu').removeClass('on');
            $('#mobile-collapse, #mobile-collapse1').removeClass('on');
            console.log('Mobile menu closed');
        } else {
            // Open mobile menu
            $navbar.addClass('mob-open');
            $toggleButton.addClass('on');
            $('.mobile-menu').addClass('on');
            $('#mobile-collapse, #mobile-collapse1').addClass('on');
            console.log('Mobile menu opened');
        }
    }

    /**
     * Handle desktop menu toggle
     */
    function toggleDesktopMenu($navbar) {
        const isCollapsed = $navbar.hasClass('navbar-collapsed');
        
        if (isCollapsed) {
            $navbar.removeClass('navbar-collapsed');
            console.log('Desktop menu expanded');
        } else {
            $navbar.addClass('navbar-collapsed');
            console.log('Desktop menu collapsed');
        }

        // Update layout after state change
        setTimeout(updateLayoutForNavbarState, CONFIG.LAYOUT_UPDATE_DELAY);

        // Call collapse hover function if available
        if (typeof collapsehover === 'function') {
            collapsehover();
        }
    }

    /**
     * Main menu toggle handler
     */
    function handleMenuToggle(e) {
        e.preventDefault();
        e.stopPropagation();

        const vw = getViewportWidth();
        const $navbar = $('.pcoded-navbar');
        const $toggleButton = $(this);

        console.log('Menu toggle clicked! Viewport width:', vw);
        console.log('Current navbar classes:', $navbar[0]?.className);

        if (vw < CONFIG.MOBILE_BREAKPOINT) {
            toggleMobileMenu($navbar, $toggleButton);
        } else {
            const $desktopNavbar = $navbar.filter(':not(.theme-horizontal)');
            toggleDesktopMenu($desktopNavbar);
        }
    }

    /**
     * Handle clicks outside mobile menu
     */
    function handleOutsideClick(e) {
        const vw = getViewportWidth();

        if (vw < CONFIG.MOBILE_BREAKPOINT) {
            const $target = $(e.target);
            const isMenuClick = $target.closest('.pcoded-navbar, #mobile-collapse, #mobile-collapse1, .mobile-menu').length > 0;

            if (!isMenuClick && $('.pcoded-navbar').hasClass('mob-open')) {
                $('.pcoded-navbar').removeClass('mob-open');
                $('#mobile-collapse, #mobile-collapse1, .mobile-menu').removeClass('on');
                console.log('Mobile menu closed by outside click');
            }
        }
    }

    /**
     * Handle window resize
     */
    function handleWindowResize() {
        const vw = getViewportWidth();
        const newViewportType = getViewportType(vw);
        const $navbar = $('.pcoded-navbar');

        // Only process if viewport type actually changed
        if (newViewportType !== currentViewport) {
            currentViewport = newViewportType;

            if (vw >= CONFIG.MOBILE_BREAKPOINT) {
                // Desktop view - remove mobile classes and apply desktop logic
                $navbar.removeClass('mob-open');
                $('#mobile-collapse, #mobile-collapse1, .mobile-menu').removeClass('on');
                
                // Apply desktop menu logic based on screen size
                if (newViewportType === 'desktop-medium') {
                    $navbar.addClass('navbar-collapsed');
                } else if (newViewportType === 'desktop-large') {
                    $navbar.removeClass('navbar-collapsed');
                }
            } else {
                // Mobile view - remove desktop classes
                $navbar.removeClass('navbar-collapsed');
            }

            // Update layout for new viewport
            setTimeout(updateLayoutForNavbarState, 100);
            
            console.log('Viewport changed:', currentViewport, 'Width:', vw, 'Navbar classes:', $navbar[0]?.className);
        }
    }

    /**
     * Initialize menu state based on viewport
     */
    function initializeMenuState() {
        const vw = getViewportWidth();
        const viewportType = getViewportType(vw);
        const $navbar = $('.pcoded-navbar');

        currentViewport = viewportType;

        switch (viewportType) {
            case 'desktop-medium':
                // Desktop medium - collapsed by default
                $navbar.addClass('navbar-collapsed');
                break;
            case 'desktop-large':
                // Large desktop - expanded by default
                $navbar.removeClass('navbar-collapsed');
                break;
            case 'mobile':
                // Mobile - ensure no desktop classes
                $navbar.removeClass('navbar-collapsed');
                break;
        }

        // Set initial layout
        setTimeout(updateLayoutForNavbarState, 200);

        console.log('Menu state initialized for viewport:', viewportType, 'Width:', vw);
    }

    /**
     * Bind event handlers
     */
    function bindEventHandlers() {
        // Menu toggle handlers with event delegation
        $(document).off('click.blazor-enhanced').on('click.blazor-enhanced', 
            '#mobile-collapse, #mobile-collapse1, .mobile-menu', 
            handleMenuToggle
        );

        // Outside click handler for mobile
        $(document).off('click.blazor-outside').on('click.blazor-outside', handleOutsideClick);

        // Prevent navbar content clicks from closing menu
        $(document).off('click.blazor-prevent').on('click.blazor-prevent', '.pcoded-navbar', function(e) {
            e.stopPropagation();
        });

        // Window resize handler with debouncing
        let resizeTimeout;
        $(window).off('resize.blazor-enhanced').on('resize.blazor-enhanced', function() {
            clearTimeout(resizeTimeout);
            resizeTimeout = setTimeout(handleWindowResize, 150);
        });

        console.log('Event handlers bound successfully');
    }

    /**
     * Check if required elements exist
     */
    function checkRequiredElements() {
        const foundElements = $('#mobile-collapse, #mobile-collapse1, .mobile-menu').length;
        console.log('Menu elements found:', foundElements);
        
        if (foundElements === 0) {
            console.warn('Mobile menu buttons not found. Check NavMenu component for correct IDs/classes.');
            return false;
        }
        
        return true;
    }

    /**
     * Main initialization function
     */
    function initializeEnhancedMenu() {
        if (isInitialized) {
            console.log('Enhanced menu already initialized');
            return;
        }

        console.log('Initializing enhanced menu system for Blazor...');

        try {
            // Bind event handlers
            bindEventHandlers();

            // Initialize menu state
            initializeMenuState();

            isInitialized = true;
            console.log('Enhanced menu system initialized successfully!');

        } catch (error) {
            console.error('Error initializing enhanced menu:', error);
        }
    }

    /**
     * Retry initialization for late-rendered elements
     */
    function retryInitialization() {
        setTimeout(function() {
            if (!checkRequiredElements()) {
                // Elements still not found, but continue anyway
                console.log('Some menu elements not found, but initialization will continue');
            }
        }, CONFIG.RETRY_DELAY);
    }

    /**
     * Helper function to set active menu items
     */
    function setActiveMenuItems() {
        if (typeof $ === 'undefined') return;

        try {
            $('.pcoded-navbar .pcoded-inner-navbar a').each(function() {
                const pageUrl = window.location.href.split(/[?#]/)[0];
                if (this.href === pageUrl && $(this).attr('href') !== '') {
                    $(this).parent('li').addClass('active');
                    if (!$('.pcoded-navbar').hasClass('theme-horizontal')) {
                        $(this).parent('li').parent().parent('.pcoded-hasmenu')
                            .addClass('active').addClass('pcoded-trigger');
                        $(this).parent('li').parent().parent('.pcoded-hasmenu')
                            .parent().parent('.pcoded-hasmenu')
                            .addClass('active').addClass('pcoded-trigger');
                    }
                }
            });
        } catch (error) {
            console.error('Error setting active menu items:', error);
        }
    }

    /**
     * Public API
     */
    window.BlazorMenuEnhancements = {
        init: initializeEnhancedMenu,
        updateLayout: updateLayoutForNavbarState,
        setActiveMenuItems: setActiveMenuItems,
        getViewportType: function() { return currentViewport; }
    };

    // Auto-initialize when DOM is ready
    $(document).ready(function() {
        // Wait for Blazor to render, then initialize
        setTimeout(initializeEnhancedMenu, CONFIG.INIT_DELAY);
        
        // Retry mechanism for late-rendered elements
        retryInitialization();

        // Initialize active menu items after a delay
        setTimeout(setActiveMenuItems, 1500);
    });

})();