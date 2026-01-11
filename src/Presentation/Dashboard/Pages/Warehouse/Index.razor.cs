
using Dashboard.Contracts.Warehouse;
using Microsoft.AspNetCore.Components;
using Resources;
using Shared.DTOs.Vendor;
using Shared.DTOs.Warehouse;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Dashboard.Pages.Warehouse
{
	public partial class Index : BaseListPage<WarehouseDto>
	{
		[Inject] private IWarehouseService WarehouseService { get; set; } = null!;

		// Tab management
		private string activeTab = "platform";
		private bool isMultiVendorEnabled = false;
		private bool isLoading = false;

		// Platform Tab - Single Warehouse (Details View)
		private WarehouseDto? platformWarehouse = null;

		// Platform Tab Search Model & Data (kept for compatibility)
		private WarehouseSearchCriteriaModel platformSearchModel = new()
		{
			IsDefaultPlatformWarehouse = true,
			PageSize = 10
		};
		private IEnumerable<WarehouseDto> platformItems = new List<WarehouseDto>();
		private int platformTotalRecords = 0;
		private int platformCurrentPage = 1;
		private int platformTotalPages = 0;

		// Vendor Tab Search Model & Data
		private WarehouseSearchCriteriaModel vendorSearchModel = new()
		{
			IsDefaultPlatformWarehouse = false,
			PageSize = 10
		};
		private IEnumerable<WarehouseDto> vendorItems = new List<WarehouseDto>();
		private int vendorTotalRecords = 0;
		private int vendorCurrentPage = 1;
		private int vendorTotalPages = 0;

		// Vendors list
		private IEnumerable<VendorDto> vendors = new List<VendorDto>();
		private Guid? selectedVendorId = null;

		// Override base properties to use active tab data
		protected new IEnumerable<WarehouseDto> items => activeTab == "platform" ? platformItems : vendorItems;
		protected new int totalRecords => activeTab == "platform" ? platformTotalRecords : vendorTotalRecords;
		protected new int currentPage => activeTab == "platform" ? platformCurrentPage : vendorCurrentPage;
		protected new int totalPages => activeTab == "platform" ? platformTotalPages : vendorTotalPages;
		protected new WarehouseSearchCriteriaModel searchModel => activeTab == "platform" ? platformSearchModel : vendorSearchModel;

		protected override string EntityName => "Warehouse Management";
		protected override string AddRoute => activeTab == "platform" ? "/warehouse/platform/add" : "/warehouse/vendor/add";
		protected override string EditRouteTemplate => "/warehouse/{id}";
		protected override string SearchEndpoint => "api/v1/Warehouse/search";

		protected override Dictionary<string, Func<WarehouseDto, object>> ExportColumns => new()
		{
			{ "Type", item => item.IsDefaultPlatformWarehouse ? "Platform" : "Vendor" },
			{ "Address", item => item.Address ?? "-" },
			{ "Email", item => item.Email ?? "-" },
			{ "Status", item => item.IsActive ? "Active" : "Inactive" }
		};

		protected override async Task OnCustomInitializeAsync()
		{
			// Check if multi-vendor is enabled
			await CheckMultiVendorStatus();

			// Load vendors if multi-vendor is enabled
			if (isMultiVendorEnabled)
			{
				await LoadVendors();
			}

			// Load Platform warehouse (single warehouse)
			await LoadPlatformWarehouse();

			// Load Vendor warehouses if multi-vendor enabled
			if (isMultiVendorEnabled)
			{
				await SearchVendorWarehouses();
			}

			StateHasChanged();
		}

		private async Task CheckMultiVendorStatus()
		{
			try
			{
				var result = await WarehouseService.IsMultiVendorEnabledAsync();
				if (result.Success)
				{
					isMultiVendorEnabled = result.Data;
					StateHasChanged();
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
				var result = await WarehouseService.GetVendorsAsync();
				if (result.Success && result.Data != null)
				{
					vendors = result.Data;
					StateHasChanged();
				}
			}
			catch (Exception ex)
			{
				await ShowErrorNotification("Error", "Failed to load vendors");
			}
		}

		// Load Platform Warehouse (Single Warehouse)
		private async Task LoadPlatformWarehouse()
		{
			try
			{
				isLoading = true;
				StateHasChanged();

				Console.WriteLine($"LoadPlatformWarehouse - IsDefaultPlatformWarehouse: {platformSearchModel.IsDefaultPlatformWarehouse}");

				var result = await WarehouseService.SearchAsync(platformSearchModel);

				if (result.Success && result.Data != null && result.Data.Items != null && result.Data.Items.Any())
				{
					// Get the first (and should be only) platform warehouse
					platformWarehouse = result.Data.Items.FirstOrDefault();
				}
				else
				{
					platformWarehouse = null;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error in LoadPlatformWarehouse: {ex.Message}");
				await ShowErrorNotification("Error", "Failed to load platform warehouse");
				platformWarehouse = null;
			}
			finally
			{
				isLoading = false;
				StateHasChanged();
			}
		}

		// Platform Warehouses Search (kept for compatibility)
		private async Task SearchPlatformWarehouses()
		{
			try
			{
				isLoading = true;
				StateHasChanged();

				Console.WriteLine($"SearchPlatformWarehouses - IsDefaultPlatformWarehouse: {platformSearchModel.IsDefaultPlatformWarehouse}");

				var result = await WarehouseService.SearchAsync(platformSearchModel);

				if (result.Success && result.Data != null)
				{
					platformItems = result.Data.Items ?? new List<WarehouseDto>();
					platformTotalRecords = result.Data.TotalRecords;
					platformTotalPages = (int)Math.Ceiling((double)platformTotalRecords / platformSearchModel.PageSize);

					// Update the single warehouse reference
					platformWarehouse = platformItems.FirstOrDefault();
				}
				else
				{
					platformItems = new List<WarehouseDto>();
					platformTotalRecords = 0;
					platformTotalPages = 0;
					platformWarehouse = null;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error in SearchPlatformWarehouses: {ex.Message}");
				await ShowErrorNotification("Error", "Failed to load platform warehouses");
				platformItems = new List<WarehouseDto>();
				platformWarehouse = null;
			}
			finally
			{
				isLoading = false;
				StateHasChanged();
			}
		}

		// Vendor Warehouses Search
		private async Task SearchVendorWarehouses()
		{
			try
			{
				isLoading = true;
				StateHasChanged();

				Console.WriteLine($"SearchVendorWarehouses - IsDefaultPlatformWarehouse: {vendorSearchModel.IsDefaultPlatformWarehouse}");

				var result = await WarehouseService.SearchVendorAsync(vendorSearchModel);

				if (result.Success && result.Data != null)
				{
					vendorItems = result.Data.Items ?? new List<WarehouseDto>();
					vendorTotalRecords = result.Data.TotalRecords;
					vendorTotalPages = (int)Math.Ceiling((double)vendorTotalRecords / vendorSearchModel.PageSize);
				}
				else
				{
					vendorItems = new List<WarehouseDto>();
					vendorTotalRecords = 0;
					vendorTotalPages = 0;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error in SearchVendorWarehouses: {ex.Message}");
				await ShowErrorNotification("Error", "Failed to load vendor warehouses");
				vendorItems = new List<WarehouseDto>();
			}
			finally
			{
				isLoading = false;
				StateHasChanged();
			}
		}

		// Override base Search method
		protected override async Task Search()
		{
			if (activeTab == "platform")
			{
				platformCurrentPage = 1;
				platformSearchModel.PageNumber = 1;
				await SearchPlatformWarehouses();
			}
			else
			{
				vendorCurrentPage = 1;
				vendorSearchModel.PageNumber = 1;
				await SearchVendorWarehouses();
			}
		}

		protected async Task SwitchToPlatform()
		{
			activeTab = "platform";
			StateHasChanged();
		}

		protected async Task SwitchToVendor()
		{
			activeTab = "vendor";
			StateHasChanged();
		}

		protected async Task OnVendorFilterChanged(ChangeEventArgs e)
		{
			if (Guid.TryParse(e.Value?.ToString(), out Guid vendorId))
			{
				selectedVendorId = vendorId;
				vendorSearchModel.VendorId = vendorId;
			}
			else
			{
				selectedVendorId = null;
				vendorSearchModel.VendorId = null;
			}

			vendorCurrentPage = 1;
			vendorSearchModel.PageNumber = 1;
			await SearchVendorWarehouses();
		}

		protected async Task OnFilterChanged(ChangeEventArgs e)
		{
			var filterValue = e.Value?.ToString();

			if (activeTab == "platform")
			{
				platformSearchModel.IsActive = filterValue switch
				{
					"active" => true,
					"inactive" => false,
					_ => null
				};
				platformCurrentPage = 1;
				platformSearchModel.PageNumber = 1;
				await SearchPlatformWarehouses();
			}
			else
			{
				vendorSearchModel.IsActive = filterValue switch
				{
					"active" => true,
					"inactive" => false,
					_ => null
				};
				vendorCurrentPage = 1;
				vendorSearchModel.PageNumber = 1;
				await SearchVendorWarehouses();
			}
		}

		protected override async Task OnPageSizeChanged(ChangeEventArgs e)
		{
			if (int.TryParse(e.Value?.ToString(), out int pageSize))
			{
				if (activeTab == "platform")
				{
					platformSearchModel.PageSize = pageSize;
					platformCurrentPage = 1;
					platformSearchModel.PageNumber = 1;
					await SearchPlatformWarehouses();
				}
				else
				{
					vendorSearchModel.PageSize = pageSize;
					vendorCurrentPage = 1;
					vendorSearchModel.PageNumber = 1;
					await SearchVendorWarehouses();
				}
			}
		}

		protected override async Task GoToPage(int page)
		{
			if (activeTab == "platform")
			{
				if (page < 1 || page > platformTotalPages) return;
				platformCurrentPage = page;
				platformSearchModel.PageNumber = page;
				await SearchPlatformWarehouses();
			}
			else
			{
				if (page < 1 || page > vendorTotalPages) return;
				vendorCurrentPage = page;
				vendorSearchModel.PageNumber = page;
				await SearchVendorWarehouses();
			}
		}

		protected override async Task<ResponseModel<IEnumerable<WarehouseDto>>> GetAllItemsAsync()
		{
			return await WarehouseService.GetAllAsync();
		}

		protected override async Task<ResponseModel<bool>> DeleteItemAsync(Guid id)
		{
			var result = await WarehouseService.DeleteAsync(id);
			return new ResponseModel<bool>
			{
				Success = result.Success,
				Message = result.Message,
				Data = result.Success,
				Errors = result.Errors
			};
		}

		protected override async Task<string> GetItemId(WarehouseDto item)
		{
			return await Task.FromResult(item.Id.ToString());
		}

		protected async Task ToggleStatus(WarehouseDto warehouse)
		{
			try
			{
				var result = await WarehouseService.ToggleStatusAsync(warehouse.Id);
				if (result.Success)
				{
					await ShowSuccessNotification("Status updated successfully");

					// Refresh the correct tab
					if (activeTab == "platform")
					{
						await LoadPlatformWarehouse();
					}
					else
					{
						await SearchVendorWarehouses();
					}
				}
				else
				{
					await ShowErrorNotification(ValidationResources.Error, result.Message ?? "Failed to update status");
				}
			}
			catch (Exception)
			{
				await ShowErrorNotification(ValidationResources.Error, "Error updating warehouse status");
			}
		}

		protected void AddWarehouse()
		{
			var route = activeTab == "platform" ? "/warehouse/platform/add" : "/warehouse/vendor/add";
			Navigation.NavigateTo(route);
		}

		protected void CreatePlatformWarehouse()
		{
			Navigation.NavigateTo("/warehouse/platform/add");
		}

		protected override async Task Edit(WarehouseDto item)
		{
			var id = await GetItemId(item);
			var type = item.IsDefaultPlatformWarehouse ? "platform" : "vendor";
			var editRoute = $"/warehouse/{type}/{id}";
			Navigation.NavigateTo(editRoute);
		}

		// Computed properties for pagination
		protected int StartRecord => totalRecords == 0 ? 0 : ((currentPage - 1) * searchModel.PageSize) + 1;
		protected int EndRecord => Math.Min(currentPage * searchModel.PageSize, totalRecords);
		protected bool CanGoToPreviousPage => currentPage > 1;
		protected bool CanGoToNextPage => currentPage < totalPages;
	}
}

//using Dashboard.Contracts.Warehouse;
//using Microsoft.AspNetCore.Components;
//using Resources;
//using Shared.DTOs.Vendor;
//using Shared.DTOs.Warehouse;
//using Shared.GeneralModels;
//using Shared.GeneralModels.SearchCriteriaModels;

//namespace Dashboard.Pages.Warehouse
//{
//	public partial class Index : BaseListPage<WarehouseDto>
//	{
//		[Inject] private IWarehouseService WarehouseService { get; set; } = null!;

//		// Tab management
//		private string activeTab = "platform";
//		private bool isMultiVendorEnabled = false;

//		// Platform Tab Search Model & Data
//		private WarehouseSearchCriteriaModel platformSearchModel = new()
//		{
//			IsDefaultPlatformWarehouse = true,
//			PageSize = 10
//		};
//		private IEnumerable<WarehouseDto> platformItems = new List<WarehouseDto>();
//		private int platformTotalRecords = 0;
//		private int platformCurrentPage = 1;
//		private int platformTotalPages = 0;

//		// Vendor Tab Search Model & Data
//		private WarehouseSearchCriteriaModel vendorSearchModel = new()
//		{
//			IsDefaultPlatformWarehouse = false,
//			PageSize = 10
//		};
//		private IEnumerable<WarehouseDto> vendorItems = new List<WarehouseDto>();
//		private int vendorTotalRecords = 0;
//		private int vendorCurrentPage = 1;
//		private int vendorTotalPages = 0;

//		// Vendors list
//		private IEnumerable<VendorDto> vendors = new List<VendorDto>();
//		private Guid? selectedVendorId = null;

//		// Override base properties to use active tab data
//		protected new IEnumerable<WarehouseDto> items => activeTab == "platform" ? platformItems : vendorItems;
//		protected new int totalRecords => activeTab == "platform" ? platformTotalRecords : vendorTotalRecords;
//		protected new int currentPage => activeTab == "platform" ? platformCurrentPage : vendorCurrentPage;
//		protected new int totalPages => activeTab == "platform" ? platformTotalPages : vendorTotalPages;
//		protected new WarehouseSearchCriteriaModel searchModel => activeTab == "platform" ? platformSearchModel : vendorSearchModel;

//		protected override string EntityName => "Warehouse Management";
//		protected override string AddRoute => activeTab == "platform" ? "/warehouse/platform/add" : "/warehouse/vendor/add";
//		protected override string EditRouteTemplate => "/warehouse/{id}";
//		protected override string SearchEndpoint => "api/v1/Warehouse/search";

//		protected override Dictionary<string, Func<WarehouseDto, object>> ExportColumns => new()
//		{
//			{ "Type", item => item.IsDefaultPlatformWarehouse ? "Platform" : "Vendor" },
//			{ "Address", item => item.Address ?? "-" },
//			{ "Email", item => item.Email ?? "-" },
//			{ "Status", item => item.IsActive ? "Active" : "Inactive" }
//		};

//		protected override async Task OnCustomInitializeAsync()
//		{
//			// Check if multi-vendor is enabled
//			await CheckMultiVendorStatus();

//			// Load vendors if multi-vendor is enabled
//			if (isMultiVendorEnabled)
//			{
//				await LoadVendors();
//			}

//			// Load Platform warehouses
//			await SearchPlatformWarehouses();

//			// Load Vendor warehouses if multi-vendor enabled
//			if (isMultiVendorEnabled)
//			{
//				await SearchVendorWarehouses();
//			}

//			StateHasChanged();
//		}

//		private async Task CheckMultiVendorStatus()
//		{
//			try
//			{
//				var result = await WarehouseService.IsMultiVendorEnabledAsync();
//				if (result.Success)
//				{
//					isMultiVendorEnabled = result.Data;
//					StateHasChanged();
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
//				var result = await WarehouseService.GetVendorsAsync();
//				if (result.Success && result.Data != null)
//				{
//					vendors = result.Data;
//					StateHasChanged();
//				}
//			}
//			catch (Exception ex)
//			{
//				await ShowErrorNotification("Error", "Failed to load vendors");
//			}
//		}

//		// Platform Warehouses Search
//		private async Task SearchPlatformWarehouses()
//		{
//			try
//			{
//				//isLoading = true;
//				StateHasChanged();

//				Console.WriteLine($"SearchPlatformWarehouses - IsDefaultPlatformWarehouse: {platformSearchModel.IsDefaultPlatformWarehouse}");

//				var result = await WarehouseService.SearchAsync(platformSearchModel);

//				if (result.Success && result.Data != null)
//				{
//					platformItems = result.Data.Items ?? new List<WarehouseDto>();
//					platformTotalRecords = result.Data.TotalRecords;
//					platformTotalPages = (int)Math.Ceiling((double)platformTotalRecords / platformSearchModel.PageSize);
//				}
//				else
//				{
//					platformItems = new List<WarehouseDto>();
//					platformTotalRecords = 0;
//					platformTotalPages = 0;
//				}
//			}
//			catch (Exception ex)
//			{
//				Console.WriteLine($"Error in SearchPlatformWarehouses: {ex.Message}");
//				await ShowErrorNotification("Error", "Failed to load platform warehouses");
//				platformItems = new List<WarehouseDto>();
//			}
//			finally
//			{
//				//isLoading = false;
//				StateHasChanged();
//			}
//		}

//		// Vendor Warehouses Search
//		private async Task SearchVendorWarehouses()
//		{
//			try
//			{
//				//isLoading = true;
//				StateHasChanged();

//				Console.WriteLine($"SearchVendorWarehouses - IsDefaultPlatformWarehouse: {vendorSearchModel.IsDefaultPlatformWarehouse}");

//				var result = await WarehouseService.SearchVendorAsync(vendorSearchModel);

//				if (result.Success && result.Data != null)
//				{
//					vendorItems = result.Data.Items ?? new List<WarehouseDto>();
//					vendorTotalRecords = result.Data.TotalRecords;
//					vendorTotalPages = (int)Math.Ceiling((double)vendorTotalRecords / vendorSearchModel.PageSize);
//				}
//				else
//				{
//					vendorItems = new List<WarehouseDto>();
//					vendorTotalRecords = 0;
//					vendorTotalPages = 0;
//				}
//			}
//			catch (Exception ex)
//			{
//				Console.WriteLine($"Error in SearchVendorWarehouses: {ex.Message}");
//				await ShowErrorNotification("Error", "Failed to load vendor warehouses");
//				vendorItems = new List<WarehouseDto>();
//			}
//			finally
//			{
//				//isLoading = false;
//				StateHasChanged();
//			}
//		}

//		// Override base Search method
//		protected override async Task Search()
//		{
//			if (activeTab == "platform")
//			{
//				platformCurrentPage = 1;
//				platformSearchModel.PageNumber = 1;
//				await SearchPlatformWarehouses();
//			}
//			else
//			{
//				vendorCurrentPage = 1;
//				vendorSearchModel.PageNumber = 1;
//				await SearchVendorWarehouses();
//			}
//		}

//		protected async Task SwitchToPlatform()
//		{
//			activeTab = "platform";
//			StateHasChanged();
//		}

//		protected async Task SwitchToVendor()
//		{
//			activeTab = "vendor";
//			StateHasChanged();
//		}

//		protected async Task OnVendorFilterChanged(ChangeEventArgs e)
//		{
//			if (Guid.TryParse(e.Value?.ToString(), out Guid vendorId))
//			{
//				selectedVendorId = vendorId;
//				vendorSearchModel.VendorId = vendorId;
//			}
//			else
//			{
//				selectedVendorId = null;
//				vendorSearchModel.VendorId = null;
//			}

//			vendorCurrentPage = 1;
//			vendorSearchModel.PageNumber = 1;
//			await SearchVendorWarehouses();
//		}

//		protected async Task OnFilterChanged(ChangeEventArgs e)
//		{
//			var filterValue = e.Value?.ToString();

//			if (activeTab == "platform")
//			{
//				platformSearchModel.IsActive = filterValue switch
//				{
//					"active" => true,
//					"inactive" => false,
//					_ => null
//				};
//				platformCurrentPage = 1;
//				platformSearchModel.PageNumber = 1;
//				await SearchPlatformWarehouses();
//			}
//			else
//			{
//				vendorSearchModel.IsActive = filterValue switch
//				{
//					"active" => true,
//					"inactive" => false,
//					_ => null
//				};
//				vendorCurrentPage = 1;
//				vendorSearchModel.PageNumber = 1;
//				await SearchVendorWarehouses();
//			}
//		}

//		protected override async Task OnPageSizeChanged(ChangeEventArgs e)
//		{
//			if (int.TryParse(e.Value?.ToString(), out int pageSize))
//			{
//				if (activeTab == "platform")
//				{
//					platformSearchModel.PageSize = pageSize;
//					platformCurrentPage = 1;
//					platformSearchModel.PageNumber = 1;
//					await SearchPlatformWarehouses();
//				}
//				else
//				{
//					vendorSearchModel.PageSize = pageSize;
//					vendorCurrentPage = 1;
//					vendorSearchModel.PageNumber = 1;
//					await SearchVendorWarehouses();
//				}
//			}
//		}

//		protected override async Task GoToPage(int page)
//		{
//			if (activeTab == "platform")
//			{
//				if (page < 1 || page > platformTotalPages) return;
//				platformCurrentPage = page;
//				platformSearchModel.PageNumber = page;
//				await SearchPlatformWarehouses();
//			}
//			else
//			{
//				if (page < 1 || page > vendorTotalPages) return;
//				vendorCurrentPage = page;
//				vendorSearchModel.PageNumber = page;
//				await SearchVendorWarehouses();
//			}
//		}

//		protected override async Task<ResponseModel<IEnumerable<WarehouseDto>>> GetAllItemsAsync()
//		{
//			return await WarehouseService.GetAllAsync();
//		}

//		protected override async Task<ResponseModel<bool>> DeleteItemAsync(Guid id)
//		{
//			var result = await WarehouseService.DeleteAsync(id);
//			return new ResponseModel<bool>
//			{
//				Success = result.Success,
//				Message = result.Message,
//				Data = result.Success,
//				Errors = result.Errors
//			};
//		}

//		protected override async Task<string> GetItemId(WarehouseDto item)
//		{
//			return await Task.FromResult(item.Id.ToString());
//		}

//		protected async Task ToggleStatus(WarehouseDto warehouse)
//		{
//			try
//			{
//				var result = await WarehouseService.ToggleStatusAsync(warehouse.Id);
//				if (result.Success)
//				{
//					await ShowSuccessNotification("Status updated successfully");

//					// Refresh the correct tab
//					if (activeTab == "platform")
//					{
//						await SearchPlatformWarehouses();
//					}
//					else
//					{
//						await SearchVendorWarehouses();
//					}
//				}
//				else
//				{
//					await ShowErrorNotification(ValidationResources.Error, result.Message ?? "Failed to update status");
//				}
//			}
//			catch (Exception)
//			{
//				await ShowErrorNotification(ValidationResources.Error, "Error updating warehouse status");
//			}
//		}

//		protected void AddWarehouse()
//		{
//			var route = activeTab == "platform" ? "/warehouse/platform/add" : "/warehouse/vendor/add";
//			Navigation.NavigateTo(route);
//		}

//		protected override async Task Edit(WarehouseDto item)
//		{
//			var id = await GetItemId(item);
//			var type = item.IsDefaultPlatformWarehouse ? "platform" : "vendor";
//			var editRoute = $"/warehouse/{type}/{id}";
//			Navigation.NavigateTo(editRoute);
//		}

//		// Computed properties for pagination
//		protected int StartRecord => totalRecords == 0 ? 0 : ((currentPage - 1) * searchModel.PageSize) + 1;
//		protected int EndRecord => Math.Min(currentPage * searchModel.PageSize, totalRecords);
//		protected bool CanGoToPreviousPage => currentPage > 1;
//		protected bool CanGoToNextPage => currentPage < totalPages;
//	}
//}

////using Dashboard.Contracts.Warehouse;
////using Microsoft.AspNetCore.Components;
////using Resources;
////using Shared.DTOs.Vendor;
////using Shared.DTOs.Warehouse;
////using Shared.GeneralModels;
////using Shared.GeneralModels.SearchCriteriaModels;

////namespace Dashboard.Pages.Warehouse
////{
////	public partial class Index : BaseListPage<WarehouseDto>
////	{
////		[Inject] private IWarehouseService WarehouseService { get; set; } = null!;

////		// Tab management
////		private string activeTab = "platform"; // "platform" or "vendor"
////		private bool isMultiVendorEnabled = false;

////		protected new WarehouseSearchCriteriaModel searchModel = new()
////		{
////			IsDefaultPlatformWarehouse = true  
////		};

////		// Vendors list
////		private IEnumerable<VendorDto> vendors = new List<VendorDto>();
////		private Guid? selectedVendorId = null;

////		protected override string EntityName => "Warehouse Management";
////		protected override string AddRoute => activeTab == "platform" ? "/warehouse/platform" : "/warehouse/vendor";
////		protected override string EditRouteTemplate => "/warehouse/{id}";
////		protected override string SearchEndpoint => "api/v1/Warehouse/search";

////		protected override Dictionary<string, Func<WarehouseDto, object>> ExportColumns => new()
////		{
////			{ "Type", item => item.IsDefaultPlatformWarehouse ? "Platform" : "Vendor" },
////			//{ "Vendor", item => item.VendorName ?? "-" },
////			{ "Address", item => item.Address ?? "-" },
////			{ "Email", item => item.Email ?? "-" },
////			{ "Status", item => item.IsActive ? "Active" : "Inactive" }
////		};

////		protected override async Task OnCustomInitializeAsync()
////		{
////			searchModel.PageSize = 10;

////			searchModel.IsDefaultPlatformWarehouse = true; // Start with platform warehouses
////			// Check if multi-vendor is enabled
////			await CheckMultiVendorStatus();

////			// Load vendors if multi-vendor is enabled
////			if (isMultiVendorEnabled)
////			{
////				await LoadVendors();
////			}

////			// Set initial filter based on active tab

////			await base.OnCustomInitializeAsync();
////		}
////		protected async Task SwitchToPlatform()
////		{
////			searchModel.IsDefaultPlatformWarehouse = true; // Start with platform warehouses

////			await SwitchTab("platform");
////		}

////		protected async Task SwitchToVendor()
////		{
////			searchModel.IsDefaultPlatformWarehouse = false; // Start with platform warehouses

////			await SwitchTab("vendor");
////		}
////		private async Task CheckMultiVendorStatus()
////		{
////			try
////			{
////				var result = await WarehouseService.IsMultiVendorEnabledAsync();
////				if (result.Success)
////				{
////					isMultiVendorEnabled = result.Data;
////					searchModel.IsDefaultPlatformWarehouse = true;
////					//isMultiVendorEnabled = true;
////					StateHasChanged();
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
////				var result = await WarehouseService.GetVendorsAsync();
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

////		protected async Task SwitchTab(string tab)
////		{
////			if (activeTab == tab) return;

////			activeTab = tab;

////			// Reset filters
////			searchModel.SearchTerm = "";
////			searchModel.IsActive = null;
////			selectedVendorId = null;
////			searchModel.VendorId = null;

////			// Set warehouse type filter based on tab
////			if (tab == "platform")
////			{
////				searchModel.IsDefaultPlatformWarehouse = true;
////			}
////			else if (tab == "vendor")
////			{
////				searchModel.IsDefaultPlatformWarehouse = false;
////			}

////			// Reset pagination
////			searchModel.PageNumber = 1;
////			currentPage = 1;

////			// Search with new filter
////			await Search();
////			StateHasChanged();
////		}

////		protected async Task OnVendorFilterChanged(ChangeEventArgs e)
////		{
////			if (Guid.TryParse(e.Value?.ToString(), out Guid vendorId))
////			{
////				selectedVendorId = vendorId;
////				searchModel.VendorId = vendorId;
////			}
////			else
////			{
////				selectedVendorId = null;
////				searchModel.VendorId = null;
////			}

////			searchModel.PageNumber = 1;
////			currentPage = 1;
////			await Search();
////		}

////		protected async Task OnFilterChanged(ChangeEventArgs e)
////		{
////			var filterValue = e.Value?.ToString();

////			if (!string.IsNullOrEmpty(filterValue))
////			{
////				searchModel.IsActive = filterValue switch
////				{
////					"active" => true,
////					"inactive" => false,
////					_ => null
////				};
////			}
////			else
////			{
////				searchModel.IsActive = null;
////			}

////			searchModel.PageNumber = 1;
////			currentPage = 1;
////			await Search();
////		}

////		protected override async Task<ResponseModel<IEnumerable<WarehouseDto>>> GetAllItemsAsync()
////		{
////			return await WarehouseService.GetAllAsync();
////		}

////		protected override async Task<ResponseModel<bool>> DeleteItemAsync(Guid id)
////		{
////			var result = await WarehouseService.DeleteAsync(id);
////			return new ResponseModel<bool>
////			{
////				Success = result.Success,
////				Message = result.Message,
////				Data = result.Success,
////				Errors = result.Errors
////			};
////		}

////		protected override async Task<string> GetItemId(WarehouseDto item)
////		{
////			return await Task.FromResult(item.Id.ToString());
////		}

////		protected async Task ToggleStatus(WarehouseDto warehouse)
////		{
////			try
////			{
////				var result = await WarehouseService.ToggleStatusAsync(warehouse.Id);
////				if (result.Success)
////				{
////					await ShowSuccessNotification("Status updated successfully");
////					await Search();
////				}
////				else
////				{
////					await ShowErrorNotification(ValidationResources.Error, result.Message ?? "Failed to update status");
////				}
////			}
////			catch (Exception)
////			{
////				await ShowErrorNotification(ValidationResources.Error, "Error updating warehouse status");
////			}
////		}

////		// Override Add to include warehouse type
////		protected void AddWarehouse()
////		{
////			var route = activeTab == "platform" ? "/warehouse/platform/add" : "/warehouse/vendor/add";
////			Navigation.NavigateTo(route);
////		}
////		protected override async Task Edit(WarehouseDto item)
////		{
////			var id = await GetItemId(item);
////			var type = item.IsDefaultPlatformWarehouse ? "platform" : "vendor";
////			var editRoute = $"/warehouse/{type}/{id}";
////			Navigation.NavigateTo(editRoute);
////		}
////	}
////}

////using Dashboard.Contracts.Warehouse;
////using Microsoft.AspNetCore.Components;
////using Resources;
////using Shared.DTOs.Warehouse;
////using Shared.GeneralModels;

////namespace Dashboard.Pages.Warehouse
////{
////    public partial class Index : BaseListPage<WarehouseDto>
////    {
////        [Inject] private IWarehouseService WarehouseService { get; set; } = null!;

////        protected override string EntityName => "Warehouse Management";
////        protected override string AddRoute => "/warehouse";
////        protected override string EditRouteTemplate => "/warehouse/{id}";
////        protected override string SearchEndpoint => "api/v1/Warehouse/search";

////        protected override Dictionary<string, Func<WarehouseDto, object>> ExportColumns => new()
////        {
////            //{ "English Name", item => item.TitleEn ?? "-" },
////            //{ "Arabic Name", item => item.TitleAr ?? "-" },
////            { "Address", item => item.Address ?? "-" },
////            //{ "Phone", item => !string.IsNullOrEmpty(item.PhoneNumber) ? $"{item.PhoneCode} {item.PhoneNumber}" : "-" },
////            { "Status", item => item.IsActive ? "Active" : "Inactive" }
////        };

////        protected override async Task<ResponseModel<IEnumerable<WarehouseDto>>> GetAllItemsAsync()
////        {
////            return await WarehouseService.GetAllAsync();
////        }

////        protected override async Task<ResponseModel<bool>> DeleteItemAsync(Guid id)
////        {
////            var result = await WarehouseService.DeleteAsync(id);
////            return new ResponseModel<bool>
////            {
////                Success = result.Success,
////                Message = result.Message,
////                Data = result.Success,
////                Errors = result.Errors
////            };
////        }

////        protected override async Task<string> GetItemId(WarehouseDto item)
////        {
////            return await Task.FromResult(item.Id.ToString());
////        }

////        protected async Task ToggleStatus(WarehouseDto warehouse)
////        {
////            try
////            {
////                var result = await WarehouseService.ToggleStatusAsync(warehouse.Id);
////                if (result.Success)
////                {
////                    await ShowSuccessNotification("Status updated successfully");
////                    await Search();
////                }
////                else
////                {
////                    await ShowErrorNotification(ValidationResources.Error, result.Message ?? "Failed to update status");
////                }
////            }
////            catch (Exception)
////            {
////                await ShowErrorNotification(ValidationResources.Error, "Error updating warehouse status");
////            }
////        }

////        protected async Task OnFilterChanged(ChangeEventArgs e)
////        {
////            var filterValue = e.Value?.ToString();

////            await Search();

////            if (!string.IsNullOrEmpty(filterValue) && items != null)
////            {
////                items = filterValue switch
////                {
////                    "active" => items.Where(w => w.IsActive),
////                    "inactive" => items.Where(w => !w.IsActive),
////                    _ => items
////                };

////                totalRecords = items.Count();
////                totalPages = (int)Math.Ceiling((double)totalRecords / searchModel.PageSize);
////                currentPage = 1;
////                searchModel.PageNumber = 1;

////                StateHasChanged();
////            }
////        }

////        protected override async Task OnCustomInitializeAsync()
////        {
////            searchModel.PageSize = 10;
////            await base.OnCustomInitializeAsync();
////        }
////    }
////}
