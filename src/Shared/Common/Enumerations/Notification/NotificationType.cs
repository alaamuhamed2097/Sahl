namespace Common.Enumerations.Notification
{
    public enum NotificationType
    {
        // Purchase & Registration Notifications
        CoursePurchase,            // Sent immediately after a new course purchase
        EmailVerification,          // Sent upon user registration for email activation
        ForgotPassword,            // Sent upon user clicked Forgot Password
        OldEmailChanged,            // Sent upon user change email for old email
        NewEmailActivation,         // Sent upon user change email for new email

        // Rewards & Certification Notifications
        CourseCompletion,           // Sent when user completes all lectures of a course
        CertificateIssued,          // Sent when a certificate is available for download

        // User Activity Notifications
        UserInactivity,              // Sent after 3 days of user inactivity

        // Send new email
        sendEmail
    }
}
