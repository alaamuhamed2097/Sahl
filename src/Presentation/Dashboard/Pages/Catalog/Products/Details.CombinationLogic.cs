using Shared.DTOs.ECommerce.Item;

namespace Dashboard.Pages.Catalog.Products
{
    /// <summary>
    /// Partial class for attribute combination generation logic
    /// </summary>
    public partial class Details
    {
        /// <summary>
        /// Generates all possible combinations of price-affecting attributes
        /// </summary>
        public void GenerateAttributeCombinations()
        {
            // Get all price-affecting attributes from category
            var priceAffectingAttributes = categoryAttributes
                    .Where(ca => ca.AffectsPricing)
             .ToList();

            if (!priceAffectingAttributes.Any())
            {
                // No price-affecting attributes, clear combinations
                Model.ItemAttributeCombinationPricings.Clear();
                return;
            }

            // Build a list of attribute options for each price-affecting attribute
            var attributeOptionSets = new List<List<AttributeOptionInfo>>();

            foreach (var attr in priceAffectingAttributes)
            {
                var itemAttr = Model.ItemAttributes.FirstOrDefault(ia => ia.AttributeId == attr.Id);
                if (itemAttr == null || string.IsNullOrWhiteSpace(itemAttr.Value))
                    continue;

                var options = new List<AttributeOptionInfo>();

                // Handle different field types
                if (attr.FieldType == Common.Enumerations.FieldType.FieldType.List ||
         attr.FieldType == Common.Enumerations.FieldType.FieldType.MultiSelectList)
                {
                    if (attr.AttributeOptions != null)
                    {
                        // User can select multiple options (comma-separated)
                        var selectedValues = itemAttr.Value.Split(',', StringSplitOptions.RemoveEmptyEntries);
                        foreach (var value in selectedValues)
                        {
                            var option = attr.AttributeOptions.FirstOrDefault(o => o.Id.ToString() == value.Trim());
                            if (option != null)
                            {
                                options.Add(new AttributeOptionInfo
                                {
                                    AttributeId = attr.Id,
                                    OptionId = option.Id,
                                    DisplayValue = option.Title
                                });
                            }
                        }
                    }
                }
                else
                {
                    // For other types (Text, Number, etc.): treat the value itself as the option
                    options.Add(new AttributeOptionInfo
                    {
                        AttributeId = attr.Id,
                        OptionId = Guid.Empty,
                        DisplayValue = itemAttr.Value
                    });
                }

                if (options.Any())
                {
                    attributeOptionSets.Add(options);
                }
            }

            // If no valid options, clear combinations
            if (!attributeOptionSets.Any())
            {
                Model.ItemAttributeCombinationPricings.Clear();
                return;
            }

            // Generate Cartesian product of all attribute options
            var combinations = GenerateCartesianProduct(attributeOptionSets);

            // Create or update combination pricings
            var newCombinations = new List<ItemAttributeCombinationPricingDto>();

            foreach (var combination in combinations)
            {
                // Create attribute IDs string (comma-separated list of option IDs or attribute IDs)
                var attributeIds = string.Join(",", combination.Select(opt =>
 opt.OptionId != Guid.Empty ? opt.OptionId.ToString() : opt.AttributeId.ToString()));

                // Check if this combination already exists
                var existing = Model.ItemAttributeCombinationPricings
                   .FirstOrDefault(c => c.AttributeIds == attributeIds);

                if (existing != null)
                {
                    // Keep existing combination with its price and quantity
                    newCombinations.Add(existing);
                }
                else
                {
                    // Create new combination with default values
                    newCombinations.Add(new ItemAttributeCombinationPricingDto
                    {
                        AttributeIds = attributeIds,
                        FinalPrice = Model.Price ?? 0,
                        Quantity = 0,
                        Image = null
                    });
                }
            }

            // Replace the combinations list
            Model.ItemAttributeCombinationPricings = newCombinations;
            StateHasChanged();
        }

        /// <summary>
        /// Generates the Cartesian product of attribute option sets
        /// </summary>
        private List<List<AttributeOptionInfo>> GenerateCartesianProduct(List<List<AttributeOptionInfo>> sets)
        {
            if (sets == null || !sets.Any())
                return new List<List<AttributeOptionInfo>>();

            var result = new List<List<AttributeOptionInfo>> { new List<AttributeOptionInfo>() };

            foreach (var set in sets)
            {
                var newResult = new List<List<AttributeOptionInfo>>();
                foreach (var existingCombo in result)
                {
                    foreach (var item in set)
                    {
                        var newCombo = new List<AttributeOptionInfo>(existingCombo) { item };
                        newResult.Add(newCombo);
                    }
                }
                result = newResult;
            }

            return result;
        }

        /// <summary>
        /// Helper class to store attribute option information
        /// </summary>
        private class AttributeOptionInfo
        {
            public Guid AttributeId { get; set; }
            public Guid OptionId { get; set; }
            public string DisplayValue { get; set; } = string.Empty;
        }

        /// <summary>
        /// Called when category changes to load attributes and clear combinations
        /// </summary>
        private async Task HandleCategoryChangeWithCombinations()
        {
            fieldValidation["CategoryId"] = Model.CategoryId != Guid.Empty;

            if (Model.CategoryId != Guid.Empty)
            {
                await LoadCategoryAttributes();
                // Clear existing combinations when category changes
                Model.ItemAttributeCombinationPricings.Clear();
            }
            else
            {
                categoryAttributes.Clear();
                Model.ItemAttributes.Clear();
                Model.ItemAttributeCombinationPricings.Clear();
            }
            StateHasChanged();
        }
    }
}
