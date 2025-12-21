using BL.Contracts.IMapper;
using BL.Contracts.Service.ECommerce.Unit;
using BL.Service.Base;
using DAL.Contracts.UnitOfWork;
using DAL.Models;
using DAL.ResultModels;
using Domains.Entities.Catalog.Unit;
using Domains.Views.Unit;
using Resources;
using Shared.DTOs.ECommerce.Unit;
using Shared.GeneralModels.SearchCriteriaModels;
using System.Linq.Expressions;

namespace BL.Service.ECommerce.Unit
{
    public class UnitService : BaseService<TbUnit, UnitDto>, IUnitService
    {
        private readonly IUnitOfWork _unitRepository;
        private readonly IBaseMapper _mapper;

        public UnitService(IBaseMapper mapper, IUnitOfWork unitRepository)
            : base(unitRepository.TableRepository<TbUnit>(), mapper)
        {
            _mapper = mapper;
            _unitRepository = unitRepository;
        }

        public async Task<PagedResult<UnitDto>> GetPageAsync(BaseSearchCriteriaModel criteriaModel)
        {
            if (criteriaModel == null)
                throw new ArgumentNullException(nameof(criteriaModel));

            if (criteriaModel.PageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

            if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

            // Base filter for active entities
            Expression<Func<VwUnitWithConversionsUnits, bool>> filter = x => !string.IsNullOrWhiteSpace(x.TitleAr) && !string.IsNullOrWhiteSpace(x.TitleEn);

            // Apply search term if provided
            if (!string.IsNullOrWhiteSpace(criteriaModel.SearchTerm))
            {
                string searchTerm = criteriaModel.SearchTerm.Trim().ToLower();
                filter = x =>
                             x.TitleAr != null && x.TitleAr.ToLower().Contains(searchTerm) ||
                             x.TitleEn != null && x.TitleEn.ToLower().Contains(searchTerm);
            }

            // Create ordering function based on SortBy and SortDirection
            Func<IQueryable<VwUnitWithConversionsUnits>, IOrderedQueryable<VwUnitWithConversionsUnits>> orderBy = null;

            if (!string.IsNullOrWhiteSpace(criteriaModel.SortBy))
            {
                var sortBy = criteriaModel.SortBy.ToLower();
                var isDescending = criteriaModel.SortDirection?.ToLower() == "desc";

                orderBy = query =>
                {
                    return sortBy switch
                    {
                        "titlear" => isDescending ? query.OrderByDescending(x => x.TitleAr) : query.OrderBy(x => x.TitleAr),
                        "titleen" => isDescending ? query.OrderByDescending(x => x.TitleEn) : query.OrderBy(x => x.TitleEn),
                        "title" => isDescending ? query.OrderByDescending(x => x.TitleAr) : query.OrderBy(x => x.TitleAr),
                        _ => query.OrderBy(x => x.TitleAr) // Default sorting
                    };
                };
            }

            var entitiesList = await _unitRepository.Repository<VwUnitWithConversionsUnits>().GetPageAsync(
                criteriaModel.PageNumber,
                criteriaModel.PageSize,
                filter,
                orderBy);

            var units = _mapper.MapList<VwUnitWithConversionsUnits, UnitDto>(entitiesList.Items);

            return new PagedResult<UnitDto>(units, entitiesList.TotalRecords);
        }

        // Async implementations
        public override async Task<UnitDto> FindByIdAsync(Guid Id)
        {
            Expression<Func<VwUnitWithConversionsUnits, bool>> filter = x => x.Id == Id;
            var vwUnitWithConversionsUnits = await _unitRepository.Repository<VwUnitWithConversionsUnits>().FindAsync(filter);

            if (vwUnitWithConversionsUnits == null)
                throw new Exception(string.Format(ValidationResources.EntityNotFound, ECommerceResources.Unit));

            return _mapper.MapModel<VwUnitWithConversionsUnits, UnitDto>(vwUnitWithConversionsUnits);
        }

        public override async Task<SaveResult> SaveAsync(UnitDto dto, Guid userId)
        {
            try
            {
                if (dto == null) throw new ArgumentNullException(nameof(dto));
                if (userId == Guid.Empty) throw new ArgumentException(UserResources.UserNotFound, nameof(userId));

                var entity = _mapper.MapModel<UnitDto, TbUnit>(dto);

                await _unitRepository.BeginTransactionAsync();

                if (dto.Id != Guid.Empty && ((dto.ConversionUnitsFrom != null && dto.ConversionUnitsFrom.Any()) || (dto.ConversionUnitsTo != null && dto.ConversionUnitsTo.Any())))
                {
                    var conversionUnits = await _unitRepository.TableRepository<TbUnitConversion>().GetAsync(c => c.ToUnitId == dto.Id || c.FromUnitId == dto.Id);
                    foreach (var conversionUnit in conversionUnits)
                    {
                        await _unitRepository.TableRepository<TbUnitConversion>().UpdateCurrentStateAsync(conversionUnit.Id, userId, true);
                    }
                }

                var saveResult = await _unitRepository.TableRepository<TbUnit>().SaveAsync(entity, userId);

                // Save conversion Units From
                foreach (var conversionUnitFromDto in dto.ConversionUnitsFrom ?? new List<ConversionUnitDto>())
                {
                    var conversionUnitFrom = _mapper.MapModel<UnitConversionDto, TbUnitConversion>(new UnitConversionDto()
                    {
                        FromUnitId = conversionUnitFromDto.ConversionUnitId,
                        ConversionFactor = (decimal)conversionUnitFromDto.ConversionFactor
                    });
                    conversionUnitFrom.ToUnitId = saveResult.Id;
                    await _unitRepository.TableRepository<TbUnitConversion>().CreateAsync(conversionUnitFrom, userId);
                }

                // Save conversion Units To
                foreach (var conversionUnitToDto in dto.ConversionUnitsTo ?? new List<ConversionUnitDto>())
                {
                    var conversionUnitTo = _mapper.MapModel<UnitConversionDto, TbUnitConversion>(new UnitConversionDto()
                    {
                        ToUnitId = conversionUnitToDto.ConversionUnitId,
                        ConversionFactor = (decimal)conversionUnitToDto.ConversionFactor
                    });
                    conversionUnitTo.FromUnitId = saveResult.Id;
                    await _unitRepository.TableRepository<TbUnitConversion>().CreateAsync(conversionUnitTo, userId);
                }

                await _unitRepository.CommitAsync();
                return saveResult;
            }
            catch (Exception ex)
            {
                _unitRepository.Rollback();
                throw new Exception(string.Format(ValidationResources.SaveEntityError, ECommerceResources.Unit), ex);
            }
        }

        public override async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            try
            {
                await _unitRepository.BeginTransactionAsync();
                await _unitRepository.TableRepository<TbUnit>().UpdateCurrentStateAsync(id, userId);

                var conversionUnits = await _unitRepository.TableRepository<TbUnitConversion>().GetAsync(c => c.ToUnitId == id || c.FromUnitId == id);
                if (conversionUnits?.Any() == true)
                {
                    foreach (var conversionUnit in conversionUnits)
                    {
                        await _unitRepository.TableRepository<TbUnitConversion>().UpdateCurrentStateAsync(conversionUnit.Id, userId, true);
                    }
                }

                await _unitRepository.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _unitRepository.Rollback();
                throw new Exception(string.Format(ValidationResources.DeleteEntityError, ECommerceResources.Unit), ex);
            }
        }
    }
}
