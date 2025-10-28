namespace Shared.DTOs.User
{
    public class RefreshTokenDto
    {
        public string Email { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
