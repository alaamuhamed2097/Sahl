using Common.Filters;
using Dashboard.Constants;
using Dashboard.Contracts.Content;
using Dashboard.Contracts.General;
using Dashboard.Models.pagintion;
using Resources;
using Shared.DTOs.Content;
using Shared.GeneralModels;

namespace Dashboard.Services.Content
{
    public class ContentAreaService : IContentAreaService
    {
        private readonly IApiService _apiService;

        public ContentAreaService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ResponseModel<IEnumerable<ContentAreaDto>>> GetAllAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<ContentAreaDto>>(ApiEndpoints.ContentArea.Get);
            }
            catch (Exception ex)
            {
                return new ResponseModel<IEnumerable<ContentAreaDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<IEnumerable<ContentAreaDto>>> GetActiveAreasAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<ContentAreaDto>>(ApiEndpoints.ContentArea.GetActive);
            }
            catch (Exception ex)
            {
                return new ResponseModel<IEnumerable<ContentAreaDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<ContentAreaDto>> GetByIdAsync(Guid id)
        {
            try
            {
                return await _apiService.GetAsync<ContentAreaDto>($"{ApiEndpoints.ContentArea.Get}/{id}");
            }
            catch (Exception ex)
            {
                return new ResponseModel<ContentAreaDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<ContentAreaDto>> GetByAreaCodeAsync(string areaCode)
        {
            try
            {
                return await _apiService.GetAsync<ContentAreaDto>($"{ApiEndpoints.ContentArea.GetByCode}/{areaCode}");
            }
            catch (Exception ex)
            {
                return new ResponseModel<ContentAreaDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<PaginatedDataModel<ContentAreaDto>>> SearchAsync(BaseSearchCriteriaModel criteria)
        {
            if (criteria == null) throw new ArgumentNullException(nameof(criteria));

            try
            {
                return await _apiService.PostAsync<BaseSearchCriteriaModel, PaginatedDataModel<ContentAreaDto>>(
                    ApiEndpoints.ContentArea.Search, criteria);
            }
            catch (Exception ex)
            {
                return new ResponseModel<PaginatedDataModel<ContentAreaDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<ContentAreaDto>> SaveAsync(ContentAreaDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            try
            {
                return await _apiService.PostAsync<ContentAreaDto, ContentAreaDto>(ApiEndpoints.ContentArea.Save, dto);
            }
            catch (Exception ex)
            {
                return new ResponseModel<ContentAreaDto>
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
                var result = await _apiService.PostAsync<Guid, ResponseModel<bool>>(ApiEndpoints.ContentArea.Delete, id);
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
                var result = await _apiService.PostAsync<Guid, ResponseModel<bool>>(ApiEndpoints.ContentArea.ToggleStatus, id);
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

    public class MediaContentService : IMediaContentService
    {
        private readonly IApiService _apiService;

        public MediaContentService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ResponseModel<IEnumerable<MediaContentDto>>> GetAllAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<MediaContentDto>>(ApiEndpoints.MediaContent.Get);
            }
            catch (Exception ex)
            {
                return new ResponseModel<IEnumerable<MediaContentDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<IEnumerable<MediaContentDto>>> GetByAreaIdAsync(Guid contentAreaId)
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<MediaContentDto>>($"{ApiEndpoints.MediaContent.GetByArea}/{contentAreaId}");
            }
            catch (Exception ex)
            {
                return new ResponseModel<IEnumerable<MediaContentDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<IEnumerable<MediaContentDto>>> GetByAreaCodeAsync(string areaCode)
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<MediaContentDto>>($"{ApiEndpoints.MediaContent.GetByAreaCode}/{areaCode}");
            }
            catch (Exception ex)
            {
                return new ResponseModel<IEnumerable<MediaContentDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<MediaContentDto>> GetByIdAsync(Guid id)
        {
            try
            {
                return await _apiService.GetAsync<MediaContentDto>($"{ApiEndpoints.MediaContent.Get}/{id}");
            }
            catch (Exception ex)
            {
                return new ResponseModel<MediaContentDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<PaginatedDataModel<MediaContentDto>>> SearchAsync(BaseSearchCriteriaModel criteria)
        {
            if (criteria == null) throw new ArgumentNullException(nameof(criteria));

            try
            {
                return await _apiService.PostAsync<BaseSearchCriteriaModel, PaginatedDataModel<MediaContentDto>>(
                    ApiEndpoints.MediaContent.Search, criteria);
            }
            catch (Exception ex)
            {
                return new ResponseModel<PaginatedDataModel<MediaContentDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<MediaContentDto>> SaveAsync(MediaContentDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            try
            {
                return await _apiService.PostAsync<MediaContentDto, MediaContentDto>(ApiEndpoints.MediaContent.Save, dto);
            }
            catch (Exception ex)
            {
                return new ResponseModel<MediaContentDto>
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
                var result = await _apiService.PostAsync<Guid, ResponseModel<bool>>(ApiEndpoints.MediaContent.Delete, id);
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
                var result = await _apiService.PostAsync<Guid, ResponseModel<bool>>(ApiEndpoints.MediaContent.ToggleStatus, id);
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

        public async Task<ResponseModel<bool>> UpdateDisplayOrderAsync(Guid id, int displayOrder)
        {
            try
            {
                var request = new { Id = id, DisplayOrder = displayOrder };
                var result = await _apiService.PostAsync<object, ResponseModel<bool>>(ApiEndpoints.MediaContent.UpdateDisplayOrder, request);
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
