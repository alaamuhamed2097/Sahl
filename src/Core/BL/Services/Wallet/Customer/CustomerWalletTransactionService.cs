using BL.Contracts.IMapper;
using BL.Contracts.Service.Wallet.Customer;
using Common.Filters;
using DAL.Contracts.UnitOfWork;
using DAL.Models;
using Domains.Entities.Wallet.Customer;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Wallet.Customer;

namespace BL.Services.Wallet.Customer
{
    public class CustomerWalletTransactionService : ICustomerWalletTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseMapper _mapper;

        public CustomerWalletTransactionService(IUnitOfWork unitOfWork, IBaseMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CustomerWalletTransactionsDto>> GetAllTransactions(Guid userId)
        {
            var wallet = await GetWalletAsync(userId);
            if (wallet == null) return new List<CustomerWalletTransactionsDto>();

            var transactions = await _unitOfWork.TableRepository<TbCustomerWalletTransaction>()
                .GetAsync(x => x.WalletId == wallet.Id,
                          orderBy: q => q.OrderByDescending(t => t.CreatedDateUtc));

            return MapToDtoList(transactions, wallet);
        }

        public async Task<PagedResult<CustomerWalletTransactionsDto>> GetPage(BaseSearchCriteriaModel criteriaModel, Guid userId)
        {
            var wallet = await GetWalletAsync(userId);
            if (wallet == null) return new PagedResult<CustomerWalletTransactionsDto>(new List<CustomerWalletTransactionsDto>(), 0);

            var query = _unitOfWork.TableRepository<TbCustomerWalletTransaction>().GetQueryable()
                .Where(x => x.WalletId == wallet.Id);

            if (!string.IsNullOrEmpty(criteriaModel.SearchTerm))
            {
                query = query.Where(x => x.ReferenceType.Contains(criteriaModel.SearchTerm) ||
                                         x.TransactionType.ToString().Contains(criteriaModel.SearchTerm));
            }

            var totalRecords = await _unitOfWork.TableRepository<TbCustomerWalletTransaction>()
                .CountAsync(x => x.WalletId == wallet.Id);

            // Note: Counting filtered records
            if (!string.IsNullOrEmpty(criteriaModel.SearchTerm))
            {
                totalRecords = query.Count();
            }

            query = ApplySorting(query, criteriaModel.SortBy, criteriaModel.SortDirection);

            var pagedTransactions = query
                .Skip((criteriaModel.PageNumber - 1) * criteriaModel.PageSize)
                .Take(criteriaModel.PageSize)
                .ToList();

            var dtos = MapToDtoList(pagedTransactions, wallet);

            return new PagedResult<CustomerWalletTransactionsDto>(dtos, totalRecords);
        }

        private async Task<TbCustomerWallet?> GetWalletAsync(Guid userId)
        {
            var wallet = await _unitOfWork.TableRepository<TbCustomerWallet>()
               .GetQueryable()
               .Include(w => w.User)
               .FirstOrDefaultAsync(w => w.UserId == userId.ToString());

            return wallet;
        }

        private IQueryable<TbCustomerWalletTransaction> ApplySorting(IQueryable<TbCustomerWalletTransaction> query, string? sortBy, string sortDirection)
        {
            bool isAsc = sortDirection?.ToLower() == "asc";

            return sortBy?.ToLower() switch
            {
                "amount" => isAsc ? query.OrderBy(x => x.Amount) : query.OrderByDescending(x => x.Amount),
                "type" => isAsc ? query.OrderBy(x => x.TransactionType) : query.OrderByDescending(x => x.TransactionType),
                "status" => isAsc ? query.OrderBy(x => x.TransactionStatus) : query.OrderByDescending(x => x.TransactionStatus),
                _ => query.OrderByDescending(x => x.CreatedDateUtc)
            };
        }

        private IEnumerable<CustomerWalletTransactionsDto> MapToDtoList(IEnumerable<TbCustomerWalletTransaction> transactions, TbCustomerWallet wallet)
        {
            var dtos = _mapper.MapList<TbCustomerWalletTransaction, CustomerWalletTransactionsDto>(transactions);

            // Enrich DTOs with Wallet/User info that might be missing if not mapped automatically
            foreach (var dto in dtos)
            {
                // Ensure context fields are set
                dto.AvailableBalanace = wallet.Balance;
                dto.PendingBalanace = wallet.PendingBalance;
                dto.UserId = wallet.UserId;
                dto.TransactionId = transactions.FirstOrDefault(t => t.CreatedDateUtc == dto.Date && t.Amount == dto.Amount)?.Id ?? dto.TransactionId; // Fallback to map if Id not mapped, but mapper should handle it.

                // Id mapping check: Automapper usually maps Id -> TransactionId if configured or same name. 
                // TbCustomerWalletTransaction has Id. DTO has TransactionId. 
                // We assuming Mapper profile exists or we should set it manually.
                var src = transactions.FirstOrDefault(t => t.Id == dto.TransactionId);
                // Actually relying on Mapper is safer if we trust it.

                if (wallet.User != null)
                {
                    dto.FirstName = wallet.User.FirstName;
                    dto.LastName = wallet.User.LastName;
                    dto.UserName = wallet.User.UserName;
                }
            }
            return dtos;
        }
    }
}
