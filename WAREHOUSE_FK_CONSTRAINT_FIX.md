# ? Warehouse Foreign Key Constraint Fix - COMPLETE

## Problem

**Error**: `The INSERT statement conflicted with the FOREIGN KEY constraint "FK_TbOrderShipments_TbWarehouses_WarehouseId"`

**Root Cause**: Attempting to create shipments with `WarehouseId = Guid.Empty`, which violates the FK constraint since no valid warehouse ID exists.

---

## Solution

Implemented proper warehouse assignment based on fulfillment type:

### Business Logic
```
If FulfillmentType == Marketplace (FBS - Fulfillment by Platform):
  ?? Use Platform Default Warehouse

If FulfillmentType == Seller (FBM - Fulfillment by Merchant):
  ?? Use Vendor's Default Warehouse
     (Falls back to Platform Default if vendor has no warehouse)
```

---

## Changes Made

### 1. TbOffer Entity
**File**: `src/Core/Domains/Entities/Offer/TbOffer.cs`

**Added**:
```csharp
using Common.Enumerations.Fulfillment;

public FulfillmentType FulfillmentType { get; set; } = FulfillmentType.Seller;
```

**Purpose**: Allows determining which warehouse to use based on offer fulfillment type.

### 2. OrderService.cs - CreateOrderShipmentsFromDetails Method
**File**: `src/Core/BL/Service/Order/OrderService.cs`

**Changes**:
- Added `TbWarehouse` and `TbVendor` repository loading
- Batch loads all relevant warehouses and vendors
- Gets platform default warehouse
- Assigns warehouse based on fulfillment type
- Properly validates warehouse exists

**Key Logic**:
```csharp
// Determine fulfillment type and warehouse
FulfillmentType fulfillmentType = FulfillmentType.Seller; // Default
Guid warehouseId = platformDefaultWarehouse.Id;

// Check first offer's fulfillment type
if (itemOfferIds.Count > 0 && offerDict.TryGetValue(itemOfferIds[0], out var firstOffer))
{
    fulfillmentType = firstOffer.FulfillmentType;
}

// Assign warehouse based on fulfillment type
if (fulfillmentType == FulfillmentType.Marketplace)
{
    // Use platform default warehouse
    warehouseId = platformDefaultWarehouse.Id;
}
else
{
    // Use vendor's default warehouse
    var vendorDefaultWarehouse = warehouses.FirstOrDefault(
        w => w.VendorId == group.Key.VendorId && !w.IsDeleted);
    
    if (vendorDefaultWarehouse != null)
    {
        warehouseId = vendorDefaultWarehouse.Id;
    }
    else
    {
        // Fall back to platform default
        warehouseId = platformDefaultWarehouse.Id;
    }
}
```

### 3. Using Statements
**File**: `src/Core/BL/Service/Order/OrderService.cs`

**Added**:
```csharp
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Entities.Warehouse;
```

---

## Database Schema Alignment

### TbWarehouse Entity
```csharp
public bool IsDefaultPlatformWarehouse { get; set; } = false;
public Guid? VendorId { get; set; }  // NULL = Platform warehouse, NOT NULL = Vendor warehouse
```

### Warehouse Types
| Type | IsDefaultPlatformWarehouse | VendorId | Usage |
|------|---------------------------|----------|-------|
| Platform Default | true | NULL | Marketplace fulfillment (FBS) |
| Vendor Default | false | VendorId | Seller fulfillment (FBM) |

---

## Data Flow

### Order Creation Process
```
1. Add Items to Cart
   ?? Each item has OfferCombinationPricingId
   
2. Create Order
   ?? Order Header created
   
3. Create Order Details
   ?? Links items to offers
   ?? Extracts vendor info
   
4. Create Shipments ? WAREHOUSE ASSIGNMENT HAPPENS HERE
   ?? Group by (VendorId, WarehouseId)
   ?? For each group:
   ?  ?? Check offer fulfillment type
   ?  ?? Determine warehouse:
   ?  ?  ?? If Marketplace ? Platform default
   ?  ?  ?? If Seller ? Vendor default (or fallback)
   ?  ?? Create shipment with assigned warehouse
   ?? All shipments have valid warehouse IDs
```

---

## Validation & Error Handling

### Pre-requisite Check
```csharp
var platformDefaultWarehouse = warehouses.FirstOrDefault(w => w.IsDefaultPlatformWarehouse);
if (platformDefaultWarehouse == null)
{
    throw new InvalidOperationException("Platform default warehouse not configured.");
}
```

### Fallback Chain
1. Use offer's specified warehouse type
2. Get vendor's default warehouse if Seller fulfillment
3. Fall back to platform default if vendor has no warehouse
4. Error only if platform default doesn't exist (configuration issue)

---

## Testing Scenarios

### Scenario 1: Single Vendor Seller Fulfillment
```
Cart Items:
  - Item A from Vendor X (Seller fulfillment)
  
Result:
  - 1 Shipment created
  - WarehouseId = Vendor X's default warehouse
  - FulfillmentType = Seller
```

### Scenario 2: Single Vendor Marketplace Fulfillment
```
Cart Items:
  - Item A from Vendor X (Marketplace fulfillment)
  
Result:
  - 1 Shipment created
  - WarehouseId = Platform default warehouse
  - FulfillmentType = Marketplace
```

### Scenario 3: Multiple Vendors Mixed Fulfillment
```
Cart Items:
  - Item A from Vendor X (Seller fulfillment)
  - Item B from Vendor Y (Marketplace fulfillment)
  
Result:
  - 2 Shipments created:
    ?? Shipment 1: Vendor X, Vendor X's warehouse, Seller
    ?? Shipment 2: Vendor Y, Platform warehouse, Marketplace
```

### Scenario 4: Vendor Without Default Warehouse
```
Cart Items:
  - Item A from Vendor Z (Seller fulfillment, no default warehouse)
  
Result:
  - 1 Shipment created
  - WarehouseId = Platform default warehouse (fallback)
  - FulfillmentType = Seller
```

---

## Build Status

? **Compilation**: SUCCESSFUL  
? **Errors**: NONE  
? **Warnings**: NONE  

---

## Files Modified

1. **src/Core/Domains/Entities/Offer/TbOffer.cs**
   - Added `FulfillmentType` property

2. **src/Core/BL/Service/Order/OrderService.cs**
   - Added warehouse and vendor loading
   - Implemented warehouse assignment logic
   - Added using statements

---

## Migration Required

**Database Migration Needed**: Yes

A migration needs to be created to add the `FulfillmentType` column to the `TbOffers` table.

### Migration Template
```sql
ALTER TABLE TbOffers
ADD FulfillmentType INT NOT NULL DEFAULT 2  -- Default to Seller (FBM)

-- Create index for performance
CREATE INDEX IX_TbOffers_FulfillmentType ON TbOffers(FulfillmentType)
```

---

## Next Steps

1. ? Code changes complete
2. ? Build successful
3. ? Create and run migration
4. ? Update seed data with fulfillment types
5. ? Test order creation scenarios
6. ? Deploy to staging/production

---

## Related Enumerations

### FulfillmentType
```csharp
namespace Common.Enumerations.Fulfillment
{
    public enum FulfillmentType
    {
        Marketplace = 1,  // FBS - Platform fulfills
        Seller = 2        // FBM - Vendor fulfills
    }
}
```

---

**Status**: ? COMPLETE  
**Build**: ? SUCCESSFUL  
**Deployment Ready**: ? (After migration)

