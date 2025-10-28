// File: Middleware/ClientIPMiddleware.cs
using Api.Extensions;
using Microsoft.AspNetCore.Http;

namespace Api.Middleware
{
    public class ClientIPMiddleware
    {
        private readonly RequestDelegate _next;

        public ClientIPMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var clientIp = context.GetClientIpAddress();
            context.Items["ClientIP"] = clientIp;

            await _next(context);
        }
    }
}