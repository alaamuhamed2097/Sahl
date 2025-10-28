using Common.Enumerations.User;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shared.DTOs.User.AccountType;
using Shared.GeneralModels.ResultModels;
using UI.Contracts.AccountType;
using UI.Contracts.User.Marketer;
using UI.Providers;

namespace UI.Pages.General
{
    public partial class Home : IDisposable
    {
        protected IEnumerable<AccountTypeDto>? accountTypes;
        protected bool gotTargetAccountType = false;
        protected MarketerPlanResult marketerPlan = new();

        // Lazy loading state
        private bool isInitialLoading = true;
        private bool loadMarketerWidget = false;
        private bool loadAdminWidget = false;
        private bool shouldShowMarketerStats = true;
        private UserStateType userState;
        private CancellationTokenSource? cancellationTokenSource;

        [Inject] protected IMarketerAccountTypeService MarketerAccountTypeService { get; set; } = null!;
        [Inject] protected IMarketerService MarketerService { get; set; } = null!;
        [Inject] protected IAccountTypeService AccountTypeService { get; set; } = null!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        [Inject] protected IJSRuntime JS { get; set; } = null!;
        [Inject] protected CookieAuthenticationStateProvider CookieAuthenticationStateProvider { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            cancellationTokenSource = new CancellationTokenSource();

            // Start fast initial loading
            await Task.Delay(100); // Brief delay to show skeleton

            try
            {
                // Load critical data in parallel
                var tasks = new List<Task>
                {
                    GetMarketerTargetAccountTypeAsync(),
                    LoadAccountTypesAsync(),
                    LoadUserStateAsync()
                };

                await Task.WhenAll(tasks);

                if(userState == UserStateType.Suspended)
                {
                    await LogOut();
                    return;
                }

                // Perform navigation check after data load
                if ((!gotTargetAccountType && shouldShowMarketerStats) || userState == UserStateType.Restricted)
                {
                    Navigation.NavigateTo("/marketerPlan");
                    return;
                }
            }
            catch (OperationCanceledException)
            {
                // Operation was cancelled
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during initialization: {ex.Message}");
                // Continue with default values
            }
            finally
            {
                isInitialLoading = false;
                StateHasChanged();

                // Auto-load components after a short delay for better UX
                _ = Task.Delay(500, cancellationTokenSource.Token).ContinueWith(async _ =>
                {
                    if (!cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        await InvokeAsync(() => AutoLoadWidgets());
                    }
                }, TaskScheduler.Default);
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && !isInitialLoading)
            {
                // Preload critical resources
                try
                {
                    await JS.InvokeVoidAsync("preloadCriticalResources");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error preloading resources: {ex.Message}");
                }
            }
        }

        private async Task GetMarketerTargetAccountTypeAsync()
        {
            try
            {
                var result = await MarketerAccountTypeService.GetMarketerTargetAccountType();
                if (result.Success && result.Data != null)
                {
                    marketerPlan = result.Data;
                    gotTargetAccountType = marketerPlan.CurrentPVs >= marketerPlan.TargetAccountTypePVs;

                    Console.WriteLine($"Marketer Plan - Current PVs: {marketerPlan.CurrentPVs}, Target PVs: {marketerPlan.TargetAccountTypePVs}, Got Target: {gotTargetAccountType}");
                }
                else
                {
                    // If we can't get the marketer plan, default to showing statistics
                    Console.WriteLine($"Failed to get marketer plan: {result.Message}");
                    gotTargetAccountType = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting marketer target account type: {ex.Message}");
                // On error, default to showing statistics
                gotTargetAccountType = true;
            }
        }

        protected async Task LoadAccountTypesAsync()
        {
            try
            {
                var result = await AccountTypeService.GetAllAsync();
                if (result.Success)
                {
                    accountTypes = result.Data;
                }
                else
                {
                    accountTypes = Enumerable.Empty<AccountTypeDto>();
                    Console.WriteLine($"Failed to load account types: {result.Message}");
                }
            }
            catch (Exception ex)
            {
                accountTypes = Enumerable.Empty<AccountTypeDto>();
                Console.WriteLine($"Error loading account types: {ex.Message}");
            }
        }

        protected async Task LoadUserStateAsync()
        {
            try
            {
                var result = await MarketerService.GetUserStatus();
                if (result.Success)
                {
                    userState = result.Data;
                }
                else
                {
                    accountTypes = Enumerable.Empty<AccountTypeDto>();
                    Console.WriteLine($"Failed to load account types: {result.Message}");
                }
            }
            catch (Exception ex)
            {
                accountTypes = Enumerable.Empty<AccountTypeDto>();
                Console.WriteLine($"Error loading account types: {ex.Message}");
            }
        }

        private async Task AutoLoadWidgets()
        {
            // Check if user is still on the page
            if (cancellationTokenSource?.Token.IsCancellationRequested == true)
                return;

            // Auto-load based on user role and preferences
            // You could implement user preference checking here
            var shouldAutoLoad = await ShouldAutoLoadWidgets();

            if (shouldAutoLoad)
            {
                await LoadMarketerWidget();
                await LoadAdminWidget();
            }
        }

        private async Task<bool> ShouldAutoLoadWidgets()
        {
            try
            {
                // Check if user prefers auto-loading (you can store this in localStorage or user settings)
                var autoLoad = await JS.InvokeAsync<bool>("getAutoLoadPreference");
                return autoLoad;
            }
            catch
            {
                // Default to auto-load for better UX
                return true;
            }
        }

        protected async Task LoadMarketerWidget()
        {
            if (!loadMarketerWidget && shouldShowMarketerStats && gotTargetAccountType)
            {
                loadMarketerWidget = true;
                StateHasChanged();
            }
        }

        protected async Task LoadAdminWidget()
        {
            if (!loadAdminWidget)
            {
                loadAdminWidget = true;
                StateHasChanged();

                // Preload any required dependencies
                //try
                //{
                //    await JS.InvokeVoidAsync("preloadAdminResources");
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine($"Error preloading admin widget resources: {ex.Message}");
                //}
            }
        }

        protected async Task LogOut()
        {
            await CookieAuthenticationStateProvider.MarkUserAsLoggedOut();
            Navigation.NavigateTo("/login", true);
        }

        public void Dispose()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
        }
    }
}
