using System;

namespace Dashboard.Constants
{
    public static class ApiEndpoints
    {
        public static class Auth
        {
            public const string Login = "api/v1/Auth/login";
            public const string Refresh = "api/v1/Auth/refresh";
        }

        public static class UserAuthentication
        {
            public const string UserInfo = "api/v1/UserAuthentication/userinfo";
        }

        public static class UserRegistration
        {
            public const string CreateVendor = "api/v1/UserRegistration/register-vendor";
        }

        public static class Token
        {
            public const string AccessToken = "api/v1/token/generate-access-token";
            public const string RegenerateRefreshToken = "api/v1/token/regenerate-refresh-token";
        }

        public static class Item
        {
            public const string Get = "api/v1/Item";
            public const string Save = "api/v1/Item/save";
            public const string UpdateStatus = "api/v1/Item/update/status";
            public const string Delete = "api/v1/Item/delete";
            public const string Search = "api/v1/Item/search";
            public const string SearchNewItemRequests = "api/v1/Item/search/requests";
        }

        public static class Attribute
        {
            public const string Get = "api/v1/Attribute";
            public const string Save = "api/v1/Attribute/save";
            public const string Delete = "api/v1/Attribute/delete";
            public const string Search = "api/v1/Attribute/search";
        }

        public static class Category
        {
            public const string Get = "api/v1/Category";
            public const string GetFinalCategories = "api/v1/Category/final-categories";
            public const string Save = "api/v1/Category/save";
            public const string ChangeTreeViewSerials = "api/v1/Category/changeTreeViewSerials";
            public const string Delete = "api/v1/Category/delete";
            public const string Search = "api/v1/Category/search";
        }

        public static class ItemCondition
        {
            public const string Get = "api/v1/item-conditions";
            public const string Save = "api/v1/item-conditions/save";
            public const string Delete = "api/v1/item-conditions/delete";
            public const string Search = "api/v1/item-conditions/search";
        }


        public static class Order
        {
            // ============================================
            // NEW ADMIN ENDPOINTS
            // ============================================

            /// <summary>
            /// Search orders with filtering and pagination
            /// GET /api/v1/admin/orders/search
            /// </summary>
            public const string Search = "api/v1/admin/orders/search";

            /// <summary>
            /// Get order details by ID
            /// GET /api/v1/admin/orders/{orderId}
            /// </summary>
            public const string GetById = "api/v1/admin/orders";

            /// <summary>
            /// Change order status
            /// POST /api/v1/admin/orders/{orderId}/change-status
            /// </summary>
            public const string ChangeStatus = "api/v1/admin/orders";

            /// <summary>
            /// Update order details
            /// PUT /api/v1/admin/orders/{orderId}
            /// </summary>
            public const string Update = "api/v1/admin/orders";

            /// <summary>
            /// Get today's orders count
            /// GET /api/v1/admin/orders/statistics/today-count
            /// </summary>
            public const string TodayCount = "api/v1/admin/orders/statistics/today-count";

            /// <summary>
            /// Update shipment status for a specific order shipment
            /// PUT /api/v1/admin/orders/{orderId}/shipments/{shipmentId}/status
            /// </summary>
            public static string ShipmentStatus(Guid orderId, Guid shipmentId) =>
                $"api/v1/admin/orders/{orderId}/shipments/{shipmentId}/status";
        }

        public static class WithdrawalMethod
        {
            public const string Get = "api/v1/WithdrawalMethod";
            public const string Save = "api/v1/WithdrawalMethod/save";
            public const string Delete = "api/v1/WithdrawalMethod/delete";
            public const string Search = "api/v1/WithdrawalMethod/search";
        }

        public static class Refund
        {
            public const string Get = "api/v1/refunds";
            public const string Update = "api/v1/refunds/update";
            public const string ChangeRefundStatus = "api/v1/refunds/changeRefundStatus";
            public const string Delete = "api/v1/refunds/delete";
            public const string Search = "api/v1/refunds/search";

			private const string Base = "api/v1/Refund";

			// Get endpoints
			public const string search = $"{Base}/search";
			public static string GetById(Guid id) => $"{Base}/{id}";
			public static string GetByOrderId(Guid orderId) => $"{Base}/order/{orderId}";

			// Update endpoint
			public const string ChangeStatus = $"{Base}/changeRefundStatus";

			// Optional: Statistics endpoint (if you want to add dashboard stats later)
			public const string GetStatistics = $"{Base}/statistics";
			public const string GetPendingCount = $"{Base}/pending-count";
		}

