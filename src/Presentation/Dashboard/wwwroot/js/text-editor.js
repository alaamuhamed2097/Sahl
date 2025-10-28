window.textEditor = {
    instances: {},
    
    // Initialize text editor with on-demand TinyMCE loading (Simplified approach)
    initialize: function (elementId, content, dotNetRef, readonly = false, height = 400) {
        console.log(`Initializing editor with ID: "${elementId}"`);
        
        // Validate parameters
        if (!elementId) {
            console.error('ElementId is null or undefined');
            return Promise.reject(new Error('ElementId is required'));
        }

        if (typeof elementId !== 'string') {
            console.error(`ElementId must be a string, got ${typeof elementId}: ${elementId}`);
            return Promise.reject(new Error('ElementId must be a string'));
        }

        // Show loading indicator immediately
        this.showLoadingIndicator(elementId);

        // Check if TinyMCE is loaded, if not load it first
        if (typeof tinymce === 'undefined') {
            console.log('TinyMCE not loaded, loading on demand...');
            
            // Use ScriptLoader to load TinyMCE core only
            return window.ScriptLoader.loadTinyMCE()
                .then(() => {
                    console.log('TinyMCE loaded successfully, initializing editor...');
                    return this.initializeEditor(elementId, content, dotNetRef, readonly, height);
                })
                .catch(error => {
                    console.error('Failed to load TinyMCE:', error);
                    this.showErrorIndicator(elementId, 'Failed to load text editor: ' + error.message);
                    return Promise.reject(error);
                });
        } else {
            // TinyMCE already loaded, initialize editor directly
            console.log('TinyMCE already available, initializing editor...');
            return this.initializeEditor(elementId, content, dotNetRef, readonly, height);
        }
    },

    // Show loading indicator while TinyMCE loads
    showLoadingIndicator: function(elementId) {
        try {
            const element = document.getElementById(elementId);
            if (element) {
                element.style.background = '#f8f9fa';
                element.style.border = '1px dashed #dee2e6';
                element.style.borderRadius = '0.375rem';
                element.style.padding = '20px';
                element.style.textAlign = 'center';
                element.style.color = '#6c757d';
                element.value = '';
                element.placeholder = 'Loading text editor...';
                element.disabled = true;
                
                // Add spinner
                const spinner = document.createElement('div');
                spinner.className = 'spinner-border spinner-border-sm text-primary';
                spinner.style.marginRight = '8px';
                spinner.setAttribute('role', 'status');
                
                if (!element.previousElementSibling || !element.previousElementSibling.classList.contains('loading-spinner')) {
                    spinner.classList.add('loading-spinner');
                    element.parentNode.insertBefore(spinner, element);
                }
            }
        } catch (error) {
            console.error('Error showing loading indicator:', error);
        }
    },

    // Show error indicator if loading fails
    showErrorIndicator: function(elementId, message) {
        try {
            const element = document.getElementById(elementId);
            if (element) {
                // Remove loading spinner
                const spinner = element.parentNode.querySelector('.loading-spinner');
                if (spinner) {
                    spinner.remove();
                }
                
                element.style.background = '#f8d7da';
                element.style.border = '1px solid #f5c6cb';
                element.style.borderRadius = '0.375rem';
                element.style.padding = '20px';
                element.style.textAlign = 'center';
                element.style.color = '#721c24';
                element.value = '';
                element.placeholder = message || 'Failed to load text editor';
                element.disabled = true;
                
                // Add retry button
                if (!element.nextElementSibling || !element.nextElementSibling.classList.contains('retry-btn')) {
                    const retryBtn = document.createElement('button');
                    retryBtn.innerHTML = '<i class="fas fa-redo"></i> Retry';
                    retryBtn.className = 'btn btn-sm btn-outline-primary mt-2 retry-btn';
                    retryBtn.onclick = () => {
                        // Remove error elements and retry
                        retryBtn.remove();
                        this.resetElementStyles(elementId);
                        this.initialize(elementId, '', null);
                    };
                    element.parentNode.insertBefore(retryBtn, element.nextSibling);
                }
            }
        } catch (error) {
            console.error('Error showing error indicator:', error);
        }
    },

    // Reset element styles to normal
    resetElementStyles: function(elementId) {
        try {
            const element = document.getElementById(elementId);
            if (element) {
                element.style.background = '';
                element.style.border = '';
                element.style.borderRadius = '';
                element.style.padding = '';
                element.style.textAlign = '';
                element.style.color = '';
                element.disabled = false;
                element.style.display = '';
                element.placeholder = 'Enter content...';
                
                // Remove loading spinner and retry button
                const spinner = element.parentNode.querySelector('.loading-spinner');
                if (spinner) spinner.remove();
                
                const retryBtn = element.parentNode.querySelector('.retry-btn');
                if (retryBtn) retryBtn.remove();
            }
        } catch (error) {
            console.error('Error resetting element styles:', error);
        }
    },

    // Initialize the actual TinyMCE editor
    initializeEditor: function (elementId, content, dotNetRef, readonly = false, height = 400) {
        return new Promise((resolve, reject) => {
            try {
                console.log(`Starting TinyMCE initialization for ${elementId}`);
                
                // Clean up any existing instance
                if (this.instances[elementId]) {
                    console.log(`Destroying existing instance for ${elementId}`);
                    this.destroy(elementId);
                }

                // Ensure element exists
                const element = document.getElementById(elementId);
                if (!element) {
                    const error = `Element with id "${elementId}" not found in DOM`;
                    console.error(error);
                    reject(new Error(error));
                    return;
                }

                // Reset element styles
                this.resetElementStyles(elementId);

                // Ensure TinyMCE is available
                if (typeof tinymce === 'undefined') {
                    const error = 'TinyMCE is not loaded';
                    console.error(error);
                    reject(new Error(error));
                    return;
                }

                // Set a timeout to prevent infinite loading
                const initTimeout = setTimeout(() => {
                    console.error(`TinyMCE initialization timeout for ${elementId}`);
                    this.showErrorIndicator(elementId, 'Editor initialization timeout');
                    reject(new Error('TinyMCE initialization timeout'));
                }, 10000); // 10 second timeout

                const editorConfig = {
                    selector: '#' + elementId,
                    height: height || 400,
                    menubar: false,
                    branding: false,
                    promotion: false,
                    plugins: [
                        'advlist', 'autolink', 'lists', 'link', 'image', 'charmap', 'preview',
                        'anchor', 'searchreplace', 'visualblocks', 'code', 'fullscreen',
                        'insertdatetime', 'media', 'table', 'help', 'wordcount'
                    ],
                    toolbar: readonly ? false : 'undo redo | blocks | ' +
                        'bold italic forecolor | alignleft aligncenter ' +
                        'alignright alignjustify | bullist numlist outdent indent | ' +
                        'removeformat | help',
                    content_style: 'body { font-family:Helvetica,Arial,sans-serif; font-size:14px; padding: 10px; }',
                    readonly: readonly || false,
                    
                    init_instance_callback: function (editor) {
                        clearTimeout(initTimeout);
                        console.log(`Editor "${elementId}" initialized successfully via callback`);
                        
                        try {
                            editor.setContent(content || '');
                            resolve(true);
                        } catch (error) {
                            console.error('Error setting initial content:', error);
                            reject(error);
                        }
                    },
                    
                    setup: function (editor) {
                        editor.on('init', function () {
                            console.log(`Editor "${elementId}" setup init event fired`);
                        });
                        
                        editor.on('LoadContent', function () {
                            console.log(`Content loaded for editor "${elementId}"`);
                        });

                        editor.on('Error', function (e) {
                            clearTimeout(initTimeout);
                            console.error(`TinyMCE Error for editor "${elementId}":`, e);
                            reject(new Error(`TinyMCE Error: ${e.message || 'Unknown error'}`));
                        });
                        
                        if (!readonly && dotNetRef) {
                            let debounceTimer;
                            
                            const handleContentChange = function() {
                                clearTimeout(debounceTimer);
                                debounceTimer = setTimeout(function() {
                                    try {
                                        const newContent = editor.getContent();
                                        if (dotNetRef && dotNetRef.invokeMethodAsync) {
                                            dotNetRef.invokeMethodAsync('OnContentChanged', newContent)
                                                .catch(err => console.error('Error invoking OnContentChanged:', err));
                                        }
                                    } catch (error) {
                                        console.error('Error in handleContentChange:', error);
                                    }
                                }, 500);
                            };
                            
                            editor.on('input', handleContentChange);
                            editor.on('change', handleContentChange);
                            editor.on('blur', handleContentChange);
                        }
                    }
                };

                console.log(`Initializing TinyMCE with config for ${elementId}`);
                tinymce.init(editorConfig);

                this.instances[elementId] = {
                    initialized: true,
                    dotNetRef: dotNetRef,
                    readonly: readonly
                };
                
                console.log(`Editor "${elementId}" setup completed`);
                
            } catch (error) {
                console.error(`Error initializing TinyMCE for "${elementId}":`, error);
                this.showErrorIndicator(elementId, 'Initialization error: ' + error.message);
                reject(error);
            }
        });
    },

    setContent: function (elementId, content) {
        console.log(`Setting content for editor: "${elementId}"`);
        
        if (!elementId) {
            console.error('ElementId is null or undefined in setContent');
            return false;
        }

        try {
            const editor = tinymce.get(elementId);
            if (editor && editor.initialized) {
                editor.setContent(content || '');
                console.log(`Content set successfully for "${elementId}"`);
                return true;
            } else {
                console.warn(`Editor "${elementId}" not found or not initialized`);
                return false;
            }
        } catch (error) {
            console.error(`Error setting content for "${elementId}":`, error);
            return false;
        }
    },

    getContent: function (elementId) {
        if (!elementId) {
            console.error('ElementId is null or undefined in getContent');
            return '';
        }

        try {
            const editor = tinymce.get(elementId);
            if (editor && editor.initialized) {
                return editor.getContent();
            } else {
                console.warn(`Editor "${elementId}" not found or not initialized`);
                return '';
            }
        } catch (error) {
            console.error(`Error getting content for "${elementId}":`, error);
            return '';
        }
    },

    getWordCount: function (elementId) {
        if (!elementId) {
            console.error('ElementId is null or undefined in getWordCount');
            return 0;
        }

        try {
            const editor = tinymce.get(elementId);
            if (editor && editor.initialized && editor.plugins.wordcount) {
                return editor.plugins.wordcount.getCount();
            }
            return 0;
        } catch (error) {
            console.error(`Error getting word count for "${elementId}":`, error);
            return 0;
        }
    },

    getCharCount: function (elementId) {
        if (!elementId) {
            console.error('ElementId is null or undefined in getCharCount');
            return 0;
        }

        try {
            const editor = tinymce.get(elementId);
            if (editor && editor.initialized) {
                return editor.getContent({ format: 'text' }).length;
            }
            return 0;
        } catch (error) {
            console.error(`Error getting char count for "${elementId}":`, error);
            return 0;
        }
    },

    insertContent: function (elementId, content) {
        if (!elementId) {
            console.error('ElementId is null or undefined in insertContent');
            return false;
        }

        try {
            const editor = tinymce.get(elementId);
            if (editor && editor.initialized) {
                editor.insertContent(content || '');
                return true;
            } else {
                console.warn(`Editor "${elementId}" not found or not initialized`);
                return false;
            }
        } catch (error) {
            console.error(`Error inserting content for "${elementId}":`, error);
            return false;
        }
    },

    focus: function (elementId) {
        if (!elementId) {
            console.error('ElementId is null or undefined in focus');
            return false;
        }

        try {
            const editor = tinymce.get(elementId);
            if (editor && editor.initialized) {
                editor.focus();
                return true;
            } else {
                console.warn(`Editor "${elementId}" not found or not initialized`);
                return false;
            }
        } catch (error) {
            console.error(`Error focusing editor "${elementId}":`, error);
            return false;
        }
    },

    destroy: function (elementId) {
        if (!elementId) {
            console.error('ElementId is null or undefined in destroy');
            return false;
        }

        try {
            const editor = tinymce.get(elementId);
            if (editor) {
                editor.destroy();
                console.log(`Editor "${elementId}" destroyed`);
            }
            delete this.instances[elementId];
            
            // Reset element styles and remove any UI elements
            this.resetElementStyles(elementId);
            
            return true;
        } catch (error) {
            console.error(`Error destroying editor "${elementId}":`, error);
            return false;
        }
    }
};

// Add global error handler for TinyMCE (only if TinyMCE is available)
function setupTinyMCEErrorHandlers() {
    if (typeof tinymce !== 'undefined') {
        tinymce.on('AddEditor', function(e) {
            console.log(`TinyMCE Editor added: ${e.editor.id}`);
        });

        tinymce.on('RemoveEditor', function(e) {
            console.log(`TinyMCE Editor removed: ${e.editor.id}`);
        });
    }
}

// Set up error handlers when TinyMCE is available
if (typeof tinymce !== 'undefined') {
    setupTinyMCEErrorHandlers();
} else {
    // Set up a listener for when TinyMCE becomes available (with timeout)
    let checkInterval = setInterval(() => {
        if (typeof tinymce !== 'undefined') {
            setupTinyMCEErrorHandlers();
            clearInterval(checkInterval);
        }
    }, 100);
    
    // Clear interval after 5 seconds to avoid infinite checking
    setTimeout(() => clearInterval(checkInterval), 5000);
}