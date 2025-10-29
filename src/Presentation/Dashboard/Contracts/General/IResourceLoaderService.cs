namespace Dashboard.Contracts.General
{
    public interface IResourceLoaderService
    {
        Task ClearAllScripts();
        void Dispose();
        Task LoadScript(string url);
        Task LoadScriptsParallel(params string[] urls);
        Task LoadScriptsSequential(params string[] urls);
        Task LoadStyleSheet(string cssPath);
        Task LoadStyleSheets(IEnumerable<string> cssPaths);
        Task<bool> RemoveScript(string url);
    }
}