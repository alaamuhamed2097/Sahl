namespace Domains.Entities.Items
{
    using Domains.Entities.Base;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class TbCategory : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string TitleAr { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string TitleEn { get; set; } = null!;

        public Guid? ParentId { get; set; }

        [Required]
        public bool IsFinal { get; set; }

        public virtual ICollection<TbCategoryAttribute> CategoryAttributes { get; set; }
        public virtual ICollection<TbItem> Items { get; set; }
    }
}
