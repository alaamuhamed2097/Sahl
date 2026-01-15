// =============================================================================
// Basit Dashboard - Final Complete Bundle
// Includes: Perfect Scrollbar + Pcoded Menu + All Helpers
// Version: 4.0 - Production Ready
// =============================================================================

(function() {
    'use strict';

    console.log('🚀 Loading Basit Final Bundle...');

    // =============================================================================
    // 1. PERFECT SCROLLBAR INTEGRATION
    // =============================================================================

    /**
     * Initialize Perfect Scrollbar
     */
    window.initializePerfectScrollbar = function(elementSelector) {
        try {
            console.log('🎯 Initializing Perfect Scrollbar...');

            const selector = elementSelector || '.pcoded-navbar';
            const elements = document.querySelectorAll(selector);

            elements.forEach(element => {
                if (!element || element.classList.contains('ps-initialized')) {
                    return;
                }

                // Check if PerfectScrollbar library exists
                if (typeof PerfectScrollbar !== 'undefined') {
                    try {
                        new PerfectScrollbar(element, {
                            wheelSpeed: 0.5,
                            wheelPropagation: false,
                            minScrollbarLength: 40,
                            suppressScrollX: true,
                            swipeEasing: true
                        });
                        element.classList.add('ps-initialized');
                        console.log('✅ PerfectScrollbar initialized');
                    } catch (err) {
                        console.warn('⚠️ PerfectScrollbar failed, using fallback');
                        applyFallbackScrollbar(element);
                    }
                } else {
                    applyFallbackScrollbar(element);
                }
            });

            return true;
        } catch (error) {
            console.error('❌ Error in initializePerfectScrollbar:', error);
            return false;
        }
    };

    function applyFallbackScrollbar(element) {
        element.style.overflowY = 'auto';
        element.style.overflowX = 'hidden';
        element.classList.add('custom-scrollbar');
    }

    window.destroyPerfectScrollbar = function(elementSelector) {
        try {
            const elements = document.querySelectorAll(elementSelector || '.ps-initialized');
            elements.forEach(element => {
                if (element) {
                    element.classList.remove('ps-initialized', 'custom-scrollbar');
                }
            });
            return true;
        } catch (error) {
            console.error('Error destroying PerfectScrollbar:', error);
            return false;
        }
    };

    // =============================================================================
    // 2. PCODED MENU FUNCTIONS
    // =============================================================================

    /**
     * Initialize Pcoded Menu
     */
    window.initializePcodedMenu = function() {
        try {
            console.log('🎯 Initializing Pcoded Menu...');

            // Initialize submenu toggles
            const menuItems = document.querySelectorAll('.pcoded-hasmenu > a');
            
            menuItems.forEach(item => {
                // Remove old listeners
                const newItem = item.cloneNode(true);
                item.parentNode.replaceChild(newItem, item);
                
                newItem.addEventListener('click', function(e) {
                    e.preventDefault();
                    const parent = this.closest('.pcoded-hasmenu');
                    
                    if (parent) {
                        const isOpen = parent.classList.toggle('pcoded-trigger');
                        const submenu = parent.querySelector('.pcoded-submenu');

                        if (submenu) {
                            if (isOpen) {
                                submenu.style.display = 'block';
                                submenu.style.maxHeight = submenu.scrollHeight + 'px';
                            } else {
                                submenu.style.maxHeight = '0';
                                setTimeout(() => {
                                    submenu.style.display = 'none';
                                }, 300);
                            }
                        }

                        // Close siblings
                        const siblings = Array.from(parent.parentElement.children);
                        siblings.forEach(sibling => {
                            if (sibling !== parent && sibling.classList.contains('pcoded-hasmenu')) {
                                sibling.classList.remove('pcoded-trigger');
                                const siblingSubmenu = sibling.querySelector('.pcoded-submenu');
                                if (siblingSubmenu) {
                                    siblingSubmenu.style.maxHeight = '0';
                                    setTimeout(() => {
                                        siblingSubmenu.style.display = 'none';
                                    }, 300);
                                }
                            }
                        });
                    }
                });
            });

            console.log('✅ Pcoded Menu initialized');
            return true;
        } catch (error) {
            console.error('❌ Error initializing menu:', error);
            return false;
        }
    };

    /**
     * Set Active Menu Items
     */
    window.setActiveMenuItems = function(menuIds) {
        try {
            if (!menuIds || menuIds.length === 0) return false;

            // Remove all active states
            document.querySelectorAll('.pcoded-inner-navbar li').forEach(li => {
                li.classList.remove('active', 'pcoded-trigger');
            });

            // Set active states
            menuIds.forEach(id => {
                const menuItem = document.querySelector(`[data-menu-id="${id}"]`) || 
                               document.getElementById(id) ||
                               document.querySelector(`[href*="${id}"]`)?.closest('li');

                if (menuItem) {
                    menuItem.classList.add('active');

                    // Open parent submenus
                    let parent = menuItem.closest('.pcoded-submenu');
                    while (parent) {
                        parent.style.display = 'block';
                        parent.style.maxHeight = 'none';
                        
                        const parentLi = parent.closest('.pcoded-hasmenu');
                        if (parentLi) {
                            parentLi.classList.add('pcoded-trigger');
                        }
                        
                        parent = parentLi?.closest('.pcoded-submenu');
                    }
                }
            });

            return true;
        } catch (error) {
            console.error('Error setting active menu items:', error);
            return false;
        }
    };

    // =============================================================================
    // 3. SIDEBAR TOGGLE FUNCTIONS
    // =============================================================================

    let sidebarOverlay = null;

    function getOrCreateOverlay() {
        if (!sidebarOverlay) {
            sidebarOverlay = document.createElement('div');
            sidebarOverlay.id = 'sidebar-overlay';
            sidebarOverlay.className = 'sidebar-overlay';
            sidebarOverlay.style.cssText = `
                position: fixed;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                background: rgba(0, 0, 0, 0.5);
                z-index: 999;
                display: none;
                opacity: 0;
                transition: opacity 0.3s ease;
            `;
            document.body.appendChild(sidebarOverlay);

            sidebarOverlay.addEventListener('click', function() {
                window.closeSidebar();
            });
        }
        return sidebarOverlay;
    }

    window.toggleSidebar = function() {
        try {
            const navbar = document.querySelector('.pcoded-navbar');
            const overlay = getOrCreateOverlay();

            if (navbar) {
                const isOpen = navbar.classList.toggle('mob-open');
                document.body.classList.toggle('sidebar-open', isOpen);
                
                if (overlay) {
                    if (isOpen) {
                        overlay.style.display = 'block';
                        setTimeout(() => overlay.style.opacity = '1', 10);
                    } else {
                        overlay.style.opacity = '0';
                        setTimeout(() => overlay.style.display = 'none', 300);
                    }
                }
            }

            return true;
        } catch (error) {
            console.error('Error toggling sidebar:', error);
            return false;
        }
    };

    window.closeSidebar = function() {
        try {
            const navbar = document.querySelector('.pcoded-navbar');
            const overlay = document.getElementById('sidebar-overlay');

            if (navbar) {
                navbar.classList.remove('mob-open');
                document.body.classList.remove('sidebar-open');
            }

            if (overlay) {
                overlay.style.opacity = '0';
                setTimeout(() => overlay.style.display = 'none', 300);
            }

            return true;
        } catch (error) {
            console.error('Error closing sidebar:', error);
            return false;
        }
    };

    // =============================================================================
    // 4. UTILITY FUNCTIONS
    // =============================================================================

    window.showNotification = function(message, type, duration) {
        type = type || 'info';
        duration = duration || 3000;

        try {
            const colors = {
                success: '#10b981',
                error: '#ef4444',
                warning: '#f59e0b',
                info: '#3b82f6'
            };

            const notification = document.createElement('div');
            notification.className = 'basit-notification';
            notification.textContent = message;
            notification.style.cssText = `
                position: fixed;
                top: 20px;
                right: 20px;
                padding: 16px 24px;
                background: ${colors[type]};
                color: white;
                border-radius: 8px;
                box-shadow: 0 4px 12px rgba(0,0,0,0.2);
                z-index: 10000;
                font-size: 14px;
                font-weight: 500;
                animation: slideInRight 0.3s ease;
            `;

            document.body.appendChild(notification);

            setTimeout(() => {
                notification.style.animation = 'slideOutRight 0.3s ease';
                setTimeout(() => notification.remove(), 300);
            }, duration);

            return true;
        } catch (error) {
            console.error('Error showing notification:', error);
            return false;
        }
    };

    window.scrollToTop = function(smooth) {
        smooth = smooth !== false;
        try {
            window.scrollTo({
                top: 0,
                behavior: smooth ? 'smooth' : 'auto'
            });
            return true;
        } catch (error) {
            console.error('Error scrolling:', error);
            return false;
        }
    };

    window.copyToClipboard = function(text) {
        try {
            if (navigator.clipboard) {
                return navigator.clipboard.writeText(text).then(() => true);
            } else {
                const textarea = document.createElement('textarea');
                textarea.value = text;
                textarea.style.position = 'fixed';
                textarea.style.opacity = '0';
                document.body.appendChild(textarea);
                textarea.select();
                const success = document.execCommand('copy');
                document.body.removeChild(textarea);
                return success;
            }
        } catch (error) {
            console.error('Error copying to clipboard:', error);
            return false;
        }
    };

    window.focusElement = function(elementId) {
        try {
            const element = document.getElementById(elementId);
            if (element) {
                element.focus();
                return true;
            }
            return false;
        } catch (error) {
            console.error('Error focusing element:', error);
            return false;
        }
    };

    window.getViewportSize = function() {
        try {
            return {
                width: window.innerWidth,
                height: window.innerHeight
            };
        } catch (error) {
            console.error('Error getting viewport size:', error);
            return { width: 0, height: 0 };
        }
    };

    // =============================================================================
    // 5. EVENT LISTENERS
    // =============================================================================

    let resizeTimeout;
    window.addEventListener('resize', function() {
        clearTimeout(resizeTimeout);
        resizeTimeout = setTimeout(function() {
            if (window.innerWidth > 992) {
                window.closeSidebar();
            }
        }, 250);
    });

    document.addEventListener('keydown', function(e) {
        if (e.key === 'Escape') {
            window.closeSidebar();
        }
    });

    // =============================================================================
    // 6. STYLES INJECTION
    // =============================================================================

    const styles = document.createElement('style');
    styles.textContent = `
        @keyframes slideInRight {
            from { transform: translateX(100%); opacity: 0; }
            to { transform: translateX(0); opacity: 1; }
        }

        @keyframes slideOutRight {
            from { transform: translateX(0); opacity: 1; }
            to { transform: translateX(100%); opacity: 0; }
        }

        .custom-scrollbar::-webkit-scrollbar {
            width: 8px;
        }

        .custom-scrollbar::-webkit-scrollbar-track {
            background: #f1f5f9;
            border-radius: 4px;
        }

        .custom-scrollbar::-webkit-scrollbar-thumb {
            background: #cbd5e1;
            border-radius: 4px;
        }

        .custom-scrollbar::-webkit-scrollbar-thumb:hover {
            background: #94a3b8;
        }

        .pcoded-navbar.mob-open {
            transform: translateX(0) !important;
        }

        @media (max-width: 992px) {
            .pcoded-navbar {
                position: fixed;
                top: 0;
                right: 0;
                height: 100vh;
                transform: translateX(100%);
                transition: transform 0.3s ease;
                z-index: 1000;
            }
        }
    `;
    document.head.appendChild(styles);

    // =============================================================================
    // 7. AUTO-INITIALIZATION
    // =============================================================================

    function autoInitialize() {
        console.log('🎯 Auto-initializing Basit components...');

        // Wait for DOM to be fully loaded
        setTimeout(() => {
            // Initialize menu
            if (document.querySelector('.pcoded-inner-navbar')) {
                window.initializePcodedMenu();
            }

            // Initialize scrollbar
            if (document.querySelector('.pcoded-navbar')) {
                window.initializePerfectScrollbar('.pcoded-navbar');
            }

            // Create overlay
            getOrCreateOverlay();

            console.log('✅ Basit components initialized');
        }, 100);
    }

    // Run on DOM ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', autoInitialize);
    } else {
        autoInitialize();
    }

    console.log('✅ Basit Final Bundle loaded successfully');

})();
