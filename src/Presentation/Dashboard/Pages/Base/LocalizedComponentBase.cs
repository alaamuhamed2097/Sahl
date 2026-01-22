using Microsoft.AspNetCore.Components;
using Resources.Services;

namespace Dashboard.Pages.Base
{
    /// <summary>
    /// Base class for components that need to respond to language changes.
    /// This is the standard pattern used in Blazor for localization.
    /// </summary>
    public abstract class LocalizedComponentBase : ComponentBase, IDisposable
    {
        [Inject]
        protected LanguageService LanguageService { get; set; } = null!;

        private bool _disposed;

        protected override void OnInitialized()
        {
            // Subscribe to language changes
            LanguageService.OnLanguageChanged += OnLanguageChangedHandler;
            base.OnInitialized();
        }

        /// <summary>
        /// Called when the language changes. Default behavior is to re-render.
        /// Override this method if you need custom behavior.
        /// </summary>
        protected virtual void OnLanguageChanged()
        {
            // Default: just re-render the component
            StateHasChanged();
        }

        private void OnLanguageChangedHandler()
        {
            // Use InvokeAsync to ensure thread safety
            InvokeAsync(() =>
            {
                OnLanguageChanged();
            });
        }

        public virtual void Dispose()
        {
            if (_disposed) return;

            // Unsubscribe to prevent memory leaks
            LanguageService.OnLanguageChanged -= OnLanguageChangedHandler;
            _disposed = true;
        }
    }
}