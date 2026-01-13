using Dashboard.Constants;
using Dashboard.Contracts.Payment;
using Dashboard.Pages.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Payment;
using Shared.GeneralModels;

namespace Dashboard.Pages.User.Student.StudentDetails.Components
{
    public partial class PaymentsTab : BaseListPage<AdminInvoiceListItemDto>
    {
        [Parameter] public Guid UserId { get; set; }

        [Inject] private IAdminInvoiceService AdminInvoiceService { get; set; } = null!;

        protected override string EntityName => AdminResources.Invoices;
        protected override string AddRoute => $"AddInvoice/{UserId}";
        protected override string EditRouteTemplate => "UpdateInvoice/{id}";
        protected override string SearchEndpoint => ApiEndpoints.AdminInvoices.Search;

        protected override Dictionary<string, Func<AdminInvoiceListItemDto, object>> ExportColumns =>
            new()
            {
                [FormResources.Id] = x => x.InvoiceId,
                [FormResources.State] = x => x.PaymentState,
                [FormResources.Price] = x => x.PaymentValue,
                [FormResources.Date] = x => (x.CreatedDateUTC?.ToString("yyyy-MM-dd HH:mm")) ?? string.Empty
            };

        protected override async Task OnBeforeSearchAsync()
        {
            // Ensure default pagination
            searchModel.PageNumber = searchModel.PageNumber < 1 ? 1 : searchModel.PageNumber;
            searchModel.PageSize = searchModel.PageSize < 1 || searchModel.PageSize > 100 ? 20 : searchModel.PageSize;
            await Task.CompletedTask;
        }

        protected override async Task Search()
        {
            try
            {
                await OnBeforeSearchAsync();

                var query = $"MemberId={UserId}&PageNumber={searchModel.PageNumber}&PageSize={searchModel.PageSize}&SearchTerm={searchModel.SearchTerm}";
                var url = $"{SearchEndpoint}?{query}";

                var result = await AdminInvoiceService.SearchAsync(UserId, searchModel);
                if (result.Success)
                {
                    items = result.Data.Data;
                    totalRecords = result.Data.TotalCount;
                    totalPages = (int)Math.Ceiling((double)totalRecords / searchModel.PageSize);
                    currentPage = searchModel.PageNumber;
                    StateHasChanged();

                    await OnAfterSearchAsync();
                }
                else if (!string.IsNullOrWhiteSpace(result.Message))
                {
                    var options = new { title = ValidationResources.Error, text = result.Message, icon = "error" };
                    await JSRuntime.InvokeVoidAsync("swal", options);
                }
            }
            catch (Exception ex)
            {
                var options = new { title = ValidationResources.Error, text = NotifiAndAlertsResources.Failed, icon = "error" };
                await JSRuntime.InvokeVoidAsync("swal", options);
            }
        }

        private void OpenCreateModal()
        {
            Add();
        }

        private void EditInvoice(string invoiceId)
        {
            Navigation.NavigateTo(EditRouteTemplate.Replace("{id}", invoiceId), true);
        }

        private async Task DeleteInvoice(string invoiceId)
        {
            var confirmed = await DeleteConfirmNotification();
            if (!confirmed) return;

            var res = await AdminInvoiceService.DeleteAsync(invoiceId);
            if (res.Success)
            {
                await ShowSuccessNotification(NotifiAndAlertsResources.DeletedSuccessfully);
                await Search();
            }
            else
            {
                await ShowErrorNotification(ValidationResources.Failed, res.Message ?? NotifiAndAlertsResources.DeleteFailed);
            }
        }

        protected override Task<ResponseModel<IEnumerable<AdminInvoiceListItemDto>>> GetAllItemsAsync()
        {
            // Not used (we override Search), return empty success
            return Task.FromResult(new ResponseModel<IEnumerable<AdminInvoiceListItemDto>>
            {
                Success = true,
                Data = Enumerable.Empty<AdminInvoiceListItemDto>()
            });
        }

        protected override async Task<ResponseModel<bool>> DeleteItemAsync(Guid id)
        {
            var res = await AdminInvoiceService.DeleteAsync(id.ToString());
            return new ResponseModel<bool>
            {
                Success = res.Success,
                Message = res.Message,
                Data = res.Success
            };
        }

        protected override Task<string> GetItemId(AdminInvoiceListItemDto item)
        {
            return Task.FromResult(item.InvoiceId);
        }
    }
}
