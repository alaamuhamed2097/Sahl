using Common.Enumeration;
using Dashboard.Contracts.Course;
using Dashboard.Contracts.Currency;
using Dashboard.Contracts.Diploma;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Package;
using Dashboard.Contracts.Payment;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Category;
using Shared.DTOs.Course;
using Shared.DTOs.Currency;
using Shared.DTOs.General;
using Shared.DTOs.Package;
using Shared.DTOs.Payment;

namespace Dashboard.Pages.User.Student.StudentDetails.Modals
{
    public partial class AddInvoice
    {
        [Parameter] public Guid UserId { get; set; }

        [Inject] private ICourseService CourseService { get; set; } = null!;
        [Inject] private IAdminInvoiceService AdminInvoiceService { get; set; } = null!;
        [Inject] private IJSRuntime JS { get; set; } = null!;
        [Inject] private NavigationManager Manager { get; set; } = null!;
        [Inject] private IDiplomaService DiplomaService { get; set; } = null!;
        [Inject] private IPackageService PackageService { get; set; } = null!;
        [Inject] private IPaymentMethodService PaymentMethodService { get; set; } = null!;
        [Inject] private IAdvertisingMethodService AdvertisingMethodService { get; set; } = null!;
        [Inject] private ICurrencyService CurrencyService { get; set; } = null!;

        // Model for form binding
        private AdminManualInvoiceRequest model = new()
        {
            CurrencyCode = "EGP",
            InvoiceDateUtc = DateTime.UtcNow,
            Items = new()
        };

        private bool isLoading = true;
        private bool isSubmitting = false;

        private string searchTerm = string.Empty;
        private List<AdminCoursesWithMultiplePricesDto> allCourses = new();
        private List<AdminCoursesWithMultiplePricesDto> filteredCourses = new();
        // Temporary storage for courses added to cart (removed from filteredCourses)
        private List<AdminCoursesWithMultiplePricesDto> tempCoursesInCart = new();

        private List<CartItemVm> cart = new();
        private List<PaymentMethodVm> paymentMethods = new();
        private List<AdvertisingMethodVm> advertisingMethods = new();

        // Currencies
        private List<CurrencyDto> availableCurrencies = new();
        private CurrencyDto? oldCurrency;
        private CurrencyDto? newCurrency;

        // Packages / Diplomas
        private string activeTab = "courses"; // courses | packages
        private List<CategoryDto> diplomas = new();
        private string? selectedDiplomaId;
        private List<AdminPackagesWithMultiplePricesDto> packages = new();
        // Temporary storage for packages added to cart (removed from packages)
        private List<AdminPackagesWithMultiplePricesDto> tempPackagesInCart = new();
        private bool isLoadingPackages = false;

        // Local datetime for binding (converts to/from UTC in model)
        private DateTime? invoiceDateLocal
        {
            get => model.InvoiceDateUtc.ToLocalTime();
            set => model.InvoiceDateUtc = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Local).ToUniversalTime() : DateTime.UtcNow;
        }

        protected override async Task OnParametersSetAsync()
        {
            // Initialize model with UserId
            model.UserId = UserId.ToString();

            var Task1 = LoadCoursesAsync();
            var Task2 = LoadPaymentMethodsAsync();
            var Task3 = LoadDiplomasAsync();
            var Task4 = LoadAdvertisingMethodsAsync();
            var Task5 = LoadCurrenciesAsync();

            await Task.WhenAll(Task1, Task2, Task3, Task4, Task5);
        }

        private async Task OnDiplomaChanged(ChangeEventArgs e)
        {
            selectedDiplomaId = e.Value?.ToString();
            await LoadPackagesAsync();
        }

        private void SetTab(string tab)
        {
            activeTab = tab;
            StateHasChanged();
        }

        private async Task LoadPaymentMethodsAsync()
        {
            try
            {
                var res = await PaymentMethodService.GetAllAsync();
                if (res != null && res.Success && res.Data != null)
                {
                    paymentMethods = res.Data;
                }
            }
            catch
            {
                // ignore, UI will show placeholder
                paymentMethods = new();
            }
        }

