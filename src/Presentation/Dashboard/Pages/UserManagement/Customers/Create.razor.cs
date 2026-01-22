using Dashboard.Contracts.Customer;
using Dashboard.Pages.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.User.Customer;

namespace Dashboard.Pages.UserManagement.Customers
{
    public partial class Create : LocalizedComponentBase
    {
        protected CustomerRegistrationDto Model { get; set; } = new();
        private bool isSaving { get; set; }

        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        [Inject] protected ICustomerService _customerService { get; set; } = null!;

        protected override void OnInitialized()
        {
            Model.PhoneCode = "+20"; // Default for Egypt
        }

        protected async Task Save()
        {
            try
            {
                isSaving = true;
                StateHasChanged();

                var result = await _customerService.RegisterCustomerAsync(Model);

                isSaving = false;
                StateHasChanged();

                if (result.Success)
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Done,
                        NotifiAndAlertsResources.RegistrationSuccessful,
                        "success");

                    await Task.Delay(1500);
                    Navigation.NavigateTo("/users/customer");
                }
                else
                {
                    var errorMessage = result.Message ?? NotifiAndAlertsResources.RegistrationFailed;

                    if (result.Errors != null && result.Errors.Any())
                    {
                        errorMessage = string.Join("<br/>", result.Errors);
                    }

                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        errorMessage,
                        "error");
                }
            }
            catch (Exception)
            {
                isSaving = false;
                StateHasChanged();

                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Failed,
                    NotifiAndAlertsResources.SomethingWentWrong,
                    "error");
            }
        }

        protected void CloseModal()
        {
            Navigation.NavigateTo("/users/customers");
        }
    }
}

//using Dashboard.Contracts.User;
//using Dashboard.Contracts.Vendor;
//using Microsoft.AspNetCore.Components;
//using Microsoft.JSInterop;
//using Resources;
//using Shared.DTOs.Customer;
//using Shared.DTOs.User.Admin;
//using Shared.DTOs.Vendor;

//namespace Dashboard.Pages.UserManagement.customers
//{
//	public partial class Create
//	{
//		protected CustomerDto Model { get; set; } = new();
//		private bool isSaving { get; set; }

//		[Inject] private IJSRuntime JSRuntime { get; set; } = default!;
//		[Inject] protected NavigationManager Navigation { get; set; } = null!;
//		[Inject] protected IVendorService _vendorService { get; set; } = null!;

//		//protected async Task Save()
//		//{
//		//	try
//		//	{
//		//		isSaving = true;
//		//		StateHasChanged(); // Force UI update to show spinner

//		//		var result = await _vendorService.CreateAsync(Model);
//		//		isSaving = false;

//		//		if (result.Success)
//		//		{
//		//			await CloseModal();
//		//			await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Done, NotifiAndAlertsResources.SavedSuccessfully, "success");
//		//		}
//		//		else
//		//		{
//		//			await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, NotifiAndAlertsResources.SaveFailed, "error");
//		//		}
//		//	}
//		//	catch (Exception ex)
//		//	{
//		//		await JSRuntime.InvokeVoidAsync("swal",
//		//			NotifiAndAlertsResources.FailedAlert,
//		//			"error");
//		//	}
//		//}

//		//protected async Task CloseModal()
//		//{
//		//	Navigation.NavigateTo("/users/vendors");
//		//}
//	}
//}
