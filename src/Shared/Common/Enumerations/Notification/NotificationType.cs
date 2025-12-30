namespace Common.Enumerations.Notification
{
    public enum NotificationType
    {
        // User Managment Notifications
        EmailVerification,          // Sent upon user registration for email activation
        PhoneNumberVerification,    // Sent upon user registration for phone number activation
        ForgotPasswordByEmail,      // Sent upon user clicked Forgot Password
        ForgotPasswordByPhone,      // Sent upon user clicked Forgot Password
        OldEmailChanged,            // Sent upon user change email for old email
        NewEmailActivation,         // Sent upon user change email for new email
    }
}
