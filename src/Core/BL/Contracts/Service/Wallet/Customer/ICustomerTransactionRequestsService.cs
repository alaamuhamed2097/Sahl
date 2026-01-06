using DAL.Models;
using Shared.DTOs.Wallet.Customer;
using Shared.GeneralModels.SearchCriteriaModels;

namespace BL.Contracts.Service.Wallet.Customer
{
    public interface ICustomerTransactionRequestsService
    {
        Task<IEnumerable<CustomerWalletTransactionsDto>> GetWalletTransactionsRequestsAsync();
        Task<PagedResult<CustomerWalletTransactionsDto>> GetPageAsync(BaseSearchCriteriaModel criteriaModel);
    }
}
