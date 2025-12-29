namespace BL.Contracts.GeneralService
{
    public interface IVerificationCodeService
    {
        Task<bool> SendCodeAsync(string identifier);
        bool VerifyCode(string identifier, string code);
        void DeleteCode(string identifier);
        int GetAttempts(string identifier);
    }
}
