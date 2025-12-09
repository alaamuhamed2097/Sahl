using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.User.OAuth
{
    public class GoogleOAuthTokenDto
    {
        [Required]
        public string IdToken { get; set; } = null!;

        public string? AccessToken { get; set; }
    }

    public class GoogleOAuthResponseDto
    {
        public string Sub { get; set; } = null!; // Google unique ID
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Picture { get; set; }
        public bool EmailVerified { get; set; }
        public string? GivenName { get; set; }
        public string? FamilyName { get; set; }
    }

    public class FacebookOAuthTokenDto
    {
        [Required]
        public string AccessToken { get; set; } = null!;
    }

    public class FacebookOAuthResponseDto
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Picture { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }

    public class OAuthSignUpDto
    {
        [Required]
        public string Provider { get; set; } = null!; // "google" or "facebook"

        [Required]
        public string ProviderId { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        public string? Picture { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
