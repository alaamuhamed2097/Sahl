using DAL.ResultModels;

namespace BL.Contracts.Service.Base
{
    public interface IBaseService<TS, TD>
    {
        #region Async
        Task<IEnumerable<TD>> GetAllAsync();
        Task<TD> FindByIdAsync(Guid Id);
        Task<SaveResult> SaveAsync(TD entity, Guid userId);
        Task<SaveResult> CreateAsync(TD entity, Guid creatorId);
        Task<SaveResult> UpdateAsync(TD entity, Guid updaterId);
        Task<bool> DeleteAsync(Guid id, Guid userId);
        #endregion
    }
}
