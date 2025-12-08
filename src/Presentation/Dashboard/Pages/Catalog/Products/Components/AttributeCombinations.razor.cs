//using Microsoft.AspNetCore.Components;
//using Shared.DTOs.ECommerce.Category;
//using Shared.DTOs.ECommerce.Item;

//namespace Dashboard.Pages.Catalog.Products.Components
//{
//    public partial class AttributeCombinations
//    {
//        [Parameter]
//        public List<ItemCombinationDto> Combinations { get; set; } = new();

//        [Parameter]
//        public List<CategoryAttributeDto> CategoryAttributes { get; set; } = new();

//        [Parameter]
//        public List<ItemAttributeDto> ItemAttributes { get; set; } = new();

//        [Parameter]
//        public EventCallback<ItemCombinationDto> OnRemoveCombination { get; set; }

//        private string GetCombinationAttributesDisplay(ItemCombinationDto combination)
//        {
//            if (combination.CombinationAttributes == null || !combination.CombinationAttributes.Any())
//                return string.Empty;

//            var attributes = new List<string>();

//            foreach (var combinationAttr in combination.CombinationAttributes)
//            {
//                foreach (var attrValue in combinationAttr.combinationAttributeValueDtos)
//                {
//                    var attribute = CategoryAttributes.FirstOrDefault(a => a.Id == attrValue.AttributeId);

//                    if (attribute != null && !string.IsNullOrEmpty(attrValue.Value))
//                    {
//                        attributes.Add($"{attribute.Title}: {attrValue.Value}");
//                    }
//                }
//            }

//            return string.Join(" | ", attributes);
//        }

//        private async Task RemoveCombination(ItemCombinationDto combination)
//        {
//            await OnRemoveCombination.InvokeAsync(combination);
//        }
//    }
//}