        private async Task LoadAdvertisingMethodsAsync()
        {
            try
            {
                var res = await AdvertisingMethodService.GetAllAsync();
                if (res != null && res.Success && res.Data != null)
                {
                    advertisingMethods = res.Data;
                }
            }
            catch
            {
                // ignore, UI will show placeholder
                advertisingMethods = new();
            }
        }

        private async Task LoadDiplomasAsync()
        {
            try
            {
                var res = await DiplomaService.GetAllAsync();
                if (res != null && res.Success && res.Data != null)
                {
                    diplomas = res.Data.ToList();
                }
            }
            catch
            {
                // ignore
                diplomas = new();
            }
        }

        private async Task LoadCurrenciesAsync()
        {
            try
            {
                var res = await CurrencyService.GetAllAsync();
                if (res != null && res.Success && res.Data != null)
                {
                    availableCurrencies = res.Data.ToList();
                    oldCurrency = availableCurrencies.FirstOrDefault(c => c.Code == model.CurrencyCode);
                }
            }
            catch
            {
                availableCurrencies = new();
            }
        }

        private async Task LoadPackagesAsync()
        {
            packages = new();
            tempPackagesInCart = new();
            if (string.IsNullOrWhiteSpace(selectedDiplomaId))
            {
                StateHasChanged();
                return;
            }

            try
            {
                isLoadingPackages = true;
                if (Guid.TryParse(selectedDiplomaId, out var id))
                {
                    var res = await PackageService.GetAllWithMultiplePricesAsync(id);
                    if (res != null && res.Success && res.Data != null)
                    {
                        packages = res.Data.ToList();
                        // Remove packages that are already in cart
                        RemoveCartItemsFromPackagesList();
                    }
                }
            }
            catch
            {
                packages = new();
            }
            finally
            {
                isLoadingPackages = false;
                StateHasChanged();
            }
        }

