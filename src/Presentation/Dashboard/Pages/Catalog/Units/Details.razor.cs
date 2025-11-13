using Dashboard.Contracts.ECommerce.Item;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.ECommerce.Unit;

namespace Dashboard.Pages.Catalog.Units
{
    public partial class Details
    {
        protected bool isSaving { get; set; }
        protected UnitDto Model { get; set; } = new() { ConversionUnitsFrom = new(), ConversionUnitsTo = new() };
        protected IEnumerable<UnitDto> AvailableUnits { get; set; } = Enumerable.Empty<UnitDto>();

        [Parameter] public Guid Id { get; set; }

        [Inject] protected IUnitService UnitService { get; set; } = null!;
        [Inject] protected IJSRuntime JSRuntime { get; set; } = null!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;

        // ? FIX: ????? ????? ???????
        private bool _initialized = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadUnits();
        }

        protected override void OnParametersSet()
        {
            // ? FIX: ???? ?? ??????? ???? infinite loop
            if (!_initialized || Id != Model.Id)
            {
                if (Id != Guid.Empty)
                {
                    _ = Edit(Id);
                }
                _initialized = true;
            }
        }

        protected async Task LoadUnits()
        {
            var result = await UnitService.GetAllAsync();
            if (result.Success)
            {
                AvailableUnits = result.Data ?? Enumerable.Empty<UnitDto>();
                StateHasChanged();
            }
        }

        protected async Task Save()
        {
            try
            {
                isSaving = true;
                StateHasChanged(); // Force UI update to show spinner
                var result = await UnitService.SaveAsync(Model);

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
            catch (Exception ex)
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
                var result = await UnitService.GetByIdAsync(id);

                if (!result.Success)
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        NotifiAndAlertsResources.FailedToRetrieveData,
                        "error");
                    return;
                }

                // Initialize model with proper empty collections if null
                Model = result.Data ?? new UnitDto
                {
                    ConversionUnitsFrom = new List<ConversionUnitDto>(),
                    ConversionUnitsTo = new List<ConversionUnitDto>()
                };

                Model.ConversionUnitsFrom = Model.ConversionUnitsFrom
                    .Where(c => c.ConversionUnitId != Model.Id)
                    .ToList();

                Model.ConversionUnitsTo = Model.ConversionUnitsTo
                    .Where(c => c.ConversionUnitId != Model.Id)
                    .ToList();

                StateHasChanged();
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Error,
                    NotifiAndAlertsResources.SaveFailed,
                    "error");
            }
        }

        protected async Task CloseModal()
        {
            Navigation.NavigateTo("/units");
        }

        private void AddNewConversionUnitFrom()
        {
            var available = AvailableUnits ?? Enumerable.Empty<UnitDto>();
            Model.ConversionUnitsFrom ??= new List<ConversionUnitDto>();
            Model.ConversionUnitsFrom.Add(new ConversionUnitDto
            {
                ConversionUnitId = Guid.Empty,
                ConversionFactor = (double)1m
            });
            StateHasChanged();
        }

        private async void RemoveConversionUnitsFrom(int index)
        {
            var confirmed = await JSRuntime.InvokeAsync<bool>("swal", new
            {
                title = NotifiAndAlertsResources.AreYouSure,
                text = NotifiAndAlertsResources.ConfirmDeleteAlert,
                icon = "warning",
                buttons = true,
                dangerMode = true,
            });

            if (confirmed)
            {
                if (index >= 0 && index < Model.ConversionUnitsFrom.Count())
                {
                    Model.ConversionUnitsFrom.RemoveAt(index);
                    StateHasChanged(); // Trigger UI update
                }
            }
        }

        private void AddNewConversionUnitTo()
        {
            var available = AvailableUnits ?? Enumerable.Empty<UnitDto>();
            Model.ConversionUnitsTo ??= new List<ConversionUnitDto>();
            Model.ConversionUnitsTo.Add(new ConversionUnitDto
            {
                ConversionUnitId = Guid.Empty,
                ConversionFactor = (double)1m
            });
            StateHasChanged();
        }

        private async void RemoveConversionUnitsTo(int index)
        {
            var confirmed = await JSRuntime.InvokeAsync<bool>("swal", new
            {
                title = NotifiAndAlertsResources.AreYouSure,
                text = NotifiAndAlertsResources.ConfirmDeleteAlert,
                icon = "warning",
                buttons = true,
                dangerMode = true,
            });

            if (confirmed)
            {
                if (index >= 0 && index < Model.ConversionUnitsTo.Count())
                {
                    Model.ConversionUnitsTo.RemoveAt(index);
                    StateHasChanged(); // Trigger UI update
                }
            }
        }
    }
}