using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Shared.DTOs.ECommerce.Category;
using Shared.DTOs.ECommerce.Item;

namespace Dashboard.Pages.Catalog.Products.Components
{
    public partial class AttributeCombinations
    {
        [Parameter]
        public List<ItemAttributeCombinationPricingDto> Combinations { get; set; } = new();

        [Parameter]
        public List<CategoryAttributeDto> CategoryAttributes { get; set; } = new();

        [Parameter]
        public List<ItemAttributeDto> ItemAttributes { get; set; } = new();

        [Parameter]
        public EventCallback<ItemAttributeCombinationPricingDto> OnRemoveCombination { get; set; }

        private string GetCombinationAttributesDisplay(string attributeIds)
        {
            if (string.IsNullOrEmpty(attributeIds))
                return string.Empty;

            var ids = attributeIds.Split(',');
            var attributes = new List<string>();

            foreach (var id in ids)
            {
                if (Guid.TryParse(id, out var attributeId))
                {
                    var attribute = CategoryAttributes.FirstOrDefault(a => a.Id == attributeId);
                    var value = ItemAttributes.FirstOrDefault(a => a.AttributeId == attributeId)?.Value;

                    if (attribute != null && !string.IsNullOrEmpty(value))
                    {
                        attributes.Add($"{attribute.Title}: {value}");
                    }
                }
            }

            return string.Join(" | ", attributes);
        }

        private async Task RemoveCombination(ItemAttributeCombinationPricingDto combination)
        {
            await OnRemoveCombination.InvokeAsync(combination);
        }
    }
}