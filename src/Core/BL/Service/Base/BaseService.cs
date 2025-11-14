using BL.Contracts.IMapper;
using BL.Contracts.Service.Base;
using DAL.Contracts.Repositories;
using DAL.ResultModels;

namespace BL.Service.Base
{
    public abstract class BaseService<TS, TD> : IBaseService<TS, TD> where TS : BaseEntity
    {
        private readonly ITableRepository<TS> _baseRepository;
        private readonly IBaseMapper _mapper;
        public BaseService(ITableRepository<TS> baseRepository, IBaseMapper mapper)
        {
            _baseRepository = baseRepository;
            _mapper = mapper;
        }

        #region Sync

        public virtual TD FindById(Guid Id)
        {
            var entity = _baseRepository.FindById(Id);
            var dto = _mapper.MapModel<TS, TD>(entity);
            return dto;
        }

        public virtual IEnumerable<TD> GetAll()
        {
            var entitiesList = _baseRepository.GetAll();
            var dtoList = _mapper.MapList<TS, TD>(entitiesList);
            return dtoList;
        }

        public virtual bool Save(TD dto, Guid userId)
        {
            var entity = _mapper.MapModel<TD, TS>(dto);
            return _baseRepository.Save(entity, userId);
        }

        public bool Create(TD dto, Guid creatorId)
        {
            var entity = _mapper.MapModel<TD, TS>(dto);
            return _baseRepository.Create(entity, creatorId, out _);
        }

        public bool Update(TD dto, Guid updaterId)
        {
            var entity = _mapper.MapModel<TD, TS>(dto);
            return _baseRepository.Update(entity, updaterId, out _);
        }

        public virtual bool Delete(Guid id, Guid userId)
        {
            return _baseRepository.UpdateCurrentState(id, userId);
        }

        #endregion

        #region Async
        public virtual async Task<TD> FindByIdAsync(Guid Id)
        {
            var entity = await _baseRepository.FindByIdAsync(Id);
            var dto = _mapper.MapModel<TS, TD>(entity);
            return dto;
        }

        public virtual async Task<IEnumerable<TD>> GetAllAsync()
        {
            var entitiesList = await _baseRepository.GetAllAsync();
            var dtoList = _mapper.MapList<TS, TD>(entitiesList);
            return dtoList;
        }

        public virtual async Task<SaveResult> SaveAsync(TD dto, Guid userId)
        {
            var entity = _mapper.MapModel<TD, TS>(dto);
            return await _baseRepository.SaveAsync(entity, userId);
        }

        public async Task<SaveResult> CreateAsync(TD dto, Guid creatorId)
        {
            var entity = _mapper.MapModel<TD, TS>(dto);
            return await _baseRepository.CreateAsync(entity, creatorId);
        }

        public async Task<SaveResult> UpdateAsync(TD dto, Guid updaterId)
        {
            var entity = _mapper.MapModel<TD, TS>(dto);
            return await _baseRepository.UpdateAsync(entity, updaterId);
        }

        public virtual async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            return await _baseRepository.UpdateCurrentStateAsync(id, userId);
        }

        #endregion
    }
}
