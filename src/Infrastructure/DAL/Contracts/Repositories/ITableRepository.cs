using DAL.ResultModels;
using Domains.Entities.Base;

namespace DAL.Contracts.Repositories
{
    public interface ITableRepository<T> : IRepository<T> where T : BaseEntity
    {
        T FindById(Guid id);
        bool Save(T model, Guid userId);
        bool Save(T model, Guid userId, out Guid id);
        bool Create(T model, Guid creatorId, out Guid id);
        bool Update(T model, Guid updaterId, out Guid id);
        bool UpdateCurrentState(Guid entityId, Guid updaterId, int newValue = 0);
        bool HardDelete(Guid id);
        bool AddRange(IEnumerable<T> entities, Guid userId);
        bool SaveChange();

        Task<T> FindByIdAsync(Guid id);
        Task<SaveResult> SaveAsync(T model, Guid userId);
        Task<SaveResult> CreateAsync(T model, Guid creatorId);
        Task<SaveResult> UpdateAsync(T model, Guid updaterId);
        Task<bool> UpdateCurrentStateAsync(Guid entityId, Guid updaterId, int newValue = 0);
        Task<bool> HardDeleteAsync(Guid id);
        Task<bool> AddRangeAsync(IEnumerable<T> entities, Guid userId);
        Task<bool> SaveChangeAsync();
    }
}
