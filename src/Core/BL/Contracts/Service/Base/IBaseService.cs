using DAL.ResultModels;

namespace BL.Contracts.Service.Base
{
    public interface IBaseService<TS, TD>
    {
        #region Sync
        IEnumerable<TD> GetAll();
        TD FindById(Guid Id);
        bool Save(TD entity, Guid userId);
        bool Create(TD entity, Guid creatorId);
        bool Update(TD entity, Guid updaterId);
        bool Delete(Guid id, Guid userId);
        #endregion

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
