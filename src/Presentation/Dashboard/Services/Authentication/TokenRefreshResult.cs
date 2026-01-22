// ============================================
// 1. Token Storage Service (Secure & Centralized)
// ============================================
namespace Dashboard.Services.Authentication
{
    public class TokenRefreshResult
    {
        public bool Success { get; set; }
        public string? NewAccessToken { get; set; }
        public bool RequiresLogin { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
