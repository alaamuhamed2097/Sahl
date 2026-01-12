using BL.Contracts.Service.Wallet.Customer;
using BL.Services.Wallet.Customer;

namespace Api.Extensions.Services
{
    public static class WalletServiceExtensions
    {
        public static IServiceCollection AddWalletServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Wallet Services
            services.AddScoped<ICustomerWalletService, CustomerWalletService>();
            services.AddScoped<ICustomerWalletTransactionService, CustomerWalletTransactionService>();
            services.AddScoped<IWalletSettingService, WalletSettingService>();

            return services;
        }
    }
}
