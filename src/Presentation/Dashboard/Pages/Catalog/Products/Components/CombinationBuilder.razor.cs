//using Common.Enumerations.Pricing;
//using Microsoft.AspNetCore.Components;
//using Microsoft.JSInterop;
//using Shared.DTOs.ECommerce.Category;
//using Shared.DTOs.ECommerce.Item;
//using static Dashboard.Pages.Catalog.Products.Components.AttributeValuesSection;

//namespace Dashboard.Pages.Catalog.Products.Components
//{
//    public partial class CombinationBuilder
//    {
//        [Parameter]
//        public List<ItemCombinationDto> Combinations { get; set; } = new();

//        [Parameter]
//        public List<CategoryAttributeDto> CategoryAttributes { get; set; } = new();

//        [Parameter]
//        public Dictionary<Guid, List<string>> PricingAttributeValues { get; set; } = new();

//        [Parameter]
//        public Dictionary<Guid, Dictionary<string, PriceModifierInfo>> ValuePriceModifiers { get; set; } = new();

//        [Parameter]
//        public EventCallback<ItemCombinationDto> OnAddCombination { get; set; }

//        [Parameter]
//        public EventCallback<ItemCombinationDto> OnUpdateCombination { get; set; }

//        [Parameter]
//        public EventCallback<ItemCombinationDto> OnRemoveCombination { get; set; }

//        [Inject] protected IJSRuntime JSRuntime { get; set; }

//        // Modal state
//        protected ItemCombinationDto EditingCombination { get; set; }
//        protected ItemCombinationDto TempCombination { get; set; } = new();

//        // Use a proper property instead of a local variable for binding
//        protected Dictionary<Guid, string> SelectedAttributeValues { get; set; } = new();

//        protected List<AttributeSelection> AttributeSelections { get; set; } = new();

//        protected override void OnParametersSet()
//        {
//            base.OnParametersSet();

//            // Initialize AttributeSelections for pricing attributes
//            AttributeSelections = CategoryAttributes?
//                .Select(attr => new AttributeSelection
//                {
//                    AttributeId = attr.AttributeId, // FIX: Use AttributeId
//                    SelectedValue = ""
//                })
//                .ToList() ?? new();
//        }

//        // Method to get selected attribute value for binding
//        protected string GetSelectedAttributeValue(Guid attributeId)
//        {
//            // Try to find the selection
//            var selection = AttributeSelections.FirstOrDefault(s => s.AttributeId == attributeId);
//            return selection?.SelectedValue ?? "";
//        }

//        // Method to set selected attribute value when changed
//        protected void SetSelectedAttributeValue(Guid attributeId, string value)
//        {
//            var selection = AttributeSelections.FirstOrDefault(s => s.AttributeId == attributeId);
//            if (selection != null)
//            {
//                selection.SelectedValue = value;
//            }
//            else
//            {
//                Console.WriteLine($"⚠️ Attribute {attributeId} not found in AttributeSelections");
//            }
//        }

//        protected async Task AddNewCombination()
//        {
//            EditingCombination = null;
//            TempCombination = new ItemCombinationDto
//            {
//                CombinationAttributes = new List<CombinationAttributeDto>(),
//                IsDefault = !Combinations.Any()
//            };

//            // Initialize selections using AttributeId
//            AttributeSelections = CategoryAttributes
//                .Select(attr => new AttributeSelection
//                {
//                    AttributeId = attr.AttributeId,  // FIX: Use AttributeId
//                    SelectedValue = ""
//                })
//                .ToList();

//            await JSRuntime.InvokeVoidAsync("eval",
//                "new bootstrap.Modal(document.getElementById('combinationModal')).show()");
//        }
//        protected async Task SaveCombination()
//        {

//            // Build combination attributes from selections
//            TempCombination.CombinationAttributes = new List<CombinationAttributeDto>();

//            var selectedCount = 0;
//            foreach (var selection in AttributeSelections.Where(s => !string.IsNullOrEmpty(s.SelectedValue)))
//            {
//                selectedCount++;

//                var attribute = CategoryAttributes.FirstOrDefault(a => a.AttributeId == selection.AttributeId);

//                if (attribute != null)
//                {
//                    // Create the combination attribute value
//                    var combinationAttrValue = new CombinationAttributeValueDto
//                    {
//                        AttributeId = selection.AttributeId,
//                        Value = selection.SelectedValue,
//                        AttributeValuePriceModifiers = new List<AttributeValuePriceModifierDto>()
//                    };

//                    // Add price modifiers if they exist
//                    if (ValuePriceModifiers != null &&
//                        ValuePriceModifiers.ContainsKey(selection.AttributeId) &&
//                        ValuePriceModifiers[selection.AttributeId].ContainsKey(selection.SelectedValue))
//                    {
//                        var modifierInfo = ValuePriceModifiers[selection.AttributeId][selection.SelectedValue];
//                        combinationAttrValue.AttributeValuePriceModifiers.Add(new AttributeValuePriceModifierDto
//                        {
//                            ModifierType = modifierInfo.Type,
//                            ModifierValue = modifierInfo.Value,
//                            PriceModifierCategory = modifierInfo.Category
//                        });
//                    }
//                    else
//                    {
//                        Console.WriteLine($"  ⚠️ No price modifier found for this value");
//                    }

