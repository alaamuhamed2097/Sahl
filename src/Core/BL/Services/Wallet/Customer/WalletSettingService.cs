using BL.Contracts.Service.Wallet.Customer;
using DAL.Contracts.UnitOfWork;
using Domains.Entities.Wallet.Customer;

namespace BL.Services.Wallet.Customer
{
    public class WalletSettingService : IWalletSettingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WalletSettingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TbWalletSetting> GetSettingsAsync()
        {
            var repo = _unitOfWork.TableRepository<TbWalletSetting>();
            // Assuming single global record. If none exists, return defaults.
            var setting = (await repo.GetAllAsync()).FirstOrDefault();
            
            if (setting == null)
            {
                // Return default object if not found in DB (fallback)
                return new TbWalletSetting();
            }

            return setting;
        }
    }
}
