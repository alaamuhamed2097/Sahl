using BL.Contracts.Service.Wallet.Customer;
using BL.Services.Wallet.Customer;

namespace Api.Extensions.Services
{
    /// <summary>
    /// Extension methods for registering wallet and payment services.
    /// </summary>
    public static class WalletServiceExtensions
    {
        /// <summary>
        /// Adds customer wallet, transactions, and wallet settings services.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
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
