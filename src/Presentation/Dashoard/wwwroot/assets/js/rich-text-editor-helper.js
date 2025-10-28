/**
 * TinyMCE Rich Text Editor Helper Functions
 * This file provides helper functions for initializing and managing TinyMCE editors in Blazor components.
 */

window.initializeTinyMCEEditor = async function (elementId, plugins, toolbar, height, readonly, placeholder, initialValue, dotNetRef) {
    try {
        console.log(`Initializing TinyMCE editor: ${elementId}`);
        
        // Store the .NET reference globally for access
        window.editorRefs = window.editorRefs || {};
        window.editorRefs[elementId] = dotNetRef;

        // Wait for TinyMCE to be available
        let attempts = 0;
        while (typeof tinymce === 'undefined' && attempts < 20) {
            await new Promise(resolve => setTimeout(resolve, 100));
            attempts++;
        }
        
        if (typeof tinymce === 'undefined') {
            console.error('TinyMCE not available after waiting');
            throw new Error('TinyMCE not available');
        }

        // Initialize TinyMCE
        tinymce.init({
            selector: `#${elementId}`,
            plugins: plugins,
            toolbar: toolbar,
            height: height,
            menubar: false,
            branding: false,
            readonly: readonly,
            placeholder: placeholder,
            content_style: 'body { font-family: -apple-system, BlinkMacSystemFont, San Francisco, Segoe UI, Roboto, Helvetica Neue, sans-serif; font-size: 14px; }',
            paste_data_images: true,
            paste_as_text: false,
            paste_enable_default_filters: true,
            paste_remove_styles_if_webkit: true,
            paste_webkit_styles: 'color font-size font-family background-color',
            automatic_uploads: true,
            file_picker_types: 'image',
            resize: 'both',
            statusbar: false,
            setup: function(editor) {
                // Handle content changes
                editor.on('change keyup paste input', function() {
                    const content = editor.getContent();
                    if (window.editorRefs && window.editorRefs[elementId]) {
                        try {
                            window.editorRefs[elementId].invokeMethodAsync('OnContentChanged', content);
                        } catch (error) {
                            console.warn('Error invoking OnContentChanged:', error);
                        }
                    }
                });
                
                // Set initial content when editor is ready
                editor.on('init', function() {
                    if (initialValue) {
                        editor.setContent(initialValue);
                    }
                    console.log(`TinyMCE editor initialized successfully: ${elementId}`);
                });

                // Handle image upload events
                editor.on('drop', function(e) {
                    handleImageDrop(e, editor, elementId);
                });

                editor.on('paste', function(e) {
                    handleImagePaste(e, editor, elementId);
                });
            }
        });

        return true;
    } catch (error) {
        console.error(`Error initializing TinyMCE editor ${elementId}:`, error);
        throw error;
    }
};

// Helper function to handle image drops
function handleImageDrop(e, editor, elementId) {
    const files = e.dataTransfer?.files;
    if (files && files.length > 0) {
        for (let i = 0; i < files.length; i++) {
            const file = files[i];
            if (file.type.startsWith('image/')) {
                handleImageFile(file, editor, elementId);
            }
        }
    }
}

// Helper function to handle image paste
function handleImagePaste(e, editor, elementId) {
    const items = e.clipboardData?.items;
    if (items) {
        for (let i = 0; i < items.length; i++) {
            const item = items[i];
            if (item.type.startsWith('image/')) {
                const file = item.getAsFile();
                if (file) {
                    handleImageFile(file, editor, elementId);
                }
            }
        }
    }
}

// Helper function to process image files
function handleImageFile(file, editor, elementId) {
    const reader = new FileReader();
    reader.onload = function(e) {
        const base64 = e.target.result;
        if (window.editorRefs && window.editorRefs[elementId]) {
            try {
                window.editorRefs[elementId].invokeMethodAsync('OnImageUploadRequested', base64, file.name);
            } catch (error) {
                console.warn('Error invoking OnImageUploadRequested:', error);
                // Fallback: insert image directly
                editor.insertContent(`<img src="${base64}" alt="${file.name}" style="max-width: 100%; height: auto;" />`);
            }
        }
    };
    reader.readAsDataURL(file);
}

// Global utility functions for external access
window.richTextEditorUtils = {
    // Get editor content by element ID
    getContent: function(elementId) {
        const editor = tinymce.get(elementId);
        return editor ? editor.getContent() : '';
    },

    // Set editor content by element ID
    setContent: function(elementId, content) {
        const editor = tinymce.get(elementId);
        if (editor) {
            editor.setContent(content);
            return true;
        }
        return false;
    },

    // Insert content into editor
    insertContent: function(elementId, content) {
        const editor = tinymce.get(elementId);
        if (editor) {
            editor.insertContent(content);
            return true;
        }
        return false;
    },

    // Focus editor
    focus: function(elementId) {
        const editor = tinymce.get(elementId);
        if (editor) {
            editor.focus();
            return true;
        }
        return false;
    },

    // Clear editor content
    clear: function(elementId) {
        const editor = tinymce.get(elementId);
        if (editor) {
            editor.setContent('');
            return true;
        }
        return false;
    },

    // Get word count
    getWordCount: function(elementId) {
        const editor = tinymce.get(elementId);
        if (editor) {
            const text = editor.getContent({ format: 'text' });
            return text.split(/\s+/).filter(word => word.length > 0).length;
        }
        return 0;
    },

    // Get character count
    getCharCount: function(elementId) {
        const editor = tinymce.get(elementId);
        if (editor) {
            const text = editor.getContent({ format: 'text' });
            return text.length;
        }
        return 0;
    },

    // Destroy editor
    destroy: function(elementId) {
        const editor = tinymce.get(elementId);
        if (editor) {
            editor.destroy();
            // Clean up references
            if (window.editorRefs && window.editorRefs[elementId]) {
                delete window.editorRefs[elementId];
            }
            return true;
        }
        return false;
    },

    // Check if editor is initialized
    isInitialized: function(elementId) {
        return !!tinymce.get(elementId);
    }
};

// Cleanup function for page unload
window.addEventListener('beforeunload', function() {
    if (window.editorRefs) {
        Object.keys(window.editorRefs).forEach(elementId => {
            const editor = tinymce.get(elementId);
            if (editor) {
                editor.destroy();
            }
        });
        window.editorRefs = {};
    }
});

console.log('TinyMCE Rich Text Editor helper functions loaded successfully.');