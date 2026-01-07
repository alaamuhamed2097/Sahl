using Domains.Entities.Wallet.Customer;

namespace BL.Contracts.Service.Wallet.Customer
{
    public interface IWalletSettingService
    {
        Task<TbWalletSetting> GetSettingsAsync();
    }
}
