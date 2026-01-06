using Domains.Entities.Wallet.Customer;
using Shared.DTOs.Wallet.Customer;

namespace BL.Mapper
{
    public partial class MappingProfile
    {
        private void ConfigureWalletMappings()
        {
            CreateMap<TbCustomerWalletTransaction, CustomerWalletTransactionsDto>().ReverseMap();
            //CreateMap<VwWalletTransactions, CustomerWalletTransactionsDto>().ReverseMap();
            //CreateMap<VwWalletTransferDetails, WalletTransferDetailsDto>().ReverseMap();
            //CreateMap<VwWalletCommissionsTransaction, WalletCommissionsTransactionDto>().ReverseMap();
            //CreateMap<VwWalletEarningDetails, WalletEarningDetailsDto>().ReverseMap();
        }
    }
}
