# ?? Foreign Key Constraint Fix - ORDER SHIPMENT

## Issue
**Error**: `The INSERT statement conflicted with the FOREIGN KEY constraint "FK_TbOrderShipments_TbShippingCompanies_ShippingCompanyId"`

**Cause**: Attempting to insert shipments with `ShippingCompanyId = Guid.Empty`, which violates the foreign key constraint.

---

## Root Cause Analysis

### 1. OrderService.cs (Shipment Creation)
```csharp
// ? BEFORE: Invalid - Guid.Empty doesn't exist in TbShippingCompanies
var shipment = new TbOrderShipment
{
    ShippingCompanyId = Guid.Empty,  // Foreign key violation!
    // ...
};

// ? AFTER: Correctly set to null
var shipment = new TbOrderShipment
{
    ShippingCompanyId = null,  // Will be assigned later
    // ...
};
```

### 2. TbOrderShipment Entity (Navigation Property)
```csharp
// ? BEFORE: Required navigation property
public virtual TbShippingCompany ShippingCompany { get; set; } = null!;

// ? AFTER: Nullable navigation property
public virtual TbShippingCompany? ShippingCompany { get; set; }
```

---

## Changes Made

### File 1: `src/Core/BL/Service/Order/OrderService.cs`
**Location**: `CreateOrderShipmentsFromDetails` method, shipment creation

**Change**:
```csharp
ShippingCompanyId = null  // Changed from Guid.Empty
```

**Impact**: Shipments are now created with no shipping company initially (will be assigned during fulfillment processing)

### File 2: `src/Core/Domains/Entities/Shipping/TbOrderShipment.cs`
**Location**: Navigation property definition

**Change**:
```csharp
public virtual TbShippingCompany? ShippingCompany { get; set; }  // Made nullable
```

**Impact**: Entity model now properly reflects that shipping company is optional at shipment creation

---

## Why This Works

1. **ShippingCompanyId as Guid?** (Already in place)
   - Database column allows NULL
   - FK constraint allows NULL values
   - Shipments can exist without a shipping company

2. **ShippingCompany as nullable?** (Just added)
   - Navigation property can be null
   - EF Core won't require loading the related entity
   - Proper null-safe access pattern

3. **Later Assignment**
   - Shipments created in "Pending" state
   - Shipping company assigned when shipment is processed
   - Two-phase approach prevents constraint violations

---

## Business Logic Flow

```
Order Creation:
  ?? Create Order
  ?? Create Order Details
  ?? Create Order Payment
  ?? Create Shipments (ShippingCompanyId = null)
  
Shipment Processing:
  ?? Assign ShippingCompany
  ?? Generate TrackingNumber
  ?? Update ShipmentStatus
```

---

## Verification

? **Build Status**: SUCCESSFUL  
? **Foreign Key**: ShippingCompanyId now nullable  
? **Navigation Property**: ShippingCompany now nullable  
? **Data Flow**: Shipments can be created without shipping company  

---

## Testing

### Unit Test Case
```csharp
[Fact]
public async Task CreateOrderFromCartAsync_CreatesShipmentsWithoutShippingCompany()
{
    // Arrange
    var customerId = "test-customer";
    var request = new CreateOrderFromCartRequest { /* ... */ };

    // Act
    var result = await orderService.CreateOrderFromCartAsync(customerId, request);

    // Assert
    var shipments = db.TbOrderShipment.Where(s => s.OrderId == result.OrderId);
    foreach (var shipment in shipments)
    {
        Assert.Null(shipment.ShippingCompanyId);
        Assert.Null(shipment.ShippingCompany);
    }
}
```

### Integration Test
```csharp
[Fact]
public async Task CreateOrderFlow_ShouldSucceed_WithoutInitialShippingCompany()
{
    // Arrange
    var cartItems = /* ... */;

    // Act
    var result = await orderService.CreateOrderFromCartAsync(customerId, request);

    // Assert
    Assert.NotNull(result.OrderId);
    var order = db.TbOrder.Find(result.OrderId);
    Assert.NotNull(order);
    
    var shipments = db.TbOrderShipment.Where(s => s.OrderId == result.OrderId).ToList();
    Assert.NotEmpty(shipments);
    Assert.All(shipments, s => Assert.Null(s.ShippingCompanyId));
}
```

---

## Related Items

- **OrderService.CreateOrderShipmentsFromDetails**: Creates shipments with null shipping company
- **TbOrderShipment Entity**: Updated to support nullable shipping company
- **OrderShipmentConfiguration**: EF Core configuration (no changes needed)
- **Shipment Processing Logic**: Responsible for assigning shipping company later

---

## Notes

- ? No database migration required (column was already nullable)
- ? No breaking changes to public APIs
- ? Proper separation of concerns (creation vs. assignment)
- ? Backwards compatible

---

**Status**: ? COMPLETE  
**Build**: ? SUCCESSFUL  
**Ready for**: Testing & Deployment

