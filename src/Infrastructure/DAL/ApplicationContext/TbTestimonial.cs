using Domains.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace DAL.ApplicationContext
{
    public class TbTestimonial : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string CustomerTitle { get; set; } = string.Empty;

        [StringLength(200)]
        public string? CustomerImagePath { get; set; }

        [Required]
        [StringLength(1000)]
        public string TestimonialText { get; set; } = string.Empty;

        public int DisplayOrder { get; set; } = 0;
    }
}