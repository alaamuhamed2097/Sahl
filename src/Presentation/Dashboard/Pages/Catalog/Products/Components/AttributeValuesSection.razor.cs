using Common.Enumerations.FieldType;
using Common.Enumerations.Pricing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Shared.DTOs.Catalog.Category;
using Shared.DTOs.Catalog.Item;

namespace Dashboard.Pages.Catalog.Products.Components
{
    public partial class AttributeValuesSection
    {
        [Parameter]
        public List<CategoryAttributeDto> CategoryAttributes { get; set; } = new();

        [Parameter]
        public EventCallback OnValuesChanged { get; set; }

        // Non-pricing attributes: Key = AttributeId, Value = single value
        private Dictionary<Guid, string> _nonPricingAttributeValues = new();

        // Non-pricing integer values
        private Dictionary<Guid, int?> _nonPricingIntValues = new();

        // Non-pricing decimal values
        private Dictionary<Guid, decimal?> _nonPricingDecimalValues = new();

        // Non-pricing boolean values (for checkboxes)
        private Dictionary<Guid, bool> _nonPricingBoolValues = new();

        // Non-pricing date values
        private Dictionary<Guid, DateTime?> _nonPricingDateValues = new();

        // ✅ MultiSelect values storage (Checkboxes)
        // Key = AttributeId, Value = List of selected option IDs
        private Dictionary<Guid, List<string>> _multiSelectValues = new();

        public class PriceModifierInfo
        {
            public PriceModifierType Type { get; set; } = PriceModifierType.Fixed;
            public decimal Value { get; set; }
            public PriceModifierCategory Category { get; set; } = PriceModifierCategory.BasePrice;
        }

        protected override void OnInitialized()
        {
            InitializeAttributes();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            InitializeAttributes();
        }

        /// <summary>
        /// Loads existing attribute values from ItemAttributes into the component's dictionaries
        /// </summary>
        public void LoadExistingValues(List<ItemAttributeDto> itemAttributes)
        {
            if (itemAttributes == null || !itemAttributes.Any())
                return;

            Console.WriteLine($"📥 LoadExistingValues - Loading {itemAttributes.Count} attributes");

            foreach (var itemAttr in itemAttributes)
            {
                var categoryAttr = CategoryAttributes.FirstOrDefault(ca => ca.AttributeId == itemAttr.AttributeId);

                if (categoryAttr == null)
                {
                    Console.WriteLine($"⚠️ Category attribute not found for AttributeId: {itemAttr.AttributeId}");
                    continue;
                }

                if (categoryAttr.AffectsPricing)
                {
                    // Skip pricing attributes - they should be handled separately
                    continue;
                }

                // Check if this attribute has predefined options
                if (categoryAttr.AttributeOptions != null && categoryAttr.AttributeOptions.Any())
                {
                    // ✅ Handle MultiSelect attributes (Checkboxes)
                    if (categoryAttr.FieldType == FieldType.MultiSelectList)
                    {
                        // MultiSelect values are stored as comma-separated option IDs
                        if (!string.IsNullOrWhiteSpace(itemAttr.Value))
                        {
                            var selectedIds = itemAttr.Value
                                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(id => id.Trim())
                                .ToList();

                            _multiSelectValues[itemAttr.AttributeId] = selectedIds;

                            Console.WriteLine($"✅ Loaded MultiSelect (Checkboxes) - {categoryAttr.Title}: {selectedIds.Count} options selected");
                        }
                        else
                        {
                            _multiSelectValues[itemAttr.AttributeId] = new List<string>();
                        }
                    }
                    else
                    {
                        // For single-select dropdown attributes, store the option ID directly
                        _nonPricingAttributeValues[itemAttr.AttributeId] = itemAttr.Value;
                        Console.WriteLine($"✅ Loaded Dropdown - {categoryAttr.Title}: {itemAttr.Value}");
                    }
                }
                else
                {
                    // For custom input fields, parse the value based on field type
                    if (categoryAttr.FieldType == FieldType.IntegerNumber)
                    {
                        if (int.TryParse(itemAttr.Value, out int intValue))
                        {
                            _nonPricingIntValues[itemAttr.AttributeId] = intValue;
                            Console.WriteLine($"✅ Loaded Integer - {categoryAttr.Title}: {intValue}");
                        }
                    }
                    else if (categoryAttr.FieldType == FieldType.DecimalNumber)
                    {
                        if (decimal.TryParse(itemAttr.Value, out decimal decimalValue))
                        {
                            _nonPricingDecimalValues[itemAttr.AttributeId] = decimalValue;
                            Console.WriteLine($"✅ Loaded Decimal - {categoryAttr.Title}: {decimalValue}");
                        }
                    }
                    else if (categoryAttr.FieldType == FieldType.CheckBox)
                    {
                        if (bool.TryParse(itemAttr.Value, out bool boolValue))
                        {
                            _nonPricingBoolValues[itemAttr.AttributeId] = boolValue;
                            Console.WriteLine($"✅ Loaded CheckBox - {categoryAttr.Title}: {boolValue}");
                        }
                    }
                    else if (categoryAttr.FieldType == FieldType.Date)
                    {
                        if (DateTime.TryParse(itemAttr.Value, out DateTime dateValue))
                        {
                            _nonPricingDateValues[itemAttr.AttributeId] = dateValue;
                            Console.WriteLine($"✅ Loaded Date - {categoryAttr.Title}: {dateValue:yyyy-MM-dd}");
                        }
                    }
                    else
                    {
                        // Text field
                        _nonPricingAttributeValues[itemAttr.AttributeId] = itemAttr.Value;
                        Console.WriteLine($"✅ Loaded Text - {categoryAttr.Title}: {itemAttr.Value}");
                    }
                }
            }

            StateHasChanged();
        }

