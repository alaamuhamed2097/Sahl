using Dashboard.Contracts.General;
using Dashboard.Contracts.Warehouse;
using Dashboard.Services.General;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Vendor;
using Shared.DTOs.Warehouse;

namespace Dashboard.Pages.Warehouse
{
	public partial class Details
	{
		private bool isSaving { get; set; }
		private IEnumerable<CountryInfo>? countries;
		private bool isMultiVendorEnabled = false;

		protected WarehouseDto Model { get; set; } = new();

		// Vendors list with new DTO
		private IEnumerable<VendorWithUserDto> vendors = new List<VendorWithUserDto>();
		private Guid? selectedVendorId = null;
		private bool showVendorValidation = false;

		[Parameter] public Guid Id { get; set; }
		[Parameter] public string? Type { get; set; } // "platform" or "vendor"

		[Inject] private IJSRuntime JSRuntime { get; set; } = default!;
		[Inject] protected NavigationManager Navigation { get; set; } = null!;
		[Inject] protected IWarehouseService WarehouseService { get; set; } = null!;
		[Inject] protected ICountryPhoneCodeService CountryPhoneCodeService { get; set; } = null!;

		// Properties for UI
		protected string PageTitle => Id == Guid.Empty ? "Add" : "Edit";
		protected bool IsEditMode => Id != Guid.Empty;
		protected bool IsVendorWarehouse => Type?.ToLower() == "vendor";
		protected bool IsPlatformWarehouse => Type?.ToLower() == "platform" || string.IsNullOrEmpty(Type);

		protected override async Task OnInitializedAsync()
		{
			await LoadCountriesAsync();
			await CheckMultiVendorStatus();

			// Load vendors for vendor warehouse in add mode
			//if (isMultiVendorEnabled && IsVendorWarehouse)
			//{
				await LoadVendors();
			//}

			// Initialize new model
			Model = new WarehouseDto
			{
				IsActive = true,
				IsDefaultPlatformWarehouse = IsPlatformWarehouse
			};

			Console.WriteLine($"OnInitializedAsync - Type: {Type}, IsPlatformWarehouse: {IsPlatformWarehouse}, IsVendorWarehouse: {IsVendorWarehouse}");
		}

		protected override async Task OnParametersSetAsync()
		{
			// If editing existing warehouse
			if (Id != Guid.Empty)
			{
				await Edit(Id);
			}
			// If adding new warehouse, set the type
			else
			{
				Model.IsDefaultPlatformWarehouse = IsPlatformWarehouse;
			}
		}

