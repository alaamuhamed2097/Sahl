using Common.Filters;
using Dashboard.Constants;
using Dashboard.Contracts.Brand;
using Dashboard.Contracts.General;
using Dashboard.Models.pagintion;
using Resources;
using Shared.DTOs.Brand;
using Shared.GeneralModels;

namespace Dashboard.Services.Brand
{
    public class BrandService : IBrandService
    {
        private readonly IApiService _apiService;

        public BrandService(IApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Get all brands.
        /// </summary>
        public async Task<ResponseModel<IEnumerable<BrandDto>>> GetAllAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<BrandDto>>(ApiEndpoints.Brand.Get);
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<IEnumerable<BrandDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Get brand by ID.
        /// </summary>
        public async Task<ResponseModel<BrandDto>> GetByIdAsync(Guid id)
        {
            try
            {
                return await _apiService.GetAsync<BrandDto>($"{ApiEndpoints.Brand.Get}/{id}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<BrandDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Get favorite brands.
        /// </summary>
        public async Task<ResponseModel<IEnumerable<BrandDto>>> GetFavoritesAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<BrandDto>>(ApiEndpoints.Brand.GetFavorites);
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<IEnumerable<BrandDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Search brands with pagination and filtering.
        /// </summary>
        public async Task<ResponseModel<PaginatedDataModel<BrandDto>>> SearchAsync(BaseSearchCriteriaModel criteria)
        {
            if (criteria == null) throw new ArgumentNullException(nameof(criteria));

            try
            {
                return await _apiService.PostAsync<BaseSearchCriteriaModel, PaginatedDataModel<BrandDto>>(
                    ApiEndpoints.Brand.Search, criteria);
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<PaginatedDataModel<BrandDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Save or update a brand.
        /// </summary>
        public async Task<ResponseModel<BrandDto>> SaveAsync(BrandDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            try
            {
                return await _apiService.PostAsync<BrandDto, BrandDto>(ApiEndpoints.Brand.Save, dto);
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<BrandDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Delete a brand by ID.
        /// </summary>
        public async Task<ResponseModel<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _apiService.PostAsync<Guid, ResponseModel<bool>>(ApiEndpoints.Brand.Delete, id);
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
                // Log error here
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.DeleteFailed
                };
            }
        }

        /// <summary>
        /// Mark or unmark a brand as favorite.
        /// </summary>
        public async Task<ResponseModel<string>> MarkAsFavoriteAsync(Guid brandId)
        {
            try
            {
                var result = await _apiService.PostAsync<Guid, ResponseModel<string>>(ApiEndpoints.Brand.MarkAsFavorite, brandId);
                if (result.Success)
                {
                    return new ResponseModel<string>
                    {
                        Success = true,
                        Message = result.Message
                    };
                }
                return new ResponseModel<string>
                {
                    Success = false,
                    Message = result.Message,
                    Errors = result.Errors
                };
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}