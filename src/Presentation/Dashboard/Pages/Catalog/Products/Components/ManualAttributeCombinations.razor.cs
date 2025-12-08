// ManualAttributeCombinations.razor.cs
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shared.DTOs.ECommerce.Category;
using Shared.DTOs.ECommerce.Item;

namespace Dashboard.Pages.Catalog.Products.Components
{
    public partial class ManualAttributeCombinations
    {
        [Parameter]
        public List<ItemCombinationDto> Combinations { get; set; } = new();

        [Parameter]
        public List<CategoryAttributeDto> CategoryAttributes { get; set; } = new();

        [Parameter]
        public EventCallback<ItemCombinationDto> OnAddCombination { get; set; }

        [Parameter]
        public EventCallback<ItemCombinationDto> OnUpdateCombination { get; set; }

        [Parameter]
        public EventCallback<ItemCombinationDto> OnRemoveCombination { get; set; }

        [Inject] protected IJSRuntime JSRuntime { get; set; }

        // Modal state
        protected ItemCombinationDto EditingCombination { get; set; }
        protected ItemCombinationDto TempCombination { get; set; } = new();
        protected string SelectedAttributeId { get; set; } = string.Empty;
        protected string SelectedAttributeValue { get; set; } = string.Empty;
        protected AttributeValuePriceModifierDto TempModifier { get; set; } = new();

        protected async Task AddNewCombination()
        {
            EditingCombination = null;
            TempCombination = new ItemCombinationDto
            {
                CombinationAttributes = new List<CombinationAttributeDto>(),
                BasePrice = 0,
                IsDefault = !Combinations.Any() // First combination is default by default
            };

            SelectedAttributeId = string.Empty;
            SelectedAttributeValue = string.Empty;
            TempModifier = new AttributeValuePriceModifierDto();

            await JSRuntime.InvokeVoidAsync("showModal", "combinationModal");
        }

        protected async Task SaveCombination()
        {
            // Validate
            if (TempCombination.BasePrice <= 0)
            {
                await JSRuntime.InvokeVoidAsync("swal", "Validation Error", "Base price must be greater than 0", "error");
                return;
            }

            if (!TempCombination.CombinationAttributes.Any())
            {
                await JSRuntime.InvokeVoidAsync("swal", "Validation Error", "At least one attribute is required", "error");
                return;
            }

            // Check for duplicates
            if (IsDuplicateCombination(TempCombination))
            {
                await JSRuntime.InvokeVoidAsync("swal", "Validation Error", "This combination already exists", "error");
                return;
            }

            // Ensure only one default combination
            if (TempCombination.IsDefault)
            {
                foreach (var combo in Combinations)
                {
                    if (combo != EditingCombination)
                        combo.IsDefault = false;
                }
            }

            if (EditingCombination == null)
            {
                // Add new combination
                await OnAddCombination.InvokeAsync(TempCombination);
            }
            else
            {
                // Update existing combination
                await OnUpdateCombination.InvokeAsync(TempCombination);
            }

            await JSRuntime.InvokeVoidAsync("hideModal", "combinationModal");
        }

        protected void AddAttributeToCombination()
        {
            if (string.IsNullOrEmpty(SelectedAttributeId) || string.IsNullOrEmpty(SelectedAttributeValue))
                return;

            var attributeId = Guid.Parse(SelectedAttributeId);
            var categoryAttr = CategoryAttributes.FirstOrDefault(a => a.Id == attributeId);

            if (categoryAttr == null)
                return;

            // Find or create the combination attribute
            var combinationAttr = TempCombination.CombinationAttributes
                .FirstOrDefault(ca => ca.combinationAttributeValueDtos.Any(v => v.AttributeId == attributeId));

            if (combinationAttr == null)
            {
                combinationAttr = new CombinationAttributeDto
                {
                    combinationAttributeValueDtos = new List<CombinationAttributeValueDto>()
                };
                TempCombination.CombinationAttributes.Add(combinationAttr);
            }

            // Check if this attribute value already exists
            if (combinationAttr.combinationAttributeValueDtos.Any(v => v.Value == SelectedAttributeValue))
            {
                return; // Already exists
            }

            // Create the attribute value
            var attributeValue = new CombinationAttributeValueDto
            {
                AttributeId = attributeId,
                Value = SelectedAttributeValue,
                AttributeValuePriceModifiers = new List<AttributeValuePriceModifierDto> { TempModifier }
            };

            combinationAttr.combinationAttributeValueDtos.Add(attributeValue);

            // Reset form
            SelectedAttributeId = string.Empty;
            SelectedAttributeValue = string.Empty;
            TempModifier = new AttributeValuePriceModifierDto();
        }

        protected void RemoveAttributeFromCombination(CombinationAttributeDto attr, CombinationAttributeValueDto val)
        {
            attr.combinationAttributeValueDtos.Remove(val);

            // Remove the attribute if no values left
            if (!attr.combinationAttributeValueDtos.Any())
            {
                TempCombination.CombinationAttributes.Remove(attr);
            }
        }

        protected async Task RemoveCombination(ItemCombinationDto combination)
        {
            var confirmed = await JSRuntime.InvokeAsync<bool>("swal", new
            {
                title = "Confirm Delete",
                text = "Are you sure you want to delete this combination?",
                icon = "warning",
                buttons = new { confirm = true },
                dangerMode = true
            });

            if (confirmed)
            {
                await OnRemoveCombination.InvokeAsync(combination);
            }
        }

        protected bool IsDuplicateCombination(ItemCombinationDto combination)
        {
            // Skip if we're editing the same combination
            if (EditingCombination != null && EditingCombination.Id == combination.Id)
                return false;

            // Get all attribute values as a sorted list
            var currentValues = combination.CombinationAttributes
                .SelectMany(ca => ca.combinationAttributeValueDtos)
                .OrderBy(v => v.AttributeId)
                .ThenBy(v => v.Value)
                .Select(v => $"{v.AttributeId}:{v.Value}")
                .ToList();

            // Compare with existing combinations
            foreach (var existing in Combinations)
            {
                if (EditingCombination != null && existing.Id == EditingCombination.Id)
                    continue;

                var existingValues = existing.CombinationAttributes
                    .SelectMany(ca => ca.combinationAttributeValueDtos)
                    .OrderBy(v => v.AttributeId)
                    .ThenBy(v => v.Value)
                    .Select(v => $"{v.AttributeId}:{v.Value}")
                    .ToList();

                if (currentValues.SequenceEqual(existingValues))
                    return true;
            }

            return false;
        }

        protected string GetCombinationAttributesDisplay(ItemCombinationDto combination)
        {
            if (combination.CombinationAttributes == null || !combination.CombinationAttributes.Any())
                return "No attributes";

            var attributes = new List<string>();

            foreach (var combinationAttr in combination.CombinationAttributes)
            {
                foreach (var attrValue in combinationAttr.combinationAttributeValueDtos)
                {
                    var attribute = CategoryAttributes.FirstOrDefault(a => a.Id == attrValue.AttributeId);
                    if (attribute != null)
                    {
                        attributes.Add($"{attribute.Title}: {attrValue.Value}");
                    }
                }
            }

            return string.Join(" | ", attributes);
        }
    }
}