using Microsoft.AspNetCore.Components;
using Shared.DTOs.ECommerce.Category;
using Shared.DTOs.ECommerce.Item;

namespace Dashboard.Pages.Catalog.Products.Components
{
    public partial class ItemAttributesSection
    {
        [Parameter]
        public List<CategoryAttributeDto> CategoryAttributes { get; set; } = new();

        [Parameter]
        public List<ItemAttributeDto> ItemAttributes { get; set; } = new();

        [Parameter]
        public EventCallback<Guid> OnRemoveAttribute { get; set; }

        [Parameter]
        public EventCallback OnGenerateCombinations { get; set; }

        [Parameter]
        public EventCallback OnAttributeValueChanged { get; set; }

        private async Task RemoveAttribute(Guid attributeId)
        {
            var existing = ItemAttributes.FirstOrDefault(a => a.AttributeId == attributeId);
            if (existing != null)
                ItemAttributes.Remove(existing);

            await OnRemoveAttribute.InvokeAsync(attributeId);
        }

        /// <summary>
        /// Toggles option selection for multi-select attributes
        /// </summary>
        private void ToggleOption(ItemAttributeDto attribute, Guid optionId)
        {
            var selectedOptions = (attribute.Value ?? "")
         .Split(',', StringSplitOptions.RemoveEmptyEntries)
           .ToList();

            var optionIdStr = optionId.ToString();

            if (selectedOptions.Contains(optionIdStr))
            {
                selectedOptions.Remove(optionIdStr);
            }
            else
            {
                selectedOptions.Add(optionIdStr);
            }

            attribute.Value = string.Join(",", selectedOptions);
            OnAttributeValueChanged.InvokeAsync();
        }
    }
}
