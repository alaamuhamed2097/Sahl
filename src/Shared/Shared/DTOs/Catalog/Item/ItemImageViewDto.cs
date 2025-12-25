using System.Text.Json.Serialization;

namespace Shared.DTOs.Catalog.Item
{
    public class ItemImageViewDto
    {
        [JsonPropertyName("Id")]
        public Guid Id { get; set; }

        [JsonPropertyName("Path")]
        public string Path { get; set; } = null!;

        [JsonPropertyName("Order")]
        public int Order { get; set; }
    }
}