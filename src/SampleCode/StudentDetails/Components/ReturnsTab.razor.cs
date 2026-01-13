using Common.Enumeration;
using Dashboard.Contracts.Refund;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Refund;

namespace Dashboard.Pages.User.Student.StudentDetails.Components
{
    public partial class ReturnsTab
    {
        [Parameter] public Guid UserId { get; set; }

        [Inject] private IAdminRefundService AdminRefundService { get; set; }
        [Inject] private IJSRuntime JS { get; set; }
        [Inject] private NavigationManager Navigation { get; set; }

        private List<RefundInvoiceDetailsDto> refunds = new();
        private HashSet<int> expandedRefunds = new();
        private bool isLoading = true;

        protected override async Task OnInitializedAsync()
        {
            await LoadRefundsAsync();
        }

        private async Task LoadRefundsAsync()
        {
            isLoading = true;
            try
            {
                var result = await AdminRefundService.GetRefundInvoicesByMemberIdAsync(UserId.ToString());
                if (result.Success)
                {
                    refunds = result.Data?.OrderByDescending(r => r.CreatedDate).ToList() ?? new();
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
                isLoading = false;
                StateHasChanged();
            }
        }

        private void ToggleRefundItems(int index)
        {
            if (expandedRefunds.Contains(index))
            {
                expandedRefunds.Remove(index);
            }
            else
            {
                expandedRefunds.Add(index);
            }
            StateHasChanged();
        }

        private void EditRefund(Guid refundId)
        {
            Navigation.NavigateTo($"/UpdateRefund/{refundId}");
        }

        private async Task DeleteRefund(Guid refundId)
        {
            var confirmed = await DeleteConfirmNotification();
            if (!confirmed) return;

            try
            {
                var result = await AdminRefundService.DeleteRefundAsync(refundId);
                if (result.Success)
                {
                    await ShowSuccess(NotifiAndAlertsResources.Success, "Refund deleted successfully");
                    await LoadRefundsAsync();
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
                StateHasChanged();
            }
        }

        // Helper method to get all items (courses + packages) for a refund
        private List<RefundItemDisplay> GetRefundItems(RefundInvoiceDetailsDto refund)
        {
            var items = new List<RefundItemDisplay>();

            // Add courses
            if (refund.RefundCourses != null)
            {
                items.AddRange(refund.RefundCourses.Select(c => new RefundItemDisplay
                {
                    ItemName = c.CourseName,
                    RefundValue = c.RequestValue,
                    Scope = "Course"
                }));
            }

            // Add packages
            if (refund.RefundPackages != null)
            {
                items.AddRange(refund.RefundPackages.Select(p => new RefundItemDisplay
                {
                    ItemName = p.PackageName,
                    RefundValue = p.RefundValue,
                    Scope = "Package"
                }));
            }

            return items;
        }

        private string GetStatusClass(RefundState status)
        {
            return status switch
            {
                RefundState.Approved => "status-completed",
                RefundState.Pending => "status-pending",
                RefundState.Rejected => "status-failed",
                _ => "status-pending"
            };
        }

        private string GetStatusText(RefundState status)
        {
            return status switch
            {
                RefundState.Approved => NotifiAndAlertsResources.Completed,
                RefundState.Pending => NotifiAndAlertsResources.Pending,
                RefundState.Rejected => NotifiAndAlertsResources.Failed,
                _ => status.ToString()
            };
        }

        private string GetItemTypeClass(string scope)
        {
            return scope?.ToLower() switch
            {
                "course" => "item-type-course",
                "package" => "item-type-package",
                _ => "item-type-other"
            };
        }

        private string GetItemTypeIcon(string scope)
        {
            return scope?.ToLower() switch
            {
                "course" => "📚",
                "package" => "📦",
                _ => "📄"
            };
        }

        private string GetItemTypeLabel(string scope)
        {
            return scope?.ToLower() switch
            {
                "course" => AdminResources.Course,
                "package" => AdminResources.Package,
                _ => scope
            };
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

            return (await JS.InvokeAsync<bool>("swal", options));
        }


        // Helper class for displaying refund items
        private class RefundItemDisplay
        {
            public string ItemName { get; set; }
            public decimal RefundValue { get; set; }
            public string Scope { get; set; }
        }
    }
}