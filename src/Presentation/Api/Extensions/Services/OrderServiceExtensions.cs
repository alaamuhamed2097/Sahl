using BL.Contracts.Service.Customer.Wishlist;
using BL.Contracts.Service.Order.Cart;
using BL.Contracts.Service.Order.Checkout;
using BL.Contracts.Service.Order.OrderProcessing;
using BL.Contracts.Service.Order.Payment;
using BL.Services.Order.Cart;
using BL.Services.Order.Checkout;
using BL.Services.Order.OrderProcessing;
using BL.Services.Order.Payment;
using BL.Services.Customer.Wishlist;

namespace Api.Extensions.Services
{
    /// <summary>
    /// Extension methods for registering order-related services (cart, checkout, payment).
    /// </summary>
    public static class OrderServiceExtensions
    {
        /// <summary>
        /// Adds order processing, cart, checkout, and payment services.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
        public static IServiceCollection AddOrderServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Order Services
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderMangmentService, OrderManagementService>();
            services.AddScoped<ICustomerAddressService, CustomerAddressService>();

            services.AddScoped<IWishlistService, WishlistService>();

            // Payment Services
            services.AddScoped<IPaymentService, PaymentService>();

            services.AddScoped<ICheckoutService, CheckoutService>();

            services.AddScoped<IOrderCreationService, OrderCreationService>();

            services.AddScoped<IOrderPaymentProcessor, OrderPaymentProcessor>();

            return services;
        }
    }
}
