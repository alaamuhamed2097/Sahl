using Resources.Enumerations;
using System.Globalization;

namespace Resources.Providers
{
    /// <summary>
    /// Base class for all resource providers to inherit common functionality
    /// </summary>
    public abstract class ResourceProvider : IResourceProvider
    {
        // Shared CultureInfo instances to avoid recreating them
        protected static readonly CultureInfo ArabicCulture = new CultureInfo("ar-EG");
        protected static readonly CultureInfo EnglishCulture = new CultureInfo("en-US");

        // Template method to be overridden by derived classes
        protected static string GetResourceInternal(System.Resources.ResourceManager resourceManager, string key, Language language)
        {
            var culture = language == Language.Arabic ? ArabicCulture : EnglishCulture;
            return resourceManager.GetString(key, culture) ?? key;
        }
    }
}
