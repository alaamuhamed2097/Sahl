using Common.Filters;
using Dashboard.Constants;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Page;
using Dashboard.Models.pagintion;
using Shared.DTOs.Page;
using Shared.GeneralModels;

namespace Dashboard.Services.Page
{
    public class PageService : IPageService
    {
        private readonly IApiService _apiService;

        public PageService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ResponseModel<PageDto>> GetByIdAsync(Guid id)
        {
            try
            {
                return await _apiService.GetAsync<PageDto>($"{ApiEndpoints.Page.GetById}/{id}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<PageDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<IEnumerable<PageDto>>> GetAllAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<PageDto>>($"{ApiEndpoints.Page.Get}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<IEnumerable<PageDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<PageDto>> GetBySlugAsync(string slug)
        {
            try
            {
                return await _apiService.GetAsync<PageDto>($"{ApiEndpoints.Page.GetBySlug}/{slug}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<PageDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<PaginatedDataModel<PageDto>>> SearchAsync(BaseSearchCriteriaModel criteria)
        {
            try
            {
                var queryString = $"?searchTerm={criteria.SearchTerm}&pageNumber={criteria.PageNumber}&pageSize={criteria.PageSize}";
                return await _apiService.GetAsync<PaginatedDataModel<PageDto>>($"{ApiEndpoints.Page.Search}{queryString}");
            }
            catch (Exception ex)
            {
                return new ResponseModel<PaginatedDataModel<PageDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<bool>> SaveAsync(PageDto pageDto)
        {
            if (pageDto == null) throw new ArgumentNullException(nameof(pageDto));
            try
            {
                return await _apiService.PostAsync<PageDto, bool>($"{ApiEndpoints.Page.Save}", pageDto);
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
                return await _apiService.PostAsync<Guid, bool>($"{ApiEndpoints.Page.Delete}", id);
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<bool>> ToggleStatusAsync(Guid id)
        {
            try
            {
                return await _apiService.PostAsync<Guid, bool>($"{ApiEndpoints.Page.ToggleStatus}", id);
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}