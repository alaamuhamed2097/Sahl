namespace Shared.DTOs.User
{
    // DTO for user info with roles
    public class UserInfoDto
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? ProfileImagePath { get; set; }
        public List<string>? Roles { get; set; }
    }
}
