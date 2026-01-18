using Common.Filters;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Resources.Enumerations;
using Shared.GeneralModels;
using System.Globalization;
using System.Security.Claims;

namespace Api.Controllers.v1.Base
{
    public class BaseController : ControllerBase
    {
        protected string? RoleName => User.FindFirst(ClaimTypes.Role)?.Value;
        protected string? UserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        protected Guid GuidUserId =>
       Guid.TryParse(UserId, out var guid) ? guid : Guid.Empty;

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
        /// <summary>
        /// Validates and normalizes base search criteria model
        /// </summary>
        protected static void ValidateBaseSearchCriteriaModel(BaseSearchCriteriaModel filter)
        {
            // Validate pagination
            filter.PageNumber = Math.Max(filter.PageNumber, 1);
            filter.PageSize = Math.Clamp(filter.PageSize, 1, 100);

            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                filter.SortBy = filter.SortBy.ToLower().Trim();
            }
        }
        /// <summary>
        /// Creates a standardized success response
        /// </summary>
        protected ResponseModel<T> CreateSuccessResponse<T>(T data, string message)
        {
            return new ResponseModel<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        /// <summary>
        /// Creates a standardized error response
        /// </summary>
        protected ResponseModel<T> CreateErrorResponse<T>(string message)
        {
            return new ResponseModel<T>
            {
                Success = false,
                Message = message
            };
        }
    }
}
