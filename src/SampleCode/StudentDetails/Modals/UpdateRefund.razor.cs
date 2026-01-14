using Common.Enumeration;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Refund;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Payment;
using Shared.DTOs.Refund;
using Shared.GeneralModels.RequestModels;

namespace Dashboard.Pages.User.Student.StudentDetails.Modals
{
    public partial class UpdateRefund
    {
        [Parameter] public string RefundId { get; set; }

        [Inject] private IAdminRefundService AdminRefundService { get; set; } = null!;
        [Inject] private IPaymentMethodService PaymentMethodService { get; set; } = null!;
        [Inject] private IJSRuntime JS { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        private AdminRefundRequest model = new() { RefundDate = DateTime.UtcNow };
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
            await LoadRefundDataAsync();
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

        private async Task LoadRefundDataAsync()
        {
            isLoading = true;
            try
            {
                var refundId = Guid.Parse(RefundId);
                var refundResponse = await AdminRefundService.GetRefundInvoiceByIdAsync(refundId);

                if (refundResponse.Success && refundResponse.Data != null)
                {
                    var refund = refundResponse.Data;
                    model = new AdminRefundRequest
                    {
                        Id = refund.Id,
                        PaymentInvoiceId = refund.PaymentInvoiceId,
                        MemberId = refund.MemberId,
                        PaymentMethodId = refund.PaymentMethodId,
                        RefundReseonId = refund.RefundReasonId,
                        RefundValue = refund.RefundValue,
                        Comment = refund.Comment,
                        RefundDate = refund.CreatedDate,
                        RefundItems = new List<ItemRefundRequest>()
                    };

                    MapRefundItems(model, refund);

                    currencyCode = refund.CurrencyName;
                }
                else
                {
                    await ShowError(ValidationResources.Error, "Failed to load refund data");
                    Navigation.NavigateTo($"/student/{model.MemberId}");
                }
            }
            catch (Exception ex)
            {
                await ShowError(ValidationResources.Error, ex.Message);
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        private void MapRefundItems(AdminRefundRequest model, RefundInvoiceDetailsDto refund)
        {
            // Map refund courses
            if (refund.RefundCourses != null)
            {
                foreach (var course in refund.RefundCourses)
                {
                    model.RefundItems.Append<ItemRefundRequest>(new ItemRefundRequest
                    {
                        Id = course.ReturnInvoiceId,
                        ItemId = course.CourseId,
                        RefundValue = course.RequestValue,
                        Scope = PromoCodeScope.Course // or whatever scope you use for courses
                    });
                }
            }

            // Map refund packages
            if (refund.RefundPackages != null)
            {
                foreach (var package in refund.RefundPackages)
                {
                    model.RefundItems.Append<ItemRefundRequest>(new ItemRefundRequest
                    {
                        Id = package.Id,
                        ItemId = package.MemberPackageId,
                        RefundValue = package.RefundValue,
                        Scope = PromoCodeScope.Package // or whatever scope you use for packages
                    });
                }
            }

            // Map to view model for display
            refundItemsVm = new List<RefundItemVm>();

            // Add courses to view model
            if (refund.RefundCourses != null)
            {
                refundItemsVm.AddRange(refund.RefundCourses.Select(course => new RefundItemVm
                {
                    Id = course.ReturnInvoiceId,
                    ItemId = course.CourseId,
                    Name = course.CourseName,
                    OriginalPrice = course.RequestValue, // You might need to adjust this if you have the original price
                    RefundValue = course.RequestValue,
                    Scope = PromoCodeScope.Course, // or whatever scope you use for courses
                    IsSelected = true
                }));
            }

            // Add packages to view model
            if (refund.RefundPackages != null)
            {
                refundItemsVm.AddRange(refund.RefundPackages.Select(pkg => new RefundItemVm
                {
                    Id = pkg.Id,
                    ItemId = pkg.MemberPackageId,
                    Name = pkg.PackageName,
                    OriginalPrice = pkg.RefundValue, // You might need to adjust this if you have the original price
                    RefundValue = pkg.RefundValue,
                    Scope = PromoCodeScope.Package, // or whatever scope you use for packages
                    IsSelected = true
                }));
            }
        }

        private void CalculateTotalRefund()
        {
            model.RefundValue = refundItemsVm.Where(i => i.IsSelected).Sum(i => i.RefundValue);
        }

        private void DistributeTotalRefund()
        {
            var selectedItems = refundItemsVm.Where(i => i.IsSelected).ToList();
            if (selectedItems.Count == 0 || model.RefundValue < 0)
                return;

            var currentTotal = selectedItems.Sum(x => x.RefundValue);

            if (currentTotal == 0)
                return;

            var remainingTotal = model.RefundValue;

            for (int i = 0; i < selectedItems.Count; i++)
            {
                var item = selectedItems[i];
                var proportion = item.RefundValue / currentTotal;

                if (i == selectedItems.Count - 1)
                {
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
                model.RefundItems = refundItemsVm
                    .Where(i => i.IsSelected)
                    .Select(i => new ItemRefundRequest
                    {
                        Id = i.Id,
                        ItemId = i.ItemId,
                        RefundValue = i.RefundValue,
                        Scope = i.Scope
                    }).ToList();

                var result = await AdminRefundService.SaveRefundAsync(model);
                if (result.Success)
                {
                    await ShowSuccess(NotifiAndAlertsResources.Success, "Refund updated successfully");
                    Navigation.NavigateTo($"student/{model.MemberId}");
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
                StateHasChanged();
            }
        }

        private void Cancel()
        {
            Navigation.NavigateTo($"student/{model.MemberId}");
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
}