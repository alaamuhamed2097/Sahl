using DAL.Models;
using Shared.DTOs.Inventory;
using Shared.GeneralModels.SearchCriteriaModels;

namespace BL.Contracts.Service.Inventory
{
    public interface IMoitemService
    {
        Task<IEnumerable<MoitemDto>> GetAllAsync();
        Task<MoitemDto?> GetByIdAsync(Guid id);
        Task<MoitemDto?> GetByDocumentNumberAsync(string documentNumber);
        Task<PaginatedDataModel<MoitemDto>> SearchAsync(BaseSearchCriteriaModel criteria);
        Task<bool> SaveAsync(MoitemDto dto, Guid userId);
        Task<bool> DeleteAsync(Guid id, Guid userId);
        Task<string> GenerateDocumentNumberAsync();
    }

    public interface IMortemService
    {
        Task<IEnumerable<MortemDto>> GetAllAsync();
        Task<MortemDto?> GetByIdAsync(Guid id);
        Task<MortemDto?> GetByDocumentNumberAsync(string documentNumber);
        Task<PaginatedDataModel<MortemDto>> SearchAsync(BaseSearchCriteriaModel criteria);
        Task<bool> SaveAsync(MortemDto dto, Guid userId);
        Task<bool> DeleteAsync(Guid id, Guid userId);
        Task<bool> UpdateStatusAsync(Guid id, int status, Guid userId);
        Task<string> GenerateDocumentNumberAsync();
    }

    public interface IMovitemsdetailService
    {
        Task<IEnumerable<MovitemsdetailDto>> GetByMoitemIdAsync(Guid moitemId);
        Task<IEnumerable<MovitemsdetailDto>> GetByMortemIdAsync(Guid mortemId);
        Task<IEnumerable<MovitemsdetailDto>> GetByWarehouseIdAsync(Guid warehouseId);
        Task<MovitemsdetailDto?> GetByIdAsync(Guid id);
        Task<bool> SaveAsync(MovitemsdetailDto dto, Guid userId);
        Task<bool> SaveRangeAsync(IEnumerable<MovitemsdetailDto> dtos, Guid userId);
        Task<bool> DeleteAsync(Guid id, Guid userId);
    }
}
