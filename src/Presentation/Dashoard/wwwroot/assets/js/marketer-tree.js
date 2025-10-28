// Marketer Tree JavaScript utilities

window.marketerTree = {
    // Initialize tree functionality
    init: function () {
        this.setupTreeInteractions();
        this.setupResponsiveHandling();
        this.setupKeyboardNavigation();
    },

    // Setup tree node interactions
    setupTreeInteractions: function () {
        // Handle node collapse/expand functionality
        document.addEventListener('click', function (e) {
            if (e.target.classList.contains('collapse-switch')) {
                const node = e.target.closest('.tree-node');
                if (node) {
                    node.classList.toggle('collapsed');
                }
            }
        });

        // Handle smooth scrolling to nodes
        const treeContainer = document.querySelector('.marketer-tree-container');
        if (treeContainer) {
            treeContainer.addEventListener('wheel', function (e) {
                e.preventDefault();
                this.scrollLeft += e.deltaY;
            });
        }
    },

    // Setup responsive handling
    setupResponsiveHandling: function () {
        const checkTreeOverflow = () => {
            const treeContainer = document.querySelector('.marketer-tree-container');
            const tree = document.querySelector('.tree');

            if (treeContainer && tree) {
                if (tree.scrollWidth > treeContainer.clientWidth) {
                    treeContainer.classList.add('overflow-x-auto');
                } else {
                    treeContainer.classList.remove('overflow-x-auto');
                }
            }
        };

        // Check on load and resize
        window.addEventListener('load', checkTreeOverflow);
        window.addEventListener('resize', checkTreeOverflow);
    },

    // Setup keyboard navigation
    setupKeyboardNavigation: function () {
        document.addEventListener('keydown', function (e) {
            const focusedNode = document.activeElement;

            if (focusedNode && focusedNode.classList.contains('node-content')) {
                const treeNode = focusedNode.closest('.tree-node');

                switch (e.key) {
                    case 'ArrowLeft':
                        // Navigate to left sibling or parent
                        marketerTree.navigateLeft(treeNode);
                        e.preventDefault();
                        break;
                    case 'ArrowRight':
                        // Navigate to right sibling or first child
                        marketerTree.navigateRight(treeNode);
                        e.preventDefault();
                        break;
                    case 'ArrowDown':
                        // Navigate to first child
                        marketerTree.navigateDown(treeNode);
                        e.preventDefault();
                        break;
                    case 'ArrowUp':
                        // Navigate to parent
                        marketerTree.navigateUp(treeNode);
                        e.preventDefault();
                        break;
                    case 'Enter':
                    case ' ':
                        // Trigger click
                        focusedNode.click();
                        e.preventDefault();
                        break;
                }
            }
        });
    },

    // Navigation functions
    navigateLeft: function (currentNode) {
        const parent = currentNode.parentElement;
        const siblings = Array.from(parent.children);
        const currentIndex = siblings.indexOf(currentNode);

        if (currentIndex > 0) {
            const leftSibling = siblings[currentIndex - 1];
            const nodeContent = leftSibling.querySelector('.node-content');
            if (nodeContent) nodeContent.focus();
        }
    },

    navigateRight: function (currentNode) {
        const parent = currentNode.parentElement;
        const siblings = Array.from(parent.children);
        const currentIndex = siblings.indexOf(currentNode);

        if (currentIndex < siblings.length - 1) {
            const rightSibling = siblings[currentIndex + 1];
            const nodeContent = rightSibling.querySelector('.node-content');
            if (nodeContent) nodeContent.focus();
        }
    },

    navigateDown: function (currentNode) {
        const children = currentNode.querySelector('.node-children');
        if (children) {
            const firstChild = children.querySelector('.tree-node .node-content');
            if (firstChild) firstChild.focus();
        }
    },

    navigateUp: function (currentNode) {
        const parentLi = currentNode.closest('ul').parentElement;
        if (parentLi && parentLi.classList.contains('tree-node')) {
            const parentContent = parentLi.querySelector('.node-content');
            if (parentContent) parentContent.focus();
        }
    },

    // Utility function to copy text to clipboard
    copyToClipboard: function (text) {
        if (navigator.clipboard && window.isSecureContext) {
            return navigator.clipboard.writeText(text);
        } else {
            // Fallback for older browsers
            const textArea = document.createElement('textarea');
            textArea.value = text;
            textArea.style.position = 'fixed';
            textArea.style.left = '-999999px';
            textArea.style.top = '-999999px';
            document.body.appendChild(textArea);
            textArea.focus();
            textArea.select();

            return new Promise((resolve, reject) => {
                try {
                    document.execCommand('copy');
                    textArea.remove();
                    resolve();
                } catch (error) {
                    textArea.remove();
                    reject(error);
                }
            });
        }
    },

    // Generate referral link
    generateReferralLink: function (pathCode, position) {
        const newPathCode = pathCode + (position === 'Left' ? '1' : '2');
        const baseUrl = window.location.origin;
        return `${baseUrl}/register?ref=${newPathCode}`;
    },

    // Auto-center tree on a specific node
    centerOnNode: function (nodeElement) {
        const container = document.querySelector('.marketer-tree-container');
        if (container && nodeElement) {
            const containerRect = container.getBoundingClientRect();
            const nodeRect = nodeElement.getBoundingClientRect();

            const scrollLeft = nodeRect.left - containerRect.left - (containerRect.width / 2) + (nodeRect.width / 2);

            container.scrollTo({
                left: container.scrollLeft + scrollLeft,
                behavior: 'smooth'
            });
        }
    },

    // Tree search functionality
    searchTree: function (searchTerm) {
        const nodes = document.querySelectorAll('.tree-node');
        let foundNodes = [];

        nodes.forEach(node => {
            const nameElement = node.querySelector('.node-name');
            const pathElement = node.querySelector('.node-details span:last-child');

            if (nameElement && pathElement) {
                const name = nameElement.textContent.toLowerCase();
                const path = pathElement.textContent.toLowerCase();

                if (name.includes(searchTerm.toLowerCase()) || path.includes(searchTerm.toLowerCase())) {
                    node.classList.add('search-highlight');
                    foundNodes.push(node);
                } else {
                    node.classList.remove('search-highlight');
                }
            }
        });

        // Center on first found node
        if (foundNodes.length > 0) {
            this.centerOnNode(foundNodes[0]);
        }

        return foundNodes;
    },

    // Clear search highlights
    clearSearchHighlights: function () {
        const highlightedNodes = document.querySelectorAll('.search-highlight');
        highlightedNodes.forEach(node => {
            node.classList.remove('search-highlight');
        });
    }
};

