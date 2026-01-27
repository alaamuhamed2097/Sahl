
using Common.Enumerations.Order;
using Common.Enumerations.Settings;
using Dashboard.Contracts.Order;
using Dashboard.Contracts.Setting;
using Dashboard.Pages.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shared.DTOs.Order.Payment.Refund;
using Shared.DTOs.Setting;

namespace Dashboard.Pages.Settings
{
	public partial class SystemSettings : LocalizedComponentBase
	{
		[Inject] private ISystemSettingsService SystemSettingsService { get; set; } = null!;
		[Inject] private IRefundService RefundService { get; set; } = null!;
		[Inject] private IJSRuntime JSRuntime { get; set; } = null!;
		[Inject] private ILogger<SystemSettings> Logger { get; set; } = null!;

		// Component state - Settings
		protected SystemSettingsViewModelDto Model { get; set; } = new();
		protected bool IsLoading { get; set; } = true;
		protected bool IsSaving { get; set; } = false;
		protected string ErrorMessage { get; set; } = string.Empty;
		protected string SuccessMessage { get; set; } = string.Empty;
		protected string ActiveTab { get; set; } = "tax";

		// Component state - Refunds
		protected List<RefundRequestDto> AllRefunds { get; set; } = new();
		protected List<RefundRequestDto> FilteredRefunds { get; set; } = new();
		protected RefundRequestDto? SelectedRefund { get; set; }
		protected bool IsLoadingRefunds { get; set; } = false;
		protected RefundStatus SelectedRefundStatus { get; set; }
		protected string SearchRefundOrderId { get; set; } = string.Empty;
		protected int PendingRefundsCount { get; set; } = 0;

		protected override async Task OnInitializedAsync()
		{
			await LoadSettings();
			await LoadRefunds();
		}

		private async Task LoadSettings()
		{
			try
			{
				IsLoading = true;
				ErrorMessage = string.Empty;

				var result = await SystemSettingsService.GetAllSettingsAsync();

				if (result.Success && result.Data != null)
				{
					Model = result.Data;
				}
				else
				{
					ErrorMessage = result.Message ?? "Failed to load settings";
					Model = GetDefaultSettings();
				}
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, "Error loading system settings");
				ErrorMessage = "An unexpected error occurred while loading settings";
				Model = GetDefaultSettings();
			}
			finally
			{
				IsLoading = false;
				StateHasChanged();
			}
		}

