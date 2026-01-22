using Dashboard.Configuration;
using Dashboard.Contracts.WithdrawalMethod;
using Dashboard.Pages.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.WithdrawalMethod;

namespace Dashboard.Pages.WithdrawalMethod
{
    public partial class Index : LocalizedComponentBase
    {
        protected string baseUrl = string.Empty;
        protected IEnumerable<WithdrawalMethodDto>? withdrawalMethods;

        [Inject] protected IWithdrawalMethodService withdrawalMethodService { get; set; } = null!;
        [Inject] protected IJSRuntime JSRuntime { get; set; } = null!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        [Inject] protected IOptions<ApiSettings> ApiOptions { get; set; } = default!;


        protected override async Task OnInitializedAsync()
        {
            baseUrl = ApiOptions.Value.BaseUrl;
            await LoadWithdrawalMethods();
        }
        protected async Task Add()
        {
            Navigation.NavigateTo("/withdrawalMethod");
        }

        protected async Task Edit(WithdrawalMethodDto item)
        {
            Navigation.NavigateTo($"/withdrawalMethod/{item.Id}");
        }

        protected async Task LoadWithdrawalMethods()
        {
            var result = await withdrawalMethodService.GetAllAsync();
            withdrawalMethods = result.Data;
            StateHasChanged();

        }

        protected async Task Delete(Guid id)
        {
            //previewImageUrl = null;

            var confirmed = await JSRuntime.InvokeAsync<bool>("swal", new
            {
                title = NotifiAndAlertsResources.AreYouSure,
                text = NotifiAndAlertsResources.ConfirmDeleteAlert,
                icon = "warning",
                buttons = new
                {
                    cancel = ActionsResources.Cancel,
                    confirm = ActionsResources.Confirm,
                },
                dangerMode = true,
            });

            if (confirmed)
            {
                var result = await withdrawalMethodService.DeleteAsync(id);
                // Refresh your data
                if (result.Success)
                {
                    await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Done, NotifiAndAlertsResources.DeletedSuccessfully, "success");
                    await LoadWithdrawalMethods();
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, NotifiAndAlertsResources.DeleteFailed, "error");
                }
            }

        }

    }
}