// Resource loading utilities
window.loadStyleSheet = function (path) {
    return new Promise((resolve, reject) => {
        const link = document.createElement('link');
        link.rel = 'stylesheet';
        link.href = path;
        link.onload = () => resolve();
        link.onerror = () => reject(new Error(`Failed to load stylesheet: ${path}`));
        document.head.appendChild(link);
    });
};

window.loadScript = function (path) {
    return new Promise((resolve, reject) => {
        const script = document.createElement('script');
        script.src = path;
        script.onload = () => resolve();
        script.onerror = () => reject(new Error(`Failed to load script: ${path}`));
        document.head.appendChild(script);
    });
};

// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', function () {
    if (window.marketerTree) {
        window.marketerTree.init();
    }
});

// Export for use in Blazor
window.blazorMarketerTree = {
    copyReferralLink: function (pathCode, position) {
        const link = window.marketerTree.generateReferralLink(pathCode, position);
        return window.marketerTree.copyToClipboard(link);
    },

    centerOnNode: function (pathCode) {
        const node = document.querySelector(`[data-path-code="${pathCode}"]`);
        if (node) {
            window.marketerTree.centerOnNode(node);
        }
    },

    searchNodes: function (searchTerm) {
        if (searchTerm) {
            return window.marketerTree.searchTree(searchTerm);
        } else {
            window.marketerTree.clearSearchHighlights();
            return [];
        }
    }
};