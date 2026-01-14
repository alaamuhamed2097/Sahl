using Common.Enumeration;
using Dashboard.Contracts.Payment;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Payment;

namespace Dashboard.Pages.User.UserDetails.Components
{
    public partial class UserInvoicesAndPackagesTab
    {
        [Parameter]
        public Guid UserId { get; set; }

        [Inject]
        protected IAdminInvoiceService AdminInvoiceService { get; set; } = null!;

        [Inject]
        protected NavigationManager Navigation { get; set; } = null!;

        [Inject]
        protected IJSRuntime JSRuntime { get; set; } = null!;

        private List<AdminInvoiceDto>? invoices;
        private bool isLoading = true;
        private HashSet<int> expandedInvoices = new HashSet<int>();

        protected override async Task OnParametersSetAsync()
        {
            await LoadInvoicesAsync();
        }

        private async Task LoadInvoicesAsync()
        {
            try
            {
                isLoading = true;
                StateHasChanged();

                var response = await AdminInvoiceService.GetMemberInvoicesWithItemsAsync(UserId.ToString());

                if (response?.Success == true && response.Data != null)
                {
                    invoices = response.Data
                        .OrderByDescending(i => i.InvoiceDateUtc)
                        .ToList();
                }
                else
                {
                    invoices = new List<AdminInvoiceDto>();
                }
            }
            catch (Exception ex)
            {
                invoices = new List<AdminInvoiceDto>();
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        private void ToggleInvoiceItems(int invoiceIndex)
        {
            if (expandedInvoices.Contains(invoiceIndex))
            {
                expandedInvoices.Remove(invoiceIndex);
            }
            else
            {
                expandedInvoices.Add(invoiceIndex);
            }
        }

        private void OpenCreateModal()
        {
            Navigation.NavigateTo($"AddInvoice/{UserId}", true);
        }

        private void EditInvoice(string invoiceId)
        {
            Navigation.NavigateTo($"UpdateInvoice/{invoiceId}", true);
        }

        private async Task DeleteInvoice(string invoiceId)
        {
            var confirmed = await DeleteConfirmNotification();
            if (!confirmed) return;

            try
            {
                var res = await AdminInvoiceService.DeleteAsync(invoiceId);
                if (res.Success)
                {
                    await ShowSuccessNotification(NotifiAndAlertsResources.DeletedSuccessfully);
                    await LoadInvoicesAsync();
                }
                else
                {
                    await ShowErrorNotification(ValidationResources.Failed, res.Message ?? NotifiAndAlertsResources.DeleteFailed);
                }
            }
            catch (Exception ex)
            {
                await ShowErrorNotification(ValidationResources.Error, NotifiAndAlertsResources.DeleteFailed);
            }
        }

        protected virtual async Task<bool> DeleteConfirmNotification()
        {
            var options = new
            {
                title = NotifiAndAlertsResources.AreYouSure,
                text = NotifiAndAlertsResources.ConfirmDeleteAlert,
                icon = "warning",
                buttons = new
                {
                    cancel = new
                    {
                        text = ActionsResources.Cancel,
                        value = false,
                        visible = true,
                        className = "",
                        closeModal = true
                    },
                    confirm = new
                    {
                        text = ActionsResources.Confirm,
                        value = true,
                        visible = true,
                        className = "swal-button--danger",
                        closeModal = true
                    }
                }
            };

            return (await JSRuntime.InvokeAsync<bool>("swal", options));
        }

        private async Task ShowSuccessNotification(string message)
        {
            var options = new { title = NotifiAndAlertsResources.Success, text = message, icon = "success" };
            await JSRuntime.InvokeVoidAsync("swal", options);
        }

        private async Task ShowErrorNotification(string title, string message)
        {
            var options = new { title = title, text = message, icon = "error" };
            await JSRuntime.InvokeVoidAsync("swal", options);
        }

        private string GetPaymentStatus(int paymentState)
        {
            return paymentState switch
            {
                1 => AdminResources.Invoice_Paid,
                0 => AdminResources.Invoice_Pending,
                _ => AdminResources.Invoice_Unknown
            };
        }

        private string GetStatusClass(int paymentState)
        {
            return paymentState switch
            {
                1 => "paid",
                0 => "hold",
                _ => "pending"
            };
        }

        private string GetItemTypeLabel(PromoCodeScope scope)
        {
            return scope switch
            {
                PromoCodeScope.Course => AdminResources.Item_Course,
                PromoCodeScope.Package => AdminResources.Item_Package,
                PromoCodeScope.Product => AdminResources.Item_Product,
                _ => AdminResources.Item_Other
            };
        }

        private string GetItemTypeIcon(PromoCodeScope scope)
        {
            return scope switch
            {
                PromoCodeScope.Course => "📚",
                PromoCodeScope.Package => "📦",
                PromoCodeScope.Product => "🛍️",
                _ => "📄"
            };
        }

        private string GetItemTypeClass(PromoCodeScope scope)
        {
            return scope switch
            {
                PromoCodeScope.Course => "course",
                PromoCodeScope.Package => "package",
                PromoCodeScope.Product => "product",
                _ => "other"
            };
        }
    }
}
