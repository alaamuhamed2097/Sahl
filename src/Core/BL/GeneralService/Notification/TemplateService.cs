using Bl.Contracts.GeneralService.Notification;
using Common.Enumerations.Notification;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Shared.GeneralModels.Parameters.Notification;
using System.Text;

namespace Bl.GeneralService.Notification;

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
        //NotificationType.BinaryCommission => language == "ar" ? "المكافأة الثنائية" : "Binary Commission",
        //NotificationType.SystemSettingsUpdated => language == "ar" ? "تم تحديث إعدادات النظام" : "System Settings Updated",

        _ => language == "ar" ? "إشعار عام" : type.ToString().Replace('_', ' ')
    };

    private string GetTitle(NotificationType type, string language) => GetSubject(type, language);
}
