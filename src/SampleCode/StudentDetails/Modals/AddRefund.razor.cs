using Common.Enumeration;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Payment;
using Dashboard.Contracts.Refund;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Payment;
using Shared.DTOs.Refund;
using Shared.GeneralModels.RequestModels;

namespace Dashboard.Pages.User.Student.StudentDetails.Modals
{
    public partial class AddRefund
    {
        [Parameter] public string ItemId { get; set; }
        [Parameter] public string Type { get; set; } // Invoice, Package, Course

        [Inject] private IAdminInvoiceService AdminInvoiceService { get; set; } = null!;
        [Inject] private IPaymentMethodService PaymentMethodService { get; set; } = null!;
        [Inject] private IAdminRefundService AdminRefundService { get; set; } = null!;
        [Inject] private IJSRuntime JS { get; set; } = null!;
        [Inject] private NavigationManager Manager { get; set; } = null!;

        private AdminRefundRequest model = new() { RefundDate = DateTime.Now };
        private DateTime refundDateLocal
        {
            get => model.RefundDate.ToLocalTime();
            set => model.RefundDate = value;
        }

        private bool isLoading = true;
        private bool isSubmitting = false;
        private string currencyCode = "USD";

        private List<PaymentMethodVm> paymentMethods = new();
        private List<RefundReasonDto> refundReasons = new();
        private List<RefundItemVm> refundItemsVm = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadPaymentMethodsAsync();
            await LoadRefundReasonsAsync();
            await LoadDataAsync();
        }

        private async Task LoadPaymentMethodsAsync()
        {
            var res = await PaymentMethodService.GetAllAsync();
            if (res.Success) paymentMethods = res.Data;
        }

        private async Task LoadRefundReasonsAsync()
        {
            var res = await AdminRefundService.GetRefundReasonsAsync();
            if (res.Success) refundReasons = res.Data;
        }

        private async Task LoadDataAsync()
        {
            isLoading = true;
            try
            {
                AdminInvoiceForEditResponse response = null;
                Guid id = Guid.Parse(ItemId);

                if (Type == "Invoice")
                {
                    var res = await AdminInvoiceService.GetInvoiceForRefundAsync(id);
                    if (res.Success) response = res.Data;
                }
                else if (Type == "Package")
                {
                    var res = await AdminInvoiceService.GetPackageForRefundAsync(id);
                    if (res.Success) response = res.Data;
                }
                else if (Type == "Course")
                {
                    var res = await AdminInvoiceService.GetCourseForRefundAsync(id);
                    if (res.Success) response = res.Data;
                }

                if (response != null)
                {
                    MapResponseToModel(response);
                }
                else
                {
                    await ShowError(ValidationResources.Error, "Failed to load data");
                }
            }
            catch (Exception ex)
            {
                await ShowError(ValidationResources.Error, ex.Message);
            }
            finally
            {
                isLoading = false;
            }
        }

        private void MapResponseToModel(AdminInvoiceForEditResponse response)
        {
            model.PaymentInvoiceId = Guid.Parse(response.InvoiceId);
            model.MemberId = response.MemberId;
            model.PaymentMethodId = response.PaymentMethodId;
            currencyCode = response.Currency;

            refundItemsVm = response.Items.Select(i => new RefundItemVm
            {
                ItemId = i.Id,
                Name = i.ItemName, // Should use localized name if available
                OriginalPrice = i.UnitPrice,
                RefundValue = i.UnitPrice,
                Scope = i.Scope,
                IsSelected = true
            }).ToList();

            CalculateTotalRefund();
        }

        private void CalculateTotalRefund()
        {
            model.RefundValue = refundItemsVm.Where(i => i.IsSelected).Sum(i => i.RefundValue);
        }

        /// <summary>
        /// Distributes the total refund value proportionally across selected items based on their current weights
        /// </summary>
        private void DistributeTotalRefund()
        {
            var selectedItems = refundItemsVm.Where(i => i.IsSelected).ToList();
            if (selectedItems.Count == 0 || model.RefundValue < 0)
                return;

            // Calculate current total and proportions
            var currentTotal = selectedItems.Sum(x => x.RefundValue);

            if (currentTotal == 0)
                return;

            // Distribute the new total proportionally
            var remainingTotal = model.RefundValue;

            for (int i = 0; i < selectedItems.Count; i++)
            {
                var item = selectedItems[i];
                var proportion = item.RefundValue / currentTotal;

                if (i == selectedItems.Count - 1)
                {
                    // Last item gets the remainder to avoid rounding errors
                    item.RefundValue = Math.Round(remainingTotal, 2);
                }
                else
                {
                    var newItemValue = Math.Round(model.RefundValue * proportion, 2);
                    item.RefundValue = newItemValue;
                    remainingTotal -= newItemValue;
                }
            }

            StateHasChanged();
        }

        private async Task SubmitRefund()
        {
            if (isSubmitting) return;

            if (model.RefundReseonId == Guid.Empty)
            {
                await ShowError(ValidationResources.Error, "Please select a refund reason");
                return;
            }

            if (!refundItemsVm.Any(i => i.IsSelected))
            {
                await ShowError(ValidationResources.Error, "Please select at least one item to refund");
                return;
            }

            isSubmitting = true;
            try
            {
                model.RefundItems = refundItemsVm.Where(i => i.IsSelected).Select(i => new ItemRefundRequest
                {
                    ItemId = i.ItemId,
                    RefundValue = i.RefundValue,
                    Scope = i.Scope
                }).ToList();

                var result = await AdminRefundService.SaveRefundAsync(model);
                if (result.Success)
                {
                    await ShowSuccess(NotifiAndAlertsResources.Success, "Refund saved successfully");
                    Manager.NavigateTo($"student/{model.MemberId}");
                }
                else
                {
                    await ShowError(ValidationResources.Error, result.Message);
                }
            }
            catch (Exception ex)
            {
                await ShowError(ValidationResources.Error, ex.Message);
            }
            finally
            {
                isSubmitting = false;
            }
        }

        private Task ShowSuccess(string title, string message)
        {
            var options = new { title, text = message, icon = "success" };
            return JS.InvokeVoidAsync("swal", options).AsTask();
        }

        private Task ShowError(string title, string message)
        {
            var options = new { title, text = message, icon = "error" };
            return JS.InvokeVoidAsync("swal", options).AsTask();
        }

    }

    public class RefundItemVm
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public string Name { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal RefundValue { get; set; }
        public PromoCodeScope Scope { get; set; }
        public bool IsSelected { get; set; }
    }
}
