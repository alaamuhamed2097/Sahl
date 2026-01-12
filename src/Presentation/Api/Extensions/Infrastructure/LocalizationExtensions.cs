using Microsoft.AspNetCore.Localization;
using Resources;
using Resources.Enumerations;
using System.Globalization;

namespace Api.Extensions.Infrastructure
{
    /// <summary>
    /// Extension methods for configuring localization and internationalization (i18n).
    /// </summary>
    public static class LocalizationExtensions
    {
        /// <summary>
        /// Adds localization services to the DI container.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
        public static IServiceCollection AddLocalizationConfiguration(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            return services;
        }

        /// <summary>
        /// Configures the request localization middleware with supported cultures.
        /// </summary>
        /// <param name="app">The IApplicationBuilder instance.</param>
        /// <returns>The IApplicationBuilder for chaining.</returns>
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