		protected async Task LoadRefunds()
		{
			try
			{
				IsLoadingRefunds = true;
				StateHasChanged();

				var result = await SystemSettingsService.GetAllRefundsAsync();

				if (result.Success && result.Data != null)
				{
					AllRefunds = result.Data.Items.ToList();
					FilterRefunds();
					PendingRefundsCount = AllRefunds.Count(r => r.RefundStatus == RefundStatus.Open);
				}
				else
				{
					AllRefunds = new List<RefundRequestDto>();
					FilteredRefunds = new List<RefundRequestDto>();
					PendingRefundsCount = 0;
				}
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, "Error loading refund requests");
				AllRefunds = new List<RefundRequestDto>();
				FilteredRefunds = new List<RefundRequestDto>();
				PendingRefundsCount = 0;
			}
			finally
			{
				IsLoadingRefunds = false;
				StateHasChanged();
			}
		}

		protected void FilterRefunds()
		{
			FilteredRefunds = AllRefunds;

			// Filter by status
			if (SelectedRefundStatus == RefundStatus.Open)
			{
				FilteredRefunds = FilteredRefunds
					.Where(r => r.RefundStatus == SelectedRefundStatus)
					.ToList();
			}

			// Filter by order ID
			if (!string.IsNullOrEmpty(SearchRefundOrderId))
			{
				FilteredRefunds = FilteredRefunds
					.Where(r => r.OrderDetailId.ToString().Contains(SearchRefundOrderId, StringComparison.OrdinalIgnoreCase))
					.ToList();
			}

			StateHasChanged();
		}

		protected async Task ApproveRefund(Guid refundId)
		{
			var confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
				"Are you sure you want to approve this refund request?");

			if (!confirmed) return;

			try
			{
				var updateDto = new UpdateRefundStatusDto
				{
					RefundId = refundId,
					NewStatus = RefundStatus.Approved,
				};

				var result = await SystemSettingsService.UpdateRefundStatusAsync(updateDto);

				if (result.Success)
				{
					SuccessMessage = "Refund request approved successfully!";
					await ShowSuccessNotification("Success", SuccessMessage);
					await LoadRefunds();
					CloseRefundModal();
				}
				else
				{
					ErrorMessage = result.Message ?? "Failed to approve refund request";
					await ShowErrorNotification("Error", ErrorMessage);
				}
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, "Error approving refund request");
				ErrorMessage = "An unexpected error occurred while approving the refund";
				await ShowErrorNotification("Error", ErrorMessage);
			}
		}

		protected async Task RejectRefund(Guid refundId)
		{
			var reason = await JSRuntime.InvokeAsync<string>("prompt",
				"Please provide a reason for rejecting this refund request:");

			if (string.IsNullOrWhiteSpace(reason)) return;

			try
			{
				var updateDto = new UpdateRefundStatusDto
				{
					RefundId = refundId,
					NewStatus = RefundStatus.Rejected,
				};

				var result = await SystemSettingsService.UpdateRefundStatusAsync(updateDto);

				if (result.Success)
				{
					SuccessMessage = "Refund request rejected successfully!";
					await ShowSuccessNotification("Success", SuccessMessage);
					await LoadRefunds();
					CloseRefundModal();
				}
				else
				{
					ErrorMessage = result.Message ?? "Failed to reject refund request";
					await ShowErrorNotification("Error", ErrorMessage);
				}
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, "Error rejecting refund request");
				ErrorMessage = "An unexpected error occurred while rejecting the refund";
				await ShowErrorNotification("Error", ErrorMessage);
			}
		}

		protected void ViewRefundDetails(RefundRequestDto refund)
		{
			SelectedRefund = refund;
			StateHasChanged();
		}

		protected void CloseRefundModal()
		{
			SelectedRefund = null;
			StateHasChanged();
		}

		protected string GetStatusBadgeClass(string status)
		{
			return status?.ToLower() switch
			{
				"pending" => "bg-warning text-dark",
				"approved" => "bg-success",
				"rejected" => "bg-danger",
				"completed" => "bg-info",
				_ => "bg-secondary"
			};
		}

		protected async Task SaveSettings()
		{
			try
			{
				IsSaving = true;
				ErrorMessage = string.Empty;
				SuccessMessage = string.Empty;
				StateHasChanged();

				var updates = new List<UpdateSystemSettingDto>
				{
                    // Tax Settings
                   new()
					{
						key = SystemSettingKey.OrderTaxPercentage,
						value = Model.OrderTaxPercentage.ToString(),
						dataType = SystemSettingDataType.Decimal,
						category = SystemSettingCategory.Tax
					},
					new()
					{
						key = SystemSettingKey.TaxIncludedInPrice,
						value = Model.TaxIncludedInPrice.ToString(),
						dataType = SystemSettingDataType.Boolean,
						category = SystemSettingCategory.Tax
					},
					new()
					{
						key = SystemSettingKey.RefundAllowedDays,
						value = Model.RefundAllowedDays.ToString(),
						dataType = SystemSettingDataType.Integer,
						category = SystemSettingCategory.RefundAllowedDays
					},
				};

				var result = await SystemSettingsService.UpdateSettingsBatchAsync(updates);

				if (result.Success)
				{
					SuccessMessage = "Settings saved successfully!";
					await ShowSuccessNotification("Success", SuccessMessage);
				}
				else
				{
					ErrorMessage = result.Message ?? "Failed to save settings";
					await ShowErrorNotification("Error", ErrorMessage);
				}
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, "Error saving system settings");
				ErrorMessage = "An unexpected error occurred while saving settings";
				await ShowErrorNotification("Error", ErrorMessage);
			}
			finally
			{
				IsSaving = false;
				StateHasChanged();
			}
		}

		protected async Task ResetToDefaults()
		{
			var confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
				"Are you sure you want to reset all settings to their default values? This action cannot be undone.");

			if (confirmed)
			{
				Model = GetDefaultSettings();
				SuccessMessage = "Settings have been reset to defaults. Don't forget to save!";
				ErrorMessage = string.Empty;
				StateHasChanged();
			}
		}

		protected void SetActiveTab(string tabName)
		{
			ActiveTab = tabName;

			// Load refunds when switching to refund tab
			if (tabName == "refund" && !AllRefunds.Any())
			{
				_ = LoadRefunds();
			}

			StateHasChanged();
		}

		protected string GetTabClass(string tabName)
		{
			return ActiveTab == tabName ? "nav-link active" : "nav-link";
		}

		private SystemSettingsViewModelDto GetDefaultSettings()
		{
			return new SystemSettingsViewModelDto
			{
				// Tax Settings
				OrderTaxPercentage = 0,
				RefundAllowedDays = 0,
				TaxIncludedInPrice = false,
			};
		}

		private async Task ShowErrorNotification(string title, string message)
		{
			await JSRuntime.InvokeVoidAsync("swal", title, message, "error");
		}

		private async Task ShowSuccessNotification(string title, string message)
		{
			await JSRuntime.InvokeVoidAsync("swal", title, message, "success");
		}
	}
}

