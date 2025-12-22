namespace Shared.DTOs.ECommerce.Item
{
    /// <summary>
    /// Request body for POST /api/items/{id}/combination
    /// </summary>
    public class CombinationRequest
    {
        public List<AttributeSelectionDto> SelectedValueIds { get; set; } = new List<AttributeSelectionDto>();
    }
}