        public static class Unit
        {
            public const string Get = "api/v1/Unit";
            public const string Save = "api/v1/Unit/save";
            public const string Delete = "api/v1/Unit/delete";
            public const string Search = "api/v1/Unit/search";
        }

        public static class Country
        {
            public const string Get = "api/v1/Country";
            public const string Save = "api/v1/Country/save";
            public const string Delete = "api/v1/Country/delete";
            public const string Search = "api/v1/Country/search";
        }

        public static class State
        {
            public const string Get = "api/v1/State";
            public const string Save = "api/v1/State/save";
            public const string Delete = "api/v1/State/delete";
            public const string Search = "api/v1/State/search";
        }

        public static class City
        {
            public const string Get = "api/v1/City";
            public const string Save = "api/v1/City/save";
            public const string Delete = "api/v1/City/delete";
            public const string Search = "api/v1/City/search";
        }


        public static class PaymentMethod
        {
            public const string GetAll = "api/v1/PaymentMethod/all";
            public const string GetActive = "api/v1/PaymentMethod";
            public const string GetById = "api/v1/PaymentMethod/{0}";
            public const string Save = "api/v1/PaymentMethod/save";
            public const string Delete = "api/v1/PaymentMethod/delete";
            public const string Search = "api/v1/PaymentMethod/search";
        }

        // Add this new class
        public static class PaymentGatewayMethod
        {
            public const string GetAll = "api/v1/PaymentGatewayMethod";
            public const string GetById = "api/v1/PaymentGatewayMethod";
        }
        public static class UserPaymentMethod
        {
            public const string Get = "api/v1/UserPaymentMethod";
            public const string GetUserPaymentMethods = "api/v1/UserPaymentMethod/GetUserPaymentMethods";
            public const string Save = "api/v1/UserPaymentMethod/save";
            public const string Delete = "api/v1/UserPaymentMethod/delete";
        }

        public static class Field
        {
            public const string Get = "api/v1/Field";
            public const string Save = "api/v1/Field/save";
            public const string Delete = "api/v1/Field/delete";
            public const string Search = "api/v1/Field/search";
            public const string GetFieldsByPaymentMethod = "api/v1/Field/GetFieldsByPaymentMethod";
        }

        public static class UserField
        {
            public const string Get = "api/v1/UserField";
            public const string Save = "api/v1/UserField/save";
            public const string Delete = "api/v1/UserField/delete";
        }

        public static class Admin
        {
            public const string Get = "api/v1/Admin";
            public const string Create = "api/v1/Admin";
            public const string Update = "api/v1/Admin";
            public const string Delete = "api/v1/Admin/delete";
            public const string Search = "api/v1/Admin/search";
        }

        public static class Vendor
        {
            public const string Get = "api/v1/VendorManagement";
            public const string Update = "api/v1/VendorManagement/update";
            public const string Delete = "api/v1/VendorManagement/delete";

            public const string Search = "api/v1/VendorManagement/search";
            public const string FindById = "api/v1/VendorManagement";
            public const string GetForSelect = "api/v1/VendorManagement/forSelect";
            public const string UpdateVendorStatus = "api/v1/VendorManagement/update-vendor-status";
            public const string UpdateUserStatus = "api/v1/VendorManagement/update-user-status";
            public const string GetUserStatus = "api/v1/VendorManagement/getStatus";
            public const string GetVendorInfo = "api/v1/VendorManagement/VendorInfo";
        }

        public static class VendorRegistration
        {
            public const string Register = "api/v1/VendorRegistration";
            public const string GetSponsor = "api/v1/VendorRegistration/getSponsor";
            public const string IsValidLink = "api/v1/VendorRegistration/IsValidLink";
        }

        public static class VendorBusinessPoints
        {
            public const string Get = "api/v1/VendorBusinessPoints";
            public const string GetDetaild = "api/v1/VendorBusinessPoints/details";
            public const string WithdrawPoints = "api/v1/VendorBusinessPoints/WithdrawPoints";
        }

        public static class Customer
        {
            public const string Get = "api/v1/Customer";
            public const string Create = "api/v1/Customer";
            //public const string Update = "api/v1/Customer";
            public const string SearchSearchWallet = "api/v1/CustomerWalletTransaction/SearchWalletTransactions";