//using Common.Enumerations.Settings;
//using Dashboard.Contracts.Setting;
//using Dashboard.Pages.Base;
//using Microsoft.AspNetCore.Components;
//using Microsoft.JSInterop;
//using Shared.DTOs.Setting;

//namespace Dashboard.Pages.Settings
//{
//    public partial class SystemSettings : LocalizedComponentBase
//    {
//        [Inject] private ISystemSettingsService SystemSettingsService { get; set; } = null!;
//        [Inject] private IJSRuntime JSRuntime { get; set; } = null!;
//        [Inject] private ILogger<SystemSettings> Logger { get; set; } = null!;

//        // Component state
//        protected SystemSettingsViewModelDto Model { get; set; } = new();
//        protected bool IsLoading { get; set; } = true;
//        protected bool IsSaving { get; set; } = false;
//        protected string ErrorMessage { get; set; } = string.Empty;
//        protected string SuccessMessage { get; set; } = string.Empty;
//        protected string ActiveTab { get; set; } = "tax";

//        protected override async Task OnInitializedAsync()
//        {
//            await LoadSettings();
//        }

//        private async Task LoadSettings()
//        {
//            try
//            {
//                IsLoading = true;
//                ErrorMessage = string.Empty;

//                var result = await SystemSettingsService.GetAllSettingsAsync();

//                if (result.Success && result.Data != null)
//                {
//                    Model = result.Data;
//                }
//                else
//                {
//                    ErrorMessage = result.Message ?? "Failed to load settings";
//                    Model = GetDefaultSettings();
//                }
//            }
//            catch (Exception ex)
//            {
//                Logger.LogError(ex, "Error loading system settings");
//                ErrorMessage = "An unexpected error occurred while loading settings";
//                Model = GetDefaultSettings();
//            }
//            finally
//            {
//                IsLoading = false;
//                StateHasChanged();
//            }
//        }

//        protected async Task SaveSettings()
//        {
//            try
//            {
//                IsSaving = true;
//                ErrorMessage = string.Empty;
//                SuccessMessage = string.Empty;
//                StateHasChanged();

//                var updates = new List<UpdateSystemSettingDto>
//                {
//                    // Tax Settings
//                   new()
//                    {
//                        key = SystemSettingKey.OrderTaxPercentage,
//                        value = Model.OrderTaxPercentage.ToString(),
//                        dataType = SystemSettingDataType.Decimal,
//                        category = SystemSettingCategory.Tax
//                    },
//                    new()
//                    {
//                        key = SystemSettingKey.TaxIncludedInPrice,
//                        value = Model.TaxIncludedInPrice.ToString(),
//                        dataType = SystemSettingDataType.Boolean,
//                        category = SystemSettingCategory.Tax
//                    },
//                    new()
//                    {
//                        key = SystemSettingKey.RefundAllowedDays,
//                        value = Model.RefundAllowedDays.ToString(),
//                        dataType = SystemSettingDataType.Integer,
//                        category = SystemSettingCategory.RefundAllowedDays
//                    },
//     //               // Shipping Settings

//     //               new() { key = SystemSettingKey.ShippingAmount, value = Model.ShippingAmount.ToString() },
//					//new() { key = SystemSettingKey.FreeShippingThreshold, value = Model.FreeShippingThreshold.ToString() },
//					//new() { key = SystemSettingKey.ShippingPerKg, value = Model.ShippingPerKg.ToString() },
//					//new() { key = SystemSettingKey.EstimatedDeliveryDays, value = Model.EstimatedDeliveryDays.ToString() },

//     //               // Order Settings
//     //               new() { key = SystemSettingKey.OrderExtraCost, value = Model.OrderExtraCost.ToString() },
//					//new() { key = SystemSettingKey.MinimumOrderAmount, value = Model.MinimumOrderAmount.ToString() },
//					//new() { key = SystemSettingKey.MaximumOrderAmount, value = Model.MaximumOrderAmount.ToString() },
//					//new() { key = SystemSettingKey.OrderCancellationPeriodHours, value = Model.OrderCancellationPeriodHours.ToString() },

