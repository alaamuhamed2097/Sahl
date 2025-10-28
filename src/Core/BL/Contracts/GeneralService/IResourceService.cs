namespace BL.Contracts.GeneralService
{
    public interface IResourceService
    {
        string GetResource(string resourceKey, string? languageHeader = null);
    }
}
