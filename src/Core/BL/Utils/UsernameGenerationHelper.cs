using Microsoft.AspNetCore.Identity;

namespace BL.Utils;

/// <summary>
/// Helper class for username generation
/// </summary>
public static class UsernameGenerationHelper
{
    /// <summary>
    /// Generates a unique username based on first name if username is not provided
    /// Format: {firstName.ToLower()}{4-digit-random-number}
    /// </summary>
    public static async Task<string> GenerateUniqueUsernameAsync(
        string firstName,
        UserManager<ApplicationUser> userManager,
        int maxAttempts = 10)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            firstName = "user";

        var baseUsername = firstName.ToLower().Trim();
        var random = new Random();

        for (int i = 0; i < maxAttempts; i++)
        {
            var randomSuffix = random.Next(1000, 10000);
            var candidateUsername = $"{baseUsername}{randomSuffix}";

            // Check if username is already taken
            var existingUser = await userManager.FindByNameAsync(candidateUsername);
            if (existingUser == null)
            {
                return candidateUsername;
            }
        }

        // Fallback: use GUID-based username if generation fails
        return $"user{Guid.NewGuid().ToString("N").Substring(0, 8)}";
    }
}
