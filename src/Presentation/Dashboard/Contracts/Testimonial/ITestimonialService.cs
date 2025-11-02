using Dashboard.Models.pagintion;
using Shared.DTOs.Testimonial;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Dashboard.Contracts.Testimonial
{
    public interface ITestimonialService
    {
        /// <summary>
        /// Get all testimonials.
        /// </summary>
        Task<ResponseModel<IEnumerable<TestimonialDto>>> GetAllAsync();

        /// <summary>
        /// Get testimonial by ID.
        /// </summary>
        Task<ResponseModel<TestimonialDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Search testimonials with pagination and filtering.
        /// </summary>
        Task<ResponseModel<PaginatedDataModel<TestimonialDto>>> SearchAsync(BaseSearchCriteriaModel criteria);

        /// <summary>
        /// Save or update a testimonial.
        /// </summary>
        Task<ResponseModel<string>> SaveAsync(TestimonialDto dto);

        /// <summary>
        /// Delete a testimonial by ID.
        /// </summary>
        Task<ResponseModel<string>> DeleteAsync(Guid id);
    }
}