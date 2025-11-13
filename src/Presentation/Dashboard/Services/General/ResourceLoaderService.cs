using Dashboard.Contracts.General;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Dashboard.Services.General
{
    public class ResourceLoaderService : IResourceLoaderService, IDisposable
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly HashSet<string> _loadedScripts = new();
        private IJSObjectReference? _cssLoaderModule;
        private NavigationManager _navigationManager;
        public ResourceLoaderService(IJSRuntime jsRuntime, NavigationManager navigationManager)
        {
            _jsRuntime = jsRuntime;
            _navigationManager = navigationManager;
        }

        /// <summary>
        /// Loads a single script if not already loaded.
        /// </summary>
        public async Task LoadScript(string url)
        {
            if (_loadedScripts.Contains(url)) return;
            
            // ✅ FIX: Remove the wwwroot prefix for production
            // In Blazor WebAssembly, all paths are relative to wwwroot
            // Adding "wwwroot/" causes 404 errors in production
            await _jsRuntime.InvokeVoidAsync("ScriptLoader.loadScript", url);
            
            _loadedScripts.Add(url);
        }

        /// <summary>
        /// Loads multiple scripts sequentially.
        /// </summary>
        public async Task LoadScriptsSequential(params string[] urls)
        {
            foreach (var url in urls)
            {
                await LoadScript(url);
            }
        }

        /// <summary>
        /// Loads multiple scripts in parallel.
        /// </summary>
        public async Task LoadScriptsParallel(params string[] urls)
        {
            var tasks = urls.Select(LoadScript).ToList();
            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Removes a script from the DOM if it exists.
        /// </summary>
        public async Task<bool> RemoveScript(string url)
        {
            var result = await _jsRuntime.InvokeAsync<bool>("ScriptLoader.removeScript", url);
            if (result) _loadedScripts.Remove(url);

            return result;
        }

        /// <summary>
        /// Loads a CSS stylesheet
        /// </summary>
        public async Task LoadStyleSheet(string cssPath)
        {
            // ✅ FIX: Simplified - no environment checking needed
            await _jsRuntime.InvokeVoidAsync("eval",
                $@"var link=document.createElement('link');
               link.rel='stylesheet';
               link.href='{cssPath}';
               document.head.appendChild(link)");
        }

        /// <summary>
        /// Loads multiple CSS stylesheets
        /// </summary>
        /// <param name="cssPaths">Array of stylesheet paths</param>
        public async Task LoadStyleSheets(IEnumerable<string> cssPaths)
        {
            foreach (var path in cssPaths)
            {
                // ✅ FIX: Simplified - no environment checking needed
                await _jsRuntime.InvokeVoidAsync("eval",
                $@"var link=document.createElement('link');
                   link.rel='stylesheet';
                   link.href='{path}';
                   document.head.appendChild(link)");
            }
        }

        /// <summary>
        /// Removes all previously loaded scripts.
        /// </summary>
        public async Task ClearAllScripts()
        {
            var scripts = _loadedScripts.ToList();
            foreach (var url in scripts)
            {
                await RemoveScript(url);
            }
        }

        /// <summary>
        /// Cleans up resources
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            if (_cssLoaderModule != null)
            {
                await _cssLoaderModule.DisposeAsync();
            }

            _loadedScripts.Clear();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Synchronous dispose for IDisposable compatibility
        /// </summary>
        public void Dispose()
        {
            DisposeAsync().GetAwaiter().GetResult();
        }
    }
}