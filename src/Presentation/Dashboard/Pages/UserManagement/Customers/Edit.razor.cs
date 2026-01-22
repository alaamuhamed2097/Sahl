using Dashboard.Contracts.Customer;
using Dashboard.Pages.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.User.Customer;

namespace Dashboard.Pages.UserManagement.Customers
{
	public partial class Edit : LocalizedComponentBase
    {
		[Parameter] public Guid Id { get; set; }

		protected CustomerUpdateByAdminDto Model { get; set; } = new();
		private bool isSaving { get; set; }
		private string userId { get; set; } = string.Empty;

		[Inject] private IJSRuntime JSRuntime { get; set; } = default!;
		[Inject] protected NavigationManager Navigation { get; set; } = default!;
		[Inject] protected ICustomerService _customerService { get; set; } = default!;

		protected override async Task OnParametersSetAsync()
		{
			await InitializeModel(Id);
		}

		protected async Task InitializeModel(Guid id)
		{
			try
			{
				var result = await _customerService.GetByIdAsync(id);

				if (!result.Success || result.Data is null)
				{
					await JSRuntime.InvokeVoidAsync("swal",
						ValidationResources.Failed,
						NotifiAndAlertsResources.FailedToRetrieveData,
						"error");
					return;
				}

				// Store userId for update
				userId = result.Data.UserId;

				// Map data to model
				Model = new CustomerUpdateByAdminDto
				{
					UserId = result.Data.UserId,
					FirstName = result.Data.FirstName,
					LastName = result.Data.LastName,
					Email = result.Data.Email,
					NewPassword = null // Don't load password
				};

				StateHasChanged();
			}
			catch (Exception ex)
			{
				await JSRuntime.InvokeVoidAsync("swal",
					ValidationResources.Error,
					ex.Message,
					"error");
			}
		}

		protected async Task Save()
		{
			try
			{
				isSaving = true;
				StateHasChanged();

				// Ensure UserId is set
				Model.UserId = userId;

				var result = await _customerService.UpdateByAdminAsync(Model);

				isSaving = false;
				StateHasChanged();

				if (result.Success)
				{
					await JSRuntime.InvokeVoidAsync("swal",
						ValidationResources.Done,
						NotifiAndAlertsResources.SavedSuccessfully,
						"success");

					CloseModal();
				}
				else
				{
					var errorMessage = result.Message ?? NotifiAndAlertsResources.SaveFailed;

					// Show detailed errors if available
					if (result.Errors != null && result.Errors.Any())
					{
						errorMessage += "<br/>" + string.Join("<br/>", result.Errors);
					}

					await JSRuntime.InvokeVoidAsync("Swal.fire", new
					{
						icon = "error",
						title = ValidationResources.Failed,
						html = errorMessage,
						confirmButtonText = "OK"
					});
				}
			}
			catch (Exception ex)
			{
				isSaving = false;
				StateHasChanged();

				await JSRuntime.InvokeVoidAsync("swal",
					ValidationResources.Error,
					ex.Message,
					"error");
			}
		}

		protected void CloseModal()
		{
			Navigation.NavigateTo("/users/customers");
		}
	}
}