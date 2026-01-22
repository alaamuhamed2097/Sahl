
using Common.Enumerations.Settings;
using Dashboard.Contracts.Setting;
using Dashboard.Pages.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shared.DTOs.Setting;

namespace Dashboard.Pages.Settings
{
    public partial class SystemSettings : LocalizedComponentBase
    {
        [Inject] private ISystemSettingsService SystemSettingsService { get; set; } = null!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = null!;
        [Inject] private ILogger<SystemSettings> Logger { get; set; } = null!;

        // Component state
        protected SystemSettingsViewModelDto Model { get; set; } = new();
        protected bool IsLoading { get; set; } = true;
        protected bool IsSaving { get; set; } = false;
        protected string ErrorMessage { get; set; } = string.Empty;
        protected string SuccessMessage { get; set; } = string.Empty;
        protected string ActiveTab { get; set; } = "tax";

        protected override async Task OnInitializedAsync()
        {
            await LoadSettings();
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
     //               // Shipping Settings

     //               new() { key = SystemSettingKey.ShippingAmount, value = Model.ShippingAmount.ToString() },
					//new() { key = SystemSettingKey.FreeShippingThreshold, value = Model.FreeShippingThreshold.ToString() },
					//new() { key = SystemSettingKey.ShippingPerKg, value = Model.ShippingPerKg.ToString() },
					//new() { key = SystemSettingKey.EstimatedDeliveryDays, value = Model.EstimatedDeliveryDays.ToString() },

     //               // Order Settings
     //               new() { key = SystemSettingKey.OrderExtraCost, value = Model.OrderExtraCost.ToString() },
					//new() { key = SystemSettingKey.MinimumOrderAmount, value = Model.MinimumOrderAmount.ToString() },
					//new() { key = SystemSettingKey.MaximumOrderAmount, value = Model.MaximumOrderAmount.ToString() },
					//new() { key = SystemSettingKey.OrderCancellationPeriodHours, value = Model.OrderCancellationPeriodHours.ToString() },

     //               // Payment Settings
     //               new() { key = SystemSettingKey.PaymentGatewayEnabled, value = Model.PaymentGatewayEnabled.ToString() },
					//new() { key = SystemSettingKey.CashOnDeliveryEnabled, value = Model.CashOnDeliveryEnabled.ToString() },

     //               // Business Settings
     //               new() { key = SystemSettingKey.MaintenanceMode, value = Model.MaintenanceMode.ToString() },
					//new() { key = SystemSettingKey.AllowGuestCheckout, value = Model.AllowGuestCheckout.ToString() },

     //               // Notification Settings
     //               new() { key = SystemSettingKey.EmailNotificationsEnabled, value = Model.EmailNotificationsEnabled.ToString() },
					//new() { key = SystemSettingKey.SmsNotificationsEnabled, value = Model.SmsNotificationsEnabled.ToString() },

     //               // Security Settings
     //               new() { key = SystemSettingKey.MaxLoginAttempts, value = Model.MaxLoginAttempts.ToString() },
					//new() { key = SystemSettingKey.SessionTimeoutMinutes, value = Model.SessionTimeoutMinutes.ToString() },
					//new() { key = SystemSettingKey.PasswordMinLength, value = Model.PasswordMinLength.ToString() }
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

                //// Shipping Settings
                //ShippingAmount = 0,
                //FreeShippingThreshold = 100,
                //ShippingPerKg = 5,
                //EstimatedDeliveryDays = 3,

                //// Order Settings
                //OrderExtraCost = 0,
                //MinimumOrderAmount = 10,
                //MaximumOrderAmount = 10000,
                //OrderCancellationPeriodHours = 24,

                //// Payment Settings
                //PaymentGatewayEnabled = true,
                //CashOnDeliveryEnabled = true,

                //// Business Settings
                //MaintenanceMode = false,
                //AllowGuestCheckout = true,

                //// Notification Settings
                //EmailNotificationsEnabled = true,
                //SmsNotificationsEnabled = false,

                //// Security Settings
                //MaxLoginAttempts = 5,
                //SessionTimeoutMinutes = 30,
                //PasswordMinLength = 8
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
//using Dashboard.Contracts.Setting;
//using Microsoft.AspNetCore.Components;
//using Microsoft.JSInterop;
//using Resources;
//using Shared.DTOs.Setting;
//using Common.Enumerations.Settings;

//namespace Dashboard.Pages.Settings
//{
//	public partial class SystemSettings : ComponentBase
//	{
//		[Inject] private ISystemSettingsService SystemSettingsService { get; set; } = null!;
//		[Inject] private IJSRuntime JSRuntime { get; set; } = null!;
//		[Inject] private ILogger<SystemSettings> Logger { get; set; } = null!;

//		// Component state
//		protected SystemSettingsViewModelDto Model { get; set; } = new();
//		protected bool IsLoading { get; set; } = true;
//		protected bool IsSaving { get; set; } = false;
//		protected string ErrorMessage { get; set; } = string.Empty;
//		protected string SuccessMessage { get; set; } = string.Empty;
//		protected string ActiveTab { get; set; } = "tax";

//		protected override async Task OnInitializedAsync()
//		{
//			await LoadSettings();
//		}

//		private async Task LoadSettings()
//		{
//			try
//			{
//				IsLoading = true;
//				ErrorMessage = string.Empty;

//				var result = await SystemSettingsService.GetAllSettingsAsync();

//				if (result.Success && result.Data != null)
//				{
//					Model = result.Data;
//				}
//				else
//				{
//					ErrorMessage = result.Message ?? "Failed to load settings";
//					Model = GetDefaultSettings();
//				}
//			}
//			catch (Exception ex)
//			{
//				Logger.LogError(ex, "Error loading system settings");
//				ErrorMessage = "An unexpected error occurred while loading settings";
//				Model = GetDefaultSettings();
//			}
//			finally
//			{
//				IsLoading = false;
//				StateHasChanged();
//			}
//		}

//		#region Save Methods for Each Tab

//		protected async Task SaveTaxSettings()
//		{
//			await SaveSettingsGroup("Tax Settings", new List<UpdateSystemSettingDto>
//			{
//				new() { Key = SystemSettingKey.OrderTaxPercentage, value = Model.OrderTaxPercentage.ToString() },
//				new() { Key = SystemSettingKey.TaxIncludedInPrice, value = Model.TaxIncludedInPrice.ToString() }
//			});
//		}

//		protected async Task SaveShippingSettings()
//		{
//			await SaveSettingsGroup("Shipping Settings", new List<UpdateSystemSettingDto>
//			{
//				new() { Key = SystemSettingKey.ShippingAmount, value = Model.ShippingAmount.ToString() },
//				new() { Key = SystemSettingKey.FreeShippingThreshold, value = Model.FreeShippingThreshold.ToString() },
//				new() { Key = SystemSettingKey.ShippingPerKg, value = Model.ShippingPerKg.ToString() },
//				new() { Key = SystemSettingKey.EstimatedDeliveryDays, value = Model.EstimatedDeliveryDays.ToString() }
//			});
//		}

//		protected async Task SaveOrderSettings()
//		{
//			await SaveSettingsGroup("Order Settings", new List<UpdateSystemSettingDto>
//			{
//				new() { Key = SystemSettingKey.OrderExtraCost, value = Model.OrderExtraCost.ToString() },
//				new() { Key = SystemSettingKey.MinimumOrderAmount, value = Model.MinimumOrderAmount.ToString() },
//				new() { Key = SystemSettingKey.MaximumOrderAmount, value = Model.MaximumOrderAmount.ToString() },
//				new() { Key = SystemSettingKey.OrderCancellationPeriodHours, value = Model.OrderCancellationPeriodHours.ToString() }
//			});
//		}

//		protected async Task SavePaymentSettings()
//		{
//			await SaveSettingsGroup("Payment Settings", new List<UpdateSystemSettingDto>
//			{
//				new() { Key = SystemSettingKey.PaymentGatewayEnabled, value = Model.PaymentGatewayEnabled.ToString() },
//				new() { Key = SystemSettingKey.CashOnDeliveryEnabled, value = Model.CashOnDeliveryEnabled.ToString() }
//			});
//		}

//		protected async Task SaveBusinessSettings()
//		{
//			await SaveSettingsGroup("Business Settings", new List<UpdateSystemSettingDto>
//			{
//				new() { Key = SystemSettingKey.MaintenanceMode, value = Model.MaintenanceMode.ToString() },
//				new() { Key = SystemSettingKey.AllowGuestCheckout, value = Model.AllowGuestCheckout.ToString() }
//			});
//		}

//		protected async Task SaveNotificationSettings()
//		{
//			await SaveSettingsGroup("Notification Settings", new List<UpdateSystemSettingDto>
//			{
//				new() { Key = SystemSettingKey.EmailNotificationsEnabled, value = Model.EmailNotificationsEnabled.ToString() },
//				new() { Key = SystemSettingKey.SmsNotificationsEnabled, value = Model.SmsNotificationsEnabled.ToString() }
//			});
//		}

//		protected async Task SaveSecuritySettings()
//		{
//			await SaveSettingsGroup("Security Settings", new List<UpdateSystemSettingDto>
//			{
//				new() { Key = SystemSettingKey.MaxLoginAttempts, value = Model.MaxLoginAttempts.ToString() },
//				new() { Key = SystemSettingKey.SessionTimeoutMinutes, value = Model.SessionTimeoutMinutes.ToString() },
//				new() { Key = SystemSettingKey.PasswordMinLength, value = Model.PasswordMinLength.ToString() }
//			});
//		}

//		#endregion

//		#region Helper Methods

//		private async Task SaveSettingsGroup(string groupName, List<UpdateSystemSettingDto> updates)
//		{
//			try
//			{
//				IsSaving = true;
//				ErrorMessage = string.Empty;
//				SuccessMessage = string.Empty;
//				StateHasChanged();

//				var result = await SystemSettingsService.UpdateSettingsBatchAsync(updates);

//				if (result.Success)
//				{
//					SuccessMessage = $"{groupName} saved successfully!";
//					await ShowSuccessNotification("Success", SuccessMessage);
//				}
//				else
//				{
//					ErrorMessage = result.Message ?? $"Failed to save {groupName}";
//					await ShowErrorNotification("Error", ErrorMessage);
//				}
//			}
//			catch (Exception ex)
//			{
//				Logger.LogError(ex, "Error saving {GroupName}", groupName);
//				ErrorMessage = $"An unexpected error occurred while saving {groupName}";
//				await ShowErrorNotification("Error", ErrorMessage);
//			}
//			finally
//			{
//				IsSaving = false;
//				StateHasChanged();
//			}
//		}

//		protected void SetActiveTab(string tabName)
//		{
//			ActiveTab = tabName;
//			// Clear messages when switching tabs
//			ErrorMessage = string.Empty;
//			SuccessMessage = string.Empty;
//			StateHasChanged();
//		}

//		protected string GetTabClass(string tabName)
//		{
//			return ActiveTab == tabName ? "nav-link active" : "nav-link";
//		}

//		private SystemSettingsViewModelDto GetDefaultSettings()
//		{
//			return new SystemSettingsViewModelDto
//			{
//				// Tax Settings
//				OrderTaxPercentage = 0,
//				TaxIncludedInPrice = false,

//				// Shipping Settings
//				ShippingAmount = 0,
//				FreeShippingThreshold = 100,
//				ShippingPerKg = 5,
//				EstimatedDeliveryDays = 3,

//				// Order Settings
//				OrderExtraCost = 0,
//				MinimumOrderAmount = 10,
//				MaximumOrderAmount = 10000,
//				OrderCancellationPeriodHours = 24,

//				// Payment Settings
//				PaymentGatewayEnabled = true,
//				CashOnDeliveryEnabled = true,

//				// Business Settings
//				MaintenanceMode = false,
//				AllowGuestCheckout = true,

//				// Notification Settings
//				EmailNotificationsEnabled = true,
//				SmsNotificationsEnabled = false,

//				// Security Settings
//				MaxLoginAttempts = 5,
//				SessionTimeoutMinutes = 30,
//				PasswordMinLength = 8
//			};
//		}

//		private async Task ShowErrorNotification(string title, string message)
//		{
//			await JSRuntime.InvokeVoidAsync("swal", title, message, "error");
//		}

//		private async Task ShowSuccessNotification(string title, string message)
//		{
//			await JSRuntime.InvokeVoidAsync("swal", title, message, "success");
//		}

//		#endregion
//	}
//}