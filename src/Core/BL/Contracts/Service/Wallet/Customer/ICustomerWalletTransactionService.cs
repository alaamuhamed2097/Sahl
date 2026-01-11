using Common.Filters;
using DAL.Models;
using Shared.DTOs.Wallet.Customer;

namespace BL.Contracts.Service.Wallet.Customer
{
    public interface ICustomerWalletTransactionService
    {
        Task<IEnumerable<CustomerWalletTransactionsDto>> GetAllTransactions(Guid userId);
        Task<PagedResult<CustomerWalletTransactionsDto>> GetPage(BaseSearchCriteriaModel criteriaModel, Guid userId);
    }
}
