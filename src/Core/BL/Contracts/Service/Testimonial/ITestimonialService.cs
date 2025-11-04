using DAL.Models;
using Shared.DTOs.Testimonial;
using Shared.GeneralModels.SearchCriteriaModels;

namespace BL.Contracts.Service.Testimonial
{
    public interface ITestimonialService
    {
        Task<IEnumerable<TestimonialDto>> GetAllAsync();
        Task<TestimonialDto?> GetByIdAsync(Guid id);
        Task<PaginatedDataModel<TestimonialDto>> SearchAsync(BaseSearchCriteriaModel criteria);
        Task<bool> SaveAsync(TestimonialDto dto, Guid userId);
        Task<bool> DeleteAsync(Guid id, Guid userId);
    }
}