namespace Shared.DTOs.Merchandising.Homepage
{
    /// <summary>
    /// DTO for reordering blocks via drag & drop
    /// </summary>
    public class BlockReorderDto
    {
        public Guid BlockId { get; set; }
        public int NewDisplayOrder { get; set; }
    }

    /// <summary>
    /// Request model for batch reordering blocks
    /// </summary>
    public class BlockReorderRequestDto
    {
        public List<BlockReorderDto> ReorderedBlocks { get; set; } = new();
    }
}
