using Common.Enumerations.User;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.User.Marketer;
using Shared.GeneralModels.SearchCriteriaModels;
using Shared.Parameters.Commission;
using UI.Configuration;
using UI.Contracts.Commission;
using UI.Contracts.User.Marketer;

namespace UI.Pages.User.Marketer.Details.Components
{
    public partial class HomeTab
    {
        [Parameter] public Guid id { get; set; }
        [Inject] protected IMarketerService MarketerService { get; set; } = null!;
        [Inject] protected ICommissionsHistoryService CommissionsHistoryService { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        [Inject] protected IOptions<ApiSettings> ApiOptions { get; set; } = default!;

        protected string baseUrl = string.Empty;
        protected decimal Total = 0;
        protected MarketerDto Model { get; set; } = new();
        protected SponsorDto Sponsor { get; set; } = new();

        protected CommissionLogsFilterRequest CommissionLogsFilterRequest { get; set; } = new();
        protected BaseSearchCriteriaModel searchModel { get; set; } = new();


        protected override async Task OnParametersSetAsync()
        {

            CommissionLogsFilterRequest.MarketerId = id;
            await SearchCommissionLogsAsync();

            baseUrl = ApiOptions.Value.BaseUrl;
            //Guid userId = Guid.Empty;
            //Guid.TryParse(id, out userId);

            var result = await MarketerService.FindById(id);
            if (!result.Success || result.Data == null)
            {
                await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, NotifiAndAlertsResources.FailedToRetrieveData, "error");
                //Navigation.NavigateTo("/NotFoundPage");
                return; // Exit immediately after navigation
            }

            Model = result.Data;
        }

        protected async Task SearchCommissionLogsAsync()
        {
            try
            {
                var result = await CommissionsHistoryService.SearchCommissionLogsAsync(searchModel, CommissionLogsFilterRequest);
                if (result.Success)
                {
                    Total = result.Data.Items.Sum(x => x.Amount);
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Error, NotifiAndAlertsResources.FailedToRetrieveData, "error");
            }
        }


        protected async Task HandleChangeUserState(UserStateType newType)
        {
            var result = await MarketerService.ChangeMarketerStates(Model.MarketerId, newType);
            if (!result.Success || result?.Data == null)
            {
                await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, NotifiAndAlertsResources.Failed, "error");
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Done, result.Message, "success");
            }

            Model.UserState = newType;
            StateHasChanged();
        }
    }
}