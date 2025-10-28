// Navigation Fix for Blazor WebAssembly
// This script helps fix persistent hover/active states in navigation menu

(function() {
    'use strict';

    // Function to clear all navigation states
    function clearNavigationStates() {
        try {
            // Remove focus from all nav elements
            const navLinks = document.querySelectorAll('.pcoded-navbar .nav-link');
            navLinks.forEach(link => {
                if (link.blur) {
                    link.blur();
                }
            });

            // Remove any stuck classes
            const navItems = document.querySelectorAll('.pcoded-navbar .nav-item');
            navItems.forEach(item => {
                // Only remove active states from non-active items
                if (!item.classList.contains('active')) {
                    item.classList.remove('pcoded-trigger', 'active');
                }
            });

            // Clear any temporary hover states
            const hoverElements = document.querySelectorAll('.pcoded-navbar [style*="background"]');
            hoverElements.forEach(element => {
                if (!element.closest('.nav-item.active')) {
                    element.style.background = '';
                    element.style.backgroundColor = '';
                }
            });
        } catch (error) {
            console.warn('Error clearing navigation states:', error);
        }
    }

    // Enhanced navigation state management
    window.enhancedNavigationFix = {
        clearStates: clearNavigationStates,
        
        // Initialize the fix
        init: function() {
            // Clear states on page load
            document.addEventListener('DOMContentLoaded', clearNavigationStates);
            
            // Clear states when Blazor finishes rendering
            if (window.Blazor) {
                window.Blazor.addEventListener('enhancedload', clearNavigationStates);
            }
            
            // Handle navigation away from nav items
            document.addEventListener('click', function(e) {
                // If clicking outside the nav area, clear temporary states
                if (!e.target.closest('.pcoded-navbar')) {
                    setTimeout(clearNavigationStates, 100);
                }
            });
            
            // Handle keyboard navigation
            document.addEventListener('keydown', function(e) {
                if (e.key === 'Escape') {
                    clearNavigationStates();
                }
            });
        }
    };

    // Auto-initialize when script loads
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', window.enhancedNavigationFix.init);
    } else {
        window.enhancedNavigationFix.init();
    }
})();