        private void InitializeAttributes()
        {
            // Initialize non-pricing attributes - USE AttributeId, not Id
            foreach (var attr in CategoryAttributes.Where(a => !a.AffectsPricing))
            {
                if (!_nonPricingAttributeValues.ContainsKey(attr.AttributeId))
                {
                    _nonPricingAttributeValues[attr.AttributeId] = "";
                }

                if (!_nonPricingIntValues.ContainsKey(attr.AttributeId))
                {
                    _nonPricingIntValues[attr.AttributeId] = null;
                }

                if (!_nonPricingDecimalValues.ContainsKey(attr.AttributeId))
                {
                    _nonPricingDecimalValues[attr.AttributeId] = null;
                }

                if (!_nonPricingBoolValues.ContainsKey(attr.AttributeId))
                {
                    _nonPricingBoolValues[attr.AttributeId] = false;
                }

                if (!_nonPricingDateValues.ContainsKey(attr.AttributeId))
                {
                    _nonPricingDateValues[attr.AttributeId] = null;
                }

                // ✅ Initialize MultiSelect values (Checkboxes)
                if (!_multiSelectValues.ContainsKey(attr.AttributeId))
                {
                    _multiSelectValues[attr.AttributeId] = new List<string>();
                }
            }
        }

        #region Non-Pricing Attributes

        private bool HasNonPricingValue(Guid attributeId)
        {
            // Check if it's a MultiSelect attribute (Checkboxes)
            var attr = CategoryAttributes.FirstOrDefault(a => a.AttributeId == attributeId);
            if (attr?.FieldType == FieldType.MultiSelectList)
            {
                return _multiSelectValues.ContainsKey(attributeId) &&
                       _multiSelectValues[attributeId].Any();
            }

            // For other types, check the string dictionary
            return _nonPricingAttributeValues.ContainsKey(attributeId) &&
                   !string.IsNullOrWhiteSpace(_nonPricingAttributeValues[attributeId]);
        }

        #endregion

        #region MultiSelect Checkbox Handling

        /// <summary>
        /// Handles individual checkbox change for MultiSelect attributes
        /// </summary>
        private async Task HandleCheckboxChange(Guid attributeId, string optionId, ChangeEventArgs e)
        {
            if (!_multiSelectValues.ContainsKey(attributeId))
            {
                _multiSelectValues[attributeId] = new List<string>();
            }

            bool isChecked = e.Value is bool b && b;

            if (isChecked)
            {
                // Add option if not already present
                if (!_multiSelectValues[attributeId].Contains(optionId))
                {
                    _multiSelectValues[attributeId].Add(optionId);
                    Console.WriteLine($"✅ Checkbox checked - AttributeId: {attributeId}, OptionId: {optionId}");
                }
            }
            else
            {
                // Remove option if present
                _multiSelectValues[attributeId].Remove(optionId);
                Console.WriteLine($"❌ Checkbox unchecked - AttributeId: {attributeId}, OptionId: {optionId}");
            }

            Console.WriteLine($"📊 Total selected for attribute {attributeId}: {_multiSelectValues[attributeId].Count}");

            await NotifyValuesChanged();
        }