            public const string Delete = "api/v1/Customer/delete";
            public const string Save = "api/v1/Customer/Save";
            public const string Register = "api/v1/UserRegistration/register-customer";
            public const string Update = "api/v1/UserRegistration/update-customer";
            public const string Search = "api/v1/Customer/search";
            public const string FindById = "api/v1/Customer";
            public const string GetForSelect = "api/v1/Customer/forSelect";
            public const string ChangeStatus = "api/v1/Customer/changeStatus";
            public const string GetUserStatus = "api/v1/Customer/getStatus";
            public const string GetVendorInfo = "api/v1/Customer/VendorInfo";
        }

        public static class CouponCode
        {
            public const string Get = "api/v1/CouponCode";
            public const string Save = "api/v1/CouponCode/save";
            public const string Delete = "api/v1/CouponCode/delete";
            public const string Search = "api/v1/CouponCode/search";
            public const string GetByVendor = "api/v1/CouponCode/vendor/{vendorId}";
        }

        public static class VendorPromoCodeParticipation
        {
            public const string AdminList = "api/v1/merchandising/vendorpromocodeparticipation/admin/list";
        }

        public static class ShippingCompany
        {
            public const string Get = "api/v1/ShippingCompany";
            public const string Save = "api/v1/ShippingCompany/save";
            public const string Delete = "api/v1/ShippingCompany/delete";
            public const string Search = "api/v1/ShippingCompany/search";
        }

        public static class Shipment
        {
            public const string GetOrderShipments = "api/v1/Shipment/order";
            public const string Track = "api/v1/Shipment/track";
            public const string GetById = "api/v1/Shipment";
            public const string UpdateStatus = "api/v1/Shipment/{0}/status";
            public const string AssignTracking = "api/v1/Shipment/{0}/tracking";
        }

        public static class UserNotification
        {
            public const string Get = "api/v1/UserNotifications";
            public const string Save = "api/v1/UserNotifications/save";
            public const string Search = "api/v1/UserNotifications/search";
            public const string MarkAsRead = "api/v1/UserNotifications/markAsRead";
        }


        public static class Currency
        {
            public const string Get = "api/v1/Currency";
            public const string Save = "api/v1/Currency";
            public const string Delete = "api/v1/Currency/delete";
            public const string Search = "api/v1/Currency/search";
            public const string SetBase = "api/v1/Currency/set-base";
            public const string UpdateRates = "api/v1/Currency/update-rates";
            public const string GetBase = "api/v1/Currency/base";
            public const string GetActive = "api/v1/Currency/active";
        }

        // Add these to the existing ApiEndpoints class
        public static class Setting
        {
            public const string Get = "api/v1/Setting";
            public const string MainBanner = "api/v1/Setting/mainBanner";
            public const string WithdrawelFeePersentage = "api/v1/Setting/withdrawelFeePersentage";
            public const string Update = "api/v1/Setting/update";
        }
        public static class SystemSettings
        {
            private const string Base = "api/v1/SystemSettings";

            // Get by type
            public static string GetDecimal(int key) => $"{Base}/decimal/{key}";
            public static string GetInt(int key) => $"{Base}/int/{key}";
            public static string GetBool(int key) => $"{Base}/bool/{key}";
            public static string GetString(int key) => $"{Base}/string/{key}";
            public static string GetDateTime(int key) => $"{Base}/datetime/{key}";

            // Update
            public const string Update = $"{Base}/update";
            public const string UpdateBatch = $"{Base}/update-batch";

            // Specific business settings
            public const string TaxRate = $"{Base}/tax-rate";
            public const string FreeShippingThreshold = $"{Base}/free-shipping-threshold";
            public const string CashOnDeliveryEnabled = $"{Base}/cash-on-delivery-enabled";
            public const string MaintenanceMode = $"{Base}/maintenance-mode";
            public const string MinimumOrderAmount = $"{Base}/minimum-order-amount";
        }

		
		public static class DevelopmentSettings
        {
            public const string Get = "api/v1/development-settings";
            public const string IsMultiVendorEnabled = "api/v1/development-settings/multi-vendor-enabled";
        }
        public static class Page
        {
            public const string Get = "api/v1/PageStatic";
            public const string GetById = "api/v1/PageStatic";
            public const string GetBySlug = "api/v1/PageStatic/by-slug";
            public const string GetByTitle = "api/v1/PageStatic/by-title";
            public const string Search = "api/v1/PageStatic/search";
            public const string Save = "api/v1/PageStatic/save";
            public const string Delete = "api/v1/PageStatic/delete";
            public const string ToggleStatus = "api/v1/PageStatic/toggle-status";
        }

