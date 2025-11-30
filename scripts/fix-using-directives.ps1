# Fix all using directives for new entities

$fixes = @{
    "src/Core/Domains/Entities/BuyBox/TbBuyBoxHistory.cs" = @"
using Domains.Entities.Base;
using Domains.Entities.Catalog.Item;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Entities.Offer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
"@
    
    "src/Core/Domains/Entities/BuyBox/TbSellerPerformanceMetrics.cs" = @"
using Domains.Entities.Base;
using Domains.Entities.ECommerceSystem.Vendor;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
"@
    
    "src/Core/Domains/Entities/Wallet/TbCustomerWallet.cs" = @"
using Common.Enumerations.Wallet;
using Domains.Entities.Base;
using Domains.Entities.Currency;
using Domains.Entities.ECommerceSystem.Customer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
"@
    
    "src/Core/Domains/Entities/Wallet/TbVendorWallet.cs" = @"
using Common.Enumerations.Wallet;
using Domains.Entities.Base;
using Domains.Entities.Currency;
using Domains.Entities.ECommerceSystem.Vendor;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Column;
"@
    
    "src/Core/Domains/Entities/Wallet/TbPlatformTreasury.cs" = @"
using Domains.Entities.Base;
using Domains.Entities.Currency;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
"@
    
    "src/Core/Domains/Entities/Loyalty/TbCustomerLoyalty.cs" = @"
using Domains.Entities.Base;
using Domains.Entities.ECommerceSystem.Customer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
"@
    
    "src/Core/Domains/Entities/Loyalty/TbLoyaltyPointsTransaction.cs" = @"
using Common.Enumerations.Loyalty;
using Domains.Entities.Base;
using Domains.Entities.ECommerceSystem.Review;
using Domains.Entities.Order;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
"@
    
    "src/Core/Domains/Entities/SellerRequest/TbSellerRequest.cs" = @"
using Common.Enumerations.SellerRequest;
using Domains.Entities.Base;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
"@
    
    "src/Core/Domains/Entities/Campaign/TbCampaignProduct.cs" = @"
using Domains.Entities.Base;
using Domains.Entities.Catalog.Item;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
"@
    
    "src/Core/Domains/Entities/Campaign/TbCampaignVendor.cs" = @"
using Domains.Entities.Base;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
"@
    
    "src/Core/Domains/Entities/Campaign/TbFlashSaleProduct.cs" = @"
using Domains.Entities.Base;
using Domains.Entities.Catalog.Item;
using Domains.Entities.ECommerceSystem.Vendor;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
"@
    
    "src/Core/Domains/Entities/Fulfillment/TbFBMInventory.cs" = @"
using Domains.Entities.Base;
using Domains.Entities.Catalog.Item;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Entities.Warehouse;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
"@
    
    "src/Core/Domains/Entities/Fulfillment/TbFBMShipment.cs" = @"
using Common.Enumerations.Fulfillment;
using Domains.Entities.Base;
using Domains.Entities.Order;
using Domains.Entities.Shipping;
using Domains.Entities.Warehouse;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
"@
    
    "src/Core/Domains/Entities/Pricing/TbQuantityPricing.cs" = @"
using Domains.Entities.Base;
using Domains.Entities.Offer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
"@
    
    "src/Core/Domains/Entities/Pricing/TbCustomerSegmentPricing.cs" = @"
using Common.Enumerations.Pricing;
using Domains.Entities.Base;
using Domains.Entities.Offer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
"@
    
    "src/Core/Domains/Entities/Pricing/TbPriceHistory.cs" = @"
using Domains.Entities.Base;
using Domains.Entities.Offer;
using Domains.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
"@
    
    "src/Core/Domains/Entities/Merchandising/TbBlockProduct.cs" = @"
using Domains.Entities.Base;
using Domains.Entities.Catalog.Item;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
"@
    
    "src/Core/Domains/Entities/SellerTier/TbVendorTierHistory.cs" = @"
using Domains.Entities.Base;
using Domains.Entities.ECommerceSystem.Vendor;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
"@
    
    "src/Core/Domains/Entities/Visibility/TbProductVisibilityRule.cs" = @"
using Common.Enumerations.Visibility;
using Domains.Entities.Base;
using Domains.Entities.Catalog.Item;
using Domains.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
"@
    
    "src/Core/Domains/Entities/Visibility/TbVisibilityLog.cs" = @"
using Domains.Entities.Base;
using Domains.Entities.Catalog.Item;
using Domains.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
"@
    
    "src/Core/Domains/Entities/BrandManagement/TbBrandRegistrationRequest.cs" = @"
using Common.Enumerations.Brand;
using Domains.Entities.Base;
using Domains.Entities.Catalog.Brand;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
"@
    
    "src/Core/Domains/Entities/BrandManagement/TbAuthorizedDistributor.cs" = @"
using Domains.Entities.Base;
using Domains.Entities.Catalog.Brand;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
"@
}

Write-Host "This script will help fix using directives. Apply these changes manually to each file." -ForegroundColor Green
Write-Host ""
foreach ($file in $fixes.Keys) {
    Write-Host "File: $file" -ForegroundColor Yellow
    Write-Host $fixes[$file]
    Write-Host ""
}
