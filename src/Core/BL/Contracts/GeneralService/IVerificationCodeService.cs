namespace BL.Contracts.GeneralService
{
    public interface IVerificationCodeService
    {
        Task<bool> SendCodeAsync(string email);
        bool VerifyCode(string email, string code);
        void DeleteCode(string email);
        int GetAttempts(string email);
    }
}
