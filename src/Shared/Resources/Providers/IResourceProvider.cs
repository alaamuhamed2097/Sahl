using Resources.Enumerations;

namespace Resources.Providers
{
    /// <summary>
    /// Interface for resource classes to implement for uniform access
    /// </summary>
    public interface IResourceProvider
    {
        static ResourceManager ResourceManager { get; }
        static string GetResource(string key, Language language) => throw new NotImplementedException();
    }
}
