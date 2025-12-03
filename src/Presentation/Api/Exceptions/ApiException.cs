namespace Api.Exceptions
{
    /// <summary>
    /// Base API exception class for all custom API exceptions
    /// </summary>
    public class ApiException : Exception
    {
        public int StatusCode { get; set; }
        public string? ErrorCode { get; set; }
        public List<string> Errors { get; set; }

        public ApiException(string message, int statusCode = 500, string? errorCode = null, List<string>? errors = null)
            : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
            Errors = errors ?? new List<string> { message };
        }
    }

    /// <summary>
    /// Exception for validation failures
    /// </summary>
    public class ValidationException : ApiException
    {
        public ValidationException(string message, List<string>? errors = null)
            : base(message, StatusCodes.Status400BadRequest, "VALIDATION_ERROR", errors ?? new List<string> { message })
        {
        }
    }

    /// <summary>
    /// Exception for resource not found scenarios
    /// </summary>
    public class ResourceNotFoundException : ApiException
    {
        public ResourceNotFoundException(string resourceName, string identifier)
            : base($"{resourceName} with identifier '{identifier}' was not found.", StatusCodes.Status404NotFound, "RESOURCE_NOT_FOUND", new List<string> { $"{resourceName} not found" })
        {
        }

        public ResourceNotFoundException(string message)
            : base(message, StatusCodes.Status404NotFound, "RESOURCE_NOT_FOUND", new List<string> { message })
        {
        }
    }

    /// <summary>
    /// Exception for unauthorized access scenarios
    /// </summary>
    public class UnauthorizedAccessException : ApiException
    {
        public UnauthorizedAccessException(string message = "You do not have permission to access this resource.")
            : base(message, StatusCodes.Status401Unauthorized, "UNAUTHORIZED", new List<string> { message })
        {
        }
    }

    /// <summary>
    /// Exception for forbidden access scenarios
    /// </summary>
    public class ForbiddenException : ApiException
    {
        public ForbiddenException(string message = "You do not have permission to perform this action.")
            : base(message, StatusCodes.Status403Forbidden, "FORBIDDEN", new List<string> { message })
        {
        }
    }

    /// <summary>
    /// Exception for business logic violations
    /// </summary>
    public class BusinessLogicException : ApiException
    {
        public BusinessLogicException(string message, List<string>? errors = null)
            : base(message, StatusCodes.Status400BadRequest, "BUSINESS_LOGIC_ERROR", errors ?? new List<string> { message })
        {
        }
    }

    /// <summary>
    /// Exception for conflict/duplicate scenarios (HTTP 409)
    /// </summary>
    public class ConflictException : ApiException
    {
        public ConflictException(string message, List<string>? errors = null)
            : base(message, StatusCodes.Status409Conflict, "CONFLICT", errors ?? new List<string> { message })
        {
        }
    }

    /// <summary>
    /// Exception for external service failures
    /// </summary>
    public class ExternalServiceException : ApiException
    {
        public ExternalServiceException(string serviceName, string message, List<string>? errors = null)
            : base($"External service '{serviceName}' error: {message}", StatusCodes.Status502BadGateway, "EXTERNAL_SERVICE_ERROR", errors ?? new List<string> { message })
        {
        }
    }

    /// <summary>
    /// Exception for rate limiting
    /// </summary>
    public class RateLimitException : ApiException
    {
        public RateLimitException(string message = "Too many requests. Please try again later.")
            : base(message, StatusCodes.Status429TooManyRequests, "RATE_LIMIT_EXCEEDED", new List<string> { message })
        {
        }
    }
}
