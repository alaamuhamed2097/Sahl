using Common.Enumerations.FieldType;
using Dashboard.Contracts.ECommerce.Category;
using Dashboard.Contracts.General;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.ECommerce.Category;

namespace Dashboard.Pages.Catalog.Attributes
{
    public partial class Details
    {
        private bool isSaving { get; set; }
        private Guid _lastLoadedId = Guid.Empty; // Track the last loaded ID

        protected IEnumerable<FieldType> fieldTypes = Enum.GetValues(typeof(FieldType)).Cast<FieldType>();
        protected FieldType fieldType
        {
            get => Model.FieldType;
            set
            {
                Model.FieldType = value;
                if ((value == FieldType.List || value == FieldType.MultiSelectList) &&
                    (Model.AttributeOptions == null || !Model.AttributeOptions.Any()))
                {
                    AddNewOption();
                }
                StateHasChanged();
            }
        }
        protected AttributeDto Model { get; set; } = new();

        [Parameter] public Guid Id { get; set; }

        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        [Inject] protected IResourceLoaderService ResourceLoaderService { get; set; } = null!;
        [Inject] protected IAttributeService AttributeService { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            // Load the attribute if Id is provided
            if (Id != Guid.Empty)
            {
                await Edit(Id);
                _lastLoadedId = Id;
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            // Only load if the Id has actually changed
            if (Id != Guid.Empty && Id != _lastLoadedId)
            {
                await Edit(Id);
                _lastLoadedId = Id;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await ResourceLoaderService.LoadStyleSheet("css/display_order.css");
            }
        }

        protected async Task Save()
        {
            try
            {
                isSaving = true;
                StateHasChanged(); // Force UI update to show spinner
                if (Model.AttributeOptions != null)
                {
                    var emptyOptions = Model.AttributeOptions.Where(o => string.IsNullOrEmpty(o.TitleAr) || string.IsNullOrEmpty(o.TitleEn));
                    Model.AttributeOptions = Model.AttributeOptions.Except(emptyOptions).ToList();
                    foreach (var option in Model.AttributeOptions)
                    {
                        option.AttributeId = Model.Id;
                    }
                }

                if (Model.FieldType == FieldType.MultiSelectList ||
                    Model.FieldType == FieldType.List ||
                    Model.FieldType == FieldType.Text ||
                    Model.FieldType == FieldType.CheckBox)
                    Model.IsRangeFieldType = false;

                if (Model.FieldType != FieldType.MultiSelectList &&
                    Model.FieldType != FieldType.List)
                    Model.AttributeOptions = new();

                var result = await AttributeService.SaveAsync(Model);

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
                var result = await AttributeService.GetByIdAsync(id);

                if (!result.Success)
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        NotifiAndAlertsResources.FailedToRetrieveData,
                        "error");
                    return;
                }
                // Initialize model with proper empty collections if null
                Model = result.Data ?? new AttributeDto
                {
                    AttributeOptions = new List<AttributeOptionDto>()
                };

                if (Model.AttributeOptions != null && Model.AttributeOptions.Any())
                {
                    foreach (var option in Model.AttributeOptions.Where(o => o.DisplayOrder == 0))
                        option.DisplayOrder = Model.AttributeOptions.Max(o => o.DisplayOrder) + 1;
                    Model.AttributeOptions = Model.AttributeOptions.OrderBy(o => o.DisplayOrder).ToList();
                }

                StateHasChanged();
            }
            catch (Exception)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Error,
                    NotifiAndAlertsResources.SaveFailed,
                    "error");
            }
        }

        protected async Task CloseModal()
        {
            Navigation.NavigateTo("/attributes");
        }

        private void AddNewOption()
        {
            Model.AttributeOptions ??= new List<AttributeOptionDto>();
            Model.AttributeOptions.Add(new AttributeOptionDto
            {
                TitleAr = "",
                TitleEn = "",
                AttributeId = Model.Id,
                DisplayOrder = GetNextAttributeOptionDisplayOrder()
            });
            StateHasChanged();
        }

        private async void RemoveOption(int index)
        {
            if (index < 0 || Model.AttributeOptions == null || index >= Model.AttributeOptions.Count)
                return;
            var confirmed = await JSRuntime.InvokeAsync<bool>("swal", new
            {
                title = NotifiAndAlertsResources.AreYouSure,
                text = NotifiAndAlertsResources.ConfirmDeleteAlert,
                icon = "warning",
                buttons = new
                {
                    cancel = ActionsResources.Cancel,
                    confirm = ActionsResources.Confirm,
                },
                dangerMode = true,
            });

            if (confirmed)
            {
                if (index >= 0 && index < Model.AttributeOptions.Count())
                {
                    Model.AttributeOptions.RemoveAt(index);
                    StateHasChanged(); // Trigger UI update
                }
            }
        }
        private async Task MoveAttributeOptionUp(int index)
        {
            if (index <= 0 || Model.AttributeOptions == null || index >= Model.AttributeOptions.Count)
                return;
            try
            {
                var currentAttributeOption = Model.AttributeOptions[index];
                var previousAttributeOption = Model.AttributeOptions[index - 1];
                // Swap display orders
                var tempOrder = currentAttributeOption.DisplayOrder;
                currentAttributeOption.DisplayOrder = previousAttributeOption.DisplayOrder;
                previousAttributeOption.DisplayOrder = tempOrder;
                // Sort the list by display order to reflect the change
                Model.AttributeOptions = Model.AttributeOptions
                    .OrderBy(attr => attr.DisplayOrder)
                    .ToList();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error moving AttributeOption up: {ex.Message}");
                await ShowErrorNotification("Error", "Failed to reorder AttributeOption");
            }
        }

        private async Task MoveAttributeOptionDown(int index)
        {
            if (Model.AttributeOptions == null || index < 0 || index >= Model.AttributeOptions.Count - 1)
                return;
            try
            {
                var currentAttributeOption = Model.AttributeOptions[index];
                var nextAttributeOption = Model.AttributeOptions[index + 1];
                // Swap display orders
                var tempOrder = currentAttributeOption.DisplayOrder;
                currentAttributeOption.DisplayOrder = nextAttributeOption.DisplayOrder;
                nextAttributeOption.DisplayOrder = tempOrder;
                // Sort the list by display order to reflect the change
                Model.AttributeOptions = Model.AttributeOptions
                    .OrderBy(attr => attr.DisplayOrder)
                    .ToList();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error moving AttributeOption down: {ex.Message}");
                await ShowErrorNotification("Error", "Failed to reorder AttributeOption");
            }
        }

        private void NormalizeAttributeOptionDisplayOrders()
        {
            if (Model.AttributeOptions == null) return;
            // Sort by current display order first
            var sortedAttributeOptions = Model.AttributeOptions
                .OrderBy(attr => attr.DisplayOrder)
                .ToList();
            // Reassign sequential display orders
            for (int i = 0; i < sortedAttributeOptions.Count; i++)
            {
                sortedAttributeOptions[i].DisplayOrder = i + 1;
            }
            Model.AttributeOptions = sortedAttributeOptions;
        }

        private int GetNextAttributeOptionDisplayOrder()
        {
            if (Model.AttributeOptions == null || !Model.AttributeOptions.Any())
                return 1;
            return Model.AttributeOptions.Max(attr => attr.DisplayOrder) + 1;
        }

        private bool CanMoveUp(int index)
        {
            return index > 0 && Model.AttributeOptions != null && Model.AttributeOptions.Count > 1;
        }

        private bool CanMoveDown(int index)
        {
            return Model.AttributeOptions != null && index < Model.AttributeOptions.Count - 1 && Model.AttributeOptions.Count > 1;
        }
        // ========== Notification Helpers ==========
        private async Task ShowErrorNotification(string title, string message)
        {
            await JSRuntime.InvokeVoidAsync("swal", title, message, "error");
        }

        private async Task ShowSuccessNotification(string title, string message)
        {
            await JSRuntime.InvokeVoidAsync("swal", title, message, "success");
        }

        private async Task ShowWarningNotification(string title, string message)
        {
            await JSRuntime.InvokeVoidAsync("swal", title, message, "warning");
        }

        private async Task<bool> ShowConfirmationDialog(string title, string message)
        {
            return await JSRuntime.InvokeAsync<bool>("swal", new
            {
                title,
                text = message,
                icon = "warning",
                buttons = new
                {
                    cancel = ActionsResources.Cancel,
                    confirm = ActionsResources.Confirm,
                },
                dangerMode = true,
            });
        }
    }
}
