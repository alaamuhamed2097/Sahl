using BL.Contracts.IMapper;
using BL.Contracts.Service.ECommerce.Unit;
using BL.Service.Base;
using DAL.Contracts.UnitOfWork;
using DAL.Models;
using Domins.Entities.Unit;
using Domins.Views.Unit;
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

        public PaginatedDataModel<UnitDto> GetPage(BaseSearchCriteriaModel criteriaModel)
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

            var entitiesList = _unitRepository.Repository<VwUnitWithConversionsUnits>().GetPage(
                criteriaModel.PageNumber,
                criteriaModel.PageSize,
                filter,
                orderBy);

            var units = _mapper.MapList<VwUnitWithConversionsUnits, UnitDto>(entitiesList.Items);

            return new PaginatedDataModel<UnitDto>(units, entitiesList.TotalRecords);
        }
        public override UnitDto FindById(Guid Id)
        {
            Expression<Func<VwUnitWithConversionsUnits, bool>> filter = x => x.Id == Id;
            var vwUnitWithConversionsUnits = _unitRepository.Repository<VwUnitWithConversionsUnits>().Find(filter);

            if (vwUnitWithConversionsUnits == null)
                throw new Exception(string.Format(ValidationResources.EntityNotFound, ECommerceResources.Unit));

            return _mapper.MapModel<VwUnitWithConversionsUnits, UnitDto>(vwUnitWithConversionsUnits);
        }
        public override bool Save(UnitDto dto, Guid userId)
        {
            try
            {
                // Validate input
                if (dto == null) throw new ArgumentNullException(nameof(dto));
                if (userId == Guid.Empty) throw new ArgumentException(UserResources.UserNotFound, nameof(userId));

                // Map and create main attribute entity
                var entity = _mapper.MapModel<UnitDto, TbUnit>(dto);

                // Begin transaction
                _unitRepository.BeginTransactionAsync();

                // delete old conversion Units if any
                if (dto.Id != Guid.Empty &&
                    dto.ConversionUnitsFrom.Any() || dto.ConversionUnitsFrom != null ||
                    dto.ConversionUnitsTo.Any() || dto.ConversionUnitsTo != null)
                {
                    var conversionUnits = _unitRepository.TableRepository<TbUnitConversion>().Get(c => c.ToUnitId == dto.Id || c.FromUnitId == dto.Id);
                    foreach (var conversionUnit in conversionUnits)
                    {
                        _unitRepository.TableRepository<TbUnitConversion>().UpdateCurrentState(conversionUnit.Id, userId, 0);
                    }
                }
                // Save main unit
                _unitRepository.TableRepository<TbUnit>().Save(entity, userId, out Guid unitId);

                // Save conversion Units From if any
                foreach (var conversionUnitFromDto in dto.ConversionUnitsFrom ?? new())
                {

                    var conversionUnitFrom = _mapper.MapModel<UnitConversionDto, TbUnitConversion>(new UnitConversionDto()
                    {
                        FromUnitId = conversionUnitFromDto.ConversionUnitId,
                        ConversionFactor = (decimal)conversionUnitFromDto.ConversionFactor
                    });
                    conversionUnitFrom.ToUnitId = unitId;
                    _unitRepository.TableRepository<TbUnitConversion>().Create(conversionUnitFrom, userId, out _);
                }
                // Save conversion Units To if any
                foreach (var conversionUnitToDto in dto.ConversionUnitsTo ?? new())
                {

                    var conversionUnitTo = _mapper.MapModel<UnitConversionDto, TbUnitConversion>(new UnitConversionDto()
                    {
                        ToUnitId = conversionUnitToDto.ConversionUnitId,
                        ConversionFactor = (decimal)conversionUnitToDto.ConversionFactor
                    });
                    conversionUnitTo.FromUnitId = unitId;
                    _unitRepository.TableRepository<TbUnitConversion>().Create(conversionUnitTo, userId, out _);
                }

                // Commit transaction
                _unitRepository.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Rollback on error
                _unitRepository.Rollback();
                throw new Exception(string.Format(ValidationResources.SaveEntityError, ECommerceResources.Unit), ex);
            }
        }
        public override bool Delete(Guid id, Guid userId)
        {
            try
            {
                _unitRepository.BeginTransactionAsync();
                _unitRepository.TableRepository<TbUnit>().UpdateCurrentState(id, userId);
                var conversionUnits = _unitRepository.TableRepository<TbUnitConversion>().Get(c => c.ToUnitId == id || c.FromUnitId == id);
                if (conversionUnits?.Count() > 0)
                {
                    foreach (var conversionUnit in conversionUnits)
                    {
                        _unitRepository.TableRepository<TbUnitConversion>().UpdateCurrentState(conversionUnit.Id, userId, 0);
                    }
                }
                _unitRepository.CommitAsync();
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
