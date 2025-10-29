namespace Dashboard.Models
{
    // Supporting class for error responses
    public class ApiErrorResponse
    {
        public string Message { get; set; }
        public string Type { get; set; }
        public string StackTrace { get; set; }
    }
}
