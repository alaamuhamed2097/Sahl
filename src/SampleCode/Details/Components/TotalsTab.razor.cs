using Microsoft.AspNetCore.Components;
using Shared.ResultModels.Commission;
using UI.Contracts.Commission;

namespace UI.Pages.User.Marketer.Details.Components
{
    public partial class TotalsTab
    {
        protected CommissionsHistoryResult CommissionsHistoryResult { get; set; } = new();

        [Parameter] public Guid id { get; set; }
        [Inject] protected ICommissionsHistoryService CommissionsHistoryService { get; set; }
        protected override async Task OnParametersSetAsync()
        {
           await LoadTotalCommissions();
        }

        protected async Task LoadTotalCommissions()
        {
            var result = await CommissionsHistoryService.GetTotalsCommissionHistoryAsync(id);
            if(result.Success)
            {
                CommissionsHistoryResult = result.Data;
            }
        }
    }
}