//using Microsoft.AspNetCore.Components;
//using Microsoft.JSInterop;

//namespace Dashboard.Components.Forms
//{
//    public partial class RichTextEditor : ComponentBase, IAsyncDisposable
//    {
//        [Inject] private IJSRuntime JSRuntime { get; set; } = null!;

//        [Parameter] public string Value { get; set; } = string.Empty;
//        [Parameter] public EventCallback<string> ValueChanged { get; set; }
//        [Parameter] public string Placeholder { get; set; } = "Enter content...";
//        [Parameter] public bool Readonly { get; set; } = false;
//        [Parameter] public string Class { get; set; } = string.Empty;
//        [Parameter] public int Height { get; set; } = 400;
//        [Parameter] public bool EnableImageUpload { get; set; } = true;
//        [Parameter] public bool EnableVideoEmbed { get; set; } = true;
//        [Parameter] public bool EnableCodeEditor { get; set; } = true;
//        [Parameter] public bool EnableSpellCheck { get; set; } = true;
//        [Parameter] public bool EnableWordCount { get; set; } = true;
//        [Parameter] public bool EnableCharCount { get; set; } = true;
//        [Parameter] public bool EnablePreview { get; set; } = true;
//        [Parameter] public bool EnableFullscreen { get; set; } = true;
//        [Parameter] public string Theme { get; set; } = "silver"; // silver, dark
//        [Parameter] public string Language { get; set; } = "en";
//        [Parameter] public int MaxCharCount { get; set; } = -1; // -1 for unlimited
//        [Parameter] public EventCallback<string> OnImageUpload { get; set; }

//        private string _elementId = string.Empty;
//        private DotNetObjectReference<RichTextEditor>? _dotNetRef;
//        private bool _isInitialized = false;
//        private bool _isRendered = false;
//        private int _retryCount = 0;
//        private const int MaxRetries = 3;

//        protected override void OnInitialized()
//        {
//            _elementId = $"editor_{Guid.NewGuid():N}";
//            _dotNetRef = DotNetObjectReference.Create(this);
//            Console.WriteLine($"RichTextEditor OnInitialized: ElementId = {_elementId}");
//        }

//        protected override async Task OnAfterRenderAsync(bool firstRender)
//        {
//            if (firstRender)
//            {
//                _isRendered = true;
//                Console.WriteLine($"RichTextEditor OnAfterRenderAsync: ElementId = {_elementId}");

//                // Add delay to ensure DOM is ready
//                await Task.Delay(250);

//                // Initialize editor
//                await InitializeEditor();
//            }
//        }

//        private async Task InitializeEditor()
//        {
//            if (_isInitialized || string.IsNullOrEmpty(_elementId))
//            {
//                Console.WriteLine($"Editor already initialized or elementId is empty: {_elementId}");
//                return;
//            }

//            try
//            {
//                Console.WriteLine($"Initializing editor: {_elementId}");

//                // Show loading state
//                await ShowEditorLoading();

//                // Try using the existing text-editor system first
//                await JSRuntime.InvokeVoidAsync("window.textEditor.initialize",
//                    _elementId,
//                    Value ?? string.Empty,
//                    _dotNetRef,
//                    Readonly,
//                    Height);

//                _isInitialized = true;
//                Console.WriteLine($"Editor {_elementId} initialized successfully");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Text editor system failed, trying fallback: {ex.Message}");
//                await InitializeWithFallback();
//            }
//        }

//        private async Task InitializeWithFallback()
//        {
//            try
//            {
//                Console.WriteLine($"Using fallback initialization for: {_elementId}");

//                var escapedValue = Value?.Replace("'", "\\'").Replace("\n", "\\n").Replace("\r", "\\r") ?? "";
//                var escapedPlaceholder = Placeholder?.Replace("'", "\\'") ?? "Enter content...";

//                await JSRuntime.InvokeVoidAsync("eval", $@"
//                    (function() {{
//                        console.log('Fallback init for {_elementId}');

//                        // Store .NET reference
//                        window.editorRefs = window.editorRefs || {{}};
//                        window.editorRefs['{_elementId}'] = {_dotNetRef};

//                        const element = document.getElementById('{_elementId}');
//                        if (!element) {{
//                            console.error('Element not found: {_elementId}');
//                            return;
//                        }}