//     //               // Payment Settings
//     //               new() { key = SystemSettingKey.PaymentGatewayEnabled, value = Model.PaymentGatewayEnabled.ToString() },
//					//new() { key = SystemSettingKey.CashOnDeliveryEnabled, value = Model.CashOnDeliveryEnabled.ToString() },

//     //               // Business Settings
//     //               new() { key = SystemSettingKey.MaintenanceMode, value = Model.MaintenanceMode.ToString() },
//					//new() { key = SystemSettingKey.AllowGuestCheckout, value = Model.AllowGuestCheckout.ToString() },

//     //               // Notification Settings
//     //               new() { key = SystemSettingKey.EmailNotificationsEnabled, value = Model.EmailNotificationsEnabled.ToString() },
//					//new() { key = SystemSettingKey.SmsNotificationsEnabled, value = Model.SmsNotificationsEnabled.ToString() },

//     //               // Security Settings
//     //               new() { key = SystemSettingKey.MaxLoginAttempts, value = Model.MaxLoginAttempts.ToString() },
//					//new() { key = SystemSettingKey.SessionTimeoutMinutes, value = Model.SessionTimeoutMinutes.ToString() },
//					//new() { key = SystemSettingKey.PasswordMinLength, value = Model.PasswordMinLength.ToString() }
//				};

//                var result = await SystemSettingsService.UpdateSettingsBatchAsync(updates);

//                if (result.Success)
//                {
//                    SuccessMessage = "Settings saved successfully!";
//                    await ShowSuccessNotification("Success", SuccessMessage);
//                }
//                else
//                {
//                    ErrorMessage = result.Message ?? "Failed to save settings";
//                    await ShowErrorNotification("Error", ErrorMessage);
//                }
//            }
//            catch (Exception ex)
//            {
//                Logger.LogError(ex, "Error saving system settings");
//                ErrorMessage = "An unexpected error occurred while saving settings";
//                await ShowErrorNotification("Error", ErrorMessage);
//            }
//            finally
//            {
//                IsSaving = false;
//                StateHasChanged();
//            }
//        }

//        protected async Task ResetToDefaults()
//        {
//            var confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
//                "Are you sure you want to reset all settings to their default values? This action cannot be undone.");

//            if (confirmed)
//            {
//                Model = GetDefaultSettings();
//                SuccessMessage = "Settings have been reset to defaults. Don't forget to save!";
//                ErrorMessage = string.Empty;
//                StateHasChanged();
//            }
//        }

//        protected void SetActiveTab(string tabName)
//        {
//            ActiveTab = tabName;
//            StateHasChanged();
//        }

//        protected string GetTabClass(string tabName)
//        {
//            return ActiveTab == tabName ? "nav-link active" : "nav-link";
//        }

//        private SystemSettingsViewModelDto GetDefaultSettings()
//        {
//            return new SystemSettingsViewModelDto
//            {
//                // Tax Settings
//                OrderTaxPercentage = 0,
//                RefundAllowedDays = 0,
//                TaxIncludedInPrice = false,

//                //// Shipping Settings
//                //ShippingAmount = 0,
//                //FreeShippingThreshold = 100,
//                //ShippingPerKg = 5,
//                //EstimatedDeliveryDays = 3,

//                //// Order Settings
//                //OrderExtraCost = 0,
//                //MinimumOrderAmount = 10,
//                //MaximumOrderAmount = 10000,
//                //OrderCancellationPeriodHours = 24,

//                //// Payment Settings
//                //PaymentGatewayEnabled = true,
//                //CashOnDeliveryEnabled = true,

//                //// Business Settings
//                //MaintenanceMode = false,
//                //AllowGuestCheckout = true,

//                //// Notification Settings
//                //EmailNotificationsEnabled = true,
//                //SmsNotificationsEnabled = false,

//                //// Security Settings
//                //MaxLoginAttempts = 5,
//                //SessionTimeoutMinutes = 30,
//                //PasswordMinLength = 8
//            };
//        }

//        private async Task ShowErrorNotification(string title, string message)
//        {
//            await JSRuntime.InvokeVoidAsync("swal", title, message, "error");
//        }

//        private async Task ShowSuccessNotification(string title, string message)
//        {
//            await JSRuntime.InvokeVoidAsync("swal", title, message, "success");
//        }
//    }
//}