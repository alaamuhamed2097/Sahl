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

		// Sorting properties
		private string currentSortColumn = "Address";
		private bool isAscending = true;

		// Platform Tab - Single Warehouse (Details View)
		private WarehouseDto? platformWarehouse = null;

		// Platform Tab Search Model & Data (kept for compatibility)
		private WarehouseSearchCriteriaModel platformSearchModel = new()
		{
			IsDefaultPlatformWarehouse = true,
			PageSize = 10,
			SortBy = "Address",
			SortDirection = "asc"
		};
		private IEnumerable<WarehouseDto> platformItems = new List<WarehouseDto>();
		private int platformTotalRecords = 0;
		private int platformCurrentPage = 1;
		private int platformTotalPages = 0;

		// Vendor Tab Search Model & Data
		private WarehouseSearchCriteriaModel vendorSearchModel = new()
		{
			IsDefaultPlatformWarehouse = false,
			PageSize = 10,
			SortBy = "Address",
			SortDirection = "asc"
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
			StateHasChanged();

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

				var result = await WarehouseService.SearchAsync(platformSearchModel);

				if (result.Success && result.Data != null && result.Data.Items != null && result.Data.Items.Any())
				{
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

				var result = await WarehouseService.SearchAsync(platformSearchModel);

				if (result.Success && result.Data != null)
				{
					platformItems = result.Data.Items ?? new List<WarehouseDto>();
					platformTotalRecords = result.Data.TotalRecords;
					platformTotalPages = (int)Math.Ceiling((double)platformTotalRecords / platformSearchModel.PageSize);
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

				Console.WriteLine($"[UI] SearchVendorWarehouses - SortBy: {vendorSearchModel.SortBy}, Direction: {vendorSearchModel.SortDirection}");

				var result = await WarehouseService.SearchVendorAsync(vendorSearchModel);

				if (result.Success && result.Data != null)
				{
					vendorItems = result.Data.Items ?? new List<WarehouseDto>();
					vendorTotalRecords = result.Data.TotalRecords;
					vendorTotalPages = (int)Math.Ceiling((double)vendorTotalRecords / vendorSearchModel.PageSize);

					Console.WriteLine($"[UI] Loaded {vendorItems.Count()} items");
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

		// Sorting method
		private async Task SortByColumn(string columnName)
		{
			Console.WriteLine($"=== SORT DEBUG ===");
			Console.WriteLine($"Column Name: {columnName}");
			Console.WriteLine($"Current Sort Column: {currentSortColumn}");
			Console.WriteLine($"Is Ascending Before: {isAscending}");

			Console.WriteLine($"[UI] SortByColumn called with: {columnName}");
			Console.WriteLine($"[UI] Current sort column: {currentSortColumn}, IsAscending: {isAscending}");

			if (currentSortColumn == columnName)
			{
				isAscending = !isAscending;
			}
			else
			{
				currentSortColumn = columnName;
				isAscending = true;
			}
			Console.WriteLine($"Is Ascending After: {isAscending}");
			Console.WriteLine($"Active Tab: {activeTab}");

			Console.WriteLine($"[UI] New IsAscending: {isAscending}");

			//if (activeTab == "platform")
			//{
			//	platformSearchModel.SortDirection = isAscending ? "asc" : "desc";
			//	platformSearchModel.SortBy = columnName;
			//	Console.WriteLine($"[UI] Platform sort - SortBy: {platformSearchModel.SortBy}, Direction: {platformSearchModel.SortDirection}");
			//	await SearchPlatformWarehouses();
			//}
			//else
			//{
			vendorSearchModel.SortDirection = isAscending ? "asc" : "desc";
			vendorSearchModel.SortBy = columnName;
			Console.WriteLine($"[UI] Vendor sort - SortBy: {vendorSearchModel.SortBy}, Direction: {vendorSearchModel.SortDirection}");
			await SearchVendorWarehouses();
			//}
		}

		// Get sort icon for column
		private string GetSortIcon(string columnName)
		{
			if (currentSortColumn != columnName)
			{
				return "fas fa-sort text-muted";
			}
			return isAscending ? "fas fa-sort-up text-primary" : "fas fa-sort-down text-primary";
		}

		// Override base Search method
		protected override async Task Search()
		{
			if (activeTab == "platform")
			{
				platformCurrentPage = 1;
				platformSearchModel.PageNumber = 1;
				platformSearchModel.SortBy = currentSortColumn;
				platformSearchModel.SortDirection = isAscending ? "asc" : "desc";
				await SearchPlatformWarehouses();
			}
			else
			{
				vendorCurrentPage = 1;
				vendorSearchModel.PageNumber = 1;
				vendorSearchModel.SortBy = currentSortColumn;
				vendorSearchModel.SortDirection = isAscending ? "asc" : "desc";
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

		protected int StartRecord => totalRecords == 0 ? 0 : ((currentPage - 1) * searchModel.PageSize) + 1;
		protected int EndRecord => Math.Min(currentPage * searchModel.PageSize, totalRecords);
		protected bool CanGoToPreviousPage => currentPage > 1;
		protected bool CanGoToNextPage => currentPage < totalPages;
	}
}


