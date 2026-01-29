# Order Details Page - Admin Dashboard Fixes

## Summary
Fixed two issues in the admin dashboard order details page:
1. **Shipments now include items** - Each shipment displays detailed item information
2. **Payment Method no longer empty** - Fixed to use PaymentMethodType enum

## Changes Made

### 1. Updated `ShipmentInfoDto` (Shared DTO)
**File:** `src/Shared/Shared/DTOs/Order/Fulfillment/Shipment/ShipmentInfoDto.cs`

- Changed `ItemIds` (List<Guid>) to `Items` (List<ShipmentItemDto>)
- Kept `TrackingNumber`, `EstimatedDeliveryDate`, and `ActualDeliveryDate` fields (for other services)
- Now includes detailed item information for each shipment

### 2. Updated `OrderRepository`
**File:** `src/Infrastructure/DAL/Repositories/Order/OrderRepository.cs`

- Added `.ThenInclude(si => si.Item)` to load item details for shipment items
- Added `.ThenInclude(si => si.OrderDetail)` to load order detail information
- This ensures all necessary data is loaded from the database

### 3. Updated `AdminOrderService`
**File:** `src/Core/BL/Services/Order/OrderProcessing/AdminOrderService.cs`

**Shipment Mapping:**
- Now maps shipment items to `ShipmentItemDto` with full details:
  - Item ID, Name (English & Arabic)
  - Item Image
  - Quantity, Unit Price, Subtotal
  - Item Combination ID (if applicable)

**Payment Method Fix:**
- Changed from: `order.OrderPayments.FirstOrDefault()?.PaymentMethod.ToString()`
- Changed to: `order.OrderPayments.FirstOrDefault()?.PaymentMethodType.ToString() ?? "Not Specified"`
- Uses the `PaymentMethodType` enum which is always populated, instead of the navigation property which may be null

### 4. Updated `CustomerOrderService`
**File:** `src/Core/BL/Services/Order/OrderProcessing/CustomerOrderService.cs`

- Applied the same shipment items mapping for consistency
- Applied the same payment method fix

### 5. Updated Admin Dashboard UI
**File:** `src/Presentation/Dashboard/Pages/Orders/Orders/Details.razor`

- Updated shipment items count to use `shipment.Items.Count` instead of `shipment.ItemIds.Count`
- Tracking number and delivery date fields remain in the UI (data is available but can be hidden if needed)

## Technical Details

### Shipment Items Structure
Each shipment now includes a list of `ShipmentItemDto` objects with:
```csharp
public class ShipmentItemDto
{
    public Guid Id { get; set; }
    public Guid ShipmentId { get; set; }
    public Guid ItemId { get; set; }
    public string ItemName { get; set; }
    public string TitleAr { get; set; }
    public string TitleEn { get; set; }
    public string? ItemImage { get; set; }
    public Guid? ItemCombinationId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SubTotal { get; set; }
}
```

### Payment Method Resolution
The payment method is now resolved using the `PaymentMethodType` enum:
- **Wallet** - For wallet payments
- **Card** - For card payments
- **Cash** - For cash on delivery
- **Not Specified** - When no payment method is found

## Testing Recommendations

1. **View Order Details** - Navigate to an order details page in the admin dashboard
2. **Check Shipments Tab** - Verify that shipments show item counts correctly
3. **Check Payment Info** - Verify that payment method is displayed (not empty)
4. **Check Customer Info** - Verify all other order information displays correctly

## Notes

- Tracking number and estimated delivery date fields are still available in the DTO but can be hidden in the UI if not needed
- The changes maintain backward compatibility with other services that use `ShipmentInfoDto`
- All database queries properly load related entities to avoid N+1 query issues
