namespace Common.Enumerations.Notification
{
    public enum NotificationType
    {
        // Existing User Management Notifications
        EmailVerification = 0,       // Sent upon user registration for email activation
        PhoneNumberVerification = 1, // Sent upon user registration for phone number activation
        ForgotPasswordByEmail = 2,   // Sent upon user clicked Forgot Password
        ForgotPasswordByPhone = 3,   // Sent upon user clicked Forgot Password
        OldEmailChanged = 4,         // Sent upon user change email for old email
        NewEmailActivation = 5,      // Sent upon user change email for new email

        // Order Lifecycle Notifications (add these)
        OrderCreated = 100,
        PaymentConfirmed = 101,
        PaymentFailed = 102,
        PaymentReminder = 103,
        OrderConfirmed = 104,
        OrderProcessing = 105,
        OrderShipped = 106,
        OrderOutForDelivery = 107,
        OrderDelivered = 108,
        OrderCancelled = 109,
        RefundProcessed = 110,
        ShipmentStatusChanged = 111,
        DeliveryAttemptFailed = 120,
        DeliveryDelayed = 121,
        DeliveryRescheduled = 122,
        ReviewRequest = 130,
        ReviewThankYou = 131,

        // ============================================
        // VENDOR NOTIFICATIONS (200-299)
        // ============================================

        /// <summary>
        /// New shipment assigned to vendor (FBM)
        /// Used for: Email, SMS, SignalR
        /// </summary>
        VendorNewShipment = 200,

        /// <summary>
        /// Shipment cancelled
        /// Used for: Email, SignalR
        /// </summary>
        VendorShipmentCancelled = 201,

        /// <summary>
        /// Order delivered confirmation
        /// Used for: Email, SignalR
        /// </summary>
        VendorOrderDelivered = 202,

        /// <summary>
        /// Low stock alert
        /// Used for: Email, SignalR
        /// </summary>
        VendorLowStockAlert = 203,

        /// <summary>
        /// Payment received notification
        /// Used for: Email, SignalR
        /// </summary>
        VendorPaymentReceived = 204
    }
}