using Dashboard.Contracts.Location;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Location;

namespace Dashboard.Pages.Location.State;

public partial class Details
{
    private bool isSaving { get; set; }

    protected StateDto Model { get; set; } = new();
    protected IEnumerable<CountryDto>? countries = Enumerable.Empty<CountryDto>();

    [Parameter] public Guid Id { get; set; }

    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
    [Inject] protected NavigationManager Navigation { get; set; } = null!;
    [Inject] protected IStateService StateService { get; set; } = null!;
    [Inject] protected ICountryService CountryService { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await LoadCountries();
        Model = new();
    }
    protected override void OnParametersSet()
    {
        if (Id != Guid.Empty)
        {
            Edit(Id);
        }
    }
    protected async Task Save()
    {
        try
        {
            isSaving = true;
            StateHasChanged(); // Force UI update to show spinner

            var result = await StateService.SaveAsync(Model);

            isSaving = false;
            if (result.Success)
            {
                await CloseModal();
                await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Done, NotifiAndAlertsResources.SavedSuccessfully, "success");
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, NotifiAndAlertsResources.SaveFailed, "error");
            }
        }
        catch (Exception)
        {
            await JSRuntime.InvokeVoidAsync("swal",
                NotifiAndAlertsResources.FailedAlert,
                "error");
        }
        finally
        {
            await CloseModal();
        }
    }

    protected async Task Edit(Guid id)
    {
        try
        {
            var result = await StateService.GetByIdAsync(id);

            if (!result.Success)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Failed,
                    NotifiAndAlertsResources.FailedToRetrieveData,
                    "error");
                return;
            }
            // Initialize model with proper empty collections if null
            Model = result.Data ?? new();
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

    protected async Task CloseModal()
    {
        Navigation.NavigateTo("/states");
    }
    protected async Task LoadCountries()
    {
        var responseModel = await CountryService.GetAllAsync();
        countries = responseModel.Data;
    }
}