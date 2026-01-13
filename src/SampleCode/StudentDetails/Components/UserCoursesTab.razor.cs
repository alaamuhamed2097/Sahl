using Common.Enumeration;
using Dashboard.Contracts.Payment;
using Microsoft.AspNetCore.Components;

namespace Dashboard.Pages.User.UserDetails.Components
{
    public partial class UserCoursesTab
    {
        [Parameter] public Guid UserId { get; set; }

        [Inject] protected IAdminInvoiceService AdminInvoiceService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        // DTO to hold combined invoice and item data
        private class CourseItemDisplay
        {
            public Guid Id { get; set; }
            public string ItemNameAr { get; set; }
            public string ItemNameEn { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal PaymentValue { get; set; }
            public DateTime InvoiceDateUtc { get; set; }
        }

        private List<CourseItemDisplay>? courses;
        private bool isLoading = true;

        protected override async Task OnParametersSetAsync()
        {
            await LoadCoursesAsync();
        }

        private async Task LoadCoursesAsync()
        {
            try
            {
                isLoading = true;
                StateHasChanged();

                var response = await AdminInvoiceService.GetMemberInvoicesWithItemsAsync(UserId.ToString());

                if (response?.Success == true && response.Data != null)
                {
                    // Filter only courses from invoice items with invoice date info
                    courses = new List<CourseItemDisplay>();
                    foreach (var invoice in response.Data)
                    {
                        if (invoice.Items != null)
                        {
                            var courseItems = invoice.Items
                            .Where(item => item.Scope == PromoCodeScope.Course)
                                 .ToList();

                            foreach (var item in courseItems)
                            {
                                courses.Add(new CourseItemDisplay
                                {
                                    Id = item.ItemId,
                                    ItemNameAr = item.NameAr,
                                    ItemNameEn = item.NameEn,
                                    Quantity = item.Quantity,
                                    UnitPrice = item.UnitPrice,
                                    PaymentValue = item.PaymentValue,
                                    InvoiceDateUtc = invoice.InvoiceDateUtc
                                });
                            }
                        }
                    }

                    // Remove duplicates based on ItemNameEn and InvoiceDateUtc
                    courses = courses.DistinctBy(c => new { c.ItemNameEn, c.InvoiceDateUtc }).ToList();
                }
                else
                {
                    courses = new List<CourseItemDisplay>();
                }
            }
            catch (Exception ex)
            {
                courses = new List<CourseItemDisplay>();
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }
    }
}
