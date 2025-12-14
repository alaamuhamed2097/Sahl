// File: Extensions/LocalizationExtensions.cs
using Microsoft.AspNetCore.Localization;
using Resources;
using Resources.Enumerations;
using System.Globalization;

namespace Api.Extensions
{
    public static class LocalizationExtensions
    {
        public static IServiceCollection AddLocalizationConfiguration(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            return services;
        }

        public static IApplicationBuilder UseLocalizationConfiguration(this IApplicationBuilder app)
        {
            // Set default language
            ResourceManager.CurrentLanguage = Language.English;

            var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en-US"),
                new CultureInfo("ar-EG")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(
                    ResourceManager.GetCultureName(ResourceManager.CurrentLanguage)),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
                RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new CookieRequestCultureProvider(),
                    new AcceptLanguageHeaderRequestCultureProvider()
                }
            });

            app.Use(async (context, next) =>
            {
                var feature = context.Features.Get<IRequestCultureFeature>();
                var culture = feature?.RequestCulture.Culture.Name ?? "en-US";
                await next();
            });

            return app;
        }
    }
}