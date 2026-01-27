using Shared.DTOs.Merchandising.Homepage;

namespace BL.Contracts.Service.Merchandising;

/// <summary>
/// Homepage Service Interface
/// </summary>
public interface IHomepageService
{
    Task<GetHomepageResponse> GetHomepageAsync(string? userId);
    Task<HomepageBlockDto?> GetBlockByIdAsync(Guid blockId, int pageNumber = 1, int pageSize = 20);
    Task<BlockItemsPagedResponse> GetBlockItemsAsync(Guid blockId, string? userId, int pageNumber = 1, int pageSize = 20);
    Task<BlockCategoriesPagedResponse> GetBlockCategoriesAsync(Guid blockId, int pageNumber = 1, int pageSize = 20);
}