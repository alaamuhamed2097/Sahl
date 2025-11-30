using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.SellerRequest
{
    public class TbRequestComment : BaseEntity
    {
        [Required]
        [ForeignKey("SellerRequest")]
        public Guid SellerRequestId { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required]
        public string Comment { get; set; } = string.Empty;

        public bool IsInternal { get; set; }

        public virtual TbSellerRequest SellerRequest { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
    }
}
