using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ErrorCodes
{
    public static class ErrorCodes
    {
        // Group errors by domain
        public static class User
        {
            private const string Prefix = "user.";
            public const string NotFound = Prefix + "not_found";
            public const string EmailExists = Prefix + "email_exists";
            public const string NotVerified = Prefix + "not_verified";
            public const string Inactive = Prefix + "inactive";
        }

        public static class Validation
        {
            private const string Prefix = "validation.";
            public const string MissingFields = Prefix + "missing_fields";
            public const string InvalidEmail = Prefix + "invalid_email";
            public const string InvalidParameters = Prefix + "invalid_parameters";
            public const string WeakPassword = Prefix + "weak_password";
            public const string InvalidFields = "invalid_fields";
            public const string NotFound = Prefix + "not_found";
        }

        public static class Auth
        {
            private const string Prefix = "auth.";
            public const string InvalidCredentials = Prefix + "invalid_credentials";
            public const string ExpiredToken = Prefix + "expired_token";
            public const string InvalidVerificationCode = Prefix + "invalid_verification_code";
        }

        public static class System
        {
            private const string Prefix = "system.";
            public const string UnexpectedError = Prefix + "unexpected_error";
            public const string ServiceUnavailable = Prefix + "service_unavailable";
        }

        public static class Event
        {
            private const string Prefix = "event.";

            // Event Related
            public const string NotFound = Prefix + "not_found";
            public const string AlreadyStarted = Prefix + "already_started";
            public const string FullyBooked = Prefix + "fully_booked";
            public const string NoDiscountAvailable = Prefix + "no_discount_available";

            // Booking Related
            public const string AlreadyBooked = Prefix + "already_booked";
            public const string BookingNotFound = Prefix + "booking_not_found";
            public const string TooManyAttempts = Prefix + "too_many_attempts";
            public const string BookingUpdateFailed = Prefix + "booking_update_failed";

            // Promo Code Related
            public const string InvalidPromoCode = Prefix + "invalid_promo_code";
            public const string InvalidCouponCode = Prefix + "invalid_coupon_code";

            // Payment Related
            public const string InvalidPaymentMethod = Prefix + "invalid_payment_method";
            public const string CashPaymentNotAllowed = Prefix + "cash_payment_not_allowed";
            public const string PaymentActivationFailed = Prefix + "payment_activation_failed";
            public const string PaymentConfirmationFailed = Prefix + "payment_confirmation_failed";
        }

        public static class Order
        {
            private const string Prefix = "order.";
            public const string NotFound = Prefix + "not_found";
            public const string InvalidItemId = Prefix + "invalid_item_id";
            public const string InvalidItemQuantity = Prefix + "invalid_item_quantity";
            public const string InsufficientStock = Prefix + "insufficient_stock";
            public const string PaymentFailed = Prefix + "payment_failed";
            public const string AlreadyProcessed = Prefix + "already_processed";
            public const string InsufficientBusinessPointsBalance = Prefix + "insufficient_business_points_balance";
        }
    }
}
