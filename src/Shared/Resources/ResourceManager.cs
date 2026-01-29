using Resources.Enumerations;
using Resources.Services;
using System.Globalization;

namespace Resources
{
    public static class ResourceManager
    {
        private static Language _currentLanguage;
        private static LanguageService _languageService;

        public static void SetLanguageService(LanguageService languageService)
        {
            _languageService = languageService;
        }

        // Property to get or set the current culture
        public static Language CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                _currentLanguage = value;
                SetCultureForResources(_currentLanguage);

                _languageService?.NotifyLanguageChanged();
            }
        }

        public static string GetCultureName(Language language)
        {
            switch (language)
            {
                case Language.English:
                    return "en-US";
                default:
                    return "ar-EG";
            }
        }

        // Method to set the culture for all resources
        private static void SetCultureForResources(Language language)
        {
            var cultureName = GetCultureName(language);
            var culture = CultureInfo.GetCultureInfo(cultureName);
            ActionsResources.Culture = culture;
            FormResources.Culture = culture;
            GeneralResources.Culture = culture;
            UserResources.Culture = culture;
            ValidationResources.Culture = culture;
            NotifiAndAlertsResources.Culture = culture;
            ECommerceResources.Culture = culture;
            BrandResources.Culture = culture;
            CurrencyResources.Culture = culture;
            CouponCodeResources.Culture = culture;
            MerchandisingResources.Culture = culture;
            OrderResources.Culture = culture;
            ReviewResources.Culture = culture;
        }

        public static void ChangeLanguage()
        {
            var newLanguage = CurrentLanguage == Language.Arabic ?
                Language.English : Language.Arabic;
            CurrentLanguage = newLanguage;
        }
    }
}