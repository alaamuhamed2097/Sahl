using System.ComponentModel.DataAnnotations;

namespace Domins.Entities.Base
{
    //Base class for entities common properties
    public class BaseSeo : BaseEntity
    {
        [MaxLength(250)]
        public string SEOTitle { get; set; } = null!;

        [MaxLength(1000)]
        public string SEODescription { get; set; } = null!;

        [MaxLength(1000)]
        public string SEOMetaTags { get; set; } = null!;
    }
}
