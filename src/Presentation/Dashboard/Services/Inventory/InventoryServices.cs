using Dashboard.Constants;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Inventory;
using Dashboard.Models.pagintion;
using Resources;
using Shared.DTOs.Inventory;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Dashboard.Services.Inventory
{
    public class InventoryMovementService : IInventoryMovementService
    {
        private readonly IApiService _apiService;

        public InventoryMovementService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ResponseModel<IEnumerable<MoitemDto>>> GetAllAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<MoitemDto>>(ApiEndpoints.InventoryMovement.Get);
            }
            catch (Exception ex)
            {
                return new ResponseModel<IEnumerable<MoitemDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<MoitemDto>> GetByIdAsync(Guid id)
        {
            try
            {
                return await _apiService.GetAsync<MoitemDto>($"{ApiEndpoints.InventoryMovement.GetById}/{id}");
            }
            catch (Exception ex)
            {
                return new ResponseModel<MoitemDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<MoitemDto>> GetByDocumentNumberAsync(string documentNumber)
        {
            try
            {
                return await _apiService.GetAsync<MoitemDto>($"{ApiEndpoints.InventoryMovement.GetByDocument}/{documentNumber}");
            }
            catch (Exception ex)
            {
                return new ResponseModel<MoitemDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<PaginatedDataModel<MoitemDto>>> SearchAsync(BaseSearchCriteriaModel criteria)
        {
            if (criteria == null) throw new ArgumentNullException(nameof(criteria));

            try
            {
                return await _apiService.PostAsync<BaseSearchCriteriaModel, PaginatedDataModel<MoitemDto>>(
                    ApiEndpoints.InventoryMovement.Search, criteria);
            }
            catch (Exception ex)
            {
                return new ResponseModel<PaginatedDataModel<MoitemDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<string>> GenerateDocumentNumberAsync()
        {
            try
            {
                return await _apiService.GetAsync<string>(ApiEndpoints.InventoryMovement.GenerateDocumentNumber);
            }
            catch (Exception ex)
            {
                return new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<MoitemDto>> SaveAsync(MoitemDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            try
            {
                return await _apiService.PostAsync<MoitemDto, MoitemDto>(ApiEndpoints.InventoryMovement.Save, dto);
            }
            catch (Exception ex)
            {
                return new ResponseModel<MoitemDto>
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
                var result = await _apiService.PostAsync<Guid, ResponseModel<bool>>(ApiEndpoints.InventoryMovement.Delete, id);
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
    }

    public class ReturnMovementService : IReturnMovementService
    {
        private readonly IApiService _apiService;

        public ReturnMovementService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ResponseModel<IEnumerable<MortemDto>>> GetAllAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<MortemDto>>(ApiEndpoints.ReturnMovement.Get);
            }
            catch (Exception ex)
            {
                return new ResponseModel<IEnumerable<MortemDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<MortemDto>> GetByIdAsync(Guid id)
        {
            try
            {
                return await _apiService.GetAsync<MortemDto>($"{ApiEndpoints.ReturnMovement.GetById}/{id}");
            }
            catch (Exception ex)
            {
                return new ResponseModel<MortemDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<PaginatedDataModel<MortemDto>>> SearchAsync(BaseSearchCriteriaModel criteria)
        {
            if (criteria == null) throw new ArgumentNullException(nameof(criteria));

            try
            {
                return await _apiService.PostAsync<BaseSearchCriteriaModel, PaginatedDataModel<MortemDto>>(
                    ApiEndpoints.ReturnMovement.Search, criteria);
            }
            catch (Exception ex)
            {
                return new ResponseModel<PaginatedDataModel<MortemDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<string>> GenerateDocumentNumberAsync()
        {
            try
            {
                return await _apiService.GetAsync<string>(ApiEndpoints.ReturnMovement.GenerateDocumentNumber);
            }
            catch (Exception ex)
            {
                return new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<MortemDto>> SaveAsync(MortemDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            try
            {
                return await _apiService.PostAsync<MortemDto, MortemDto>(ApiEndpoints.ReturnMovement.Save, dto);
            }
            catch (Exception ex)
            {
                return new ResponseModel<MortemDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<bool>> UpdateStatusAsync(Guid id, int status)
        {
            try
            {
                var request = new { Id = id, Status = status };
                var result = await _apiService.PostAsync<object, ResponseModel<bool>>(ApiEndpoints.ReturnMovement.UpdateStatus, request);
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

        public async Task<ResponseModel<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _apiService.PostAsync<Guid, ResponseModel<bool>>(ApiEndpoints.ReturnMovement.Delete, id);
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
    }
}
