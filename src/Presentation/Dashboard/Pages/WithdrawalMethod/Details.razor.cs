using Common.Enumerations.FieldType;
using Dashboard.Configuration;
using Dashboard.Contracts.General;
using Dashboard.Contracts.WithdrawalMethod;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.WithdrawalMethod;
using Shared.DTOs.WithdrawelMethod;

namespace Dashboard.Pages.WithdrawalMethod
{
    public partial class Details
    {
        private bool isSaving { get; set; }
        protected string baseUrl = string.Empty;
        private IBrowserFile? selectedFile;
        private string? previewImageUrl;

        protected WithdrawalMethodDto Model { get; set; } = new();
        protected IEnumerable<FieldType> fieldTypes;

        [Parameter] public Guid Id { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        [Inject] protected IResourceLoaderService ResourceLoaderService { get; set; } = null!;
        [Inject] protected IWithdrawalMethodService WithdrawalMethodService { get; set; } = null!;
        [Inject] protected IOptions<ApiSettings> ApiOptions { get; set; } = default!;

        protected override void OnInitialized()
        {
            baseUrl = ApiOptions.Value.BaseUrl;
            fieldTypes = Enum.GetValues(typeof(FieldType)).Cast<FieldType>();
        }
        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                ResourceLoaderService.LoadScript("Common/imageHandler/imageHandler.js");
            }
            return Task.CompletedTask;
        }
        protected override void OnParametersSet()
        {
            if (Id != Guid.Empty)
            {
                previewImageUrl = null;
                Edit(Id);
            }
        }

        protected async Task Save()
        {
            try
            {
                isSaving = true;
                StateHasChanged(); // Force UI update to show spinner
                var result = await WithdrawalMethodService.SaveAsync(Model);

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
        }

        protected async Task Edit(Guid id)
        {
            try
            {
                var result = await WithdrawalMethodService.GetByIdAsync(id);
                if (!result.Success)
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        NotifiAndAlertsResources.FailedToRetrieveData,
                        "error");
                    return;
                }
                Model = result.Data ?? new();
                Model.ImageFile = $"{baseUrl}/{Model.ImagePath}";
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

        private async Task HandleSelectedImage(InputFileChangeEventArgs e)
        {
            try
            {
                selectedFile = e.File;

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var extension = Path.GetExtension(selectedFile.Name).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        NotifiAndAlertsResources.Error,
                        ValidationResources.InvalidImageFormat,
                        "error");
                    selectedFile = null;
                    return;
                }

                const int maxFileSize = 5 * 1024 * 1024; // 5MB
                if (selectedFile.Size > maxFileSize)
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        NotifiAndAlertsResources.Error,
                        string.Format(ValidationResources.ImageSizeLimitExceeded, 5),
                        "error");
                    selectedFile = null;
                    return;
                }

                try
                {
                    using var stream = selectedFile.OpenReadStream(maxAllowedSize: maxFileSize);
                    using var ms = new MemoryStream();
                    await stream.CopyToAsync(ms);
                    var imageBytes = ms.ToArray();
                    var base64 = Convert.ToBase64String(imageBytes);

                    previewImageUrl = await JSRuntime.InvokeAsync<string>("resizeImage", base64, selectedFile.ContentType);

                    if (string.IsNullOrEmpty(previewImageUrl))
                    {
                        await JSRuntime.InvokeVoidAsync("swal",
                        NotifiAndAlertsResources.Error,
                        ValidationResources.ErrorProcessingImage,
                        "error");
                    }

                    Model.ImageFile = previewImageUrl.Replace($"data:{selectedFile.ContentType};base64,", "");
                }
                catch (Exception ex)
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        NotifiAndAlertsResources.Error,
                        ValidationResources.ErrorProcessingImage,
                        "error");
                    selectedFile = null;
                    previewImageUrl = null;
                }
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    NotifiAndAlertsResources.Error,
                    ValidationResources.ErrorProcessingImage,
                    "error");
                selectedFile = null;
                previewImageUrl = null;
            }
        }

        // Alternative: Using a Dictionary for better performance if this is called frequently
        private static readonly Dictionary<FieldType, string> FieldTypeDisplayNames = new()
        {
            [FieldType.Text] = ECommerceResources.Text,
            [FieldType.IntegerNumber] = ECommerceResources.IntegerNumber,
            [FieldType.DecimalNumber] = ECommerceResources.DecimalNumber,
            [FieldType.Date] = ECommerceResources.Date,
            [FieldType.CheckBox] = ECommerceResources.CheckBox,
            [FieldType.Email] = ECommerceResources.Email,
            [FieldType.PhoneNumber] = FormResources.PhoneNumber,
        };

        private string GetFieldTypeDisplayName(FieldType fieldType)
        {
            return FieldTypeDisplayNames.TryGetValue(fieldType, out var displayName)
            ? displayName
            : fieldType.ToString();
        }

        protected async Task CloseModal()
        {
            Navigation.NavigateTo("/withdrawalMethods");
        }

        protected void AddField()
        {
            if (Model.Fields == null)
                Model.Fields = new List<FieldDto>();

            var newField = new FieldDto
            {
                FieldType = FieldType.Text,
                TitleAr = string.Empty,
                TitleEn = string.Empty
            };

            Model.Fields.Add(newField);
            StateHasChanged();
        }

        protected void RemoveField(FieldDto field)
        {
            Model.Fields.Append(field);
        }
    }
}
