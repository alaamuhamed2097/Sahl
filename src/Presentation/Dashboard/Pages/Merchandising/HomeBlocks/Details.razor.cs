using Common.Enumerations.Merchandising;
using Dashboard.Contracts.Campaign;
using Dashboard.Contracts.ECommerce.Category;
using Dashboard.Contracts.ECommerce.Item;
using Dashboard.Contracts.Merchandising;
using Dashboard.Contracts.Notification;
using Dashboard.Pages.Base;
using Microsoft.AspNetCore.Components;
using Resources;
using Shared.DTOs.Campaign;
using Shared.DTOs.Catalog.Category;
using Shared.DTOs.Catalog.Item;
using Shared.DTOs.Merchandising.Homepage;

namespace Dashboard.Pages.Merchandising.HomeBlocks
{
    public partial class Details : LocalizedComponentBase
    {
        [Inject] protected IAdminBlockService AdminBlockService { get; set; } = null!;
        [Inject] protected ICampaignService CampaignService { get; set; } = null!;
        [Inject] protected INotificationService NotificationService { get; set; } = null!;
        [Inject] protected IItemService ItemService { get; set; } = null!;
        [Inject] protected ICategoryService CategoryService { get; set; } = null!;
        [Inject] protected NavigationManager NavigationManager { get; set; } = null!;

        [Parameter]
        public Guid? Id { get; set; }

        protected AdminBlockCreateDto formModel = new();
        protected List<CampaignDto> campaigns = new();
        protected List<ItemDto> selectedItems = new();
        protected List<CategoryDto> selectedCategories = new();
        protected DateTime? CreatedDate;
        protected DateTime? LastModifiedDate;

        protected bool IsEditMode => Id.HasValue;
        protected bool IsLoading = true;
        protected bool IsSaving = false;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                // Load campaigns for dropdown
                var campaignResponse = await CampaignService.GetAllAsync();
                if (campaignResponse.Success && campaignResponse.Data != null)
                {
                    campaigns = campaignResponse.Data;
                }

