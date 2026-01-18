namespace Shared.DTOs.Merchandising.Homepage
{
    /// <summary>
    /// DTO for admin block list view
    /// </summary>
    public class AdminBlockListDto
    {
        public Guid Id { get; set; }

        public string TitleAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;
        public string Layout { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public bool IsVisible { get; set; }

        public DateTime? VisibleFrom { get; set; }
        public DateTime? VisibleTo { get; set; }

        public DateTime CreatedDateUtc { get; set; }
        public DateTime? LastModifiedDateUtc { get; set; }

        // Preview & Enhanced Info
        public string? PreviewImageUrl { get; set; }
        public string? PreviewDescription { get; set; }
        public bool IsPreviewMode { get; set; }

        // Computed properties
        public bool IsCurrentlyVisible
        {
            get
            {
                var now = DateTime.UtcNow;
                if (!IsVisible) return false;
                if (VisibleFrom.HasValue && now < VisibleFrom) return false;
                if (VisibleTo.HasValue && now > VisibleTo) return false;
                return true;
            }
        }

        public string StatusBadge
        {
            get
            {
                if (!IsVisible) return "Hidden";
                if (IsCurrentlyVisible) return "Active";
                return "Scheduled";
            }
        }
    }
}
