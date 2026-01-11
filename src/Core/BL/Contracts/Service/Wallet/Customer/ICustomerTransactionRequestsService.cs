using Common.Filters;
using DAL.Models;
using Shared.DTOs.Wallet.Customer;

namespace BL.Contracts.Service.Wallet.Customer
{
    public interface ICustomerTransactionRequestsService
    {
        Task<IEnumerable<CustomerWalletTransactionsDto>> GetWalletTransactionsRequestsAsync();
        Task<PagedResult<CustomerWalletTransactionsDto>> GetPageAsync(BaseSearchCriteriaModel criteriaModel);
    }
}
