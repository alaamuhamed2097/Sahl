using System.Text.Json;
using Resources;
using Shared.GeneralModels;
using Microsoft.EntityFrameworkCore;
using Api.Exceptions;

namespace Api.Middleware
{
    /// <summary>
    /// Centralized exception handling middleware for all API requests
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred while processing the request.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ResponseModel<object>
            {
                Success = false,
                Message = NotifiAndAlertsResources.SomethingWentWrongAlert
            };

            switch (exception)
            {
                case ArgumentNullException argNullEx:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response.Message = "Invalid input provided.";
                    response.Errors = new List<string> { argNullEx.ParamName ?? "Unknown parameter is null" };
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    break;

                case ArgumentException argEx:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response.Message = "Invalid argument provided.";
                    response.Errors = new List<string> { argEx.Message };
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    break;

                case InvalidOperationException invOpEx:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response.Message = "Invalid operation. The current state does not allow this operation.";
                    response.Errors = new List<string> { invOpEx.Message };
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    break;

                case KeyNotFoundException keyNotFoundEx:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    response.Message = "Resource not found.";
                    response.Errors = new List<string> { keyNotFoundEx.Message };
                    response.StatusCode = StatusCodes.Status404NotFound;
                    break;

                case System.UnauthorizedAccessException unauthorizedEx:
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    response.Message = "You are not authorized to perform this action.";
                    response.Errors = new List<string> { unauthorizedEx.Message };
                    response.StatusCode = StatusCodes.Status401Unauthorized;
                    break;

                case OperationCanceledException canceledEx:
                    context.Response.StatusCode = StatusCodes.Status408RequestTimeout;
                    response.Message = "The request was cancelled or timed out.";
                    response.Errors = new List<string> { canceledEx.Message };
                    response.StatusCode = StatusCodes.Status408RequestTimeout;
                    break;

                case ApiException apiEx:
                    context.Response.StatusCode = apiEx.StatusCode;
                    response.Message = apiEx.Message;
                    response.Errors = apiEx.Errors;
                    response.StatusCode = apiEx.StatusCode;
                    response.ErrorCode = apiEx.ErrorCode;
                    break;

                case DbUpdateConcurrencyException concurrencyEx:
                    context.Response.StatusCode = StatusCodes.Status409Conflict;
                    response.Message = "The record has been modified by another user. Please refresh and try again.";
                    response.Errors = new List<string> { concurrencyEx.Message };
                    response.StatusCode = StatusCodes.Status409Conflict;
                    break;

                case DbUpdateException dbEx:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.Message = "An error occurred while updating the database. Please try again later.";
                    response.Errors = new List<string> { "Database operation failed" };
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    break;

                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.Message = exception.Message;
                    response.Errors = new List<string> { exception.Message };
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    break;
            }

            var jsonOptions = new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            return context.Response.WriteAsJsonAsync(response, jsonOptions);
        }
    }
}
