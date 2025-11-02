using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shared.DTOs.Testimonial;
using Shared.GeneralModels;
using Dashboard.Contracts.Testimonial;
using Resources;

namespace Dashboard.Pages.Marketing.Testimonial
{
    public partial class Index : BaseListPage<TestimonialDto>
    {
        [Inject] private ITestimonialService TestimonialService { get; set; } = null!;

        // Abstract properties implementation
        protected override string EntityName => "Testimonials";
        protected override string AddRoute => "/testimonial-modal";
        protected override string EditRouteTemplate => "/testimonial-modal/{id}";
        protected override string SearchEndpoint => "api/Testimonial/search";

        // Export columns configuration
        protected override Dictionary<string, Func<TestimonialDto, object>> ExportColumns => new()
        {
            { "Customer Name", item => item.CustomerName },
            { "Customer Title", item => item.CustomerTitle },
            { "Testimonial Text", item => item.TestimonialText },
            { "Has Image", item => !string.IsNullOrEmpty(item.CustomerImagePath) ? "Yes" : "No" },
            { "Display Order", item => item.DisplayOrder },
            { "Created Date", item => item.CreatedDateUtc.ToString("dd/MM/yyyy") }
        };

        // Abstract methods implementation
        protected override async Task<ResponseModel<IEnumerable<TestimonialDto>>> GetAllItemsAsync()
        {
            return await TestimonialService.GetAllAsync();
        }

        protected override async Task<ResponseModel<bool>> DeleteItemAsync(Guid id)
        {
            var result = await TestimonialService.DeleteAsync(id);
            return new ResponseModel<bool>
            {
                Success = result.Success,
                Message = result.Message,
                Data = result.Success,
                Errors = result.Errors
            };
        }

        protected override async Task<string> GetItemId(TestimonialDto item)
        {
            return await Task.FromResult(item.Id.ToString());
        }

        // Helper method to get proper image source with base URL
        protected string GetImageSourceForDisplay(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return string.Empty;

            // Check if it's already a full data URL (Base64)
            if (imagePath.StartsWith("data:image/"))
                return imagePath;

            // Check if it's a base64 string without data prefix (new uploads)
            if (imagePath.Length > 200 && !imagePath.StartsWith("http"))
                return $"data:image/png;base64,{imagePath}";

            // Check if it's already a full URL
            if (imagePath.StartsWith("http://") || imagePath.StartsWith("https://"))
                return imagePath;

            // If it's a relative path to an image on the server, prepend base URL
            return baseUrl.TrimEnd('/') + "/" + imagePath.TrimStart('/');
        }

        // Custom methods specific to testimonials
        protected async Task Preview(TestimonialDto testimonial)
        {
            try
            {
                var customerImageSrc = GetImageSourceForDisplay(testimonial.CustomerImagePath);
                
                var testimonialHtml = $@"
                    <div class='row'>
                        <div class='col-md-3 text-center'>
                            {(string.IsNullOrEmpty(customerImageSrc)
                                ? "<div class='bg-secondary rounded-circle d-flex align-items-center justify-content-center text-white mx-auto' style='width: 80px; height: 80px;'><i class='fas fa-user fa-2x'></i></div>"
                                : $"<img src='{customerImageSrc}' alt='{testimonial.CustomerName}' class='rounded-circle' style='width: 80px; height: 80px; object-fit: cover;' />")}
                            <h5 class='mt-3 mb-1'>{testimonial.CustomerName}</h5>
                            <p class='text-muted small'>{testimonial.CustomerTitle}</p>
                        </div>
                        <div class='col-md-9'>
                            <div class='p-3 bg-light rounded'>
                                <i class='fas fa-quote-left text-primary me-2'></i>
                                <span style='font-style: italic;'>{testimonial.TestimonialText}</span>
                                <i class='fas fa-quote-right text-primary ms-2'></i>
                            </div>
                            <div class='mt-2 small text-muted'>
                                <strong>Display Order:</strong> {testimonial.DisplayOrder} | 
                                <strong>Created:</strong> {testimonial.CreatedDateUtc:dd/MM/yyyy}
                            </div>
                        </div>
                    </div>";

                await JSRuntime.InvokeVoidAsync("showTestimonialPreview", testimonialHtml);
            }
            catch (Exception ex)
            {
                await ShowErrorNotification(ValidationResources.Error, "Failed to preview testimonial");
            }
        }

        protected async Task OnFilterChanged(ChangeEventArgs e)
        {
            var filterValue = e.Value?.ToString();

            // Reset to show all items first
            await Search();

            if (!string.IsNullOrEmpty(filterValue) && items != null)
            {
                items = filterValue switch
                {
                    "withImage" => items.Where(t => !string.IsNullOrEmpty(t.CustomerImagePath)),
                    "recent" => items.Where(t => t.CreatedDateUtc >= DateTime.UtcNow.AddDays(-30)),
                    _ => items
                };

                // Update pagination info
                totalRecords = items.Count();
                totalPages = (int)Math.Ceiling((double)totalRecords / searchModel.PageSize);
                currentPage = 1;
                searchModel.PageNumber = 1;

                StateHasChanged();
            }
        }

        // Override search to handle testimonial-specific logic if needed
        protected override async Task OnAfterSearchAsync()
        {
            // Add any testimonial-specific post-search logic here
            await base.OnAfterSearchAsync();
        }

        // Override initialization if needed
        protected override async Task OnCustomInitializeAsync()
        {
            // Add any testimonial-specific initialization logic here
            searchModel.PageSize = 10;
            await base.OnCustomInitializeAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                // Add custom JavaScript for testimonial preview
                await JSRuntime.InvokeVoidAsync("eval", @"
                    window.showTestimonialPreview = function(html) {
                        document.getElementById('testimonialPreviewContent').innerHTML = html;
                        var modal = new bootstrap.Modal(document.getElementById('testimonialPreviewModal'));
                        modal.show();
                    };
                ");
            }
        }
    }
}