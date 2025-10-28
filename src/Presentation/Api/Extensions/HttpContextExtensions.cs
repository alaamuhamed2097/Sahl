// File: Extensions/HttpContextExtensions.cs
using System.Net;

namespace Api.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetClientIpAddress(this HttpContext context)
        {
            if (context == null)
                return string.Empty;

            // Try middleware-set IP first
            if (context.Items.TryGetValue("ClientIP", out var clientIp) && clientIp is string ip && !string.IsNullOrEmpty(ip))
            {
                return ip;
            }

            // X-Forwarded-For (can contain multiple IPs)
            var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedFor))
            {
                var firstIp = forwardedFor.Split(',').First().Trim();
                if (IPAddress.TryParse(firstIp, out _))
                {
                    return firstIp;
                }
            }

            // X-Real-IP
            var realIp = context.Request.Headers["X-Real-IP"].FirstOrDefault();
            if (!string.IsNullOrEmpty(realIp) && IPAddress.TryParse(realIp, out _))
            {
                return realIp;
            }

            // Cloudflare
            var cfIp = context.Request.Headers["CF-Connecting-IP"].FirstOrDefault();
            if (!string.IsNullOrEmpty(cfIp) && IPAddress.TryParse(cfIp, out _))
            {
                return cfIp;
            }

            // Fallback to direct connection IP
            var remoteIp = context.Connection?.RemoteIpAddress;
            if (remoteIp != null)
            {
                if (remoteIp.IsIPv4MappedToIPv6)
                {
                    return remoteIp.MapToIPv4().ToString();
                }
                return remoteIp.ToString();
            }

            return "127.0.0.1";
        }
    }
}