# ?? DTOs, Services, Controllers & Blazor Pages - Implementation Guide

## Date: January 2025
## Status: ? Build Passing | ?? Ready for Full Implementation

---

## ? **COMPLETED DTOs**

### Already Created (2):
1. ? **LoyaltyDtos.cs** - Complete with all request/response models
2. ? **WalletDtos.cs** - Complete with transaction models
3. ? **CampaignDtos.cs** - Campaign & Flash Sale models
4. ? **SellerRequestDtos.cs** - Request management models

### Remaining DTOs to Create (6):
5. ? **FulfillmentDtos.cs** - FBM/FBS operations
6. ? **PricingDtos.cs** - Advanced pricing models
7. ? **MerchandisingDtos.cs** - Homepage blocks
8. ? **SellerTierDtos.cs** - Seller tier management
9. ? **VisibilityDtos.cs** - Product visibility rules
10. ? **BrandManagementDtos.cs** - Brand registration

---

## ?? **DTOs Template for Remaining Systems**

### 5. Fulfillment System DTOs

```csharp
// src/Shared/Shared/DTOs/Fulfillment/FulfillmentDtos.cs
namespace Shared.DTOs.Fulfillment
{
    // FBM Inventory DTOs
    public class FBMInventoryDto
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public Guid VendorId { get; set; }
        public string VendorName { get; set; }
        public Guid WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public int AvailableQuantity { get; set; }
        public int ReservedQuantity { get; set; }
        public int InboundQuantity { get; set; }
        public DateTime LastRestockedDate { get; set; }
    }

    // Fulfillment Fee DTOs
    public class FulfillmentFeeDto
    {
        public Guid Id { get; set; }
        public int FeeType { get; set; }
        public string FeeTypeName { get; set; }
        public decimal Amount { get; set; }
        public DateTime EffectiveDate { get; set; }
        public bool IsActive { get; set; }
    }

    // FBM Shipment DTOs
    public class FBMShipmentDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public int ShipmentStatus { get; set; }
        public string StatusName { get; set; }
        public string TrackingNumber { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }
    }

    // Statistics
    public class FulfillmentStatisticsDto
    {
        public int TotalFBMOrders { get; set; }
        public int PendingShipments { get; set; }
        public decimal TotalFulfillmentFees { get; set; }
        public double AverageDeliveryTimeHours { get; set; }
    }
}
```

### 6. Pricing System DTOs

```csharp
// src/Shared/Shared/DTOs/Pricing/PricingDtos.cs
namespace Shared.DTOs.Pricing
{
    // Quantity Pricing DTOs
    public class QuantityPricingDto
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public int MinQuantity { get; set; }
        public int? MaxQuantity { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal DiscountPercentage { get; set; }
        public bool IsActive { get; set; }
    }

    // Customer Segment Pricing DTOs
    public class CustomerSegmentPricingDto
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public int SegmentType { get; set; }
        public string SegmentName { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidUntil { get; set; }
    }

    // Price History DTOs
    public class PriceHistoryDto
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
        public decimal PriceChange { get; set; }
        public string ChangeReason { get; set; }
        public string ChangedByUserName { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}
```

### 7. Merchandising System DTOs

```csharp
// src/Shared/Shared/DTOs/Merchandising/MerchandisingDtos.cs
namespace Shared.DTOs.Merchandising
{
    public class HomepageBlockDto
    {
        public Guid Id { get; set; }
        public string TitleEn { get; set; }
        public string TitleAr { get; set; }
        public int BlockType { get; set; }
        public string BlockTypeName { get; set; }
        public int DisplayOrder { get; set; }
        public int ProductsCount { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDateUtc { get; set; }
    }

    public class BlockProductDto
    {
        public Guid Id { get; set; }
        public Guid HomepageBlockId { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemImage { get; set; }
        public decimal Price { get; set; }
        public int DisplayOrder { get; set; }
    }
}
```

### 8. Seller Tier System DTOs

