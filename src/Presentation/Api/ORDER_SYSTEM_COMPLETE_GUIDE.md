# ??? Complete Order System Implementation Guide
## 8-Stage Order Flow - Sahl E-Commerce Platform

**Version**: 1.0  
**Date**: December 2025  
**Status**: Implementation Ready  

---

## ?? Table of Contents

1. [System Architecture](#system-architecture)
2. [8-Stage Order Flow](#8-stage-order-flow)
3. [DTOs & Models](#dtos--models)
4. [Service Layer](#service-layer)
5. [API Endpoints](#api-endpoints)
6. [Database Schema](#database-schema)
7. [Implementation Checklist](#implementation-checklist)

---

## System Architecture

### Layered Architecture

```
???????????????????????????????????????
?     API Layer (Controllers)          ? ? HTTP Requests
???????????????????????????????????????
?     Service Layer (BL)               ? ? Business Logic
?  ?? ICartService                    ?
?  ?? ICheckoutService                ?
?  ?? IOrderService                   ?
?  ?? IPaymentService                 ?
?  ?? IShipmentService                ?
?  ?? IFulfillmentService             ?
?  ?? IDeliveryService                ?
???????????????????????????????????????
?     Mapper Layer                     ? ? DTO Conversion
???????????????????????????????????????
?     Repository Layer (DAL)           ? ? Data Access
???????????????????????????????????????
?     Database (EF Core)               ? ? Persistence
???????????????????????????????????????
```

### Single Responsibility Principle

Each service handles one specific stage:

| Service | Responsibility | Stage |
|---------|-----------------|-------|
| `ICartService` | Shopping cart management | 1 |
| `ICheckoutService` | Order validation & preview | 2 |
| `IOrderService` | Order creation & management | 3 |
| `IShipmentService` | Shipment splitting & tracking | 4 |
| `IPaymentService` | Payment processing & refunds | 5 |
| `IFulfillmentService` | FBA/FBM processing | 6 |
| `IShipmentService` | Shipping coordination | 7 |
| `IDeliveryService` | Delivery & returns | 8 |

### Authorization & Single Responsibility

Each endpoint follows explicit authorization:

```csharp
// Stage 1: Add to Cart (Customer)
[Authorize(Roles = "Customer")]

// Stage 2: Checkout (Customer)
[Authorize(Roles = "Customer")]

// Stage 3: Create Order (Customer)
[Authorize(Roles = "Customer")]

// Stage 4: Split Shipments (Admin/System)
[Authorize(Roles = "Admin,System")]

// Stage 5: Process Payment (Customer + Payment Gateway)
[Authorize(Roles = "Customer")]

// Stage 6: Fulfillment (Admin/Vendor)
[Authorize(Roles = "Admin,Vendor")]

// Stage 7: Shipping (ShippingCompany/Admin)
[Authorize(Roles = "Admin,ShippingCompany")]

// Stage 8: Delivery (ShippingCompany/Admin)
[Authorize(Roles = "Admin,ShippingCompany")]
```

---

## 8-Stage Order Flow

### Stage 1: Add to Cart ??

**Endpoint**: `POST /api/v1/cart/add-item`

**Request**:
```json
{
  "itemId": "uuid",
  "offerId": "uuid",
  "itemCombinationId": "uuid-or-null",
  "quantity": 2
}
```

**Response**:
```json
{
  "id": "uuid",
  "cartItemId": "uuid",
  "itemId": "uuid",
  "offerId": "uuid",
  "quantity": 2,
  "unitPrice": 100.00,
  "itemName": "Product Name",
  "vendorName": "Vendor Name",
  "isAvailable": true,
  "message": "Item added to cart successfully"
}
```

**Business Logic**:
1. Get active cart for customer (create if not exists)
2. Check stock availability from Offer
3. Add/update cart item
4. Return cart summary

**Key Points**:
- Each cart item is linked to a specific `OfferId` (specific vendor & price)
- Same product from different vendors = separate cart items
- Quantity can be updated by adding again

---

### Stage 2: Checkout Review ?

**Endpoint**: `POST /api/v1/checkout/prepare`

**Request**:
```json
{
  "deliveryAddressId": "uuid"
}
```

**Response**:
```json
{
  "subTotal": 450.00,
  "estimatedShipping": 50.00,
  "taxAmount": 70.00,
  "total": 570.00,
  "items": [
    {
      "cartItemId": "uuid",
      "itemId": "uuid",
      "itemName": "Product 1",
      "quantity": 2,
      "unitPrice": 100.00,
      "subTotal": 200.00,
      "vendorName": "Vendor A",
      "vendorId": "uuid",
      "warehouseId": "uuid",
      "warehouseName": "Cairo",
      "isAvailable": true
    }
  ],
  "expectedShipments": [
    {
      "vendorId": "uuid",
      "vendorName": "Vendor A",
      "warehouseId": "uuid",
      "warehouseName": "Cairo",
      "itemCount": 2,
      "itemNames": ["Product 1", "Product 2"],
      "subTotal": 200.00,
      "estimatedShippingCost": 25.00,
      "estimatedDeliveryDate": "2025-12-10"
    }
  ],
  "totalShipmentsExpected": 1,
  "message": "Checkout prepared successfully"
}
```

**Business Logic**:
1. Validate cart exists and not empty
2. Check stock for each item
3. Calculate totals (subtotal, tax, shipping)
4. Group by `(VendorId, WarehouseId)` to preview shipments
5. Calculate estimated delivery dates

**Key Points**:
- Validates item availability
- Shows expected shipment count
- Calculates accurate shipping per vendor
- Shows all items that will be shipped

---

### Stage 3: Create Order ??

**Endpoint**: `POST /api/v1/orders`

**Request**:
```json
{
  "deliveryAddressId": "uuid",
  "paymentMethodId": "uuid",
  "couponCode": "SAVE10",
  "notes": "Please handle with care"
}
```

**Response**:
```json
{
  "id": "uuid",
  "orderId": "uuid",
  "orderNumber": "ORD-20251203-12345",
  "totalAmount": 570.00,
  "subTotal": 450.00,
  "shippingAmount": 50.00,
  "taxAmount": 70.00,
  "orderStatus": "PaymentPending",
  "paymentStatus": "Pending",
  "shipmentCount": 1,
  "orderDetails": [
    {
      "orderDetailId": "uuid",
      "itemId": "uuid",
      "itemName": "Product 1",
      "quantity": 2,
      "unitPrice": 100.00,
      "subTotal": 200.00,
      "taxAmount": 30.00,
      "discountAmount": 10.00,
      "vendorId": "uuid",
      "vendorName": "Vendor A",
      "warehouseId": "uuid",
      "warehouseName": "Cairo"
    }
  ],
  "message": "Order created successfully"
}
```

**Database Changes**:
```sql
-- Insert into TbOrder
INSERT INTO TbOrder (Id, Number, CustomerId, Price, ShippingAmount, TaxAmount, 
                     OrderStatus, PaymentStatus, DeliveryAddressId)
VALUES (orderId, 'ORD-...', customerId, 450, 50, 70, 1, 1, addressId)

-- Insert into TbOrderDetail (for each cart item)
INSERT INTO TbOrderDetail (OrderId, ItemId, OfferId, VendorId, WarehouseId, 
                           Quantity, UnitPrice, SubTotal, TaxAmount, DiscountAmount)
VALUES (orderId, itemId, offerId, vendorId, warehouseId, qty, price, total, tax, discount)

-- Disable cart
UPDATE TbShoppingCart SET IsActive = 0 WHERE Id = cartId
```

**Business Logic**:
1. Validate delivery address belongs to customer
2. Create TbOrder with status = `PaymentPending`
3. Create TbOrderDetail for each cart item
4. Link each OrderDetail to vendor & warehouse
5. Calculate and store totals
6. Clear/disable shopping cart
7. Return order summary

**Key Points**:
- Order NOT yet confirmed - awaiting payment
- All items grouped but NOT yet split into shipments
- Each OrderDetail has VendorId & WarehouseId
- Status = PaymentPending until payment succeeds

---

### Stage 4: Split to Shipments ??

**Endpoint**: `POST /api/v1/shipments/split-order/{orderId}`

**Response**:
```json
[
  {
    "id": "uuid",
    "shipmentId": "uuid",
    "shipmentNumber": "SHP-001",
    "orderId": "uuid",
    "orderNumber": "ORD-20251203-12345",
    "vendorId": "uuid",
    "vendorName": "Vendor A",
    "warehouseId": "uuid",
    "warehouseName": "Cairo",
    "shipmentStatus": "Pending",
    "subTotal": 200.00,
    "shippingCost": 25.00,
    "totalAmount": 225.00,
    "trackingNumber": null,
    "estimatedDeliveryDate": "2025-12-10",
    "actualDeliveryDate": null,
    "items": [
      {
        "shipmentItemId": "uuid",
        "itemId": "uuid",
        "itemName": "Product 1",
        "quantity": 2,
        "unitPrice": 100.00,
        "subTotal": 200.00
      }
    ],
    "statusHistory": []
  }
]
```

**Database Changes**:
```sql
-- Insert into TbOrderShipment
INSERT INTO TbOrderShipment (OrderId, VendorId, WarehouseId, ShipmentNumber, 
                             ShipmentStatus, SubTotal, ShippingCost, TotalAmount)
VALUES (orderId, vendorId, warehouseId, 'SHP-001', 1, 200, 25, 225)

-- Insert into TbOrderShipmentItem for each item in shipment
INSERT INTO TbOrderShipmentItem (ShipmentId, OrderDetailId, ItemId, Quantity, 
                                 UnitPrice, SubTotal)
VALUES (shipmentId, orderDetailId, itemId, qty, price, total)

-- Insert into TbShipmentStatusHistory (initial status)
INSERT INTO TbShipmentStatusHistory (ShipmentId, Status, StatusDate, Notes)
VALUES (shipmentId, 1, NOW(), 'Shipment created')
```

**Grouping Logic**:
```csharp
var shipmentGroups = orderDetails
    .GroupBy(od => new { od.VendorId, od.WarehouseId })
    .ToList();

// Each group = 1 shipment
// Shipment 1: Vendor A + Cairo Warehouse
// Shipment 2: Vendor A + Alex Warehouse  
// Shipment 3: Vendor B + Cairo Warehouse
// etc.
```

**Business Logic**:
1. Fetch all OrderDetails for order
2. Group by `(VendorId, WarehouseId)`
3. For each group:
   - Create TbOrderShipment
   - Create TbOrderShipmentItems
   - Add initial status history entry
4. Calculate shipping for each shipment
5. Update order status to `Confirmed`

**Key Points**:
- Automatic based on OrderDetails vendor+warehouse
- Each shipment is independent
- Shipment status = Pending initially
- Shipping cost calculated per shipment

---

### Stage 5: Payment Processing ??

**Endpoint**: `POST /api/v1/payment/process`

**Request**:
```json
{
  "orderId": "uuid",
  "paymentMethodId": "uuid",
  "paymentGatewayReference": "gateway-txn-id",
  "notes": "Credit card payment"
}
```

**Response**:
```json
{
  "id": "uuid",
  "orderId": "uuid",
  "orderNumber": "ORD-20251203-12345",
  "paymentId": "uuid",
  "amount": 570.00,
  "paymentStatus": "Completed",
  "transactionId": "TXN-123456",
  "processedAt": "2025-12-03T10:30:00Z",
  "success": true,
  "message": "Payment processed successfully"
}
```

**Database Changes**:
```sql
-- Insert into TbOrderPayment
INSERT INTO TbOrderPayment (OrderId, PaymentMethodId, Amount, PaymentStatus, 
                            TransactionId, PaidAt)
VALUES (orderId, paymentMethodId, 570.00, 2, 'TXN-123456', NOW())

-- Update TbOrder
UPDATE TbOrder 
SET PaymentStatus = 2,              -- Completed
    OrderStatus = 3,                -- Processing
    PaymentDate = NOW()
WHERE Id = orderId

-- Update TbOrderShipment
UPDATE TbOrderShipment
SET ShipmentStatus = 2              -- Processing
WHERE OrderId = orderId
```

**Business Logic**:
1. Validate order exists and status = PaymentPending
2. Call payment gateway API
3. If successful:
   - Create TbOrderPayment record
   - Update order status = Processing
   - Update all shipments status = Processing
4. If failed:
   - Create TbOrderPayment with Failed status
   - Keep order in PaymentPending
5. Return payment result

**Key Points**:
- Integrates with external payment gateway
- Updates both Order and Shipments on success
- Transactional - all or nothing
- Stores transaction ID for reconciliation

---

### Stage 6: Fulfillment (FBA/FBM) ??

**Endpoints**:
- FBA: `POST /api/v1/delivery/{shipmentId}/process-fba`
- FBM: `POST /api/v1/delivery/{shipmentId}/process-fbm`

**FBA Response**:
```json
{
  "id": "uuid",
  "shipmentId": "uuid",
  "shipmentNumber": "SHP-001",
  "newStatus": "ReadyForShipping",
  "trackingNumber": "TRACK-123456",
  "message": "FBA shipment processed and ready for shipping"
}
```

**FBM Response**:
```json
{
  "id": "uuid",
  "shipmentId": "uuid",
  "shipmentNumber": "SHP-001",
  "newStatus": "AwaitingPickup",
  "trackingNumber": null,
  "message": "FBM notification sent to vendor - awaiting shipment"
}
```

**FBA Process**:
```csharp
public async Task ProcessFBAShipmentAsync(Guid shipmentId)
{
    // 1. Reserve inventory from platform warehouse
    await _inventoryService.ReserveStock(shipmentId);
    
    // 2. Generate shipping label with tracking number
    var label = await _labelService.GenerateShippingLabel(shipmentId);
    
    // 3. Update shipment status
    shipment.ShipmentStatus = ShipmentStatus.ReadyForShipping;
    shipment.TrackingNumber = label.TrackingNumber;
    
    // 4. Add to fulfillment queue
    await _fulfillmentQueue.EnqueueAsync(shipmentId);
    
    // 5. Notify customer
    await _notificationService.NotifyCustomerShipmentReady(shipmentId);
}
```

**FBM Process**:
```csharp
public async Task ProcessFBMShipmentAsync(Guid shipmentId)
{
    // 1. Notify vendor with shipment details
    await _notificationService.NotifyVendorPrepareShipment(shipmentId);
    
    // 2. Update shipment status
    shipment.ShipmentStatus = ShipmentStatus.AwaitingPickup;
    
    // 3. Vendor will update status when picked up
}
```

**Key Points**:
- FBA: Platform handles fulfillment
- FBM: Vendor handles fulfillment
- Each has different workflow
- Both update shipment status

---

### Stage 7: Shipping ??

**Endpoint**: `PUT /api/v1/shipments/{shipmentId}/status`

**Request**:
```json
{
  "newStatus": "Shipped",
  "location": "Cairo Distribution Center",
  "notes": "Picked up by FedEx at 10:30 AM"
}
```

**Response**:
```json
{
  "id": "uuid",
  "shipmentId": "uuid",
  "shipmentNumber": "SHP-001",
  "shipmentStatus": "Shipped",
  "trackingNumber": "TRACK-123456",
  "estimatedDeliveryDate": "2025-12-08",
  "message": "Shipment status updated to Shipped"
}
```

**Tracking Public Endpoint**: `GET /api/v1/shipments/track/{trackingNumber}`

**Response**:
```json
{
  "id": "uuid",
  "shipmentId": "uuid",
  "shipmentNumber": "SHP-001",
  "trackingNumber": "TRACK-123456",
  "currentStatus": "InTransit",
  "estimatedDeliveryDate": "2025-12-08",
  "actualDeliveryDate": null,
  "events": [
    {
      "status": "Shipped",
      "eventDate": "2025-12-05",
      "location": "Cairo",
      "description": "Shipment picked up"
    },
    {
      "status": "InTransit",
      "eventDate": "2025-12-06",
      "location": "Giza",
      "description": "In transit to destination"
    }
  ]
}
```

**Business Logic**:
1. Update shipment status
2. Add to ShipmentStatusHistory
3. If status = Shipped:
   - Set trackingNumber (if not set)
   - Update order status if all shipped
4. If status = OutForDelivery:
   - Send notification to customer
5. Track via public endpoint (no auth required)

**Key Points**:
- Public tracking available (no auth)
- Status history maintained
- Event timeline for customer visibility

---

### Stage 8: Delivery & Completion ??

**Endpoint**: `POST /api/v1/delivery/{shipmentId}/complete-delivery`

**Request**:
```json
{
  "shipmentId": "uuid",
  "receivedBy": "Customer Name",
  "notes": "Delivered in good condition"
}
```

**Response**:
```json
{
  "id": "uuid",
  "shipmentId": "uuid",
  "shipmentNumber": "SHP-001",
  "deliveryDate": "2025-12-08T14:30:00Z",
  "currentStatus": "Delivered",
  "isOrderComplete": true,
  "message": "Delivery confirmed. Order #ORD-20251203-12345 is now complete!"
}
```

**Database Changes**:
```sql
-- Update TbOrderShipment
UPDATE TbOrderShipment
SET ShipmentStatus = 6,             -- Delivered
    ActualDeliveryDate = NOW()
WHERE Id = shipmentId

-- Insert delivery confirmation
INSERT INTO TbShipmentStatusHistory (ShipmentId, Status, StatusDate, Notes)
VALUES (shipmentId, 6, NOW(), 'Delivered to customer')

-- Check if all shipments delivered
-- If yes, update TbOrder
UPDATE TbOrder
SET OrderStatus = 9,                -- Completed
    OrderDeliveryDate = NOW()
WHERE Id = orderId
```

**Business Logic**:
1. Validate shipment not already delivered
2. Update shipment status = Delivered
3. Set actual delivery date
4. Add status history entry
5. Check if ALL shipments for order are delivered
6. If yes:
   - Update order status = Completed
   - Send completion notification
   - Request review/feedback from customer
7. If no:
   - Wait for other shipments

**Key Points**:
- Only after ALL shipments delivered
- Triggers customer review request
- Order status = Completed marks end of flow

---

### Stage 8.5: Returns & Refunds ??

**Endpoint**: `POST /api/v1/delivery/{shipmentId}/initiate-return`

**Request**:
```json
{
  "reason": "Product defective",
  "description": "The product doesn't work as advertised",
  "issueImages": ["image-url-1", "image-url-2"]
}
```

**Response**:
```json
{
  "id": "uuid",
  "returnId": "uuid",
  "shipmentId": "uuid",
  "shipmentNumber": "SHP-001",
  "reason": "Product defective",
  "status": "Pending",
  "createdAt": "2025-12-08",
  "message": "Return request initiated. Please expect a refund within 5-7 business days."
}
```

**Return Workflow**:
```
1. Customer initiates return (30 days after delivery)
2. Admin/Vendor reviews return request
3. If approved:
   - Generate return shipping label
   - Customer ships back
   - Admin receives and inspects
   - Process refund
4. If rejected:
   - Notify customer with reason
```

**Key Points**:
- 30-day return window
- Admin can approve/reject
- Automatic refund processing
- Creates return shipment

---

## DTOs & Models

### Request DTOs

```csharp
// Stage 1
public class AddToCartRequest
{
    public Guid ItemId { get; set; }
    public Guid OfferId { get; set; }
    public Guid? ItemCombinationId { get; set; }
    public int Quantity { get; set; }
}

// Stage 2
public class PrepareCheckoutRequest
{
    public Guid DeliveryAddressId { get; set; }
}

// Stage 3
public class CreateOrderFromCartRequest
{
    public Guid DeliveryAddressId { get; set; }
    public Guid PaymentMethodId { get; set; }
    public string? CouponCode { get; set; }
    public string? Notes { get; set; }
}

// Stage 5
public class ProcessPaymentRequest
{
    public Guid OrderId { get; set; }
    public Guid PaymentMethodId { get; set; }
    public string? PaymentGatewayReference { get; set; }
    public string? Notes { get; set; }
}

// Stage 7
public class UpdateShipmentStatusRequest
{
    public string NewStatus { get; set; } = null!;
    public string? Location { get; set; }
    public string? Notes { get; set; }
}

// Stage 8
public class CompleteDeliveryRequest
{
    public Guid ShipmentId { get; set; }
    public string? ReceivedBy { get; set; }
    public string? Notes { get; set; }
}

public class InitiateReturnRequest
{
    public Guid ShipmentId { get; set; }
    public string Reason { get; set; } = null!;
    public string? Description { get; set; }
    public List<string>? IssueImages { get; set; }
}
```

### Response DTOs

All response DTOs are in `src/Shared/Shared/DTOs/ECommerce/Order/OrderFlowDtos.cs`

---

## Service Layer

### ICartService
- Add to cart
- Update quantity
- Remove items
- Get summary
- Clear cart

### ICheckoutService
- Prepare checkout
- Validate items
- Calculate totals
- Preview shipments

### IOrderService
- Create from cart
- Get order details
- Get customer orders
- Cancel order
- Get completion status

### IPaymentService
- Process payment
- Get payment status
- Process refund
- Verify payment

### IShipmentService
- Split order into shipments
- Get shipment details
- Update status
- Assign tracking
- Track shipment
- Get vendor shipments

### IFulfillmentService
- Process FBA
- Process FBM
- Reserve inventory
- Release inventory
- Determine fulfillment type

### IDeliveryService
- Complete delivery
- Confirm order completion
- Initiate return
- Process return
- Get delivery info

---

## API Endpoints

### Cart Endpoints (Public)
```
POST    /api/v1/cart/add-item
GET     /api/v1/cart/summary
PUT     /api/v1/cart/update-item
DELETE  /api/v1/cart/remove-item/{cartItemId}
DELETE  /api/v1/cart/clear
GET     /api/v1/cart/count
```

### Checkout Endpoints (Customer)
```
POST    /api/v1/checkout/prepare
POST    /api/v1/checkout/preview-shipments
POST    /api/v1/checkout/validate
```

### Order Endpoints (Customer/Admin)
```
POST    /api/v1/orders
GET     /api/v1/orders/{orderId}
GET     /api/v1/orders/number/{orderNumber}
GET     /api/v1/orders/customer/me
GET     /api/v1/orders/{orderId}/completion-status
POST    /api/v1/orders/{orderId}/cancel
```

### Payment Endpoints (Customer/Admin)
```
POST    /api/v1/payment/process
GET     /api/v1/payment/status/{orderId}
GET     /api/v1/payment/order/{orderId}
POST    /api/v1/payment/verify
POST    /api/v1/payment/refund
```

### Shipment Endpoints (Admin/Vendor/Public)
```
POST    /api/v1/shipments/split-order/{orderId}
GET     /api/v1/shipments/{shipmentId}
GET     /api/v1/shipments/number/{shipmentNumber}
GET     /api/v1/shipments/order/{orderId}
PUT     /api/v1/shipments/{shipmentId}/status
PUT     /api/v1/shipments/{shipmentId}/tracking
GET     /api/v1/shipments/track/{trackingNumber}
GET     /api/v1/shipments/vendor/my-shipments
```

### Fulfillment Endpoints (Admin/Vendor)
```
POST    /api/v1/delivery/{shipmentId}/process-fba
POST    /api/v1/delivery/{shipmentId}/process-fbm
```

### Delivery Endpoints (Admin/ShippingCompany)
```
POST    /api/v1/delivery/{shipmentId}/complete-delivery
POST    /api/v1/delivery/order/{orderId}/complete
GET     /api/v1/delivery/{shipmentId}/delivery-info
POST    /api/v1/delivery/{shipmentId}/initiate-return
POST    /api/v1/delivery/{shipmentId}/process-return
```

---

## Database Schema

### Existing Tables (Already Created)
- `TbOrder` - Main order record
- `TbOrderDetail` - Line items in order
- `TbOrderShipment` - Shipment splits
- `TbOrderShipmentItem` - Items in shipment
- `TbOrderPayment` - Payment records
- `TbShipmentStatusHistory` - Status tracking
- `TbShoppingCart` - Customer cart
- `TbShoppingCartItem` - Cart items
- `TbReturnOrder` - Return requests

### Required Columns (Already Present)
```sql
-- TbOrder
OrderStatus (int)               -- OrderProgressStatus enum
PaymentStatus (int)             -- PaymentStatus enum
ShippingAmount (decimal)
TaxAmount (decimal)
OrderDeliveryDate (datetime)
PaymentDate (datetime)

-- TbOrderDetail
VendorId (Guid FK)
WarehouseId (Guid FK)
TaxAmount (decimal)
DiscountAmount (decimal)

-- TbOrderShipment
VendorId (Guid FK)
WarehouseId (Guid FK)
FulfillmentType (int)           -- FulfillmentType enum
ShipmentStatus (int)            -- ShipmentStatus enum
TrackingNumber (nvarchar)
EstimatedDeliveryDate (datetime)
ActualDeliveryDate (datetime)
ShippingCost (decimal)
SubTotal (decimal)
TotalAmount (decimal)

-- TbShipmentStatusHistory
ShipmentId (Guid FK)
Status (int)
StatusDate (datetime)
Location (nvarchar)
Notes (nvarchar)
```

---

## Implementation Checklist

### Phase 1: Foundation ?
- [x] Update Enumerations (PaymentStatus, OrderProgressStatus)
- [x] Create OrderFlowDtos.cs with all DTOs
- [x] Update Service Interfaces
- [x] Update OrderService Interface

### Phase 2: Implementation (Next)
- [ ] Implement CartService
  - [ ] AddToCartAsync
  - [ ] UpdateCartItemAsync
  - [ ] RemoveFromCartAsync
  - [ ] ClearCartAsync
  - [ ] GetCartSummaryAsync

- [ ] Implement CheckoutService
  - [ ] PrepareCheckoutAsync
  - [ ] PreviewShipmentsAsync
  - [ ] ValidateCheckoutAsync
  - [ ] CalculateShippingAsync
  - [ ] ApplyCouponAsync

- [ ] Implement OrderService
  - [ ] CreateOrderFromCartAsync
  - [ ] GetOrderByIdAsync
  - [ ] GetOrderByNumberAsync
  - [ ] GetCustomerOrdersAsync
  - [ ] CancelOrderAsync
  - [ ] GetOrderCompletionStatusAsync

- [ ] Implement PaymentService
  - [ ] ProcessPaymentAsync
  - [ ] GetPaymentStatusAsync
  - [ ] ProcessRefundAsync
  - [ ] VerifyPaymentAsync

- [ ] Implement ShipmentService
  - [ ] SplitOrderIntoShipmentsAsync
  - [ ] GetShipmentByIdAsync
  - [ ] UpdateShipmentStatusAsync
  - [ ] AssignTrackingNumberAsync
  - [ ] TrackShipmentAsync
  - [ ] GetVendorShipmentsAsync

- [ ] Implement FulfillmentService
  - [ ] ProcessFBAShipmentAsync
  - [ ] ProcessFBMShipmentAsync
  - [ ] ReserveInventoryAsync
  - [ ] ReleaseInventoryAsync

- [ ] Implement DeliveryService
  - [ ] CompleteDeliveryAsync
  - [ ] ConfirmOrderCompletionAsync
  - [ ] InitiateReturnAsync
  - [ ] ProcessReturnAsync
  - [ ] GetShipmentDeliveryInfoAsync

### Phase 3: Mapper Profiles
- [ ] OrderMappingProfile (Order, OrderDetail, Shipment)
- [ ] PaymentMappingProfile
- [ ] CartMappingProfile

### Phase 4: Integration Testing
- [ ] Test full order flow (Stages 1-8)
- [ ] Test payment processing
- [ ] Test return workflow
- [ ] Test authorization/roles
- [ ] Test error handling

### Phase 5: Documentation
- [ ] API Documentation (Swagger)
- [ ] Service Documentation
- [ ] Database Queries
- [ ] Error Codes Reference

---

## Key Design Decisions

1. **Single Responsibility**: Each service handles one stage only
2. **Authorization**: Explicit role checks on each endpoint
3. **State Machine**: Order progresses through defined states
4. **Async Operations**: All operations are async
5. **Transaction Safety**: DB operations wrapped in transactions
6. **Audit Trail**: All state changes logged in history tables
7. **Public Tracking**: Tracking available without authentication
8. **Flexible Fulfillment**: Supports both FBA and FBM models

---

## Error Handling

### Common Error Codes

```csharp
// 400 Bad Request
CART_ITEM_NOT_AVAILABLE     // Stock not available
INVALID_DELIVERY_ADDRESS    // Address doesn't exist
INVALID_PAYMENT_METHOD      // Payment method not valid
CHECKOUT_VALIDATION_FAILED  // Cart validation failed

// 404 Not Found
ORDER_NOT_FOUND
SHIPMENT_NOT_FOUND
CART_NOT_FOUND

// 409 Conflict
ORDER_ALREADY_SHIPPED       // Cannot cancel shipped order
ORDER_ALREADY_CANCELLED     // Order already cancelled
SHIPMENT_ALREADY_DELIVERED  // Cannot modify delivered shipment

// 403 Forbidden
UNAUTHORIZED_ACCESS         // User doesn't own order
INVALID_ROLE_FOR_ACTION     // User doesn't have required role

// 500 Server Error
PAYMENT_GATEWAY_ERROR       // External service failed
INVENTORY_SYSTEM_ERROR      // Inventory service error
```

---

## Performance Considerations

1. **Caching**: Cache cart items for 1 hour
2. **Lazy Loading**: Load shipments only when requested
3. **Pagination**: Limit order lists to 10-50 per page
4. **Indexing**: Index on CustomerID, OrderNumber, TrackingNumber
5. **Async Queue**: Async payment processing via Hangfire

---

## Security Considerations

1. **Authentication**: All endpoints except tracking require auth
2. **Authorization**: Role-based access control
3. **Data Validation**: Input validation on all DTOs
4. **PII Protection**: Sensitive data not logged
5. **HTTPS Only**: All APIs require HTTPS
6. **Rate Limiting**: Prevent abuse (10 requests/sec per user)

---

## Next Steps

1. Review this document with stakeholders
2. Implement Phase 2 (Services)
3. Create unit tests
4. Implement Phase 3 (Mappers)
5. Run integration tests
6. Deploy to staging
7. UAT testing
8. Production deployment

---

**Document Version**: 1.0  
**Last Updated**: December 2025  
**Status**: Ready for Implementation
