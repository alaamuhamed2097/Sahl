namespace Dashboard.Constants
{
    public static class ApiEndpoints
    {
        public static class Auth
        {
            public const string EmailLogin = "api/Auth/email-login";
            public const string LogOut = "api/Auth/admin-logout";
        }

        public static class Token
        {
            public const string AccessToken = "api/Token/generate-access-token";
        }

        public static class Item
        {
            public const string Get = "api/Item";
            public const string GetItemInAdmin = "api/Item/GetItemInAdmin";
            public const string Save = "api/Item/save";
            public const string SaveInAdmin = "api/Item/SaveInAdmin";
            public const string UpdateItemVisibilityScope = "api/Item/update-visibility";
            public const string Delete = "api/Item/delete";
            public const string Search = "api/Item/SearchInAdmin";
            public const string GetItemsInPeriod = "api/Item/get_items_inPeriod";
        }

        public static class ItemCondition
        {
            public const string Get = "api/ItemCondition";
            public const string Save = "api/ItemCondition/save";
            public const string Delete = "api/ItemCondition/delete";
            public const string Search = "api/ItemCondition/search";
        }

        public static class Specialization
        {
            public const string Get = "api/Specialization";
            public const string Save = "api/Specialization/save";
            public const string Delete = "api/Specialization/delete";
            public const string Search = "api/Specialization/search";
        }
        public static class UserProfile
        {
            public const string GetUserProfile = "api/UserProfile/profile";
            public const string UpdateUserProfile = "api/UserProfile/profile";
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

        public static class Unit
        {
            public const string Get = "api/Unit";
            public const string Save = "api/Unit/save";
            public const string Delete = "api/Unit/delete";
            public const string Search = "api/Unit/search";
        }
        public static class Currency
        {
            public const string GetAllCurrencies = "api/Currency/currencies";
            public const string Get = "api/Currency";
            public const string Save = "api/Currency/save";
            public const string Delete = "api/Currency/delete";
            public const string Search = "api/Currency/search";
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

        public static class VideoProvider
        {
            public const string Get = "api/VideoProvider";
            public const string Save = "api/VideoProvider/save";
            public const string Delete = "api/VideoProvider/delete";
        }

        public static class Rank
        {
            public const string Get = "api/Rank";
            public const string Save = "api/Rank/save";
            public const string Delete = "api/Rank/delete";
        }

        public static class FAQ
        {
            public const string Get = "api/FAQ";
            public const string Save = "api/FAQ/save";
            public const string Delete = "api/FAQ/delete";
            public const string Search = "api/FAQ/search";
        }
        public static class ContactMethod
        {
            public const string Get = "api/ContactMethod";
            public const string Save = "api/ContactMethod/save";
            public const string Delete = "api/ContactMethod/delete";
            public const string Search = "api/ContactMethod/search";
        }

        public static class AccountType
        {
            public const string Get = "api/AccountType";
            public const string Save = "api/AccountType/save";
            public const string Delete = "api/AccountType/delete";
        }

        public static class MainBanner
        {
            public const string Get = "api/MainBanner";
            public const string Save = "api/MainBanner/save";
            public const string Delete = "api/MainBanner/delete";
            public const string Search = "api/MainBanner/search";
        }

        public static class UserPaymentMethod
        {
            public const string Get = "api/UserPaymentMethod";
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

        public static class Customer
        {
            public const string Get = "api/Customer";
            public const string ViewAdvertiserById = "api/Customer/viewAdvertiser";
            public const string Search = "api/Customer/search";
            public const string Register = "api/Customer/register_customer";
            public const string Update = "api/Customer/update_customer";
            public const string Delete = "api/Customer/delete_customer";
        }

        public static class Setting
        {
            public const string Get = "api/Setting";
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
    }
}
