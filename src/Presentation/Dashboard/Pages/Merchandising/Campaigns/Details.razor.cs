using Common.Enumerations.User;
using Dashboard.Contracts.Campaign;
using Dashboard.Contracts.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Campaign;
using Shared.GeneralModels;

namespace Dashboard.Pages.Merchandising.Campaigns
{
    [Authorize(Roles = nameof(UserRole.Admin))]
    public partial class Details
    {
        private bool IsLoading { get; set; }
        private bool IsSaving { get; set; }

        protected CampaignDto Model { get; set; } = new();
        protected List<CampaignItemDto> CampaignItems { get; set; } = new();

        [Parameter] public string? CampaignId { get; set; }

        [Inject] protected ICampaignService CampaignService { get; set; } = null!;
        [Inject] protected NavigationManager NavigationManager { get; set; } = null!;
        [Inject] protected INotificationService NotificationService { get; set; } = null!;
        [Inject] protected IJSRuntime JSRuntime { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(CampaignId) && Guid.TryParse(CampaignId, out var campaignId))
            {
                await LoadCampaignAsync(campaignId);
            }
            else
            {
                Model = new CampaignDto
                {
                    Id = Guid.Empty,
                    IsActive = true,
                    IsFlashSale = false,
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(7)
                };
            }
        }

        private async Task LoadCampaignAsync(Guid campaignId)
        {
            IsLoading = true;
            try
            {
                var response = await CampaignService.GetCampaignByIdAsync(campaignId);
                if (response.Success && response.Data != null)
                {
                    Model = response.Data;
                    await LoadCampaignItemsAsync(campaignId);
                }
                else
                {
                    await NotificationService.ShowErrorAsync(response.Message ?? "Failed to load campaign");
                    NavigationManager.NavigateTo("/campaigns");
                }
            }
            catch (Exception ex)
            {
                await NotificationService.ShowErrorAsync($"Error loading campaign: {ex.Message}");
                NavigationManager.NavigateTo("/campaigns");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadCampaignItemsAsync(Guid campaignId)
        {
            try
            {
                var response = await CampaignService.GetCampaignItemsAsync(campaignId);
                if (response.Success && response.Data != null)
                {
                    CampaignItems = response.Data.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading campaign items: {ex.Message}");
            }
        }

        private async Task Save()
        {
            IsSaving = true;
            try
            {
                ResponseModel<CampaignDto>? response;

                if (Model.Id == Guid.Empty)
                {
                    // Create new campaign
                    var createDto = new CreateCampaignDto
                    {
                        NameEn = Model.NameEn,
                        NameAr = Model.NameAr,
                        StartDate = Model.StartDate,
                        EndDate = Model.EndDate,
                        IsActive = Model.IsActive,
                        IsFlashSale = Model.IsFlashSale,
                        FlashSaleEndTime = Model.FlashSaleEndTime,
                        MaxQuantityPerUser = Model.MaxQuantityPerUser,
                        BadgeTextEn = Model.BadgeTextEn,
                        BadgeTextAr = Model.BadgeTextAr,
                        BadgeColor = Model.BadgeColor
                    };

                    response = await CampaignService.CreateCampaignAsync(createDto);
                    if (response.Success)
                    {
                        await NotificationService.ShowSuccessAsync("Campaign created successfully!");
                        NavigationManager.NavigateTo("/campaigns");
                    }
                    else
                    {
                        await NotificationService.ShowErrorAsync(response.Message ?? "Failed to create campaign");
                    }
                }
                else
                {
                    // Update existing campaign
                    var updateDto = new UpdateCampaignDto
                    {
                        Id = Model.Id,
                        NameEn = Model.NameEn,
                        NameAr = Model.NameAr,
                        StartDate = Model.StartDate,
                        EndDate = Model.EndDate,
                        IsActive = Model.IsActive,
                        IsFlashSale = Model.IsFlashSale,
                        FlashSaleEndTime = Model.FlashSaleEndTime,
                        MaxQuantityPerUser = Model.MaxQuantityPerUser,
                        BadgeTextEn = Model.BadgeTextEn,
                        BadgeTextAr = Model.BadgeTextAr,
                        BadgeColor = Model.BadgeColor
                    };

                    response = await CampaignService.UpdateCampaignAsync(Model.Id, updateDto);
                    if (response.Success)
                    {
                        await NotificationService.ShowSuccessAsync("Campaign updated successfully!");
                        NavigationManager.NavigateTo("/campaigns");
                    }
                    else
                    {
                        await NotificationService.ShowErrorAsync(response.Message ?? "Failed to update campaign");
                    }
                }
            }
            catch (Exception ex)
            {
                await NotificationService.ShowErrorAsync($"Error saving campaign: {ex.Message}");
            }
            finally
            {
                IsSaving = false;
            }
        }

        private void Cancel()
        {
            NavigationManager.NavigateTo("/campaigns");
        }

        private async Task RemoveItem(Guid itemId)
        {
            // Simple confirmation using browser dialog for now
            var proceed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to remove this item from the campaign?");
            if (proceed)
            {
                try
                {
                    var response = await CampaignService.RemoveItemFromCampaignAsync(Model.Id, itemId);
                    if (response.Success)
                    {
                        await NotificationService.ShowSuccessAsync("Item removed successfully");
                        CampaignItems.RemoveAll(x => x.Id == itemId);
                        await InvokeAsync(StateHasChanged);
                    }
                    else
                    {
                        await NotificationService.ShowErrorAsync(response.Message ?? "Failed to remove item");
                    }
                }
                catch (Exception ex)
                {
                    await NotificationService.ShowErrorAsync($"Error removing item: {ex.Message}");
                }
            }
        }
    }
}