                // Load block data if editing
                if (IsEditMode)
                {
                    var response = await AdminBlockService.GetBlockByIdAsync(Id.Value);
                    if (response.Success && response.Data != null)
                    {
                        formModel = response.Data;
                        // Convert DTOs to their corresponding types
                        if (formModel.Items?.Any() == true)
                        {
                            var itemIds = formModel.Items.Select(i => i.ItemId).ToList();
                            var itemsResponse = await ItemService.GetAllAsync();
                            if (itemsResponse.Success && itemsResponse.Data != null)
                            {
                                selectedItems = itemsResponse.Data
                                    .Where(i => itemIds.Contains(i.Id))
                                    .OrderBy(i => formModel.Items.FirstOrDefault(bi => bi.ItemId == i.Id)?.DisplayOrder ?? 0)
                                    .ToList();
                            }
                        }
                        if (formModel.Categories?.Any() == true)
                        {
                            var categoryIds = formModel.Categories.Select(c => c.CategoryId).ToList();
                            var categoriesResponse = await CategoryService.GetAllAsync();
                            if (categoriesResponse.Success && categoriesResponse.Data != null)
                            {
                                selectedCategories = categoriesResponse.Data
                                    .Where(c => categoryIds.Contains(c.Id))
                                    .OrderBy(c => formModel.Categories.FirstOrDefault(bc => bc.CategoryId == c.Id)?.DisplayOrder ?? 0)
                                    .ToList();
                            }
                        }
                    }
                    else
                    {
                        await NotificationService.ShowErrorAsync(MerchandisingResources.BlockNotFound);
                        NavigationManager.NavigateTo("/home-blocks");
                    }
                }
                else
                {
                    // Set defaults for new block
                    formModel = new AdminBlockCreateDto
                    {
                        IsVisible = true,
                        ShowViewAllLink = true,
                        Layout = BlockLayout.Carousel,
                        Type = HomepageBlockType.ManualItems.ToString()
                    };
                    selectedItems = new();
                    selectedCategories = new();
                }
            }
            catch (Exception ex)
            {
                await NotificationService.ShowErrorAsync($"{MerchandisingResources.ErrorLoadingData}: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected async Task HandleSubmit()
        {
            if (!ValidateSelectionCountByLayout(out var layoutError))
            {
                await NotificationService.ShowErrorAsync(layoutError!);
                return;
            }

            IsSaving = true;

            try
            {
                if (IsEditMode)
                {
                    var response = await AdminBlockService.UpdateBlockAsync(Id.Value, formModel);
                    if (response.Success)
                    {
                        await NotificationService.ShowSuccessAsync(MerchandisingResources.BlockUpdatedSuccessfully);
                        NavigationManager.NavigateTo("/home-blocks");
                    }
                    else
                    {
                        await NotificationService.ShowErrorAsync(response.Message);
                    }
                }
                else
                {
                    var response = await AdminBlockService.CreateBlockAsync(formModel);
                    if (response.Success)
                    {
                        await NotificationService.ShowSuccessAsync(MerchandisingResources.BlockCreatedSuccessfully);
                        NavigationManager.NavigateTo("/home-blocks");
                    }
                    else
                    {
                        await NotificationService.ShowErrorAsync(response.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                await NotificationService.ShowErrorAsync($"{MerchandisingResources.ErrorSavingBlock}: {ex.Message}");
            }
            finally
            {
                IsSaving = false;
            }
        }

        /// <summary>
        /// Validates that the number of selected items/categories matches the layout requirements.
        /// Featured/FullWidth: exactly 1; Two-Row Grid: 1–2; Compact Grid: 1–4.
        /// </summary>
        protected bool ValidateSelectionCountByLayout(out string? errorMessage)
        {
            errorMessage = null;

            if (formModel.Type != HomepageBlockType.ManualItems.ToString() &&
                formModel.Type != HomepageBlockType.ManualCategories.ToString())
            {
                return true;
            }

            int count = formModel.Type == HomepageBlockType.ManualItems.ToString()
                ? (formModel.Items?.Count ?? 0)
                : (formModel.Categories?.Count ?? 0);

            switch (formModel.Layout)
            {
                case BlockLayout.Featured:
                case BlockLayout.FullWidth:
                    if (count != 1)
                    {
                        errorMessage = MerchandisingResources.LayoutValidationFeaturedFullWidth;
                        return false;
                    }
                    break;
                case BlockLayout.TwoRows:
                    if (count < 1 || count > 2)
                    {
                        errorMessage = MerchandisingResources.LayoutValidationTwoRowGrid;
                        return false;
                    }
                    break;
                case BlockLayout.Compact:
                    if (count < 1 || count > 4)
                    {
                        errorMessage = MerchandisingResources.LayoutValidationCompactGrid;
                        return false;
                    }
                    break;
            }

            return true;
        }

        protected void OnCampaignSelected(ChangeEventArgs e)
        {
            var value = e.Value?.ToString();
            if (Guid.TryParse(value, out var campaignId))
            {
                formModel.CampaignId = campaignId;
            }
        }

        protected Task OnSelectedItemsChanged(List<ItemDto> items)
        {
            selectedItems = items;
            formModel.Items = items.Select((item, index) => new AdminBlockItemDto
            {
                ItemId = item.Id,
                ItemNameEn = item.TitleEn,
                ItemNameAr = item.TitleAr,
                ItemImage = item.ThumbnailImage,
                Price = item.BaseSalesPrice ?? item.BasePrice ?? 0,
                DisplayOrder = index + 1
            }).ToList();
            return Task.CompletedTask;
        }

        protected Task OnSelectedCategoriesChanged(List<CategoryDto> categories)
        {
            selectedCategories = categories;
            formModel.Categories = categories.Select((category, index) => new AdminBlockCategoryDto
            {
                CategoryId = category.Id,
                CategoryNameEn = category.TitleEn,
                CategoryNameAr = category.TitleAr,
                CategoryImage = category.ImageUrl,
                DisplayOrder = index + 1
            }).ToList();
            return Task.CompletedTask;
        }

        protected string GetLayoutLabel(BlockLayout layout)
        {
            return layout switch
            {
                BlockLayout.Carousel => MerchandisingResources.CarouselSlider,
                BlockLayout.TwoRows => MerchandisingResources.TwoRowGrid,
                BlockLayout.Featured => MerchandisingResources.FeaturedShowcase,
                BlockLayout.Compact => MerchandisingResources.CompactGrid,
                BlockLayout.FullWidth => MerchandisingResources.FullWidthBanner,
                _ => layout.ToString()
            };
        }

        protected string GetLayoutDescription(BlockLayout layout)
        {
            return layout switch
            {
                BlockLayout.Carousel => MerchandisingResources.CarouselDescription,
                BlockLayout.TwoRows => MerchandisingResources.TwoRowGridDescription,
                BlockLayout.Featured => MerchandisingResources.FeaturedDescription,
                BlockLayout.Compact => MerchandisingResources.CompactDescription,
                BlockLayout.FullWidth => MerchandisingResources.FullWidthDescription,
                _ => string.Empty
            };
        }

        protected string GetLayoutIcon(BlockLayout layout)
        {
            return layout switch
            {
                BlockLayout.Carousel => "icon-chevrons-right",
                BlockLayout.TwoRows => "icon-grid",
                BlockLayout.Featured => "icon-star",
                BlockLayout.Compact => "icon-layout",
                BlockLayout.FullWidth => "icon-maximize",
                _ => "icon-square"
            };
        }

        protected string GetBlockTypeLabel(HomepageBlockType type)
        {
            return type switch
            {
                HomepageBlockType.ManualItems => MerchandisingResources.ManualItemsSelection,
                HomepageBlockType.ManualCategories => MerchandisingResources.ManualCategoriesSelection,
                HomepageBlockType.Campaign => MerchandisingResources.CampaignBased,
                HomepageBlockType.Dynamic => MerchandisingResources.DynamicRules,
                HomepageBlockType.Personalized => MerchandisingResources.AIPersonalized,
                _ => type.ToString()
            };
        }

        protected string GetBlockTypeDescription(HomepageBlockType type)
        {
            return type switch
            {
                HomepageBlockType.ManualItems => MerchandisingResources.ManualItemsDescription,
                HomepageBlockType.ManualCategories => MerchandisingResources.ManualCategoriesDescription,
                HomepageBlockType.Campaign => MerchandisingResources.CampaignDescription,
                HomepageBlockType.Dynamic => MerchandisingResources.DynamicDescription,
                HomepageBlockType.Personalized => MerchandisingResources.PersonalizedDescription,
                _ => string.Empty
            };
        }

        protected string GetBlockTypeIcon(HomepageBlockType type)
        {
            return type switch
            {
                HomepageBlockType.ManualItems => "icon-hand",
                HomepageBlockType.ManualCategories => "icon-folder",
                HomepageBlockType.Campaign => "icon-tag",
                HomepageBlockType.Dynamic => "icon-zap",
                HomepageBlockType.Personalized => "icon-users",
                _ => "icon-square"
            };
        }

        protected string GetDynamicSourceLabel(DynamicBlockSource source)
        {
            return source switch
            {
                DynamicBlockSource.BestSellers => MerchandisingResources.BestSellers,
                DynamicBlockSource.NewArrivals => MerchandisingResources.NewArrivals,
                DynamicBlockSource.TopRated => MerchandisingResources.TopRated,
                DynamicBlockSource.Trending => MerchandisingResources.Trending,
                DynamicBlockSource.MostWishlisted => MerchandisingResources.MostWishlisted,
                _ => source.ToString()
            };
        }

        protected string GetPersonalizationSourceLabel(PersonalizationSource source)
        {
            return source switch
            {
                PersonalizationSource.ViewHistory => MerchandisingResources.BasedOnViewHistory,
                PersonalizationSource.PurchaseHistory => MerchandisingResources.BasedOnPurchaseHistory,
                PersonalizationSource.RecentlyViewed => MerchandisingResources.RecentlyViewed,
                PersonalizationSource.Wishlist => MerchandisingResources.FromWishlist,
                _ => source.ToString()
            };
        }
    }
}