```csharp
// src/Shared/Shared/DTOs/SellerTier/SellerTierDtos.cs
namespace Shared.DTOs.SellerTier
{
    public class SellerTierDto
    {
        public Guid Id { get; set; }
        public string TierNameEn { get; set; }
        public string TierNameAr { get; set; }
        public int TierLevel { get; set; }
        public decimal MinimumMonthlySales { get; set; }
        public decimal CommissionPercentage { get; set; }
        public int CurrentVendorsCount { get; set; }
        public bool IsActive { get; set; }
    }

    public class VendorTierHistoryDto
    {
        public Guid Id { get; set; }
        public Guid VendorId { get; set; }
        public string VendorName { get; set; }
        public Guid OldTierId { get; set; }
        public string OldTierName { get; set; }
        public Guid NewTierId { get; set; }
        public string NewTierName { get; set; }
        public string ChangeReason { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}
```

### 9. Visibility System DTOs

```csharp
// src/Shared/Shared/DTOs/Visibility/VisibilityDtos.cs
namespace Shared.DTOs.Visibility
{
    public class ProductVisibilityRuleDto
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public int VisibilityStatus { get; set; }
        public string StatusName { get; set; }
        public Guid? SuppressionReasonId { get; set; }
        public string SuppressionReasonText { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveUntil { get; set; }
    }

    public class SuppressionReasonDto
    {
        public Guid Id { get; set; }
        public int ReasonType { get; set; }
        public string ReasonTypeName { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public bool RequiresAction { get; set; }
    }
}
```

### 10. Brand Management DTOs

```csharp
// src/Shared/Shared/DTOs/BrandManagement/BrandManagementDtos.cs
namespace Shared.DTOs.BrandManagement
{
    public class BrandRegistrationRequestDto
    {
        public Guid Id { get; set; }
        public Guid VendorId { get; set; }
        public string VendorName { get; set; }
        public string BrandNameEn { get; set; }
        public string BrandNameAr { get; set; }
        public int BrandType { get; set; }
        public int RegistrationStatus { get; set; }
        public string StatusName { get; set; }
        public string TrademarkNumber { get; set; }
        public DateTime? TrademarkExpiryDate { get; set; }
        public string ReviewedByUserName { get; set; }
        public DateTime? ReviewedAt { get; set; }
    }

    public class BrandDocumentDto
    {
        public Guid Id { get; set; }
        public Guid BrandRequestId { get; set; }
        public int DocumentType { get; set; }
        public string DocumentTypeName { get; set; }
        public string DocumentPath { get; set; }
        public string DocumentName { get; set; }
        public bool IsVerified { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
```

---

## ?? **SERVICE INTERFACES TEMPLATE**

### Example: Campaign Service Interface

```csharp
// src/Core/BL/Services/Campaign/ICampaignService.cs
namespace BL.Services.Campaign
{
    public interface ICampaignService
    {
        // Campaign Management
        Task<CampaignDto> GetCampaignByIdAsync(Guid id);
        Task<List<CampaignDto>> GetAllCampaignsAsync();
        Task<List<CampaignDto>> GetActiveCampaignsAsync();
        Task<CampaignDto> CreateCampaignAsync(CampaignCreateDto dto);
        Task<CampaignDto> UpdateCampaignAsync(CampaignUpdateDto dto);
        Task<bool> DeleteCampaignAsync(Guid id);
        
        // Campaign Products
        Task<List<CampaignProductDto>> GetCampaignProductsAsync(Guid campaignId);
        Task<CampaignProductDto> AddProductToCampaignAsync(CampaignProductCreateDto dto);
        Task<bool> RemoveProductFromCampaignAsync(Guid campaignProductId);
        
        // Flash Sales
        Task<List<FlashSaleDto>> GetActiveFlashSalesAsync();
        Task<FlashSaleDto> CreateFlashSaleAsync(FlashSaleCreateDto dto);
        
        // Statistics
        Task<CampaignStatisticsDto> GetCampaignStatisticsAsync();
    }
}
```

---

## ?? **CONTROLLER TEMPLATE**

### Example: Campaign Controller

