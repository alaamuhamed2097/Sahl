namespace Dashboard.Constants
{
    public static class ApiEndpoints
    {
        public static class Auth
        {
            public const string Login = "api/Auth/login";
        }

        public static class UserAuthentication
        {
            public const string UserInfo = "api/UserAuthentication/userinfo";
        }

        public static class Token
        {
            public const string AccessToken = "api/token/generate-access-token";
            public const string RegenerateRefreshToken = "api/token/regenerate-refresh-token";
        }


        public static class Item
        {
            public const string Get = "api/Item";
            public const string Save = "api/Item/save";
            public const string Delete = "api/Item/delete";
            public const string Search = "api/Item/search";
        }

        public static class Attribute
        {
            public const string Get = "api/Attribute";
            public const string Save = "api/Attribute/save";
            public const string Delete = "api/Attribute/delete";
            public const string Search = "api/Attribute/search";
        }

        public static class Category
        {
            public const string Get = "api/Category";
            public const string Save = "api/Category/save";
            public const string ChangeTreeViewSerials = "api/Category/changeTreeViewSerials";
            public const string Delete = "api/Category/delete";
            public const string Search = "api/Category/search";
        }

        public static class Order
        {
            public const string Get = "api/Order";
            public const string Save = "api/Order/save";
            public const string ChangeOrderStatus = "api/Order/changeOrderStatus";
            public const string Delete = "api/Order/delete";
            public const string Search = "api/Order/search";
            public const string SearchForUserId = "api/Order/searchForUserId";
            public const string UserSearch = "api/Order/userSearch";
            public const string ReferralOrders = "api/Order/referralOrders";
            public const string GetOrderNumber = "api/Order/orderNumber";
        }

        public static class Refund
        {
            public const string Get = "api/Refund";
            public const string Save = "api/Refund/save";
            public const string ChangeRefundStatus = "api/Refund/changeRefundStatus";
            public const string Delete = "api/Refund/delete";
            public const string Search = "api/Refund/search";
        }

        public static class Unit
        {
            public const string Get = "api/Unit";
            public const string Save = "api/Unit/save";
            public const string Delete = "api/Unit/delete";
            public const string Search = "api/Unit/search";
        }

        public static class Country
        {
            public const string Get = "api/Country";
            public const string Save = "api/Country/save";
            public const string Delete = "api/Country/delete";
            public const string Search = "api/Country/search";
        }

        public static class State
        {
            public const string Get = "api/State";
            public const string Save = "api/State/save";
            public const string Delete = "api/State/delete";
            public const string Search = "api/State/search";
        }

        public static class City
        {
            public const string Get = "api/City";
            public const string Save = "api/City/save";
            public const string Delete = "api/City/delete";
            public const string Search = "api/City/search";
        }


        public static class PaymentMethod
        {
            public const string Get = "api/PaymentMethod";
            public const string Save = "api/PaymentMethod/save";
            public const string Delete = "api/PaymentMethod/delete";
            public const string Search = "api/PaymentMethod/search";
        }

        // Add this new class
        public static class PaymentGatewayMethod
        {
            public const string GetAll = "api/PaymentGatewayMethod";
            public const string GetById = "api/PaymentGatewayMethod";
        }
        public static class UserPaymentMethod
        {
            public const string Get = "api/UserPaymentMethod";
            public const string GetUserPaymentMethods = "api/UserPaymentMethod/GetUserPaymentMethods";
            public const string Save = "api/UserPaymentMethod/save";
            public const string Delete = "api/UserPaymentMethod/delete";
        }

        public static class Field
        {
            public const string Get = "api/Field";
            public const string Save = "api/Field/save";
            public const string Delete = "api/Field/delete";
            public const string Search = "api/Field/search";
            public const string GetFieldsByPaymentMethod = "api/Field/GetFieldsByPaymentMethod";
        }

        public static class UserField
        {
            public const string Get = "api/UserField";
            public const string Save = "api/UserField/save";
            public const string Delete = "api/UserField/delete";
        }

        public static class Admin
        {
            public const string Get = "api/Admin";
            public const string Create = "api/Admin";
            public const string Update = "api/Admin";
            public const string Delete = "api/Admin/delete";
            public const string Search = "api/Admin/search";
        }
		public static class Vendor
		{
			public const string Get = "api/Vendor";
			public const string Create = "api/Vendor";
			public const string Update = "api/Vendor";
			public const string Delete = "api/Vendor/delete";

			public const string Search = "api/Vendor/search";
			public const string FindById = "api/Vendor";
			public const string GetForSelect = "api/Vendor/forSelect";
			public const string ChangeStatus = "api/Vendor/changeStatus";
			public const string GetUserStatus = "api/Vendor/getStatus";
			public const string GetVendorInfo = "api/Vendor/VendorInfo";
		}
		
		

		public static class VendorRegistration
        {
            public const string Register = "api/VendorRegistration";
            public const string GetSponsor = "api/VendorRegistration/getSponsor";
            public const string IsValidLink = "api/VendorRegistration/IsValidLink";
        }

        public static class VendorBusinessPoints
        {
            public const string Get = "api/VendorBusinessPoints";
            public const string GetDetaild = "api/VendorBusinessPoints/details";
            public const string WithdrawPoints = "api/VendorBusinessPoints/WithdrawPoints";
        }

		public static class Customer
		{
			public const string Search = "api/Customer/search";
			public const string FindById = "api/Customer";
			public const string GetForSelect = "api/Customer/forSelect";
			public const string ChangeStatus = "api/Customer/changeStatus";
			public const string GetUserStatus = "api/Customer/getStatus";
			public const string GetVendorInfo = "api/Customer/VendorInfo";
		}

		public static class PromoCode
        {
            public const string Get = "api/PromoCodes";
            public const string Save = "api/PromoCodes/save";
            public const string Delete = "api/PromoCodes/delete";
            public const string Search = "api/PromoCodes/search";
        }

        public static class ShippingCompany
        {
            public const string Get = "api/ShippingCompany";
            public const string Save = "api/ShippingCompany/save";
            public const string Delete = "api/ShippingCompany/delete";
            public const string Search = "api/ShippingCompany/search";
        }

        public static class UserNotification
        {
            public const string Get = "api/UserNotifications";
            public const string Save = "api/UserNotifications/save";
            public const string Search = "api/UserNotifications/search";
            public const string MarkAsRead = "api/UserNotifications/markAsRead";
        }


        public static class Currency
        {
            public const string Get = "api/Currency";
            public const string Save = "api/Currency";
            public const string Delete = "api/Currency/delete";
            public const string Search = "api/Currency/search";
            public const string SetBase = "api/Currency/set-base";
            public const string UpdateRates = "api/Currency/update-rates";
            public const string GetBase = "api/Currency/base";
            public const string GetActive = "api/Currency/active";
        }

        // Add these to the existing ApiEndpoints class
        public static class Setting
        {
            public const string Get = "api/Setting";
            public const string MainBanner = "api/Setting/mainBanner";
            public const string WithdrawelFeePersentage = "api/Setting/withdrawelFeePersentage";
            public const string Update = "api/Setting/update";
        }

        public static class Page
        {
            public const string Get = "api/Page";
            public const string GetById = "api/Page";
            public const string GetBySlug = "api/Page/by-slug";
            public const string GetByTitle = "api/Page/by-title";
            public const string Search = "api/Page/search";
            public const string Save = "api/Page/save";
            public const string Delete = "api/Page/delete";
            public const string ToggleStatus = "api/Page/toggle-status";
        }

        // Brand endpoints
        public static class Brand
        {
            public const string Get = "api/Brand";
            public const string GetFavorites = "api/Brand/favorites";
            public const string Search = "api/Brand/search";
            public const string Save = "api/Brand/save";
            public const string Delete = "api/Brand/delete";
            public const string MarkAsFavorite = "api/Brand/mark-favorite";
        }

        // Testimonial endpoints
        public static class Testimonial
        {
            public const string Get = "api/Testimonial";
            public const string Search = "api/Testimonial/search";
            public const string Save = "api/Testimonial/save";
            public const string Delete = "api/Testimonial/delete";
        }

        public static class AdminStatistics
        {
            public const string Get = "api/AdminStatistics";
            public const string GetByDateRange = "api/AdminStatistics/daterange";
            public const string GetSummary = "api/AdminStatistics/summary";
        }

        public static class VendorStatistics
        {
            public const string Get = "api/VendorStatistics";
            public const string GetByDateRange = "api/VendorStatistics/dateRange";
        }
    }
}
