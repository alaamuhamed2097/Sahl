namespace Shared.DTOs.Merchandising.Homepage
{
    /// <summary>
    /// Complete homepage response
    /// </summary>
    public class GetHomepageResponse
    {
        public List<HomepageBlockDto> Blocks { get; set; } = new();
        public int TotalBlocks { get; set; }
        public DateTime LoadedAt { get; set; }
    }
}