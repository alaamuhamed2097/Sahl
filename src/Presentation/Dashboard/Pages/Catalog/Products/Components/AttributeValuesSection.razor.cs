using Common.Enumerations.FieldType;
using Common.Enumerations.Pricing;
using Microsoft.AspNetCore.Components;
using Shared.DTOs.ECommerce.Category;
using Shared.DTOs.ECommerce.Item;

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

        // Pricing attributes: Key = AttributeId, Value = List of values
        private Dictionary<Guid, List<string>> _pricingAttributeValues = new();

        // Pricing integer values
        private Dictionary<Guid, List<int?>> _pricingIntValues = new();

        // Pricing decimal values
        private Dictionary<Guid, List<decimal?>> _pricingDecimalValues = new();

        // FIXED: Price modifiers by INDEX instead of VALUE
        // Key = AttributeId, Value = Dictionary of INDEX to modifier info
        private Dictionary<Guid, Dictionary<int, PriceModifierInfo>> _priceModifiersByIndex = new();

        // Make this class public to fix accessibility issue
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
            }

            // Initialize pricing attributes - USE AttributeId, not Id
            foreach (var attr in CategoryAttributes.Where(a => a.AffectsPricing))
            {
                if (!_pricingAttributeValues.ContainsKey(attr.AttributeId))
                {
                    _pricingAttributeValues[attr.AttributeId] = new List<string> { "" };
                }

                if (!_pricingIntValues.ContainsKey(attr.AttributeId))
                {
                    _pricingIntValues[attr.AttributeId] = new List<int?> { null };
                }

                if (!_pricingDecimalValues.ContainsKey(attr.AttributeId))
                {
                    _pricingDecimalValues[attr.AttributeId] = new List<decimal?> { null };
                }

                // FIXED: Initialize modifiers by index using AttributeId
                if (!_priceModifiersByIndex.ContainsKey(attr.AttributeId))
                {
                    _priceModifiersByIndex[attr.AttributeId] = new Dictionary<int, PriceModifierInfo>();
                }
            }
        }

        #region Non-Pricing Attributes

        private bool HasNonPricingValue(Guid attributeId)
        {
            return _nonPricingAttributeValues.ContainsKey(attributeId) &&
                   !string.IsNullOrWhiteSpace(_nonPricingAttributeValues[attributeId]);
        }

        #endregion

        #region Pricing Attributes

        private List<string> GetPricingValues(Guid attributeId)
        {
            if (!_pricingAttributeValues.ContainsKey(attributeId))
            {
                _pricingAttributeValues[attributeId] = new List<string> { "" };
            }
            return _pricingAttributeValues[attributeId];
        }

        private bool HasPricingValue(Guid attributeId)
        {
            var values = GetPricingValues(attributeId);
            return values.Any(v => !string.IsNullOrWhiteSpace(v));
        }

        private async Task AddPricingValue(Guid attributeId)
        {
            var values = GetPricingValues(attributeId);
            var newIndex = values.Count;
            values.Add("");

            // Add corresponding entries for numeric values
            if (_pricingIntValues.ContainsKey(attributeId))
            {
                _pricingIntValues[attributeId].Add(null);
            }

            if (_pricingDecimalValues.ContainsKey(attributeId))
            {
                _pricingDecimalValues[attributeId].Add(null);
            }

            // FIXED: Initialize modifier for the new index
            if (!_priceModifiersByIndex.ContainsKey(attributeId))
            {
                _priceModifiersByIndex[attributeId] = new Dictionary<int, PriceModifierInfo>();
            }
            _priceModifiersByIndex[attributeId][newIndex] = new PriceModifierInfo();

            await OnValuesChanged.InvokeAsync();
            StateHasChanged();
        }

        private async Task RemovePricingValue(Guid attributeId, int index)
        {
            var values = GetPricingValues(attributeId);
            if (values.Count > 1 && index < values.Count)
            {
                values.RemoveAt(index);

                // Remove corresponding entries for numeric values
                if (_pricingIntValues.ContainsKey(attributeId) && index < _pricingIntValues[attributeId].Count)
                {
                    _pricingIntValues[attributeId].RemoveAt(index);
                }

                if (_pricingDecimalValues.ContainsKey(attributeId) && index < _pricingDecimalValues[attributeId].Count)
                {
                    _pricingDecimalValues[attributeId].RemoveAt(index);
                }

                // FIXED: Remove modifier for this index and reindex
                if (_priceModifiersByIndex.ContainsKey(attributeId))
                {
                    var modifiers = _priceModifiersByIndex[attributeId];
                    var newModifiers = new Dictionary<int, PriceModifierInfo>();

                    // Reindex all modifiers after the removed index
                    foreach (var kvp in modifiers.OrderBy(k => k.Key))
                    {
                        if (kvp.Key < index)
                        {
                            // Keep indices before removed item
                            newModifiers[kvp.Key] = kvp.Value;
                        }
                        else if (kvp.Key > index)
                        {
                            // Shift indices after removed item
                            newModifiers[kvp.Key - 1] = kvp.Value;
                        }
                        // Skip the removed index
                    }

                    _priceModifiersByIndex[attributeId] = newModifiers;
                }

                await OnValuesChanged.InvokeAsync();
            }
        }

        private bool IsPricingOptionSelected(Guid attributeId, Guid optionId)
        {
            var values = GetPricingValues(attributeId);
            return values.Contains(optionId.ToString());
        }

        private async Task TogglePricingOption(Guid attributeId, Guid optionId)
        {
            var values = GetPricingValues(attributeId);
            var optionIdStr = optionId.ToString();

            if (values.Contains(optionIdStr))
            {
                var index = values.IndexOf(optionIdStr);
                values.Remove(optionIdStr);

                // FIXED: Remove modifier by index
                if (_priceModifiersByIndex.ContainsKey(attributeId) &&
                    _priceModifiersByIndex[attributeId].ContainsKey(index))
                {
                    _priceModifiersByIndex[attributeId].Remove(index);
                }
            }
            else
            {
                var newIndex = values.Count;
                values.Add(optionIdStr);

                // FIXED: Initialize modifier by index
                if (!_priceModifiersByIndex.ContainsKey(attributeId))
                {
                    _priceModifiersByIndex[attributeId] = new Dictionary<int, PriceModifierInfo>();
                }
                if (!_priceModifiersByIndex[attributeId].ContainsKey(newIndex))
                {
                    _priceModifiersByIndex[attributeId][newIndex] = new PriceModifierInfo();
                }
            }

            await OnValuesChanged.InvokeAsync();
        }

        #endregion

        #region Price Modifier Methods

        // FIXED: Get modifier info by INDEX instead of value
        public PriceModifierInfo GetModifierInfoByIndex(Guid attributeId, int index)
        {
            if (!_priceModifiersByIndex.ContainsKey(attributeId))
            {
                _priceModifiersByIndex[attributeId] = new Dictionary<int, PriceModifierInfo>();
            }

            if (!_priceModifiersByIndex[attributeId].ContainsKey(index))
            {
                _priceModifiersByIndex[attributeId][index] = new PriceModifierInfo();
            }

            return _priceModifiersByIndex[attributeId][index];
        }

        #endregion

        // Public method to get all attribute data
        public (Dictionary<Guid, string> NonPricingValues,
                Dictionary<Guid, List<string>> PricingValues,
                Dictionary<Guid, Dictionary<string, PriceModifierInfo>> PriceModifiers) GetAllAttributeData()
        {

            // Update non-pricing values from numeric fields
            foreach (var attr in CategoryAttributes.Where(a => !a.AffectsPricing))
            {
                if (attr.FieldType == FieldType.IntegerNumber && _nonPricingIntValues.ContainsKey(attr.AttributeId))
                {
                    _nonPricingAttributeValues[attr.AttributeId] = _nonPricingIntValues[attr.AttributeId]?.ToString() ?? "";
                }
                else if (attr.FieldType == FieldType.DecimalNumber && _nonPricingDecimalValues.ContainsKey(attr.AttributeId))
                {
                    _nonPricingAttributeValues[attr.AttributeId] = _nonPricingDecimalValues[attr.AttributeId]?.ToString() ?? "";
                }
                else if (attr.FieldType == FieldType.CheckBox && _nonPricingBoolValues.ContainsKey(attr.AttributeId))
                {
                    _nonPricingAttributeValues[attr.AttributeId] = _nonPricingBoolValues[attr.AttributeId].ToString();
                }
                else if (attr.FieldType == FieldType.Date && _nonPricingDateValues.ContainsKey(attr.AttributeId))
                {
                    _nonPricingAttributeValues[attr.AttributeId] = _nonPricingDateValues[attr.AttributeId]?.ToString("yyyy-MM-dd") ?? "";
                }
            }

            Console.WriteLine($"  Non-pricing attributes: {_nonPricingAttributeValues.Count}");

            // Update pricing values from numeric fields
            foreach (var attr in CategoryAttributes.Where(a => a.AffectsPricing))
            {
                Console.WriteLine($"  📊 Processing pricing attribute: {attr.Title} (AttributeId: {attr.AttributeId})");

                if (!_pricingAttributeValues.ContainsKey(attr.AttributeId))
                {
                    Console.WriteLine($"    ⚠️ Not found in _pricingAttributeValues!");
                    continue;
                }

                var values = _pricingAttributeValues[attr.AttributeId];
                Console.WriteLine($"    Raw values count: {values.Count}");

                if (attr.FieldType == FieldType.IntegerNumber && _pricingIntValues.ContainsKey(attr.AttributeId))
                {
                    for (int i = 0; i < values.Count; i++)
                    {
                        if (i < _pricingIntValues[attr.AttributeId].Count)
                        {
                            values[i] = _pricingIntValues[attr.AttributeId][i]?.ToString() ?? "";
                        }
                    }
                }
                else if (attr.FieldType == FieldType.DecimalNumber && _pricingDecimalValues.ContainsKey(attr.AttributeId))
                {
                    for (int i = 0; i < values.Count; i++)
                    {
                        if (i < _pricingDecimalValues[attr.AttributeId].Count)
                        {
                            values[i] = _pricingDecimalValues[attr.AttributeId][i]?.ToString() ?? "";
                        }
                    }
                }

                // Log the actual values
                var nonEmptyValues = values.Where(v => !string.IsNullOrWhiteSpace(v)).ToList();
                Console.WriteLine($"    Non-empty values: {nonEmptyValues.Count}");
                foreach (var val in nonEmptyValues)
                {
                    Console.WriteLine($"      ✓ '{val}'");
                }
            }

            Console.WriteLine($"  Total pricing attributes in dictionary: {_pricingAttributeValues.Count}");

            // Convert index-based modifiers to value-based modifiers for compatibility
            var valueBasedModifiers = new Dictionary<Guid, Dictionary<string, PriceModifierInfo>>();

            foreach (var attrKvp in _priceModifiersByIndex)
            {
                var attributeId = attrKvp.Key;
                var indexModifiers = attrKvp.Value;

                if (!valueBasedModifiers.ContainsKey(attributeId))
                {
                    valueBasedModifiers[attributeId] = new Dictionary<string, PriceModifierInfo>();
                }

                // Map each index to its corresponding value
                var values = GetPricingValues(attributeId);
                foreach (var modKvp in indexModifiers)
                {
                    var index = modKvp.Key;
                    var modifier = modKvp.Value;

                    if (index < values.Count)
                    {
                        var value = values[index];
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            valueBasedModifiers[attributeId][value] = modifier;
                        }
                    }
                }
            }

            return (_nonPricingAttributeValues, _pricingAttributeValues, valueBasedModifiers);
        }

    }
}