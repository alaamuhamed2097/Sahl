using Dashboard.Models.pagintion;
using Shared.DTOs.Inventory;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Dashboard.Contracts.Inventory
{
    public interface IInventoryMovementService
    {
        Task<ResponseModel<IEnumerable<MoitemDto>>> GetAllAsync();
        Task<ResponseModel<MoitemDto>> GetByIdAsync(Guid id);
        Task<ResponseModel<MoitemDto>> GetByDocumentNumberAsync(string documentNumber);
        Task<ResponseModel<PaginatedDataModel<MoitemDto>>> SearchAsync(BaseSearchCriteriaModel criteria);
        Task<ResponseModel<string>> GenerateDocumentNumberAsync();
        Task<ResponseModel<MoitemDto>> SaveAsync(MoitemDto dto);
        Task<ResponseModel<bool>> DeleteAsync(Guid id);
    }

    public interface IReturnMovementService
    {
        Task<ResponseModel<IEnumerable<MortemDto>>> GetAllAsync();
        Task<ResponseModel<MortemDto>> GetByIdAsync(Guid id);
        Task<ResponseModel<PaginatedDataModel<MortemDto>>> SearchAsync(BaseSearchCriteriaModel criteria);
        Task<ResponseModel<string>> GenerateDocumentNumberAsync();
        Task<ResponseModel<MortemDto>> SaveAsync(MortemDto dto);
        Task<ResponseModel<bool>> UpdateStatusAsync(Guid id, int status);
        Task<ResponseModel<bool>> DeleteAsync(Guid id);
    }
}
