using Dashboard.Constants;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Warehouse;
using Dashboard.Models.pagintion;
using Resources;
using Shared.DTOs.Warehouse;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Dashboard.Services.Warehouse
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IApiService _apiService;

        public WarehouseService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ResponseModel<IEnumerable<WarehouseDto>>> GetAllAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<WarehouseDto>>(ApiEndpoints.Warehouse.Get);
            }
            catch (Exception ex)
            {
                return new ResponseModel<IEnumerable<WarehouseDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<IEnumerable<WarehouseDto>>> GetActiveWarehousesAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<WarehouseDto>>(ApiEndpoints.Warehouse.GetActive);
            }
            catch (Exception ex)
            {
                return new ResponseModel<IEnumerable<WarehouseDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<WarehouseDto>> GetByIdAsync(Guid id)
        {
            try
            {
                return await _apiService.GetAsync<WarehouseDto>($"{ApiEndpoints.Warehouse.Get}/{id}");
            }
            catch (Exception ex)
            {
                return new ResponseModel<WarehouseDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<PaginatedDataModel<WarehouseDto>>> SearchAsync(BaseSearchCriteriaModel criteria)
        {
            if (criteria == null) throw new ArgumentNullException(nameof(criteria));

            try
            {
                return await _apiService.PostAsync<BaseSearchCriteriaModel, PaginatedDataModel<WarehouseDto>>(
                    ApiEndpoints.Warehouse.Search, criteria);
            }
            catch (Exception ex)
            {
                return new ResponseModel<PaginatedDataModel<WarehouseDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<WarehouseDto>> SaveAsync(WarehouseDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            try
            {
                return await _apiService.PostAsync<WarehouseDto, WarehouseDto>(ApiEndpoints.Warehouse.Save, dto);
            }
            catch (Exception ex)
            {
                return new ResponseModel<WarehouseDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _apiService.PostAsync<Guid, ResponseModel<bool>>(ApiEndpoints.Warehouse.Delete, id);
                if (result.Success)
                {
                    return new ResponseModel<bool>
                    {
                        Success = true,
                        Message = result.Message
                    };
                }
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = result.Message,
                    Errors = result.Errors
                };
            }
            catch (Exception)
            {
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.DeleteFailed
                };
            }
        }

        public async Task<ResponseModel<bool>> ToggleStatusAsync(Guid id)
        {
            try
            {
                var result = await _apiService.PostAsync<Guid, ResponseModel<bool>>(ApiEndpoints.Warehouse.ToggleStatus, id);
                if (result.Success)
                {
                    return new ResponseModel<bool>
                    {
                        Success = true,
                        Message = result.Message
                    };
                }
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = result.Message,
                    Errors = result.Errors
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
