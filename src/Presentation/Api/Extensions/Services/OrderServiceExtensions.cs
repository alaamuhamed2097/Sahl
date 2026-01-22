using BL.Contracts.Service.Customer.Wishlist;
using BL.Contracts.Service.Order.Cart;
using BL.Contracts.Service.Order.Checkout;
using BL.Contracts.Service.Order.Fulfillment;
using BL.Contracts.Service.Order.OrderProcessing;
using BL.Contracts.Service.Order.Payment;
using BL.Contracts.Service.VendorWarehouse;
using BL.Services.Customer.Wishlist;
using BL.Services.Order.Cart;
using BL.Services.Order.Checkout;
using BL.Services.Order.Fulfillment;
using BL.Services.Order.OrderProcessing;
using BL.Services.Order.Payment;
using BL.Services.Warehouse;

namespace Api.Extensions.Services
{
    public static class OrderServiceExtensions
    {
        public static IServiceCollection AddOrderServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Order Services
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICustomerAddressService, CustomerAddressService>();
            services.AddScoped<IVendorWarehouseService, VendorWarehouseService>();
            services.AddScoped<IAdminOrderService, AdminOrderService>();
            services.AddScoped<ICustomerOrderService, CustomerOrderService>();
            services.AddScoped<IVendorOrderService, VendorOrderService>();
            services.AddScoped<ICustomerAddressService, CustomerAddressService>();

            services.AddScoped<IWishlistService, WishlistService>();

            // Payment Services
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IPaymentMethodService, PaymentMethodService>();

            services.AddScoped<ICheckoutService, CheckoutService>();

            services.AddScoped<IOrderCreationService, OrderCreationService>();

            services.AddScoped<IOrderPaymentProcessor, OrderPaymentProcessor>();
            services.AddScoped<IShipmentService, ShipmentService>();
            services.AddScoped<IRefundService, RefundService>();

            return services;
        }
    }
}
