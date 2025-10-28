namespace DAL.Exceptions
{
    public class NotificationException : Exception
    {
        public NotificationException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