        private async Task LoadCoursesAsync()
        {
            try
            {
                isLoading = true;
                var res = await CourseService.GetForPaymentAsync();
                if (res != null && res.Success && res.Data != null)
                {
                    allCourses = res.Data.ToList();
                    ApplyFilter();
                }
                else
                {
                    allCourses = new();
                    filteredCourses = new();
                }
            }
            catch (Exception ex)
            {
                await ShowError(ValidationResources.Error ?? "خطأ", ex.Message);
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        private void ApplyFilter()
        {
            var term = (searchTerm ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(term))
            {
                filteredCourses = allCourses
                    .OrderBy(c => c.DisplayOrder)
                    .Take(200)
                    .ToList();
            }
            else
            {
                filteredCourses = allCourses
                    .Where(c => (c.TitleAr ?? string.Empty).Contains(term, StringComparison.OrdinalIgnoreCase)
                             || (c.TitleEn ?? string.Empty).Contains(term, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(c => c.DisplayOrder)
                    .Take(200)
                    .ToList();
            }

            // Remove cart items from filtered list
            RemoveCartItemsFromFilteredCourses();
        }

        /// <summary>
        /// Remove courses that are already in cart from the filtered list
        /// </summary>
        private void RemoveCartItemsFromFilteredCourses()
        {
            foreach (var cartItem in cart.Where(x => x.Scope == PromoCodeScope.Course))
            {
                filteredCourses.RemoveAll(c => c.CourseId == cartItem.Id);
            }
        }

        /// <summary>
        /// Remove packages that are already in cart from the packages list
        /// </summary>
        private void RemoveCartItemsFromPackagesList()
        {
            foreach (var cartItem in cart.Where(x => x.Scope == PromoCodeScope.Package))
            {
                packages.RemoveAll(p => p.PackageId == cartItem.Id);
            }
        }

        private void AddCourseToCart(AdminCoursesWithMultiplePricesDto c)
        {
            var existing = cart.FirstOrDefault(x => x.Id == c.CourseId);
            if (existing != null)
            {
                // Remove from cart (move back to filtered list)
                cart.RemoveAll(x => x.Id == c.CourseId);
                var courseToRestore = tempCoursesInCart.FirstOrDefault(x => x.CourseId == c.CourseId);
                if (courseToRestore != null)
                {
                    tempCoursesInCart.Remove(courseToRestore);
                    filteredCourses.Add(courseToRestore);
                    // Re-apply filter to maintain order
                    ApplyFilter();
                }
            }
            else
            {
                // Add to cart (move from filtered list)
                var courseToAdd = filteredCourses.FirstOrDefault(x => x.CourseId == c.CourseId);
                if (courseToAdd != null)
                {
                    filteredCourses.Remove(courseToAdd);
                    tempCoursesInCart.Add(courseToAdd);

                    cart.Add(new CartItemVm
                    {
                        Id = c.CourseId,
                        Name = c.TitleAr ?? c.TitleEn ?? "Course",
                        Quantity = 1,
                        Scope = PromoCodeScope.Course,
                        UnitPrice = Math.Round(c.Prices.First(p => p.Currency == model.CurrencyCode).SalesPrice, 2)
                    });
                }
            }

            SyncCartToModel();
        }

        private void RemoveFromCart(Guid id)
        {
            var cartItem = cart.FirstOrDefault(x => x.Id == id);
            if (cartItem == null) return;

            cart.RemoveAll(x => x.Id == id);

            // Restore to appropriate list
            if (cartItem.Scope == PromoCodeScope.Course)
            {
                var courseToRestore = tempCoursesInCart.FirstOrDefault(x => x.CourseId == id);
                if (courseToRestore != null)
                {
                    tempCoursesInCart.Remove(courseToRestore);
                    filteredCourses.Add(courseToRestore);
                    // Re-apply filter to maintain order
                    ApplyFilter();
                }
            }
            else if (cartItem.Scope == PromoCodeScope.Package)
            {
                var packageToRestore = tempPackagesInCart.FirstOrDefault(x => x.PackageId == id);
                if (packageToRestore != null)
                {
                    tempPackagesInCart.Remove(packageToRestore);
                    packages.Add(packageToRestore);
                }
            }

            SyncCartToModel();
        }

        private void AddPackageToCart(AdminPackagesWithMultiplePricesDto p)
        {
            var packageId = p?.PackageId ?? Guid.Empty;
            if (packageId == Guid.Empty) return;

            var existing = cart.FirstOrDefault(x => x.Id == packageId);
            if (existing != null)
            {
                // Remove from cart (move back to packages list)
                cart.RemoveAll(x => x.Id == packageId);
                var packageToRestore = tempPackagesInCart.FirstOrDefault(x => x.PackageId == packageId);
                if (packageToRestore != null)
                {
                    tempPackagesInCart.Remove(packageToRestore);
                    packages.Add(packageToRestore);
                }
            }
            else
            {
                // Add to cart (move from packages list)
                var packageToAdd = packages.FirstOrDefault(x => x.PackageId == packageId);
                if (packageToAdd != null)
                {
                    packages.Remove(packageToAdd);
                    tempPackagesInCart.Add(packageToAdd);

                    cart.Add(new CartItemVm
                    {
                        Id = packageId,
                        Name = p.TitleAr ?? p.TitleEn ?? "Package",
                        Quantity = 1,
                        Scope = PromoCodeScope.Package,
                        UnitPrice = Math.Round(p.Prices.First(p => p.Currency == model.CurrencyCode).Price, 2)
                    });
                }
            }

            SyncCartToModel();
        }

        private void SyncCartToModel()
        {
            // Update model items from cart
            model.Items = cart.Select(x => new AdminCheckoutCartItemDto
            {
                Id = x.Id,
                Quantity = Math.Max(1, x.Quantity),
                Scope = x.Scope,
                UnitPrice = x.UnitPrice
            }).ToList();

            // Calculate total
            model.FinalTotalLocal = Math.Round(
                cart.Sum(x => x.UnitPrice * Math.Max(1, x.Quantity)),
                2,
                MidpointRounding.AwayFromZero
            );

            StateHasChanged();
        }

        /// <summary>
        /// Distributes the FinalTotalLocal proportionally across cart items based on their current weights
        /// </summary>
        private void DistributeTotalProportionally()
        {
            if (cart.Count == 0 || model.FinalTotalLocal < 0)
                return;

            // Calculate current total and proportions
            var currentTotal = cart.Sum(x => x.UnitPrice * Math.Max(1, x.Quantity));

            if (currentTotal == 0)
                return;

            // Distribute the new total proportionally
            var remainingTotal = model.FinalTotalLocal;

            for (int i = 0; i < cart.Count; i++)
            {
                var item = cart[i];
                var quantity = Math.Max(1, item.Quantity);
                var itemTotal = item.UnitPrice * quantity;
                var proportion = itemTotal / currentTotal;

                if (i == cart.Count - 1)
                {
                    // Last item gets the remainder to avoid rounding errors
                    item.UnitPrice = Math.Round(remainingTotal / quantity, 2);
                }
                else
                {
                    var newItemTotal = Math.Round(model.FinalTotalLocal * proportion, 2);
                    item.UnitPrice = Math.Round(newItemTotal / quantity, 2);
                    remainingTotal -= newItemTotal;
                }
            }

            // Update model items
            model.Items = cart.Select(x => new AdminCheckoutCartItemDto
            {
                Id = x.Id,
                Quantity = Math.Max(1, x.Quantity),
                Scope = x.Scope,
                UnitPrice = x.UnitPrice
            }).ToList();

            StateHasChanged();
        }

        private void OnCurrencyChangedAfter()
        {
            // Recalculate cart prices based on new currency for all cart items
            foreach (var item in cart)
            {
                var course = tempCoursesInCart.FirstOrDefault(c => c.CourseId == item.Id);
                if (course != null)
                {
                    var priceInfo = course.Prices.FirstOrDefault(p => p.Currency == model.CurrencyCode);
                    if (priceInfo != null)
                    {
                        item.UnitPrice = Math.Round(priceInfo.SalesPrice, 2);
                    }
                }
                else
                {
                    var package = tempPackagesInCart.FirstOrDefault(p => p.PackageId == item.Id);
                    if (package != null)
                    {
                        var priceInfo = package.Prices?.FirstOrDefault(p => p.Currency == model.CurrencyCode);
                        if (priceInfo != null)
                        {
                            item.UnitPrice = Math.Round(priceInfo.Price, 2);
                        }
                    }
                }
            }

            SyncCartToModel();
        }

        private async Task SubmitManualInvoice()
        {
            if (isSubmitting) return;
            if (cart.Count == 0)
            {
                await ShowError(NotifiAndAlertsResources.Alert, "السلة فارغة");
                return;
            }

            try
            {
                isSubmitting = true;

                // Ensure model is synced
                SyncCartToModel();

                // Normalize currency code
                model.CurrencyCode = model.CurrencyCode?.Trim().ToUpperInvariant() ?? "USD";

                var result = await AdminInvoiceService.CreateAsync(model);

                if (result != null && result.Success)
                {
                    await ShowSuccess(NotifiAndAlertsResources.Success, "تم إنشاء الفاتورة بنجاح");

                    // Reset form
                    cart.Clear();
                    model = new AdminManualInvoiceRequest
                    {
                        UserId = UserId.ToString(),
                        CurrencyCode = "EGP",
                        InvoiceDateUtc = DateTime.UtcNow,
                        Items = new()
                    };

                    Manager.NavigateTo($"student/{UserId}");
                }
                else
                {
                    var msg = result?.Message ?? "فشل إنشاء الفاتورة";
                    await ShowError(ValidationResources.Failed, msg);
                }
            }
            catch (Exception ex)
            {
                await ShowError(ValidationResources.Error, ex.Message);
            }
            finally
            {
                isSubmitting = false;
                StateHasChanged();
            }
        }

        private Task ShowSuccess(string title, string message)
        {
            var options = new { title, text = message, icon = "success" };
            return JS.InvokeVoidAsync("swal", options).AsTask();
        }

        private Task ShowError(string title, string message)
        {
            var options = new { title, text = message, icon = "error" };
            return JS.InvokeVoidAsync("swal", options).AsTask();
        }
    }
}
