using Dashboard.Constants;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Testimonial;
using Dashboard.Models.pagintion;
using Shared.DTOs.Testimonial;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Dashboard.Services.Testimonial
{
    public class TestimonialService : ITestimonialService
    {
        private readonly IApiService _apiService;

        public TestimonialService(IApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Get all testimonials.
        /// </summary>
        public async Task<ResponseModel<IEnumerable<TestimonialDto>>> GetAllAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<TestimonialDto>>(ApiEndpoints.Testimonial.Get);
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<IEnumerable<TestimonialDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Get testimonial by ID.
        /// </summary>
        public async Task<ResponseModel<TestimonialDto>> GetByIdAsync(Guid id)
        {
            try
            {
                return await _apiService.GetAsync<TestimonialDto>($"{ApiEndpoints.Testimonial.Get}/{id}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<TestimonialDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Search testimonials with pagination and filtering.
        /// </summary>
        public async Task<ResponseModel<PaginatedDataModel<TestimonialDto>>> SearchAsync(BaseSearchCriteriaModel criteria)
        {
            if (criteria == null) throw new ArgumentNullException(nameof(criteria));

            try
            {
                return await _apiService.PostAsync<BaseSearchCriteriaModel, PaginatedDataModel<TestimonialDto>>(
                    ApiEndpoints.Testimonial.Search, criteria);
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<PaginatedDataModel<TestimonialDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Save or update a testimonial.
        /// </summary>
        public async Task<ResponseModel<string>> SaveAsync(TestimonialDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            try
            {
                return await _apiService.PostAsync<TestimonialDto, string>(ApiEndpoints.Testimonial.Save, dto);
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

        /// <summary>
        /// Delete a testimonial by ID.
        /// </summary>
        public async Task<ResponseModel<string>> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _apiService.PostAsync<Guid, ResponseModel<string>>(ApiEndpoints.Testimonial.Delete, id);
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