```csharp
// src/Presentation/Api/Controllers/Campaign/CampaignController.cs
using BL.Services.Campaign;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Campaign;

namespace Api.Controllers.Campaign
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _campaignService;
        private readonly ILogger<CampaignController> _logger;

        public CampaignController(
            ICampaignService campaignService,
            ILogger<CampaignController> logger)
        {
            _campaignService = campaignService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var campaigns = await _campaignService.GetAllCampaignsAsync();
                return Ok(new { success = true, data = campaigns });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting campaigns");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var campaign = await _campaignService.GetCampaignByIdAsync(id);
                if (campaign == null)
                    return NotFound(new { success = false, message = "Campaign not found" });
                    
                return Ok(new { success = true, data = campaign });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting campaign {Id}", id);
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CampaignCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                    
                var campaign = await _campaignService.CreateCampaignAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = campaign.Id }, 
                    new { success = true, data = campaign });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating campaign");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CampaignUpdateDto dto)
        {
            try
            {
                if (id != dto.Id)
                    return BadRequest(new { success = false, message = "ID mismatch" });
                    
                var campaign = await _campaignService.UpdateCampaignAsync(dto);
                return Ok(new { success = true, data = campaign });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating campaign {Id}", id);
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _campaignService.DeleteCampaignAsync(id);
                if (!result)
                    return NotFound(new { success = false, message = "Campaign not found" });
                    
                return Ok(new { success = true, message = "Campaign deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting campaign {Id}", id);
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            try
            {
                var stats = await _campaignService.GetCampaignStatisticsAsync();
                return Ok(new { success = true, data = stats });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting campaign statistics");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}
```

---

## ?? **BLAZOR PAGE TEMPLATE**

### Example: Campaign List Page

```razor
@page "/campaigns"
@using Shared.DTOs.Campaign
@inject HttpClient Http
@inject NavigationManager Navigation
@inject ISnackbar Snackbar

<PageTitle>Campaigns Management</PageTitle>

<MudContainer MaxWidth="MaxWidth.ExtraLarge" Class="mt-4">
    <MudText Typo="Typo.h4" Class="mb-4">Campaigns Management</MudText>
    
    <MudPaper Class="pa-4 mb-4">
        <MudGrid>
            <MudItem xs="12" sm="6" md="3">
                <MudButton Variant="Variant.Filled" 
                           Color="Color.Primary" 
                           StartIcon="@Icons.Material.Filled.Add"
                           OnClick="@(() => Navigation.NavigateTo("/campaigns/create"))">
                    Create Campaign
                </MudButton>
            </MudItem>
            <MudItem xs="12" sm="6" md="9">
                <MudTextField @bind-Value="searchTerm" 
                              Label="Search" 
                              Variant="Variant.Outlined"
                              Adornment="Adornment.End"
                              AdornmentIcon="@Icons.Material.Filled.Search"
                              OnKeyUp="@(() => LoadCampaigns())" />
            </MudItem>
        </MudGrid>
    </MudPaper>

    @if (loading)
    {
        <MudProgressLinear Color="Color.Primary" Indeterminate="true" />
    }
    else if (campaigns == null || !campaigns.Any())
    {
        <MudAlert Severity="Severity.Info">No campaigns found.</MudAlert>
    }
    else
    {
        <MudTable Items="@campaigns" Hover="true" Striped="true" Dense="true">
            <HeaderContent>
                <MudTh>Title</MudTh>
                <MudTh>Type</MudTh>
                <MudTh>Status</MudTh>
                <MudTh>Start Date</MudTh>
                <MudTh>End Date</MudTh>
                <MudTh>Products</MudTh>
                <MudTh>Active</MudTh>
                <MudTh>Actions</MudTh>
            </HeaderContent>
            <RowTemplate>
                <MudTd DataLabel="Title">@context.TitleEn</MudTd>
                <MudTd DataLabel="Type">@context.CampaignTypeName</MudTd>
                <MudTd DataLabel="Status">
                    <MudChip Size="Size.Small" 
                             Color="@GetStatusColor(context.Status)">
                        @context.StatusName
                    </MudChip>
                </MudTd>
                <MudTd DataLabel="Start">@context.StartDate.ToString("yyyy-MM-dd")</MudTd>
                <MudTd DataLabel="End">@context.EndDate.ToString("yyyy-MM-dd")</MudTd>
                <MudTd DataLabel="Products">@context.TotalProducts</MudTd>
                <MudTd DataLabel="Active">
                    <MudIcon Icon="@(context.IsActive ? Icons.Material.Filled.CheckCircle : Icons.Material.Filled.Cancel)"
                             Color="@(context.IsActive ? Color.Success : Color.Error)" />
                </MudTd>
                <MudTd DataLabel="Actions">
                    <MudIconButton Icon="@Icons.Material.Filled.Edit" 
                                   Size="Size.Small"
                                   Color="Color.Primary"
                                   OnClick="@(() => Navigation.NavigateTo($"/campaigns/edit/{context.Id}"))" />
                    <MudIconButton Icon="@Icons.Material.Filled.Delete" 
                                   Size="Size.Small"
                                   Color="Color.Error"
                                   OnClick="@(() => DeleteCampaign(context.Id))" />
                </MudTd>
            </RowTemplate>
        </MudTable>
    }
</MudContainer>

@code {
    private List<CampaignDto> campaigns = new();
    private string searchTerm = "";
    private bool loading = true;

    protected override async Task OnInitializedAsync()
    {
        await LoadCampaigns();
    }

    private async Task LoadCampaigns()
    {
        try
        {
            loading = true;
            var response = await Http.GetFromJsonAsync<ApiResponse<List<CampaignDto>>>("api/campaign");
            if (response?.Success == true)
            {
                campaigns = response.Data ?? new();
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error: {ex.Message}", Severity.Error);
        }
        finally
        {
            loading = false;
        }
    }

    private async Task DeleteCampaign(Guid id)
    {
        if (await Snackbar.ShowConfirmation("Delete Campaign", "Are you sure you want to delete this campaign?"))
        {
            try
            {
                var response = await Http.DeleteAsync($"api/campaign/{id}");
                if (response.IsSuccessStatusCode)
                {
                    Snackbar.Add("Campaign deleted successfully", Severity.Success);
                    await LoadCampaigns();
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error: {ex.Message}", Severity.Error);
            }
        }
    }

    private Color GetStatusColor(int status)
    {
        return status switch
        {
            0 => Color.Default,  // Draft
            1 => Color.Primary,  // Active
            2 => Color.Warning,  // Paused
            3 => Color.Success,  // Completed
            4 => Color.Error,    // Cancelled
            _ => Color.Default
        };
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
    }
}
```