//                        // Try TinyMCE if available, otherwise use enhanced textarea
//                        if (typeof tinymce !== 'undefined') {{
//                            console.log('TinyMCE available, initializing...');

//                            tinymce.init({{
//                                selector: '#{_elementId}',
//                                height: {Height},
//                                menubar: false,
//                                branding: false,
//                                readonly: {Readonly.ToString().ToLower()},
//                                plugins: 'lists link paste',
//                                toolbar: {(Readonly ? "false" : "'undo redo | bold italic | bullist numlist | link'")},
//                                content_style: 'body {{ font-family: inherit; font-size: 14px; padding: 10px; }}',
//                                setup: function(editor) {{
//                                    editor.on('init', function() {{
//                                        editor.setContent('{escapedValue}');
//                                        console.log('TinyMCE ready for {_elementId}');
//                                    }});

//                                    if (!{Readonly.ToString().ToLower()}) {{
//                                        editor.on('change keyup input', function() {{
//                                            const content = editor.getContent();
//                                            if (window.editorRefs && window.editorRefs['{_elementId}']) {{
//                                                window.editorRefs['{_elementId}'].invokeMethodAsync('OnContentChanged', content);
//                                            }}
//                                        }});
//                                    }}
//                                }}
//                            }});
//                        }} else {{
//                            console.log('TinyMCE not available, using enhanced textarea');

//                            // Enhanced textarea fallback
//                            element.value = '{escapedValue}';
//                            element.placeholder = '{escapedPlaceholder}';
//                            element.style.minHeight = '{Height}px';
//                            element.style.fontFamily = 'inherit';
//                            element.style.fontSize = '14px';
//                            element.style.padding = '12px';
//                            element.style.border = '1px solid #ced4da';
//                            element.style.borderRadius = '0.375rem';
//                            element.style.resize = 'vertical';
//                            element.readonly = {Readonly.ToString().ToLower()};

//                            if (!{Readonly.ToString().ToLower()}) {{
//                                element.addEventListener('input', function() {{
//                                    if (window.editorRefs && window.editorRefs['{_elementId}']) {{
//                                        window.editorRefs['{_elementId}'].invokeMethodAsync('OnContentChanged', element.value);
//                                    }}
//                                }});
//                            }}

//                            console.log('Enhanced textarea ready for {_elementId}');
//                        }}
//                    }})();
//                ");

//                _isInitialized = true;
//                Console.WriteLine($"Fallback initialized successfully for {_elementId}");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Fallback failed: {ex.Message}");
//                await ShowEditorError($"Failed to initialize editor: {ex.Message}");

//                // Retry with delay
//                if (_retryCount < MaxRetries)
//                {
//                    _retryCount++;
//                    Console.WriteLine($"Retrying... (attempt {_retryCount})");
//                    await Task.Delay(2000);
//                    await InitializeEditor();
//                }
//            }
//        }

//        private async Task ShowEditorLoading()
//        {
//            try
//            {
//                StateHasChanged();
//                await Task.Delay(50);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error showing loading: {ex.Message}");
//            }
//        }

//        private async Task ShowEditorError(string message)
//        {
//            try
//            {
//                await JSRuntime.InvokeVoidAsync("eval", $@"
//                    var element = document.getElementById('{_elementId}');
//                    if (element) {{
//                        element.style.background = '#f8d7da';
//                        element.style.border = '1px solid #f5c6cb';
//                        element.style.borderRadius = '0.375rem';
//                        element.style.padding = '20px';
//                        element.style.textAlign = 'center';
//                        element.style.color = '#721c24';
//                        element.value = '';
//                        element.placeholder = '{message}';
//                        element.disabled = true;
//                    }}
//                ");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error showing error: {ex.Message}");
//            }
//        }

//        [JSInvokable]
//        public async Task OnContentChanged(string content)
//        {
//            if (Value != content)
//            {
//                Value = content;
//                await ValueChanged.InvokeAsync(content);
//            }
//        }

//        [JSInvokable]
//        public async Task OnImageUploadRequested(string base64Data, string fileName)
//        {
//            if (OnImageUpload.HasDelegate)
//            {
//                await OnImageUpload.InvokeAsync(base64Data);
//            }
//        }

