using Bl.Contracts.GeneralService.Notification;
using Common.Enumerations.Notification;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Shared.GeneralModels.Parameters.Notification;
using System.Text;

namespace Bl.GeneralService.Notification
{
    public class TemplateService : ITemplateService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger _logger;
        private readonly string _templatesRootPath;

        public TemplateService(IWebHostEnvironment environment, ILogger logger)
        {
            _environment = environment;
            _logger = logger;
            _templatesRootPath = Path.Combine(_environment.WebRootPath, "NotificationTemplates");
        }

        public async Task<string> LoadTemplateAsync(string templateName, NotificationChannel channel, NotificationType type, string language = "ar")
        {
            // Resolve file name (fallback: NotificationType in snake_case)
            var baseName = !string.IsNullOrEmpty(templateName)
                ? templateName
                : ToSnakeCase(type.ToString());

            var fileExtension = GetFileExtension(channel);
            var cultureFileName = $"{baseName}.{language}.{fileExtension}";
            var fallbackFileName = $"{baseName}.{fileExtension}";

            var cultureFilePath = Path.Combine(_templatesRootPath, channel.ToString(), cultureFileName);
            var fallbackFilePath = Path.Combine(_templatesRootPath, channel.ToString(), fallbackFileName);

            // Try culture-specific version
            if (File.Exists(cultureFilePath))
            {
                return await File.ReadAllTextAsync(cultureFilePath);
            }

            // Fall back to default file without language
            if (File.Exists(fallbackFilePath))
            {
                return await File.ReadAllTextAsync(fallbackFilePath);
            }

            // Neither exists – throw
            _logger.Error($"Culture-specific template not found: Using fallback: {fallbackFilePath}");
            throw new FileNotFoundException($"Notification template not found: {cultureFilePath} or fallback: {fallbackFilePath}");
        }

        public string ProcessTemplate(string template, Dictionary<string, string> parameters)
        {
            if (parameters == null || parameters.Count == 0)
                return template;

            foreach (var (key, value) in parameters)
            {
                var placeholder = $"{{{key}}}";
                template = template.Replace(placeholder, value ?? string.Empty);
            }

            return template;
        }

        public async Task<NotificationTemplate> GetTemplateAsync(
            string templateName,
            NotificationChannel channel,
            NotificationType type,
            string language = "ar")
        {
            var content = await LoadTemplateAsync(templateName, channel, type, language);

            return new NotificationTemplate
            {
                Name = templateName ?? type.ToString(),
                Channel = channel,
                Type = type,
                Content = content,
                Subject = GetSubject(type, language),
                Title = GetTitle(type, language)
            };
        }

        private string GetFileExtension(NotificationChannel channel) => channel switch
        {
            NotificationChannel.Email => "html",
            _ => "txt"
        };

        private string ToSnakeCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            var sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                var c = input[i];
                if (char.IsUpper(c) && i > 0)
                    sb.Append('_');
                sb.Append(char.ToLowerInvariant(c));
            }
            return sb.ToString();
        }

        private string GetSubject(NotificationType type, string language = "ar") => type switch
        {
            NotificationType.BinaryCommission => language == "ar" ? "المكافأة الثنائية" : "Binary Commission",
            NotificationType.BinaryCommissionStatus => language == "ar" ? "حالة المكافأة الثنائية" : "Binary Commission Status",
            NotificationType.DirectCommission => language == "ar" ? "العمولة المباشرة" : "Direct Commission",
            NotificationType.LevelCommission => language == "ar" ? "العمولة على المستويات" : "Level Commission",
            NotificationType.LevelCommissionStatus => language == "ar" ? "حالة العمولة على المستويات" : "Level Commission Status",
            NotificationType.RecruitmentCommission => language == "ar" ? "عمولة التوظيف" : "Recruitment Commission",
            NotificationType.FirstAccountTypeAchieved => language == "ar" ? "مبروك! تم تفعيل أول حساب في شبكتك" : "Congratulations! First account activated in your network",
            NotificationType.UpgradeAccountType => language == "ar" ? "مبروك الترقية!" : "Congratulations on Your Upgrade!",
            NotificationType.MarketerAccountActivated => language == "ar" ? "تم تفعيل حساب المسوق" : "Marketer Account Activated",
            NotificationType.NewDirectMarketerRegistered => language == "ar" ? "مبروك! مسوق جديد في فريقك المباشر" : "Congratulations! New direct marketer",
            NotificationType.NewIndirectMarketerJoined => language == "ar" ? "تم توسيع فريقك!" : "Your team expanded!",
            NotificationType.NewOrderCreated => language == "ar" ? "تم إنشاء طلب جديد" : "New Order Created",
            NotificationType.OrderStatusChanged => language == "ar" ? "تغير حالة الطلب" : "Order Status Changed",
            NotificationType.RankBonus => language == "ar" ? "مكافأة الرتبة" : "Rank Bonus",
            NotificationType.RankBonusStatus => language == "ar" ? "حالة مكافأة الرتبة" : "Rank Bonus Status",
            NotificationType.RankPromotion => language == "ar" ? "ترقية في الرتبة" : "Rank Promotion",
            NotificationType.PvPointsAdded => language == "ar" ? "تمت إضافة نقاط PV جديدة" : "New PV points added ",
            NotificationType.TeamPvPoints => language == "ar" ? "نقاط فريق PV" : "Team PV Points",
            NotificationType.SystemSettingsUpdated => language == "ar" ? "تم تحديث إعدادات النظام" : "System Settings Updated",

            _ => language == "ar" ? "إشعار عام" : type.ToString().Replace('_', ' ')
        };

        private string GetTitle(NotificationType type, string language) => GetSubject(type, language);
    }
}