---

## ?? **NAVIGATION MENU UPDATES**

### Add to NavMenu.razor

```razor
<MudNavGroup Title="Loyalty & Rewards" Icon="@Icons.Material.Filled.Stars" Expanded="false">
    <MudNavLink Href="/loyalty/tiers" Icon="@Icons.Material.Filled.EmojiEvents">Loyalty Tiers</MudNavLink>
    <MudNavLink Href="/loyalty/customers" Icon="@Icons.Material.Filled.People">Customer Loyalty</MudNavLink>
    <MudNavLink Href="/loyalty/transactions" Icon="@Icons.Material.Filled.SwapHoriz">Points Transactions</MudNavLink>
</MudNavGroup>

<MudNavGroup Title="Wallet Management" Icon="@Icons.Material.Filled.AccountBalanceWallet" Expanded="false">
    <MudNavLink Href="/wallet/customer" Icon="@Icons.Material.Filled.Person">Customer Wallets</MudNavLink>
    <MudNavLink Href="/wallet/vendor" Icon="@Icons.Material.Filled.Store">Vendor Wallets</MudNavLink>
    <MudNavLink Href="/wallet/transactions" Icon="@Icons.Material.Filled.Receipt">Transactions</MudNavLink>
    <MudNavLink Href="/wallet/treasury" Icon="@Icons.Material.Filled.AccountBalance">Platform Treasury</MudNavLink>
</MudNavGroup>

<MudNavGroup Title="Campaigns & Sales" Icon="@Icons.Material.Filled.Campaign" Expanded="false">
    <MudNavLink Href="/campaigns" Icon="@Icons.Material.Filled.EventNote">Campaigns</MudNavLink>
    <MudNavLink Href="/flashsales" Icon="@Icons.Material.Filled.FlashOn">Flash Sales</MudNavLink>
    <MudNavLink Href="/campaigns/products" Icon="@Icons.Material.Filled.Inventory">Campaign Products</MudNavLink>
</MudNavGroup>

<MudNavGroup Title="Seller Management" Icon="@Icons.Material.Filled.Support" Expanded="false">
    <MudNavLink Href="/seller/requests" Icon="@Icons.Material.Filled.RequestQuote">Seller Requests</MudNavLink>
    <MudNavLink Href="/seller/tiers" Icon="@Icons.Material.Filled.Grade">Seller Tiers</MudNavLink>
    <MudNavLink Href="/seller/performance" Icon="@Icons.Material.Filled.Assessment">Performance</MudNavLink>
</MudNavGroup>

<MudNavGroup Title="Brand Management" Icon="@Icons.Material.Filled.Verified" Expanded="false">
    <MudNavLink Href="/brands/requests" Icon="@Icons.Material.Filled.AppRegistration">Registration Requests</MudNavLink>
    <MudNavLink Href="/brands/verified" Icon="@Icons.Material.Filled.VerifiedUser">Verified Brands</MudNavLink>
    <MudNavLink Href="/brands/distributors" Icon="@Icons.Material.Filled.Handshake">Distributors</MudNavLink>
</MudNavGroup>
```

