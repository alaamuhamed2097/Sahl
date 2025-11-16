using Dashboard.Contracts.User;
using Dashboard.Contracts.Vendor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Customer;
using Shared.DTOs.User.Admin;
using Shared.DTOs.Vendor;

namespace Dashboard.Pages.UserManagement.customers
{
	public partial class Create
	{
		protected CustomerDto Model { get; set; } = new();
		private bool isSaving { get; set; }

		[Inject] private IJSRuntime JSRuntime { get; set; } = default!;
		[Inject] protected NavigationManager Navigation { get; set; } = null!;
		[Inject] protected IVendorService _vendorService { get; set; } = null!;

		//protected async Task Save()
		//{
		//	try
		//	{
		//		isSaving = true;
		//		StateHasChanged(); // Force UI update to show spinner

		//		var result = await _vendorService.CreateAsync(Model);
		//		isSaving = false;

		//		if (result.Success)
		//		{
		//			await CloseModal();
		//			await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Done, NotifiAndAlertsResources.SavedSuccessfully, "success");
		//		}
		//		else
		//		{
		//			await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, NotifiAndAlertsResources.SaveFailed, "error");
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		await JSRuntime.InvokeVoidAsync("swal",
		//			NotifiAndAlertsResources.FailedAlert,
		//			"error");
		//	}
		//}

		//protected async Task CloseModal()
		//{
		//	Navigation.NavigateTo("/users/vendors");
		//}
	}
}
