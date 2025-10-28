namespace BL.GeneralService
{

    //public class ResourceService : IResourceService
    //{
    //    private static readonly Dictionary<string, Func<string, Language, string>> _resourceMap =
    //        new Dictionary<string, Func<string, Language, string>>(StringComparer.OrdinalIgnoreCase)
    //        {
    //            ["ActionsResources"] = ActionsResources.GetResource,
    //            ["FormResources"] = FormResources.GetResource,
    //            ["GeneralResources"] = GeneralResources.GetResource,
    //            ["GeneralUIResources"] = GeneralUIResources.GetResource,
    //            ["NotifiAndAlertsResources"] = NotifiAndAlertsResources.GetResource,
    //            ["UserResources"] = UserResources.GetResource,
    //            ["ValidationResources"] = ValidationResources.GetResource,
    //            //["ECommerceResources"] = ECommerceResources.GetResource
    //        };

    //    public string GetResource(string resourceKey, string? languageHeader = null)
    //    {
    //        var language = GetLanguageFromHeader(languageHeader);

    //        foreach (var mapping in _resourceMap)
    //        {
    //            if (resourceKey == mapping.Key)
    //                return mapping.Value(resourceKey, language);
    //        }

    //        return resourceKey;
    //    }

    //    private Language GetLanguageFromHeader(string? languageHeader)
    //    {
    //        if (string.IsNullOrEmpty(languageHeader))
    //            return Language.Arabic; // Default

    //        return languageHeader.StartsWith("ar", StringComparison.OrdinalIgnoreCase)
    //            ? Language.Arabic
    //            : Language.English;
    //    }
    //}
}