        // Brand endpoints
        public static class Brand
        {
            public const string Get = "api/v1/Brand";
            public const string GetFavorites = "api/v1/Brand/favorites";
            public const string Search = "api/v1/Brand/search";
            public const string Save = "api/v1/Brand/save";
            public const string Delete = "api/v1/Brand/delete";
            public const string MarkAsFavorite = "api/v1/Brand/mark-favorite";
        }

        // Testimonial endpoints
        public static class Testimonial
        {
            public const string Get = "api/v1/Testimonial";
            public const string Search = "api/v1/Testimonial/search";
            public const string Save = "api/v1/Testimonial/save";
            public const string Delete = "api/v1/Testimonial/delete";
        }

        // Warehouse endpoints
        public static class Warehouse
        {
            public const string Get = "api/v1/Warehouse";
            public const string GetActive = "api/v1/Warehouse/active";
            public const string Search = "api/v1/Warehouse/search";
            public const string market = "api/v1/Warehouse/market";
            public const string SearchVendor = "api/v1/Warehouse/search-vendor";
            public const string withUsers = "api/v1/Warehouse/vendors-select";
            public const string Save = "api/v1/Warehouse/save";
            public const string Delete = "api/v1/Warehouse/delete";
            public const string ToggleStatus = "api/v1/Warehouse/toggle-status";
            public const string GetVendors = "api/v1/Warehouse/vendors";
            public const string IsMultiVendorEnabled = "api/v1/development-settings/multi-vendor-enabled";
        }

        // Vendor Warehouse endpoints
        public static class VendorWarehouse
        {
            public const string GetMarketWarehouse = "api/v1/vendors/market/warehouse";
        }

        // Inventory Movement endpoints
        public static class InventoryMovement
        {
            public const string Get = "api/v1/InventoryMovement";
            public const string GetById = "api/v1/InventoryMovement";
            public const string GetByDocument = "api/v1/InventoryMovement/by-document";
            public const string Search = "api/v1/InventoryMovement/search";
            public const string GenerateDocumentNumber = "api/v1/InventoryMovement/generate-document-number";
            public const string Save = "api/v1/InventoryMovement/save";
            public const string Delete = "api/v1/InventoryMovement/delete";
        }

        // Return Movement endpoints
        public static class ReturnMovement
        {
            public const string Get = "api/v1/ReturnMovement";
            public const string GetById = "api/v1/ReturnMovement";
            public const string Search = "api/v1/ReturnMovement/search";
            public const string GenerateDocumentNumber = "api/v1/ReturnMovement/generate-document-number";
            public const string Save = "api/v1/ReturnMovement/save";
            public const string UpdateStatus = "api/v1/ReturnMovement/update-status";
            public const string Delete = "api/v1/ReturnMovement/delete";
        }


        // Media Content endpoints
        public static class MediaContent
        {
            public const string Get = "api/v1/MediaContent";
            public const string GetByArea = "api/v1/MediaContent/by-area";
            public const string GetByAreaCode = "api/v1/MediaContent/by-area-code";
            public const string Search = "api/v1/MediaContent/search";
            public const string Save = "api/v1/MediaContent/save";
            public const string Delete = "api/v1/MediaContent/delete";
            public const string ToggleStatus = "api/v1/MediaContent/toggle-status";
            public const string UpdateDisplayOrder = "api/v1/MediaContent/update-display-order";
        }

        public static class AdminStatistics
        {
            public const string Get = "api/v1/AdminStatistics";
            public const string GetByDateRange = "api/v1/AdminStatistics/daterange";
            public const string GetSummary = "api/v1/AdminStatistics/summary";
        }

        public static class VendorStatistics
        {
            public const string Get = "api/v1/VendorStatistics";
            public const string GetByDateRange = "api/v1/VendorStatistics/dateRange";
        }

        // Campaign endpoints
        public static class Campaign
        {
            public const string Get = "api/v1/Campaign";
            public const string GetById = "api/v1/Campaign/{0}";
            public const string Search = "api/v1/Campaign/search";
            public const string Create = "api/v1/Campaign";
            public const string Update = "api/v1/Campaign/{0}";
            public const string Delete = "api/v1/Campaign/{0}";
            public const string Activate = "api/v1/Campaign/{0}/activate";
            public const string Deactivate = "api/v1/Campaign/{0}/deactivate";
            public const string Products = "api/v1/Campaign/{0}/products";
            public const string AddProduct = "api/v1/Campaign/products";
        }
        public static class ItemReview
        {
            private const string Base = "api/v1/ItemReview";

