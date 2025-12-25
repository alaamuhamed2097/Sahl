using Shared.DTOs.Merchandising.Homepage;

namespace BL.Contracts.Service.Merchandising
{
    /// <summary>
    /// Homepage Service Interface
    /// </summary>
    public interface IHomepageService
    {
        /// <summary>
        /// Get complete homepage with all blocks
        /// </summary>
        Task<GetHomepageResponse> GetHomepageAsync(string? userId);

        /// <summary>
        /// Get single block by ID
        /// </summary>
        Task<HomepageBlockDto?> GetBlockByIdAsync(Guid blockId);
    }
}