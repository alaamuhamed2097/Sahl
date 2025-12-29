namespace Bl.Contracts.GeneralService.CMS
{
    public interface IPasswordGenerator
    {
        string GenerateRandomPassword(int length = 6);
    }
}
