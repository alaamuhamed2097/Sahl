using Resources;
using Shared.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Testimonial
{
    public class TestimonialDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(100, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(100, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string CustomerTitle { get; set; } = string.Empty;

        // Changed to Base64 for image upload
        [RequiredImageOnCreate]
        public string? Base64Image { get; set; } // Base64 from Blazor input
        public string? CustomerImagePath { get; set; }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(1000, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string TestimonialText { get; set; } = string.Empty;

        public int DisplayOrder { get; set; } = 0;

        public DateTime CreatedDateUtc { get; set; }

        public DateTime? UpdatedDateUtc { get; set; }
    }
}