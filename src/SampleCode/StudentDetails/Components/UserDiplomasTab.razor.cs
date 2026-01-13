using Common.Enumeration;
using Dashboard.Configuration;
using Dashboard.Contracts.Payment;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace Dashboard.Pages.User.UserDetails.Components
{
    public partial class UserDiplomasTab
    {
        [Parameter] public Guid UserId { get; set; }

        [Inject] protected IAdminInvoiceService AdminInvoiceService { get; set; } = null!;
        [Inject] protected IOptions<ApiSettings> ApiOptions { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        // DTO to hold combined invoice and item data
        private class PackageItemDisplay
        {
            public Guid Id { get; set; }
            public string ItemNameEn { get; set; }
            public string SubItemNameEn { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal PaymentValue { get; set; }
            public DateTime InvoiceDateUtc { get; set; }
        }

        private List<PackageItemDisplay>? packages;
        private bool isLoading = true;

        protected override async Task OnParametersSetAsync()
        {
            await LoadPackagesAsync();
        }

        private async Task LoadPackagesAsync()
        {
            try
            {
                isLoading = true;
                StateHasChanged();

                var response = await AdminInvoiceService.GetMemberInvoicesWithItemsAsync(UserId.ToString());

                if (response?.Success == true && response.Data != null)
                {
                    // Filter only packages from invoice items with invoice date info
                    packages = new List<PackageItemDisplay>();
                    foreach (var invoice in response.Data)
                    {
                        if (invoice.Items != null)
                        {
                            var packageItems = invoice.Items
                                .Where(item => item.Scope == PromoCodeScope.Package)
                                .ToList();

                            foreach (var item in packageItems)
                            {
                                packages.Add(new PackageItemDisplay
                                {
                                    Id = item.ItemId,
                                    ItemNameEn = item.NameEn,
                                    SubItemNameEn = item.SubNameEn,
                                    Quantity = item.Quantity,
                                    UnitPrice = item.UnitPrice,
                                    PaymentValue = item.PaymentValue,
                                    InvoiceDateUtc = invoice.InvoiceDateUtc
                                });
                            }
                        }
                    }

                    // Remove duplicates based on ItemNameEn and InvoiceDateUtc
                    packages = packages.DistinctBy(p => new { p.ItemNameEn, p.InvoiceDateUtc }).ToList();
                }
                else
                {
                    packages = new List<PackageItemDisplay>();
                }
            }
            catch (Exception ex)
            {
                packages = new List<PackageItemDisplay>();
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }
    }
}
