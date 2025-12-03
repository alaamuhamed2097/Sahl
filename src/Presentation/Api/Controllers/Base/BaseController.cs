using Microsoft.AspNetCore.Mvc;
using Resources;
using Resources.Enumerations;
using Shared.GeneralModels;
using System.Globalization;
using System.Security.Claims;
using Asp.Versioning;

namespace Api.Controllers.Base
{
    public class BaseController : ControllerBase
    {
        protected readonly Serilog.ILogger _logger;
        public BaseController(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        protected string? RoleName => User.FindFirst(ClaimTypes.Role)?.Value;
        protected string? UserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        protected Guid GuidUserId =>
       Guid.TryParse(UserId, out var guid) ? guid : Guid.NewGuid();

        protected string GetResource<T>(string resourceKey)
        {
            var userLanguage = Request.Headers["x-language"].ToString().ToLower();
            if (string.IsNullOrEmpty(userLanguage))
            {
                userLanguage = "ar-EG";
            }

            var language = userLanguage.StartsWith("ar", StringComparison.OrdinalIgnoreCase)
                ? Language.Arabic
            : Language.English;

            var cultureName = ResourceManager.GetCultureName(language) ?? "ar-EG";
            var culture = CultureInfo.GetCultureInfo(cultureName);

            return nameof(T) switch
            {
                "ActionsResources" => ActionsResources.ResourceManager.GetString(resourceKey, culture),
                "FormResources" => FormResources.ResourceManager.GetString(resourceKey, culture),
                "GeneralResources" => GeneralResources.ResourceManager.GetString(resourceKey, culture),
                "NotifiAndAlertsResources" => NotifiAndAlertsResources.ResourceManager.GetString(resourceKey, culture),
                "UserResources" => UserResources.ResourceManager.GetString(resourceKey, culture),
                _ => resourceKey
            };
        }

        /// <summary>
        /// Gets the API version from the route.
        /// </summary>
        protected string GetApiVersion()
        {
            var version = HttpContext.GetRequestedApiVersion();
            return version?.ToString() ?? "1.0";
        }
    }
}