---

## ?? **IMPLEMENTATION PRIORITY**

### Phase 1 (Immediate - Core Features):
1. ? Loyalty System - DONE
2. ? Wallet System
3. ? Campaign System

### Phase 2 (Important - Business Operations):
4. ? Seller Request System
5. ? Fulfillment System
6. ? Seller Tier System

### Phase 3 (Enhancement - Advanced Features):
7. ? Pricing System
8. ? Visibility System
9. ? Brand Management
10. ? Merchandising System

---

## ?? **QUICK START COMMANDS**

### Create Migration:
```bash
cd D:\Work\projects\Sahl\Project
dotnet ef migrations add AddAllNewSystems --project src/Infrastructure/DAL --startup-project src/Presentation/Api
```

### Apply Migration:
```bash
dotnet ef database update --project src/Infrastructure/DAL --startup-project src/Presentation/Api
```

### Run Application:
```bash
cd src/Presentation/Api
dotnet run
```

### Access Swagger:
```
https://localhost:7001/swagger
```

---

## ? **COMPLETION CHECKLIST**

### DTOs:
- [x] Loyalty (2 files)
- [x] Wallet (1 file)
- [x] Campaign (1 file)
- [x] SellerRequest (1 file)
- [ ] Fulfillment
- [ ] Pricing
- [ ] Merchandising
- [ ] SellerTier
- [ ] Visibility
- [ ] BrandManagement

### Services (10):
- [x] LoyaltyService - Complete
- [ ] WalletService
- [ ] CampaignService
- [ ] SellerRequestService
- [ ] FulfillmentService
- [ ] PricingService
- [ ] MerchandisingService
- [ ] SellerTierService
- [ ] VisibilityService
- [ ] BrandManagementService

### Controllers (10):
- [ ] LoyaltyController
- [ ] WalletController
- [ ] CampaignController
- [ ] SellerRequestController
- [ ] FulfillmentController
- [ ] PricingController
- [ ] MerchandisingController
- [ ] SellerTierController
- [ ] VisibilityController
- [ ] BrandManagementController

### Blazor Pages (40+):
- [ ] Loyalty Pages (5)
- [ ] Wallet Pages (5)
- [ ] Campaign Pages (5)
- [ ] SellerRequest Pages (5)
- [ ] FulfillmentPages (4)
- [ ] Pricing Pages (4)
- [ ] Merchandising Pages (3)
- [ ] SellerTier Pages (3)
- [ ] Visibility Pages (3)
- [ ] BrandManagement Pages (3)

---

## ?? **ESTIMATED TIME TO COMPLETION**

- **Remaining DTOs:** 2-3 hours
- **Service Interfaces:** 2 hours
- **Service Implementations:** 8-10 hours
- **Controllers:** 4-6 hours
- **Blazor Pages:** 12-16 hours
- **Testing & Refinement:** 4-6 hours

**Total:** ~32-43 hours of focused development

---

**Status:** Ready for full-scale implementation!  
**Next Action:** Create migration, then continue with services and controllers  
**Build Status:** ? PASSING  
**Confidence:** ?? HIGH