        /// <summary>
        /// Checks if a specific option is selected in a MultiSelect attribute
        /// </summary>
        private bool IsOptionSelected(Guid attributeId, string optionId)
        {
            return _multiSelectValues.ContainsKey(attributeId) &&
                   _multiSelectValues[attributeId].Contains(optionId);
        }

        /// <summary>
        /// Removes a specific option from MultiSelect attribute (via badge button)
        /// </summary>
        private async Task RemoveMultiSelectOption(Guid attributeId, string optionId)
        {
            if (_multiSelectValues.ContainsKey(attributeId))
            {
                _multiSelectValues[attributeId].Remove(optionId);
                Console.WriteLine($"🗑️ Removed option via badge - AttributeId: {attributeId}, OptionId: {optionId}");
                await NotifyValuesChanged();
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Notifies the parent component that values have changed
        /// </summary>
        private async Task NotifyValuesChanged()
        {
            await OnValuesChanged.InvokeAsync();
        }

        #endregion

        /// <summary>
        /// Gets all attribute data for saving to the database
        /// </summary>
        public Dictionary<Guid, string> GetAllAttributeData()
        {
            Console.WriteLine("📤 GetAllAttributeData - Preparing attribute values for save");

            // Update non-pricing values from numeric fields and MultiSelect
            foreach (var attr in CategoryAttributes.Where(a => !a.AffectsPricing))
            {
                // ✅ Handle MultiSelect attributes (Checkboxes)
                if (attr.FieldType == FieldType.MultiSelectList)
                {
                    if (_multiSelectValues.ContainsKey(attr.AttributeId))
                    {
                        // Store MultiSelect values as comma-separated option IDs
                        var selectedIds = _multiSelectValues[attr.AttributeId];
                        _nonPricingAttributeValues[attr.AttributeId] = string.Join(",", selectedIds);

                        Console.WriteLine($"📦 MultiSelect (Checkboxes) - {attr.Title}: {selectedIds.Count} options = {_nonPricingAttributeValues[attr.AttributeId]}");
                    }
                    else
                    {
                        _nonPricingAttributeValues[attr.AttributeId] = "";
                    }
                    continue;
                }

                // Skip attributes with predefined options - they're already handled by the dropdown binding
                if (attr.AttributeOptions != null && attr.AttributeOptions.Any())
                {
                    // Dropdown values are already in _nonPricingAttributeValues
                    Console.WriteLine($"📦 Dropdown - {attr.Title}: {_nonPricingAttributeValues[attr.AttributeId]}");
                    continue;
                }

                // Handle custom field types
                if (attr.FieldType == FieldType.IntegerNumber && _nonPricingIntValues.ContainsKey(attr.AttributeId))
                {
                    _nonPricingAttributeValues[attr.AttributeId] = _nonPricingIntValues[attr.AttributeId]?.ToString() ?? "";
                    Console.WriteLine($"📦 Integer - {attr.Title}: {_nonPricingAttributeValues[attr.AttributeId]}");
                }
                else if (attr.FieldType == FieldType.DecimalNumber && _nonPricingDecimalValues.ContainsKey(attr.AttributeId))
                {
                    _nonPricingAttributeValues[attr.AttributeId] = _nonPricingDecimalValues[attr.AttributeId]?.ToString() ?? "";
                    Console.WriteLine($"📦 Decimal - {attr.Title}: {_nonPricingAttributeValues[attr.AttributeId]}");
                }
                else if (attr.FieldType == FieldType.CheckBox && _nonPricingBoolValues.ContainsKey(attr.AttributeId))
                {
                    _nonPricingAttributeValues[attr.AttributeId] = _nonPricingBoolValues[attr.AttributeId].ToString();
                    Console.WriteLine($"📦 CheckBox - {attr.Title}: {_nonPricingAttributeValues[attr.AttributeId]}");
                }
                else if (attr.FieldType == FieldType.Date && _nonPricingDateValues.ContainsKey(attr.AttributeId))
                {
                    _nonPricingAttributeValues[attr.AttributeId] = _nonPricingDateValues[attr.AttributeId]?.ToString("yyyy-MM-dd") ?? "";
                    Console.WriteLine($"📦 Date - {attr.Title}: {_nonPricingAttributeValues[attr.AttributeId]}");
                }
                else
                {
                    Console.WriteLine($"📦 Text - {attr.Title}: {_nonPricingAttributeValues[attr.AttributeId]}");
                }
            }

            return _nonPricingAttributeValues;
        }
    }
}