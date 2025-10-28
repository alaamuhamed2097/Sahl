namespace Shared.GeneralModels.Parameters.Notification
{
    public class EmailRequest
    {
        public EmailRequest(string to, string subject, string body, bool isHtml = true, List<string> attachments = null)
        {
            if (string.IsNullOrWhiteSpace(to))
                throw new ArgumentException("To email address is required.", nameof(to));

            To = to;
            Subject = subject ?? throw new ArgumentNullException(nameof(subject));
            Body = body ?? throw new ArgumentNullException(nameof(body));
            IsHtml = isHtml;
            Attachments = attachments ?? new List<string>();
        }

        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; } = true;
        public List<string> Attachments { get; set; } = new List<string>();
    }
}