//        public async Task SetContentAsync(string content)
//        {
//            if (_isInitialized && !string.IsNullOrEmpty(_elementId))
//            {
//                try
//                {
//                    await JSRuntime.InvokeVoidAsync("eval", $@"
//                        if (window.textEditor && window.textEditor.setContent) {{
//                            window.textEditor.setContent('{_elementId}', '{content?.Replace("'", "\\'")}');
//                        }} else if (tinymce && tinymce.get('{_elementId}')) {{
//                            tinymce.get('{_elementId}').setContent('{content?.Replace("'", "\\'")}');
//                        }} else {{
//                            var element = document.getElementById('{_elementId}');
//                            if (element) element.value = '{content?.Replace("'", "\\'")}';
//                        }}
//                    ");
//                    Value = content;
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Error setting content: {ex.Message}");
//                }
//            }
//        }

//        public async Task<string> GetContentAsync()
//        {
//            if (_isInitialized && !string.IsNullOrEmpty(_elementId))
//            {
//                try
//                {
//                    return await JSRuntime.InvokeAsync<string>("eval", $@"
//                        if (window.textEditor && window.textEditor.getContent) {{
//                            return window.textEditor.getContent('{_elementId}');
//                        }} else if (tinymce && tinymce.get('{_elementId}')) {{
//                            return tinymce.get('{_elementId}').getContent();
//                        }} else {{
//                            var element = document.getElementById('{_elementId}');
//                            return element ? element.value : '';
//                        }}
//                    ");
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Error getting content: {ex.Message}");
//                }
//            }
//            return Value;
//        }

//        public async Task<int> GetWordCountAsync()
//        {
//            if (_isInitialized && EnableWordCount && !string.IsNullOrEmpty(_elementId))
//            {
//                try
//                {
//                    return await JSRuntime.InvokeAsync<int>("eval", $@"
//                        if (window.textEditor && window.textEditor.getWordCount) {{
//                            return window.textEditor.getWordCount('{_elementId}');
//                        }} else if (tinymce && tinymce.get('{_elementId}')) {{
//                            const content = tinymce.get('{_elementId}').getContent({{format: 'text'}});
//                            return content.split(/\s+/).filter(word => word.length > 0).length;
//                        }} else {{
//                            var element = document.getElementById('{_elementId}');
//                            if (element) {{
//                                const text = element.value;
//                                return text.split(/\s+/).filter(word => word.length > 0).length;
//                            }}
//                            return 0;
//                        }}
//                    ");
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Error getting word count: {ex.Message}");
//                }
//            }
//            return 0;
//        }

//        public async Task<int> GetCharCountAsync()
//        {
//            if (_isInitialized && EnableCharCount && !string.IsNullOrEmpty(_elementId))
//            {
//                try
//                {
//                    return await JSRuntime.InvokeAsync<int>("eval", $@"
//                        if (window.textEditor && window.textEditor.getCharCount) {{
//                            return window.textEditor.getCharCount('{_elementId}');
//                        }} else if (tinymce && tinymce.get('{_elementId}')) {{
//                            const content = tinymce.get('{_elementId}').getContent({{format: 'text'}});
//                            return content.length;
//                        }} else {{
//                            var element = document.getElementById('{_elementId}');
//                            return element ? element.value.length : 0;
//                        }}
//                    ");
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Error getting char count: {ex.Message}");
//                }
//            }
//            return 0;
//        }

//        public async Task InsertContentAsync(string content)
//        {
//            if (_isInitialized && !string.IsNullOrEmpty(_elementId))
//            {
//                try
//                {
//                    var escapedContent = content?.Replace("'", "\\'") ?? "";
//                    await JSRuntime.InvokeVoidAsync("eval", $@"
//                        if (window.textEditor && window.textEditor.insertContent) {{
//                            window.textEditor.insertContent('{_elementId}', '{escapedContent}');
//                        }} else if (tinymce && tinymce.get('{_elementId}')) {{
//                            tinymce.get('{_elementId}').insertContent('{escapedContent}');
//                        }} else {{
//                            var element = document.getElementById('{_elementId}');
//                            if (element) {{
//                                element.value += '{escapedContent}';
//                            }}
//                        }}
//                    ");
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Error inserting content: {ex.Message}");
//                }
//            }
//        }

