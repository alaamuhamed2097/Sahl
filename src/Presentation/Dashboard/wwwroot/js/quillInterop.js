window.initQuillEditor = (editorId, dotNetHelper, initialValue, placeholder, height) => {
    const container = document.getElementById(editorId);
    if (!container) {
        console.error('Quill container not found:', editorId);
        return;
    }

    // Initialize Quill editor
    const quill = new Quill(container, {
        theme: 'snow',
        placeholder: placeholder || 'Start typing...',
        modules: {
            toolbar: [
                [{ 'header': [1, 2, 3, 4, 5, 6, false] }],
                [{ 'font': [] }],
                [{ 'size': ['small', false, 'large', 'huge'] }],
                ['bold', 'italic', 'underline', 'strike'],
                [{ 'color': [] }, { 'background': [] }],
                [{ 'script': 'sub' }, { 'script': 'super' }],
                [{ 'list': 'ordered' }, { 'list': 'bullet' }],
                [{ 'indent': '-1' }, { 'indent': '+1' }],
                [{ 'direction': 'rtl' }],
                [{ 'align': [] }],
                ['link', 'image', 'video'],
                ['blockquote', 'code-block'],
                ['clean']
            ]
        }
    });

    // Set initial content
    if (initialValue) {
        quill.root.innerHTML = initialValue;
    }

    // Set height
    if (height) {
        container.style.height = height + 'px';
    }

    // Listen for text changes
    quill.on('text-change', function () {
        const html = quill.root.innerHTML;
        dotNetHelper.invokeMethodAsync('UpdateValue', html);
    });

    // Store reference for cleanup
    window[editorId] = quill;
};

window.destroyQuillEditor = (editorId) => {
    if (window[editorId]) {
        delete window[editorId];
    }
};

window.getQuillContent = (editorId) => {
    const quill = window[editorId];
    return quill ? quill.root.innerHTML : '';
};

window.setQuillContent = (editorId, content) => {
    const quill = window[editorId];
    if (quill) {
        quill.root.innerHTML = content;
    }
};