		private async Task CheckMultiVendorStatus()
		{
			try
			{
				var result = await WarehouseService.IsMultiVendorEnabledAsync();
				await LoadVendors();
				if (result.Success)
				{
					isMultiVendorEnabled = result.Data;
					Console.WriteLine($"Multi-vendor enabled: {isMultiVendorEnabled}");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error checking multi-vendor status: {ex.Message}");
			}
		}

		private async Task LoadVendors()
		{
			try
			{
				Console.WriteLine("Loading vendors...");
				var result = await WarehouseService.GetActiveVendorsAsync();

				if (result.Success && result.Data != null)
				{
					vendors = result.Data;
					Console.WriteLine($"Loaded {vendors.Count()} vendors");

					// Log vendor details for debugging
					foreach (var vendor in vendors)
					{
						Console.WriteLine($"Vendor: {vendor.UserName} - {vendor.Email} - ID: {vendor.VendorId}");
					}

					StateHasChanged();
				}
				else
				{
					Console.WriteLine($"Failed to load vendors: {result.Message}");
					vendors = new List<VendorWithUserDto>();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Exception loading vendors: {ex.Message}");
				await ShowErrorNotification("Error", "Failed to load vendors");
				vendors = new List<VendorWithUserDto>();
			}
		}

		/// <summary>
		/// Method called when vendor selection changes from select element
		/// </summary>
		private async Task OnVendorSelectionChanged(ChangeEventArgs e)
		{
			var value = e.Value?.ToString();
			Console.WriteLine($"OnVendorSelectionChanged called - value: {value}");

			if (!string.IsNullOrEmpty(value) && Guid.TryParse(value, out Guid vendorId))
			{
				selectedVendorId = vendorId;
				showVendorValidation = false;
				await OnVendorChanged();
			}
			else
			{
				selectedVendorId = null;
				Model.VendorId = null;
				Model.Email = string.Empty;
				StateHasChanged();
			}
		}

		/// <summary>
		/// Method called when vendor selection changes
		/// </summary>
		private async Task OnVendorChanged()
		{
			Console.WriteLine($"OnVendorChanged called - selectedVendorId: {selectedVendorId}");

			if (selectedVendorId.HasValue && selectedVendorId.Value != Guid.Empty)
			{
				// Find selected vendor
				var selectedVendor = vendors?.FirstOrDefault(v => v.VendorId == selectedVendorId.Value);

				if (selectedVendor != null)
				{
					Console.WriteLine($"Selected vendor: {selectedVendor.UserName} - {selectedVendor.Email}");

					// Update Model with vendor information
					Model.VendorId = selectedVendor.VendorId;
					Model.Email = selectedVendor.Email;

					StateHasChanged();
				}
				else
				{
					Console.WriteLine("Selected vendor not found in list");
				}
			}
			else
			{
				Console.WriteLine("Clearing vendor information");
				// Clear vendor information
				Model.VendorId = null;
				Model.Email = string.Empty;
			}
		}

		private async Task LoadCountriesAsync()
		{
			try
			{
				if (CountryPhoneCodeService != null)
				{
					countries = CountryPhoneCodeService.GetAllCountries(
						ResourceManager.CurrentLanguage == Resources.Enumerations.Language.Arabic ? "ar" : "en");
				}
			}
			catch (Exception)
			{
				countries = CountryPhoneCodeService?.GetFallbackCountries();
			}
		}

		protected async Task Save()
		{
			try
			{
				// Validation for Vendor Warehouses
				if (IsVendorWarehouse)
				{
					// In Add mode, check vendor selection
					if (!IsEditMode && !Model.VendorId.HasValue)
					{
						showVendorValidation = true;
						StateHasChanged();
						await ShowErrorNotification(ValidationResources.Error, "Please select a vendor");
						return;
					}

					// Check email
					if (string.IsNullOrWhiteSpace(Model.Email))
					{
						await ShowErrorNotification(ValidationResources.Error, "Vendor email is required");
						return;
					}
				}

				showVendorValidation = false;
				isSaving = true;
				StateHasChanged();

				// Set warehouse type
				Model.IsDefaultPlatformWarehouse = IsPlatformWarehouse;

				// Clear VendorId if it's a platform warehouse
				if (IsPlatformWarehouse)
				{
					Model.VendorId = null;
					Model.Email = null;
				}

				Console.WriteLine($"Saving warehouse - Type: {(IsPlatformWarehouse ? "Platform" : "Vendor")}, VendorId: {Model.VendorId}, Email: {Model.Email}");

				var result = await WarehouseService.SaveAsync(Model);
				isSaving = false;

				if (result.Success)
				{
					await ShowSuccessNotification(NotifiAndAlertsResources.SavedSuccessfully);
					await CloseModal();
				}
				else
				{
					await ShowErrorNotification(ValidationResources.Failed, result.Message ?? NotifiAndAlertsResources.SaveFailed);
				}
			}
			catch (Exception ex)
			{
				isSaving = false;
				Console.WriteLine($"Save exception: {ex.Message}");
				await ShowErrorNotification(NotifiAndAlertsResources.FailedAlert, ex.Message ?? NotifiAndAlertsResources.SomethingWentWrong);
			}
		}

		protected async Task Edit(Guid id)
		{
			try
			{
				Console.WriteLine($"Editing warehouse with ID: {id}");

				var result = await WarehouseService.GetByIdAsync(id);
				if (!result.Success)
				{
					await ShowErrorNotification(ValidationResources.Failed, NotifiAndAlertsResources.FailedToRetrieveData);
					return;
				}

				Model = result.Data ?? new();

				// Update Type based on loaded data
				Type = Model.IsDefaultPlatformWarehouse ? "platform" : "vendor";

				Console.WriteLine($"Loaded warehouse - Type: {Type}, Email: {Model.Email}, Address: {Model.Address}");

				// Load vendors if it's a vendor warehouse (for dropdown display if needed)
				if (!Model.IsDefaultPlatformWarehouse && isMultiVendorEnabled)
				{
					await LoadVendors();
				}

				// Set selected vendor if it's a vendor warehouse
				if (Model.VendorId.HasValue)
				{
					selectedVendorId = Model.VendorId.Value;
					Console.WriteLine($"Selected vendor ID: {selectedVendorId}");
				}

				StateHasChanged();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Edit exception: {ex.Message}");
				await ShowErrorNotification(ValidationResources.Error, ex.Message ?? NotifiAndAlertsResources.SomethingWentWrong);
			}
		}
		// Handle parent category selection change
		private async Task OnParentCategoryChanged(ChangeEventArgs e)
		{
			if (Guid.TryParse(e.Value?.ToString(), out var VendorId))
			{
				Model.VendorId = VendorId;
			}
			else
			{
				Model.VendorId = Guid.Empty;
			}
			StateHasChanged();
		}
		protected async Task CloseModal()
		{
			Navigation.NavigateTo("/warehouses", true);
		}

		private async Task ShowErrorNotification(string title, string message)
		{
			await JSRuntime.InvokeVoidAsync("swal", title, message, "error");
		}

		private async Task ShowSuccessNotification(string message)
		{
			await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Done, message, "success");
		}
	}
}

//using Dashboard.Contracts.General;
//using Dashboard.Contracts.Warehouse;
//using Dashboard.Services.General;
//using Microsoft.AspNetCore.Components;
//using Microsoft.JSInterop;
//using Resources;
//using Shared.DTOs.Vendor;
//using Shared.DTOs.Warehouse;

//namespace Dashboard.Pages.Warehouse
//{
//	public partial class Details
//	{
//		private bool isSaving { get; set; }
//		private IEnumerable<CountryInfo>? countries;
//		private bool isMultiVendorEnabled = false;

//		protected WarehouseDto Model { get; set; } = new();

//		// Vendors list with new DTO
//		private IEnumerable<VendorWithUserDto> vendors = new List<VendorWithUserDto>();
//		private Guid? selectedVendorId = null;

//		[Parameter] public Guid Id { get; set; }
//		[Parameter] public string? Type { get; set; } // "platform" or "vendor"

//		[Inject] private IJSRuntime JSRuntime { get; set; } = default!;
//		[Inject] protected NavigationManager Navigation { get; set; } = null!;
//		[Inject] protected IWarehouseService WarehouseService { get; set; } = null!;
//		[Inject] protected ICountryPhoneCodeService CountryPhoneCodeService { get; set; } = null!;

//		// Properties for UI
//		protected string PageTitle => Id == Guid.Empty ? "Add" : "Edit";
//		protected bool IsEditMode => Id != Guid.Empty;
//		protected bool IsVendorWarehouse => Type?.ToLower() == "vendor";
//		protected bool IsPlatformWarehouse => Type?.ToLower() == "platform" || string.IsNullOrEmpty(Type);

//		protected override async Task OnInitializedAsync()
//		{
//			await LoadCountriesAsync();
//			await CheckMultiVendorStatus();

//			// Load vendors for vendor warehouse in add mode
//			if (isMultiVendorEnabled && IsVendorWarehouse)
//			{
//				await LoadVendors();
//			}

//			// Initialize new model
//			Model = new WarehouseDto
//			{
//				IsActive = true,
//				IsDefaultPlatformWarehouse = IsPlatformWarehouse
//			};

//			Console.WriteLine($"OnInitializedAsync - Type: {Type}, IsPlatformWarehouse: {IsPlatformWarehouse}, IsVendorWarehouse: {IsVendorWarehouse}");
//		}

//		protected override async Task OnParametersSetAsync()
//		{
//			// If editing existing warehouse
//			if (Id != Guid.Empty)
//			{
//				await Edit(Id);
//			}
//			// If adding new warehouse, set the type
//			else
//			{
//				Model.IsDefaultPlatformWarehouse = IsPlatformWarehouse;
//			}
//		}

//		private async Task CheckMultiVendorStatus()
//		{
//			try
//			{
//				var result = await WarehouseService.IsMultiVendorEnabledAsync();
//				if (result.Success)
//				{
//					isMultiVendorEnabled = result.Data;
//					Console.WriteLine($"Multi-vendor enabled: {isMultiVendorEnabled}");
//				}
//			}
//			catch (Exception ex)
//			{
//				Console.WriteLine($"Error checking multi-vendor status: {ex.Message}");
//			}
//		}

//		private async Task LoadVendors()
//		{
//			try
//			{
//				Console.WriteLine("Loading vendors...");
//				var result = await WarehouseService.GetActiveVendorsAsync();

//				if (result.Success && result.Data != null)
//				{
//					vendors = result.Data;
//					Console.WriteLine($"Loaded {vendors.Count()} vendors");

//					// Log vendor details for debugging
//					foreach (var vendor in vendors)
//					{
//						Console.WriteLine($"Vendor: {vendor.UserName} - {vendor.Email} - ID: {vendor.VendorId}");
//					}

//					StateHasChanged();
//				}
//				else
//				{
//					Console.WriteLine($"Failed to load vendors: {result.Message}");
//					vendors = new List<VendorWithUserDto>();
//				}
//			}
//			catch (Exception ex)
//			{
//				Console.WriteLine($"Exception loading vendors: {ex.Message}");
//				await ShowErrorNotification("Error", "Failed to load vendors");
//				vendors = new List<VendorWithUserDto>();
//			}
//		}

//		/// <summary>
//		/// Method called when vendor selection changes
//		/// </summary>
//		private async Task OnVendorChanged()
//		{
//			Console.WriteLine($"OnVendorChanged called - selectedVendorId: {selectedVendorId}");

//			if (selectedVendorId.HasValue && selectedVendorId.Value != Guid.Empty)
//			{
//				// Find selected vendor
//				var selectedVendor = vendors?.FirstOrDefault(v => v.VendorId == selectedVendorId.Value);

//				if (selectedVendor != null)
//				{
//					Console.WriteLine($"Selected vendor: {selectedVendor.UserName} - {selectedVendor.Email}");

//					// Update Model with vendor information
//					Model.VendorId = selectedVendor.VendorId;
//					Model.Email = selectedVendor.Email;

//					StateHasChanged();
//				}
//				else
//				{
//					Console.WriteLine("Selected vendor not found in list");
//				}
//			}
//			else
//			{
//				Console.WriteLine("Clearing vendor information");
//				// Clear vendor information
//				Model.VendorId = null;
//				Model.Email = string.Empty;
//			}
//		}

//		private async Task LoadCountriesAsync()
//		{
//			try
//			{
//				if (CountryPhoneCodeService != null)
//				{
//					countries = CountryPhoneCodeService.GetAllCountries(
//						ResourceManager.CurrentLanguage == Resources.Enumerations.Language.Arabic ? "ar" : "en");
//				}
//			}
//			catch (Exception)
//			{
//				countries = CountryPhoneCodeService?.GetFallbackCountries();
//			}
//		}

//		protected async Task Save()
//		{
//			try
//			{
//				// Validation for Vendor Warehouses
//				if (IsVendorWarehouse)
//				{
//					// In Add mode, check vendor selection
//					if (!IsEditMode && !Model.VendorId.HasValue)
//					{
//						await ShowErrorNotification(ValidationResources.Error, "Please select a vendor");
//						return;
//					}

//					// Check email
//					if (string.IsNullOrWhiteSpace(Model.Email))
//					{
//						await ShowErrorNotification(ValidationResources.Error, "Vendor email is required");
//						return;
//					}
//				}

//				isSaving = true;
//				StateHasChanged();

//				// Set warehouse type
//				Model.IsDefaultPlatformWarehouse = IsPlatformWarehouse;

//				// Clear VendorId if it's a platform warehouse
//				if (IsPlatformWarehouse)
//				{
//					Model.VendorId = null;
//					Model.Email = null;
//				}

//				Console.WriteLine($"Saving warehouse - Type: {(IsPlatformWarehouse ? "Platform" : "Vendor")}, VendorId: {Model.VendorId}, Email: {Model.Email}");

//				var result = await WarehouseService.SaveAsync(Model);
//				isSaving = false;

//				if (result.Success)
//				{
//					await ShowSuccessNotification(NotifiAndAlertsResources.SavedSuccessfully);
//					await CloseModal();
//				}
//				else
//				{
//					await ShowErrorNotification(ValidationResources.Failed, result.Message ?? NotifiAndAlertsResources.SaveFailed);
//				}
//			}
//			catch (Exception ex)
//			{
//				isSaving = false;
//				Console.WriteLine($"Save exception: {ex.Message}");
//				await ShowErrorNotification(NotifiAndAlertsResources.FailedAlert, ex.Message ?? NotifiAndAlertsResources.SomethingWentWrong);
//			}
//		}

//		protected async Task Edit(Guid id)
//		{
//			try
//			{
//				Console.WriteLine($"Editing warehouse with ID: {id}");

//				var result = await WarehouseService.GetByIdAsync(id);
//				if (!result.Success)
//				{
//					await ShowErrorNotification(ValidationResources.Failed, NotifiAndAlertsResources.FailedToRetrieveData);
//					return;
//				}

//				Model = result.Data ?? new();

//				// Update Type based on loaded data
//				Type = Model.IsDefaultPlatformWarehouse ? "platform" : "vendor";

//				Console.WriteLine($"Loaded warehouse - Type: {Type}, Email: {Model.Email}, Address: {Model.Address}");

//				// Load vendors if it's a vendor warehouse (for dropdown display if needed)
//				if (!Model.IsDefaultPlatformWarehouse && isMultiVendorEnabled)
//				{
//					await LoadVendors();
//				}

//				// Set selected vendor if it's a vendor warehouse
//				if (Model.VendorId.HasValue)
//				{
//					selectedVendorId = Model.VendorId.Value;
//					Console.WriteLine($"Selected vendor ID: {selectedVendorId}");
//				}

//				StateHasChanged();
//			}
//			catch (Exception ex)
//			{
//				Console.WriteLine($"Edit exception: {ex.Message}");
//				await ShowErrorNotification(ValidationResources.Error, ex.Message ?? NotifiAndAlertsResources.SomethingWentWrong);
//			}
//		}

//		protected async Task CloseModal()
//		{
//			Navigation.NavigateTo("/warehouses", true);
//		}

//		private async Task ShowErrorNotification(string title, string message)
//		{
//			await JSRuntime.InvokeVoidAsync("swal", title, message, "error");
//		}

//		private async Task ShowSuccessNotification(string message)
//		{
//			await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Done, message, "success");
//		}
//	}
//}

////using Dashboard.Contracts.General;
////using Dashboard.Contracts.Warehouse;
////using Dashboard.Services.General;
////using Microsoft.AspNetCore.Components;
////using Microsoft.JSInterop;
////using Resources;
////using Shared.DTOs.Vendor;
////using Shared.DTOs.Warehouse;

////namespace Dashboard.Pages.Warehouse
////{
////	public partial class Details
////	{
////		private bool isSaving { get; set; }
////		private IEnumerable<CountryInfo>? countries;
////		private bool isMultiVendorEnabled = false;

////		protected WarehouseDto Model { get; set; } = new();

////		// Vendors list with new DTO
////		private IEnumerable<VendorWithUserDto> vendors = new List<VendorWithUserDto>();
////		private Guid? selectedVendorId = null;

////		[Parameter] public Guid Id { get; set; }
////		[Parameter] public string? Type { get; set; } // "platform" or "vendor"

////		[Inject] private IJSRuntime JSRuntime { get; set; } = default!;
////		[Inject] protected NavigationManager Navigation { get; set; } = null!;
////		[Inject] protected IWarehouseService WarehouseService { get; set; } = null!;
////		[Inject] protected ICountryPhoneCodeService CountryPhoneCodeService { get; set; } = null!;

////		// Properties for UI
////		protected string PageTitle => Id == Guid.Empty ? "Add" : "Edit";
////		protected bool IsEditMode => Id != Guid.Empty;
////		protected bool IsVendorWarehouse => Type?.ToLower() == "vendor";
////		protected bool IsPlatformWarehouse => Type?.ToLower() == "platform" || string.IsNullOrEmpty(Type);

////		protected override async Task OnInitializedAsync()
////		{
////			await LoadCountriesAsync();
////			await CheckMultiVendorStatus();

////			if (isMultiVendorEnabled && IsVendorWarehouse)
////			{
////				await LoadVendors();
////			}

////			// Initialize new model
////			Model = new WarehouseDto
////			{
////				IsActive = true,
////				IsDefaultPlatformWarehouse = IsPlatformWarehouse
////			};
////		}

////		protected override async Task OnParametersSetAsync()
////		{
////			// If editing existing warehouse
////			if (Id != Guid.Empty)
////			{
////				await Edit(Id);
////			}
////			// If adding new warehouse, set the type
////			else
////			{
////				Model.IsDefaultPlatformWarehouse = IsPlatformWarehouse;
////			}
////		}

////		private async Task CheckMultiVendorStatus()
////		{
////			try
////			{
////				var result = await WarehouseService.IsMultiVendorEnabledAsync();
////				if (result.Success)
////				{
////					isMultiVendorEnabled = result.Data;
////				}
////			}
////			catch (Exception ex)
////			{
////				Console.WriteLine($"Error checking multi-vendor status: {ex.Message}");
////			}
////		}

////		private async Task LoadVendors()
////		{
////			try
////			{
////				var result = await WarehouseService.GetActiveVendorsAsync();
////				if (result.Success && result.Data != null)
////				{
////					vendors = result.Data;
////					StateHasChanged();
////				}
////			}
////			catch (Exception ex)
////			{
////				await ShowErrorNotification("Error", "Failed to load vendors");
////			}
////		}

////		/// <summary>
////		/// Method called when vendor selection changes
////		/// </summary>
////		private async Task OnVendorChanged()
////		{
////			if (selectedVendorId.HasValue && selectedVendorId.Value != Guid.Empty)
////			{
////				// Find selected vendor
////				var selectedVendor = vendors?.FirstOrDefault(v => v.VendorId == selectedVendorId.Value);

////				if (selectedVendor != null)
////				{
////					// Update Model with vendor information
////					Model.VendorId = selectedVendor.VendorId;
////					Model.Email = selectedVendor.Email;


////					StateHasChanged();
////				}
////			}
////			else
////			{
////				// Clear vendor information
////				Model.VendorId = null;
////				Model.Email = string.Empty;

////			}
////		}

////		private async Task LoadCountriesAsync()
////		{
////			try
////			{
////				if (CountryPhoneCodeService != null)
////				{
////					countries = CountryPhoneCodeService.GetAllCountries(
////						ResourceManager.CurrentLanguage == Resources.Enumerations.Language.Arabic ? "ar" : "en");
////				}
////			}
////			catch (Exception)
////			{
////				countries = CountryPhoneCodeService?.GetFallbackCountries();
////			}
////		}

////		protected async Task Save()
////		{
////			try
////			{
////				// Validation
////				if (IsVendorWarehouse && !Model.VendorId.HasValue)
////				{
////					await ShowErrorNotification(ValidationResources.Error, "Please select a vendor");
////					return;
////				}

////				if (IsVendorWarehouse && string.IsNullOrWhiteSpace(Model.Email))
////				{
////					await ShowErrorNotification(ValidationResources.Error, "Vendor email is required");
////					return;
////				}

////				isSaving = true;
////				StateHasChanged();

////				// Set warehouse type
////				Model.IsDefaultPlatformWarehouse = IsPlatformWarehouse;

////				// Clear VendorId if it's a platform warehouse
////				if (IsPlatformWarehouse)
////				{
////					Model.VendorId = null;
////				}

////				var result = await WarehouseService.SaveAsync(Model);
////				isSaving = false;

////				if (result.Success)
////				{
////					await ShowSuccessNotification(NotifiAndAlertsResources.SavedSuccessfully);
////					await CloseModal();
////				}
////				else
////				{
////					await ShowErrorNotification(ValidationResources.Failed, result.Message ?? NotifiAndAlertsResources.SaveFailed);
////				}
////			}
////			catch (Exception ex)
////			{
////				isSaving = false;
////				await ShowErrorNotification(NotifiAndAlertsResources.FailedAlert, ex.Message ?? NotifiAndAlertsResources.SomethingWentWrong);
////			}
////		}

////		protected async Task Edit(Guid id)
////		{
////			try
////			{
////				var result = await WarehouseService.GetByIdAsync(id);
////				if (!result.Success)
////				{
////					await ShowErrorNotification(ValidationResources.Failed, NotifiAndAlertsResources.FailedToRetrieveData);
////					return;
////				}

////				Model = result.Data ?? new();

////				// Update Type based on loaded data
////				Type = Model.IsDefaultPlatformWarehouse ? "platform" : "vendor";

////				// Set selected vendor if it's a vendor warehouse
////				if (Model.VendorId.HasValue)
////				{
////					selectedVendorId = Model.VendorId.Value;
////				}

////				StateHasChanged();
////			}
////			catch (Exception ex)
////			{
////				await ShowErrorNotification(ValidationResources.Error, ex.Message ?? NotifiAndAlertsResources.SomethingWentWrong);
////			}
////		}

////		protected async Task CloseModal()
////		{
////			Navigation.NavigateTo("/warehouses", true);
////		}

////		private async Task ShowErrorNotification(string title, string message)
////		{
////			await JSRuntime.InvokeVoidAsync("swal", title, message, "error");
////		}

////		private async Task ShowSuccessNotification(string message)
////		{
////			await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Done, message, "success");
////		}
////	}
////}


//////using Dashboard.Contracts.General;
//////using Dashboard.Contracts.Warehouse;
//////using Dashboard.Services.General;
//////using Microsoft.AspNetCore.Components;
//////using Microsoft.JSInterop;
//////using Resources;
//////using Shared.DTOs.Vendor;
//////using Shared.DTOs.Warehouse;

//////namespace Dashboard.Pages.Warehouse
//////{
//////	public partial class Details
//////	{
//////		private bool isSaving { get; set; }
//////		private IEnumerable<CountryInfo>? countries;
//////		private bool isMultiVendorEnabled = false;

//////		protected WarehouseDto Model { get; set; } = new();
//////		// Vendors list
//////		private IEnumerable<VendorWithUserDto> vendors = new List<VendorWithUserDto>();
//////		private Guid? selectedVendorId = null;
//////		[Parameter] public Guid Id { get; set; }
//////		[Parameter] public string? Type { get; set; } // "platform" or "vendor"

//////		[Inject] private IJSRuntime JSRuntime { get; set; } = default!;
//////		[Inject] protected NavigationManager Navigation { get; set; } = null!;
//////		[Inject] protected IWarehouseService WarehouseService { get; set; } = null!;
//////		[Inject] protected ICountryPhoneCodeService CountryPhoneCodeService { get; set; } = null!;

//////		// Properties for UI
//////		protected string PageTitle => Id == Guid.Empty ? "Add" : "Edit";
//////		protected bool IsVendorWarehouse => Type?.ToLower() == "vendor";
//////		protected bool IsPlatformWarehouse => Type?.ToLower() == "platform" || string.IsNullOrEmpty(Type);

//////		protected override async Task OnInitializedAsync()
//////		{
//////			await LoadCountriesAsync();
//////			await CheckMultiVendorStatus();

//////			if (isMultiVendorEnabled)
//////			{
//////				await LoadVendors();
//////			}

//////			// Initialize new model
//////			Model = new WarehouseDto
//////			{
//////				IsActive = true,
//////				IsDefaultPlatformWarehouse = IsPlatformWarehouse
//////			};
//////		}

//////		protected override async Task OnParametersSetAsync()
//////		{
//////			// If editing existing warehouse
//////			if (Id != Guid.Empty)
//////			{
//////				await Edit(Id);
//////			}
//////			// If adding new warehouse, set the type
//////			else
//////			{
//////				Model.IsDefaultPlatformWarehouse = IsPlatformWarehouse;
//////			}
//////		}

//////		private async Task CheckMultiVendorStatus()
//////		{
//////			try
//////			{
//////				var result = await WarehouseService.IsMultiVendorEnabledAsync();
//////				if (result.Success)
//////				{
//////					isMultiVendorEnabled = result.Data;
//////				}
//////			}
//////			catch (Exception ex)
//////			{
//////				Console.WriteLine($"Error checking multi-vendor status: {ex.Message}");
//////			}
//////		}

//////		private async Task LoadVendors()
//////		{
//////			try
//////			{
//////				var result = await WarehouseService.GetActiveVendorsAsync();
//////				if (result.Success && result.Data != null)
//////				{
//////					vendors = result.Data;
//////					StateHasChanged();
//////				}
//////			}
//////			catch (Exception ex)
//////			{
//////				await ShowErrorNotification("Error", "Failed to load vendors");
//////			}
//////		}

//////		private async Task LoadCountriesAsync()
//////		{
//////			try
//////			{
//////				if (CountryPhoneCodeService != null)
//////				{
//////					countries = CountryPhoneCodeService.GetAllCountries(
//////						ResourceManager.CurrentLanguage == Resources.Enumerations.Language.Arabic ? "ar" : "en");
//////				}
//////			}
//////			catch (Exception)
//////			{
//////				countries = CountryPhoneCodeService?.GetFallbackCountries();
//////			}
//////		}

//////		protected async Task Save()
//////		{
//////			try
//////			{
//////				// Validation
//////				if (IsVendorWarehouse && !Model.VendorId.HasValue)
//////				{
//////					await ShowErrorNotification(ValidationResources.Error, "Please select a vendor");
//////					return;
//////				}

//////				isSaving = true;
//////				StateHasChanged();

//////				// Set warehouse type
//////				Model.IsDefaultPlatformWarehouse = IsPlatformWarehouse;

//////				// Clear VendorId if it's a platform warehouse
//////				if (IsPlatformWarehouse)
//////				{
//////					Model.VendorId = null;
//////				}

//////				var result = await WarehouseService.SaveAsync(Model);
//////				isSaving = false;

//////				if (result.Success)
//////				{
//////					await ShowSuccessNotification(NotifiAndAlertsResources.SavedSuccessfully);
//////					await CloseModal();
//////				}
//////				else
//////				{
//////					await ShowErrorNotification(ValidationResources.Failed, result.Message ?? NotifiAndAlertsResources.SaveFailed);
//////				}
//////			}
//////			catch (Exception ex)
//////			{
//////				isSaving = false;
//////				await ShowErrorNotification(NotifiAndAlertsResources.FailedAlert, ex.Message ?? NotifiAndAlertsResources.SomethingWentWrong);
//////			}
//////		}

//////		protected async Task Edit(Guid id)
//////		{
//////			try
//////			{
//////				var result = await WarehouseService.GetByIdAsync(id);
//////				if (!result.Success)
//////				{
//////					await ShowErrorNotification(ValidationResources.Failed, NotifiAndAlertsResources.FailedToRetrieveData);
//////					return;
//////				}

//////				Model = result.Data ?? new();

//////				// Update Type based on loaded data
//////				Type = Model.IsDefaultPlatformWarehouse ? "platform" : "vendor";

//////				StateHasChanged();
//////			}
//////			catch (Exception ex)
//////			{
//////				await ShowErrorNotification(ValidationResources.Error, ex.Message ?? NotifiAndAlertsResources.SomethingWentWrong);
//////			}
//////		}

//////		protected async Task CloseModal()
//////		{
//////			Navigation.NavigateTo("/warehouses", true);
//////		}

//////		private async Task ShowErrorNotification(string title, string message)
//////		{
//////			await JSRuntime.InvokeVoidAsync("swal", title, message, "error");
//////		}

//////		private async Task ShowSuccessNotification(string message)
//////		{
//////			await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Done, message, "success");
//////		}
//////	}
//////}


//////using Dashboard.Contracts.General;
//////using Dashboard.Contracts.Warehouse;
//////using Dashboard.Services.General;
//////using Microsoft.AspNetCore.Components;
//////using Microsoft.JSInterop;
//////using Resources;
//////using Shared.DTOs.Warehouse;

//////namespace Dashboard.Pages.Warehouse
//////{
//////    public partial class Details
//////    {
//////        private bool isSaving { get; set; }
//////        private IEnumerable<CountryInfo>? countries;

//////        protected WarehouseDto Model { get; set; } = new();

//////        [Parameter] public Guid Id { get; set; }

//////        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
//////        [Inject] protected NavigationManager Navigation { get; set; } = null!;
//////        [Inject] protected IWarehouseService WarehouseService { get; set; } = null!;
//////        [Inject] protected ICountryPhoneCodeService CountryPhoneCodeService { get; set; } = null!;

//////        protected override async Task OnInitializedAsync()
//////        {
//////            await LoadCountriesAsync();
//////            Model = new WarehouseDto { IsActive = true };
//////        }

//////        protected override void OnParametersSet()
//////        {
//////            if (Id != Guid.Empty)
//////            {
//////                Edit(Id);
//////            }
//////        }

//////        private async Task LoadCountriesAsync()
//////        {
//////            try
//////            {
//////                if (CountryPhoneCodeService != null)
//////                {
//////                    countries = CountryPhoneCodeService.GetAllCountries(
//////                        ResourceManager.CurrentLanguage == Resources.Enumerations.Language.Arabic ? "ar" : "en");

//////                    //if (string.IsNullOrEmpty(Model.PhoneCode))
//////                    //{
//////                    //    Model.PhoneCode = "+20"; // Default to Egypt
//////                    //}
//////                }
//////            }
//////            catch (Exception)
//////            {
//////                countries = CountryPhoneCodeService?.GetFallbackCountries();
//////            }
//////        }

//////        protected async Task Save()
//////        {
//////            try
//////            {
//////                isSaving = true;
//////                StateHasChanged();

//////                var result = await WarehouseService.SaveAsync(Model);

//////                isSaving = false;
//////                if (result.Success)
//////                {
//////                    await ShowSuccessNotification(NotifiAndAlertsResources.SavedSuccessfully);
//////                    await CloseModal();
//////                }
//////                else
//////                {
//////                    await ShowErrorNotification(ValidationResources.Failed, NotifiAndAlertsResources.SaveFailed);
//////                }
//////            }
//////            catch (Exception)
//////            {
//////                await ShowErrorNotification(NotifiAndAlertsResources.FailedAlert, NotifiAndAlertsResources.SomethingWentWrong);
//////            }
//////        }

//////        protected async Task Edit(Guid id)
//////        {
//////            try
//////            {
//////                var result = await WarehouseService.GetByIdAsync(id);

//////                if (!result.Success)
//////                {
//////                    await ShowErrorNotification(ValidationResources.Failed, NotifiAndAlertsResources.FailedToRetrieveData);
//////                    return;
//////                }

//////                Model = result.Data ?? new();
//////                StateHasChanged();
//////            }
//////            catch (Exception)
//////            {
//////                await ShowErrorNotification(ValidationResources.Error, NotifiAndAlertsResources.SomethingWentWrong);
//////            }
//////        }

//////        protected async Task CloseModal()
//////        {
//////            Navigation.NavigateTo("/warehouses", true);
//////        }

//////        private async Task ShowErrorNotification(string title, string message)
//////        {
//////            await JSRuntime.InvokeVoidAsync("swal", title, message, "error");
//////        }

//////        private async Task ShowSuccessNotification(string message)
//////        {
//////            await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Done, message, "success");
//////        }
//////    }
//////}