            public const string Get = Base;
            public const string Update = $"{Base}/update";
            public const string Delete = $"{Base}/delete";
            public const string Search = $"{Base}/search";
            public const string GetStats = $"{Base}/Item-review-summery";
            public const string ChangeStatus = $"{Base}/changeStatus";
        }

        public static class VendorReview
        {
            private const string BaseUrl = "api/v1/VendorReview";

            // Get Operations
            public static string GetById(Guid reviewId) => $"{BaseUrl}/{reviewId}";
            public static string GetByVendorId(Guid vendorId) => $"{BaseUrl}/vendorReviews-by/{vendorId}";
            public static string GetVendorReviews(Guid vendorId) => $"{BaseUrl}/vendor-by/{vendorId}";
            public static string GetCustomerReviews(Guid customerId) => $"{BaseUrl}/customer/{customerId}";
            public static string GetVerified(Guid vendorId) => $"{BaseUrl}/vendor/{vendorId}/verified";
            public static string GetNonVerified(Guid vendorId) => $"{BaseUrl}/vendor/{vendorId}/non-verified";

            // Search & Filter
            public static string Search => $"{BaseUrl}/search-admin-vendorreviews";

            // Statistics
            public static string GetStats(Guid vendorId) => $"{BaseUrl}/vendor-review-stats-by/{vendorId}";
            public static string GetAverageRating(Guid vendorId) => $"{BaseUrl}/average-rating-by/{vendorId}";
            public static string GetReviewCount(Guid vendorId) => $"{BaseUrl}/vendorReview-count/{vendorId}";

            // Admin Operations
            public static string GetPending => $"{BaseUrl}/pending";
            public static string Approve => $"{BaseUrl}/approve";
            public static string Reject => $"{BaseUrl}/reject";
            public static string Delete => $"{BaseUrl}/delete";
        }

        public static class HomePageSlider
        {
            private const string Base = "api/v1/HomePageSlider";

            // Single Operations
            public const string Get = Base;
            public const string GetById = $"{Base}";
            public const string GetAll = $"{Base}/all";
            public const string Create = $"{Base}/save";
            public const string Update = $"{Base}/save";
            public const string Delete = $"{Base}";
            public static string UpdateDisplayOrder(Guid sliderId) => $"{Base}/{sliderId}/display-order";
        }

        public static class ReviewReport
        {
            private const string Base = "api/v1/ReviewReport";
            public const string Submit = $"{Base}/Submit";
            public const string Get = Base;
            public const string Search = $"{Base}/Search";
            public const string GetByItemReviewId = $"{Base}/reports-by-Item";
            public const string Resolve = $"{Base}/resolve";
            public const string MarkAsFlagged = $"{Base}/MarkAsFlagged";
        }
        // Wallet endpoints
        public static class Wallet
        {
            public static class Customer
            {
                public const string Get = "api/v1/Wallet/customer";
                public const string GetByCustomerId = "api/v1/Wallet/customer/{0}";
                public const string MyWallet = "api/v1/Wallet/customer/my-wallet";
                public const string Create = "api/v1/Wallet/customer/{0}";
                public const string Balance = "api/v1/Wallet/customer/{0}/balance";
            }

            public static class Vendor
            {
                public const string Get = "api/v1/Wallet/vendor";
                public const string GetByVendorId = "api/v1/Wallet/vendor/{0}";
                public const string Create = "api/v1/Wallet/vendor/{0}";
            }

            public static class Transactions
            {
                public const string Get = "api/v1/Wallet/transactions";
                public const string Search = "api/v1/Wallet/transactions/search";
                public const string GetById = "api/v1/Wallet/transactions/{0}";
                public const string Approve = "api/v1/Wallet/transactions/{0}/approve";
                public const string Reject = "api/v1/Wallet/transactions/{0}/reject";
            }


            public const string Deposit = "api/v1/Wallet/deposit";
            public const string Withdrawal = "api/v1/Wallet/withdrawal";
            public const string Treasury = "api/v1/Wallet/treasury";
            public const string TreasuryUpdate = "api/v1/Wallet/treasury/update";
        }

        public static class AdminDashboard
        {
            private const string Base = "api/v1/admin/dashboard";
            public const string Summary = $"{Base}/summary";
            public static string SummaryByPeriod(string period) => $"{Base}/summary/{period}";
            public const string TopProducts = $"{Base}/top-products";
            public const string TopVendors = $"{Base}/top-vendors";
            public const string Statistics = $"{Base}/statistics";
        }
    }
}
