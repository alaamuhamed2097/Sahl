namespace Shared.DTOs.User
{
    public class RefreshTokenRequestDto
    {
        public string Email { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
