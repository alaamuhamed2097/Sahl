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
            Console.WriteLine($"?? GenerateAttributeCombinations - START");
            
            // Get all price-affecting attributes from category
            var priceAffectingAttributes = categoryAttributes
                    .Where(ca => ca.AffectsPricing)
             .ToList();

            Console.WriteLine($"?? Found {priceAffectingAttributes.Count} price-affecting attributes");

            if (!priceAffectingAttributes.Any())
            {
                Console.WriteLine($"?? No price-affecting attributes, ensuring default combination exists");
                // No price-affecting attributes, ensure we have a default combination
                if (!Model.ItemAttributeCombinationPricings.Any())
                {
                    Model.ItemAttributeCombinationPricings.Add(new ItemAttributeCombinationPricingDto
                    {
                        AttributeIds = string.Empty,
                        Price = 0,
                        SalesPrice = 0,
                        Quantity = 0,
                        IsDefault = true
                    });
                }
                else
                {
                    // Ensure at least one is marked as default
                    EnsureSingleDefaultCombination();
                }
                return;
            }

            // Build a list of attribute options for each price-affecting attribute
            var attributeOptionSets = new List<List<AttributeOptionInfo>>();
            // Store mapping of option IDs to display values for later retrieval
            var optionDisplayMap = new Dictionary<Guid, string>();

            foreach (var attr in priceAffectingAttributes)
            {
                Console.WriteLine($"  ?? Processing attribute: {attr.Title} (AttributeId: {attr.AttributeId})");
                Console.WriteLine($"     FieldType: {attr.FieldType}");
                Console.WriteLine($"     Options available: {attr.AttributeOptions?.Count ?? 0}");
                
                // FIX: Use AttributeId instead of Id
                var itemAttr = Model.ItemAttributes.FirstOrDefault(ia => ia.AttributeId == attr.AttributeId);
                if (itemAttr == null)
                {
                    Console.WriteLine($"     ?? No ItemAttribute found for this attribute");
                    continue;
                }
                
                Console.WriteLine($"     ItemAttribute Value: '{itemAttr.Value}'");
                
                if (string.IsNullOrWhiteSpace(itemAttr.Value))
                {
                    Console.WriteLine($"     ?? Value is empty, skipping");
                    continue;
                }

                var options = new List<AttributeOptionInfo>();

                // Handle different field types
                if (attr.FieldType == Common.Enumerations.FieldType.FieldType.List ||
         attr.FieldType == Common.Enumerations.FieldType.FieldType.MultiSelectList)
                {
                    Console.WriteLine($"     ?? List/MultiSelectList type");
                    if (attr.AttributeOptions != null && attr.AttributeOptions.Any())
                    {
                        // User can select multiple options (comma-separated)
                        var selectedValues = itemAttr.Value.Split(',', StringSplitOptions.RemoveEmptyEntries);
                        Console.WriteLine($"     Selected values: {string.Join(", ", selectedValues)}");
                        
                        foreach (var value in selectedValues)
                        {
                            var option = attr.AttributeOptions.FirstOrDefault(o => o.Id.ToString() == value.Trim());
                            if (option != null)
                            {
                                Console.WriteLine($"       ? Matched option: {option.Title} (ID: {option.Id})");
                                options.Add(new AttributeOptionInfo
                                {
                                    AttributeId = attr.AttributeId,  // FIX: Use AttributeId
                                    AttributeName = attr.Title,
                                    OptionId = option.Id,
                                    DisplayValue = option.Title
                                });
                                optionDisplayMap[option.Id] = $"{attr.Title}: {option.Title}";
                            }
                            else
                            {
                                Console.WriteLine($"       ?? No option found for value: {value.Trim()}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"     ?? No options available for this attribute");
                    }
                }
                else
                {
                    Console.WriteLine($"     ?? Other type (Text/Number/Date)");
                    // For other types: Split by comma to allow multiple values
                    var values = itemAttr.Value.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(v => v.Trim())
                        .Where(v => !string.IsNullOrWhiteSpace(v))
                        .ToList();
                    
                    Console.WriteLine($"     Found {values.Count} value(s): {string.Join(", ", values)}");
                    
                    foreach (var value in values)
                    {
                        // Use the attribute AttributeId + value hash as a consistent identifier
                        var valueIdentifier = $"{attr.AttributeId}:{value}";
                        var valueId = GenerateConsistentGuid(valueIdentifier);
                        
                        options.Add(new AttributeOptionInfo
                        {
                            AttributeId = attr.AttributeId,  // FIX: Use AttributeId
                            AttributeName = attr.Title,
                            OptionId = valueId,
                            DisplayValue = value
                        });
                        optionDisplayMap[valueId] = $"{attr.Title}: {value}";
                        Console.WriteLine($"       ? Added value: {value} (ID: {valueId})");
                    }
                }

                if (options.Any())
                {
                    Console.WriteLine($"     ? Added {options.Count} options to set");
                    attributeOptionSets.Add(options);
                }
                else
                {
                    Console.WriteLine($"     ?? No valid options found");
                }
            }

            // Store the mapping for display purposes
            _optionDisplayMap = optionDisplayMap;

            // If no valid options, ensure default combination
            if (!attributeOptionSets.Any())
            {
                Console.WriteLine($"?? No valid option sets, ensuring default combination exists");
                if (!Model.ItemAttributeCombinationPricings.Any())
                {
                    Model.ItemAttributeCombinationPricings.Add(new ItemAttributeCombinationPricingDto
                    {
                        AttributeIds = string.Empty,
                        Price = 0,
                        SalesPrice = 0,
                        Quantity = 0,
                        IsDefault = true
                    });
                }
                else
                {
                    EnsureSingleDefaultCombination();
                }
                return;
            }

            Console.WriteLine($"?? Total attribute option sets: {attributeOptionSets.Count}");

            // Generate Cartesian product of all attribute options
            var combinations = GenerateCartesianProduct(attributeOptionSets);
            Console.WriteLine($"? Generated {combinations.Count} combinations");

            // Remember which combination was default before
            var previousDefaultId = Model.ItemAttributeCombinationPricings
                .FirstOrDefault(c => c.IsDefault)?.Id ?? Guid.Empty;
            var previousDefaultAttributeIds = Model.ItemAttributeCombinationPricings
                .FirstOrDefault(c => c.IsDefault)?.AttributeIds ?? string.Empty;

            // Create or update combination pricings
            var newCombinations = new List<ItemAttributeCombinationPricingDto>();

            foreach (var combination in combinations)
            {
                // Create attribute IDs string (comma-separated list of option IDs)
                var attributeIds = string.Join(",", combination.Select(opt => opt.OptionId.ToString()));

                Console.WriteLine($"  ?? Combination: {string.Join(" | ", combination.Select(c => c.DisplayValue))}");

                // Check if this combination already exists
                var existing = Model.ItemAttributeCombinationPricings
                   .FirstOrDefault(c => c.AttributeIds == attributeIds);

                if (existing != null)
                {
                    Console.WriteLine($"     ? Keeping existing combination");
                    // Keep existing combination with its price
                    newCombinations.Add(existing);
                }
                else
                {
                    Console.WriteLine($"     ? Creating new combination");
                    // Get default price from existing combinations or use 0
                    var defaultPrice = Model.ItemAttributeCombinationPricings.Any() 
                        ? Model.ItemAttributeCombinationPricings.First().Price 
                        : 0;
                    
                    // Create new combination with default values
                    newCombinations.Add(new ItemAttributeCombinationPricingDto
                    {
                        AttributeIds = attributeIds,
                        Price = defaultPrice,
                        SalesPrice = defaultPrice,
                        Quantity = 0,
                        IsDefault = false
                    });
                }
            }

            // Replace the combinations list
            Model.ItemAttributeCombinationPricings = newCombinations;

            // Restore or set default
            if (previousDefaultId != Guid.Empty)
            {
                var restoredDefault = newCombinations.FirstOrDefault(c => 
                    c.Id == previousDefaultId || c.AttributeIds == previousDefaultAttributeIds);
                if (restoredDefault != null)
                {
                    restoredDefault.IsDefault = true;
                    Console.WriteLine($"? Restored previous default combination");
                }
                else
                {
                    // Previous default not found, set first as default
                    newCombinations.First().IsDefault = true;
                    Console.WriteLine($"?? Previous default not found, set first as default");
                }
            }
            else
            {
                // No previous default, set first as default
                newCombinations.First().IsDefault = true;
                Console.WriteLine($"? Set first combination as default");
            }

            Console.WriteLine($"? GenerateAttributeCombinations - COMPLETE - Total combinations: {newCombinations.Count}");
            StateHasChanged();
        }
        
        /// <summary>
        /// Ensures only one combination is marked as default
        /// </summary>
        private void EnsureSingleDefaultCombination()
        {
            var defaultCombinations = Model.ItemAttributeCombinationPricings
                .Where(c => c.IsDefault)
                .ToList();

            if (defaultCombinations.Count == 0)
            {
                // No default, set first as default
                if (Model.ItemAttributeCombinationPricings.Any())
                {
                    Model.ItemAttributeCombinationPricings.First().IsDefault = true;
                    Console.WriteLine($"? Set first combination as default");
                }
            }
            else if (defaultCombinations.Count > 1)
            {
                // Multiple defaults, keep only first
                Console.WriteLine($"?? Multiple default combinations found, keeping only first");
                foreach (var combo in defaultCombinations.Skip(1))
                {
                    combo.IsDefault = false;
                }
            }
        }
        
        /// <summary>
        /// Generates a consistent GUID based on a string identifier
        /// </summary>
        private Guid GenerateConsistentGuid(string identifier)
        {
            // MD5 is not supported in Blazor WebAssembly
            // Use a deterministic hash-based approach instead
            var hash = identifier.GetHashCode();
            var bytes = new byte[16];
            
            // Fill the byte array with deterministic data based on the string
            var idBytes = System.Text.Encoding.UTF8.GetBytes(identifier);
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)(idBytes[i % idBytes.Length] ^ (hash >> (i % 4 * 8)));
            }
            
            return new Guid(bytes);
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
            public string AttributeName { get; set; } = string.Empty;
            public Guid OptionId { get; set; }
            public string DisplayValue { get; set; } = string.Empty;
        }
        
        /// <summary>
        /// Called when category changes to load attributes and clear combinations
        /// </summary>
        private async Task HandleCategoryChangeWithCombinations()
        {
            try
            {
                Console.WriteLine($"?? HandleCategoryChangeWithCombinations - CategoryId: {Model.CategoryId}");

                fieldValidation["CategoryId"] = Model.CategoryId != Guid.Empty;

                if (Model.CategoryId != Guid.Empty)
                {
                    Console.WriteLine($"?? Loading category attributes...");
                    await LoadCategoryAttributes();

                    Console.WriteLine($"?? Resetting to default combination...");
                    // Reset to default combination when category changes
                    var defaultPrice = Model.ItemAttributeCombinationPricings.Any() 
                        ? Model.ItemAttributeCombinationPricings.First(c => c.IsDefault)?.Price ?? Model.ItemAttributeCombinationPricings.First().Price
                        : 0;
                    
                    Model.ItemAttributeCombinationPricings = new List<ItemAttributeCombinationPricingDto>
                    {
                        new ItemAttributeCombinationPricingDto
                        {
                            AttributeIds = string.Empty,
                            Price = defaultPrice,
                            SalesPrice = defaultPrice,
                            Quantity = 0,
                            IsDefault = true
                        }
                    };

                    Console.WriteLine($"? Category change completed - Attributes loaded: {categoryAttributes.Count}");
                }
                else
                {
                    Console.WriteLine($"?? Category cleared - resetting all data");
                    categoryAttributes.Clear();
                    Model.ItemAttributes.Clear();
                    // Keep default combination
                    Model.ItemAttributeCombinationPricings = new List<ItemAttributeCombinationPricingDto>
                    {
                        new ItemAttributeCombinationPricingDto
                        {
                            AttributeIds = string.Empty,
                            Price = 0,
                            SalesPrice = 0,
                            Quantity = 0,
                            IsDefault = true
                        }
                    };
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? Error in HandleCategoryChangeWithCombinations: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