//        public async Task FocusAsync()
//        {
//            if (_isInitialized && !string.IsNullOrEmpty(_elementId))
//            {
//                try
//                {
//                    await JSRuntime.InvokeVoidAsync("eval", $@"
//                        if (window.textEditor && window.textEditor.focus) {{
//                            window.textEditor.focus('{_elementId}');
//                        }} else if (tinymce && tinymce.get('{_elementId}')) {{
//                            tinymce.get('{_elementId}').focus();
//                        }} else {{
//                            var element = document.getElementById('{_elementId}');
//                            if (element) element.focus();
//                        }}
//                    ");
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Error focusing: {ex.Message}");
//                }
//            }
//        }

//        public async Task ClearAsync()
//        {
//            if (_isInitialized && !string.IsNullOrEmpty(_elementId))
//            {
//                try
//                {
//                    await SetContentAsync("");
//                    Value = string.Empty;
//                    await ValueChanged.InvokeAsync(Value);
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Error clearing: {ex.Message}");
//                }
//            }
//        }

//        public async Task<bool> IsInitializedAsync()
//        {
//            return _isInitialized;
//        }

//        public async ValueTask DisposeAsync()
//        {
//            if (_isInitialized && !string.IsNullOrEmpty(_elementId))
//            {
//                try
//                {
//                    await JSRuntime.InvokeVoidAsync("eval", $@"
//                        if (window.textEditor && window.textEditor.destroy) {{
//                            window.textEditor.destroy('{_elementId}');
//                        }} else if (tinymce && tinymce.get('{_elementId}')) {{
//                            tinymce.get('{_elementId}').destroy();
//                        }}
//                        if (window.editorRefs && window.editorRefs['{_elementId}']) {{
//                            delete window.editorRefs['{_elementId}'];
//                        }}
//                    ");
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Error disposing: {ex.Message}");
//                }
//            }

//            _dotNetRef?.Dispose();
//        }
//    }
//}

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Dashboard.Components.Forms
{
	public class RichTextEditorBase : ComponentBase, IAsyncDisposable
	{
		[Parameter] public string Value { get; set; } = string.Empty;
		[Parameter] public EventCallback<string> ValueChanged { get; set; }
		[Parameter] public string Placeholder { get; set; } = "Start typing...";
		[Parameter] public int Height { get; set; } = 300;
		[Parameter] public string Language { get; set; } = "en";

		[Parameter] public bool EnableImageUpload { get; set; } = true;
		[Parameter] public bool EnableVideoEmbed { get; set; } = true;
		[Parameter] public bool EnableCodeEditor { get; set; } = true;
		[Parameter] public bool EnableSpellCheck { get; set; } = true;
		[Parameter] public bool EnableWordCount { get; set; } = true;
		[Parameter] public bool EnableCharCount { get; set; } = true;
		[Parameter] public bool EnablePreview { get; set; } = true;
		[Parameter] public bool EnableFullscreen { get; set; } = true;
		[Parameter] public string Theme { get; set; } = "snow";
		[Parameter] public int MaxCharCount { get; set; } = 10000;

		[Inject] protected IJSRuntime JSRuntime { get; set; } = null!;

		protected string EditorId = $"quill-editor-{Guid.NewGuid():N}";
		protected DotNetObjectReference<RichTextEditorBase>? dotNetReference;
		protected bool isInitialized = false;

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				try
				{
					dotNetReference = DotNetObjectReference.Create(this);
					await JSRuntime.InvokeVoidAsync("initQuillEditor",
						EditorId,
						dotNetReference,
						Value,
						Placeholder,
						Height);
					isInitialized = true;
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error initializing Quill: {ex.Message}");
				}
			}
		}

		[JSInvokable]
		public async Task UpdateValue(string newValue)
		{
			if (Value != newValue)
			{
				Value = newValue;
				await ValueChanged.InvokeAsync(Value);
			}
		}

		public async ValueTask DisposeAsync()
		{
			try
			{
				if (isInitialized)
				{
					await JSRuntime.InvokeVoidAsync("destroyQuillEditor", EditorId);
				}
				dotNetReference?.Dispose();
			}
			catch (Exception)
			{
				// Ignore errors during disposal
			}
		}
	}
}
