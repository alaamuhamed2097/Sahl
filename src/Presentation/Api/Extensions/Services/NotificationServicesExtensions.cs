using Bl.Contracts.GeneralService.Notification;
using Bl.GeneralService;
using Bl.GeneralService.Notification;
using BL.Contracts.GeneralService;
using BL.Contracts.GeneralService.Notification;
using BL.GeneralService;
using BL.GeneralService.Notification;

namespace Api.Extensions.Services
{
    /// <summary>
    /// Extension methods for registering notification and communication services.
    /// </summary>
    public static class NotificationServicesExtensions
    {
        /// <summary>
        /// Adds notification, email, SMS, and SignalR services.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
        public static IServiceCollection AddNotificationServices(this IServiceCollection services)
        {
            // Communication Services
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IEmailProviderService, MailGunProviderService>();
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<ISmsProviderService, SmsProviderService>();
            services.AddScoped<ISignalRProviderService, SignalRProviderService>();
            services.AddScoped<ISignalRNotificationService, SignalRNotificationService>();

            // Notification Services
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IUserNotificationService, UserNotificationService>();

            // Template and Verification Services
            services.AddScoped<ITemplateService, TemplateService>();
            services.AddScoped<IVerificationCodeService, VerificationCodeService>();

            return services;
        }
    }
}
