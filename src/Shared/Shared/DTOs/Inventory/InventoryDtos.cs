using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Inventory
{
    public class MoitemDto
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string DocumentNumber { get; set; } = string.Empty;

        public DateTime DocumentDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(20)]
        public string MovementType { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Notes { get; set; }

        public string? UserId { get; set; }

        public decimal TotalAmount { get; set; }

        public List<MovitemsdetailDto> Details { get; set; } = new();
    }

    public class MortemDto
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string DocumentNumber { get; set; } = string.Empty;

        public DateTime DocumentDate { get; set; } = DateTime.UtcNow;

        [StringLength(500)]
        public string? Reason { get; set; }

        public Guid? OrderId { get; set; }

        public string? UserId { get; set; }

        public decimal TotalAmount { get; set; }

        public int Status { get; set; }

        public List<MovitemsdetailDto> Details { get; set; } = new();
    }

    public class MovitemsdetailDto
    {
        public Guid Id { get; set; }

        public Guid? MoitemId { get; set; }

        public Guid? MortemId { get; set; }

        [Required]
        public Guid ItemId { get; set; }

        [Required]
        public Guid WarehouseId { get; set; }

        public Guid? ItemAttributeCombinationPricingId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public string? ItemTitle { get; set; }
        public string? WarehouseTitle { get; set; }
    }
}
