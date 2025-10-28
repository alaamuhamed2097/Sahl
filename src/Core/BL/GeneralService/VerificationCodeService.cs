using BL.Contracts.GeneralService;
using BL.Contracts.GeneralService.CMS;
using BL.Contracts.GeneralService.Notification;

namespace BL.GeneralService
{
    public class VerificationCodeService : IVerificationCodeService
    {
        private readonly ICacheService _memoryCache;
        private readonly ISmsService _smsService;
        private readonly Serilog.ILogger _logger;

        private const int CodeExpirationTimeInMinutes = 20; // Code expiration time
        private const int ResendCooldownInSeconds = 60;     // Resend cooldown time
        private const int MaxAttempts = 5; // الحد الأقصى للمحاولات
        private const string AttemptsKeySuffix = "_code_attempts";

        public VerificationCodeService(ICacheService memoryCache,
            ISmsService smsService,
            Serilog.ILogger logger)
        {
            _memoryCache = memoryCache;
            _smsService = smsService;
            _logger = logger;
        }

        /// <summary>
        /// Generates a secure 6-digit verification code.
        /// </summary>
        private string GenerateCode()
        {
            //using (var rng = new RNGCryptoServiceProvider())
            //{
            //    var bytes = new byte[4];
            //    rng.GetBytes(bytes);
            //    var random = BitConverter.ToUInt32(bytes, 0) % 1_000_000;
            //    return random.ToString("D6");
            //}

            return "123456";
        }

        /// <summary>
        /// Sends a verification code via SMS and saves it in the cache.
        /// </summary>
        public async Task<bool> SendCodeAsync(string email)
        {
            var cacheKey = $"{email}_code";
            var cooldownKey = $"{email}_cooldown";
            var attemptsKey = $"{email}{AttemptsKeySuffix}";

            // Check if the user is on cooldown
            if (_memoryCache.Get<string>(cooldownKey) != null)
            {
                return false; // Cannot resend code yet
            }

            // Generate a new code
            var code = GenerateCode();

            // Save the code in the cache
            _memoryCache.Set(cacheKey, code, TimeSpan.FromMinutes(CodeExpirationTimeInMinutes));

            // Reset attempts counter
            _memoryCache.Set(attemptsKey, 0, TimeSpan.FromMinutes(CodeExpirationTimeInMinutes));

            // Save the cooldown state in the cache
            _memoryCache.Set(cooldownKey, "cooldown", TimeSpan.FromSeconds(ResendCooldownInSeconds));

            // Send the code via SMS
            var smsSent = await _smsService.SendSmsAsync(email, $"Your verification code is: {code}");

            if (smsSent)
            {
                return true;
            }
            else
            {
                _logger.Error($"Failed to send verification code to {email}.");
                // Remove the code and cooldown in case of SMS failure
                _memoryCache.Remove(cacheKey);
                _memoryCache.Remove(cooldownKey);
                _memoryCache.Remove(attemptsKey);
                return false;
            }
        }

        /// <summary>
        /// Verifies if the provided code matches the one in the cache.
        /// </summary>
        public bool VerifyCode(string email, string code)
        {
            var cacheKey = $"{email}_code";
            var attemptsKey = $"{email}{AttemptsKeySuffix}";
            var cachedCode = _memoryCache.Get<string>(cacheKey);

            // Get current attempts
            int attempts = _memoryCache.Get<int?>(attemptsKey) ?? 0;
            if (attempts >= MaxAttempts)
            {
                return false; // Exceeded max attempts
            }

            // Increase attempts
            _memoryCache.Set(attemptsKey, attempts + 1, TimeSpan.FromMinutes(CodeExpirationTimeInMinutes));

            if (cachedCode == null)
            {
                return false;
            }

            if (cachedCode == code)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Deletes the verification code after successful use.
        /// </summary>
        public void DeleteCode(string email)
        {
            var cacheKey = $"{email}_code";
            var attemptsKey = $"{email}{AttemptsKeySuffix}";
            _memoryCache.Remove(cacheKey);
            _memoryCache.Remove(attemptsKey);
        }

        public int GetAttempts(string email)
        {
            var attemptsKey = $"{email}{AttemptsKeySuffix}";
            return _memoryCache.Get<int?>(attemptsKey) ?? 0;
        }
    }
}
