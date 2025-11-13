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

        // Store multiple values for pricing attributes
        private Dictionary<Guid, List<string>> _multiValueAttributes = new();

        private async Task RemoveAttribute(Guid attributeId)
        {
            var existing = ItemAttributes.FirstOrDefault(a => a.AttributeId == attributeId);
            if (existing != null)
                ItemAttributes.Remove(existing);

            // Clean up multi-value storage
            if (_multiValueAttributes.ContainsKey(attributeId))
                _multiValueAttributes.Remove(attributeId);

            await OnRemoveAttribute.InvokeAsync(attributeId);
        }

        /// <summary>
        /// Toggles option selection for multi-select attributes
        /// </summary>
        private async Task ToggleOption(ItemAttributeDto attribute, Guid optionId)
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

            // FIX: Use AttributeId instead of Id
            var categoryAttr = CategoryAttributes.FirstOrDefault(ca => ca.AttributeId == attribute.AttributeId);
            if (categoryAttr?.AffectsPricing == true)
            {
                await OnGenerateCombinations.InvokeAsync();
            }

            await OnAttributeValueChanged.InvokeAsync();
        }

        /// <summary>
        /// Handles value change for pricing attributes to auto-generate combinations
        /// </summary>
        private async Task HandlePricingAttributeValueChanged(ItemAttributeDto attribute)
        {
            // FIX: Use AttributeId instead of Id
            var categoryAttr = CategoryAttributes.FirstOrDefault(ca => ca.AttributeId == attribute.AttributeId);
            if (categoryAttr?.AffectsPricing == true)
            {
                await OnGenerateCombinations.InvokeAsync();
            }
            await OnAttributeValueChanged.InvokeAsync();
        }

        /// <summary>
        /// Gets or initializes the multi-value list for an attribute
        /// </summary>
        private List<string> GetMultiValueList(Guid attributeId, string currentValue)
        {
            if (!_multiValueAttributes.ContainsKey(attributeId))
            {
                // Initialize from current value if exists
                if (!string.IsNullOrWhiteSpace(currentValue))
                {
                    _multiValueAttributes[attributeId] = currentValue
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(v => v.Trim())
                        .ToList();
                }
                else
                {
                    _multiValueAttributes[attributeId] = new List<string> { "" };
                }
            }
            return _multiValueAttributes[attributeId];
        }

        /// <summary>
        /// Adds a new value input for pricing attribute
        /// </summary>
        private void AddNewValue(Guid attributeId)
        {
            var list = GetMultiValueList(attributeId, "");
            list.Add("");
            StateHasChanged();
        }

        /// <summary>
        /// Removes a value from the list
        /// </summary>
        private async Task RemoveValue(Guid attributeId, int index, ItemAttributeDto attribute)
        {
            var list = GetMultiValueList(attributeId, attribute.Value);
            if (list.Count > 1 && index < list.Count)
            {
                list.RemoveAt(index);
                UpdateAttributeValue(attributeId, attribute);
                await HandlePricingAttributeValueChanged(attribute);
            }
        }

        /// <summary>
        /// Updates the attribute value from the multi-value list
        /// </summary>
        private void UpdateAttributeValue(Guid attributeId, ItemAttributeDto attribute)
        {
            if (_multiValueAttributes.ContainsKey(attributeId))
            {
                var values = _multiValueAttributes[attributeId]
                    .Where(v => !string.IsNullOrWhiteSpace(v))
                    .ToList();
                attribute.Value = string.Join(",", values);
            }
        }

        /// <summary>
        /// Handles value change in multi-value input
        /// </summary>
        private async Task OnMultiValueChanged(Guid attributeId, int index, string newValue, ItemAttributeDto attribute)
        {
            var list = GetMultiValueList(attributeId, attribute.Value);
            if (index < list.Count)
            {
                list[index] = newValue?.Trim() ?? "";
                UpdateAttributeValue(attributeId, attribute);
                await HandlePricingAttributeValueChanged(attribute);
            }
        }
    }
}
