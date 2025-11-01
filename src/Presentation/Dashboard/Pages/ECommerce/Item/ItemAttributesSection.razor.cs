using Microsoft.AspNetCore.Components;
using Shared.DTOs.ECommerce.Category;
using Shared.DTOs.ECommerce.Item;

namespace Dashboard.Pages.ECommerce.Item
{
    public partial class ItemAttributesSection
    {
        [Parameter]
        public List<CategoryAttributeDto> CategoryAttributes { get; set; } = new();

        [Parameter]
        public List<ItemAttributeDto> ItemAttributes { get; set; } = new();

        [Parameter]
        public EventCallback<Guid> OnRemoveAttribute { get; set; }

        private async Task RemoveAttribute(Guid attributeId)
        {
            var existing = ItemAttributes.FirstOrDefault(a => a.AttributeId == attributeId);
            if (existing != null)
                ItemAttributes.Remove(existing);

            await OnRemoveAttribute.InvokeAsync(attributeId);
        }
    }
}
