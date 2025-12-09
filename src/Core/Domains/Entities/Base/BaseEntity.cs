using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Base
{
    //Base class for entities common properties
    public class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public bool IsDeleted { get; set; }

        // Changed back to Guid from string
        public Guid CreatedBy { get; set; }

        public DateTime CreatedDateUtc { get; set; }

        // Changed back to Guid? from string?
        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedDateUtc { get; set; }
    }
}
