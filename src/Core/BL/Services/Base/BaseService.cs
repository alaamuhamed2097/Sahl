using AutoMapper;
using BL.Contracts.IMapper;
using BL.Contracts.Service.Base;
using DAL.Contracts.Repositories;
using DAL.Contracts.Repositories.Merchandising;
using DAL.ResultModels;
using Domains.Entities.Base;

namespace BL.Services.Base;

public abstract class BaseService<TS, TD> : IBaseService<TS, TD> where TS : BaseEntity
{
    private readonly ITableRepository<TS> _baseRepository;
		private readonly IBaseMapper _mapper;
	private ICampaignItemRepository campaignItemRepository;
	private IMapper mapper;

	public BaseService(ITableRepository<TS> baseRepository, IBaseMapper mapper)
    {
        _baseRepository = baseRepository;
        _mapper = mapper;
    }

	protected BaseService(ICampaignItemRepository campaignItemRepository, IMapper mapper)
	{
		this.campaignItemRepository = campaignItemRepository;
		this.mapper = mapper;
	}

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
        return await _baseRepository.UpdateIsDeletedAsync(id, userId);
    }

    #endregion
}
