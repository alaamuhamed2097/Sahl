namespace Shared.GeneralModels.ResultModels
{
    public class SignInResult : OperationResult
    {
        public string Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string ProfileImagePath { get; set; } = "default.png";
        public string? Token { get; set; } = null!;
        public string? RefreshToken { get; set; } = null!;
        public string Role { get; set; } = "User";
    }
}