//                    var combinationAttr = new CombinationAttributeDto
//                    {
//                        combinationAttributeValueDtos = new List<CombinationAttributeValueDto>
//                {
//                    combinationAttrValue
//                }
//                    };

//                    TempCombination.CombinationAttributes.Add(combinationAttr);
//                }
//                else
//                {
//                    Console.WriteLine($"⚠️ Could not find attribute with AttributeId: {selection.AttributeId}");
//                }
//            }

//            // Check if all attributes are selected (if this is your business requirement)
//            if (TempCombination.CombinationAttributes.Count < CategoryAttributes.Count)
//            {
//                await JSRuntime.InvokeVoidAsync("swal", "Validation Error",
//                    $"Please fill in all required attributes. {TempCombination.CombinationAttributes.Count}/{CategoryAttributes.Count} selected.", "error");
//                return;
//            }

//            // Original validation: at least one attribute must be selected
//            if (!TempCombination.CombinationAttributes.Any())
//            {
//                await JSRuntime.InvokeVoidAsync("swal", "Validation Error", "At least one attribute must be selected", "error");
//                return;
//            }

//            // Check for duplicates
//            if (IsDuplicateCombination(TempCombination))
//            {
//                await JSRuntime.InvokeVoidAsync("swal", "Validation Error",
//                    "This combination already exists", "error");
//                return;
//            }

//            // Ensure only one default combination
//            if (TempCombination.IsDefault)
//            {
//                foreach (var combo in Combinations)
//                {
//                    if (combo != EditingCombination)
//                        combo.IsDefault = false;
//                }
//            }

//            if (EditingCombination == null)
//            {
//                await OnAddCombination.InvokeAsync(TempCombination);
//            }
//            else
//            {
//                await OnUpdateCombination.InvokeAsync(TempCombination);
//            }

//            // Close modal
//            await JSRuntime.InvokeVoidAsync("eval",
//                "bootstrap.Modal.getInstance(document.getElementById('combinationModal')).hide()");
//        }

//        protected async Task RemoveCombination(ItemCombinationDto combination)
//        {
//            var confirmed = await JSRuntime.InvokeAsync<bool>("swal", new
//            {
//                title = "Confirm Delete",
//                text = "Are you sure you want to delete this combination?",
//                icon = "warning",
//                buttons = new { confirm = true },
//                dangerMode = true
//            });

//            if (confirmed)
//            {
//                await OnRemoveCombination.InvokeAsync(combination);
//            }
//        }

//        protected bool IsDuplicateCombination(ItemCombinationDto combination)
//        {
//            // Get all attribute values as a sorted list
//            var currentValues = combination.CombinationAttributes
//                .SelectMany(ca => ca.combinationAttributeValueDtos)
//                .OrderBy(v => v.AttributeId)
//                .ThenBy(v => v.Value)
//                .Select(v => $"{v.AttributeId}:{v.Value}")
//                .ToList();

//            // Compare with existing combinations
//            foreach (var existing in Combinations)
//            {
//                if (EditingCombination != null && existing.Id == EditingCombination.Id)
//                    continue;

//                var existingValues = existing.CombinationAttributes
//                    .SelectMany(ca => ca.combinationAttributeValueDtos)
//                    .OrderBy(v => v.AttributeId)
//                    .ThenBy(v => v.Value)
//                    .Select(v => $"{v.AttributeId}:{v.Value}")
//                    .ToList();

//                if (currentValues.SequenceEqual(existingValues))
//                    return true;
//            }

//            return false;
//        }

//        protected string GetCombinationAttributesDisplay(ItemCombinationDto combination)
//        {
//            if (combination.CombinationAttributes == null || !combination.CombinationAttributes.Any())
//                return "No attributes";

//            var attributes = new List<string>();

//            foreach (var combinationAttr in combination.CombinationAttributes)
//            {
//                foreach (var attrValue in combinationAttr.combinationAttributeValueDtos)
//                {
//                    // FIX: Use AttributeId to find the attribute
//                    var attribute = CategoryAttributes.FirstOrDefault(a => a.AttributeId == attrValue.AttributeId);
//                    if (attribute != null)
//                    {
//                        var displayValue = GetDisplayValue(attribute, attrValue.Value);
//                        attributes.Add($"{attribute.Title}: {displayValue}");
//                    }
//                }
//            }

//            return string.Join(" | ", attributes);
//        }

//        private string GetDisplayValue(CategoryAttributeDto attribute, string value)
//        {
//            if (attribute.AttributeOptions != null)
//            {
//                var option = attribute.AttributeOptions.FirstOrDefault(o => o.Id.ToString() == value);
//                if (option != null)
//                    return option.Title;
//            }
//            return value ?? "";
//        }

//        // Helper method to check if we have valid attribute values
//        protected bool HasValidAttributeValues()
//        {
//            if (PricingAttributeValues == null || !PricingAttributeValues.Any())
//                return false;

//            // Check if at least one attribute has non-empty values
//            return PricingAttributeValues.Any(kvp =>
//                kvp.Value != null && kvp.Value.Any(v => !string.IsNullOrWhiteSpace(v)));
//        }
//    }

//    public class AttributeSelection
//    {
//        public Guid AttributeId { get; set; }
//        public string SelectedValue { get; set; } = "";
//    }
//}