using Common.Filters;
using Shared.DTOs.Catalog.Item;

namespace BL.Contracts.Service.Customer
{
    public interface ICustomerRecommendedItemsService
    {
        Task<PagedSpSearchResultDto> SearchCustomerRecommendedItemsAsync(
                                        BaseSearchCriteriaModel filter,
                                        string? userId ,
                                        CancellationToken cancellationToken = default);
    }
}