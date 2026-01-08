using BL.Contracts.Service.WithdrawalMethod;
using BL.Services.WithdrawalMethod;

namespace Api.Extensions.Services
{
    public static class WithdrawalMethodServicesExtensions
    {
        public static IServiceCollection AddWithdrawalMethodServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Withdrawal Method Services
            services.AddScoped<IWithdrawalMethodService, WithdrawalMethodService>();
            services.AddScoped<IUserWithdrawalMethodService, UserWithdrawalMethodService>();

            return services;
        }
    }
}
