# ?? Order System Implementation Roadmap

## Overview

This document provides a step-by-step implementation roadmap for the complete 8-stage order system in the Sahl e-commerce platform.

## Current Status ?

- [x] DTOs created (OrderFlowDtos.cs)
- [x] Enumerations enhanced (PaymentStatus, OrderProgressStatus)
- [x] Service interfaces updated
- [x] Database schema verified (no migrations needed)
- [x] OrderService template created
- [x] Architecture documentation completed

## Implementation Phases

### Phase 1: Core Services Implementation (Weeks 1-2)

#### Week 1: Foundation Services

**Monday-Wednesday: CartService**
```csharp
File: src/Core/BL/Service/Order/CartService.cs
Implements:
  - AddToCartAsync(customerId, request)
  - RemoveFromCartAsync(customerId, cartItemId)
  - UpdateCartItemAsync(customerId, request)
  - ClearCartAsync(customerId)
  - GetCartSummaryAsync(customerId)
  - GetCartItemCountAsync(customerId)

Tests:
  - Add item to cart (new cart creation)
  - Add same item with different offer
  - Update quantity
  - Remove item
  - Clear cart
  - Get count
```

**Thursday-Friday: CheckoutService**
```csharp
File: src/Core/BL/Service/Order/CheckoutService.cs
Implements:
  - PrepareCheckoutAsync(customerId, request)
  - PreviewShipmentsAsync(customerId)
  - ValidateCheckoutAsync(customerId, addressId)
  - CalculateShippingAsync(customerId, addressId)
  - ApplyCouponAsync(customerId, couponCode)

Tests:
  - Prepare checkout with multiple vendors
  - Preview shipment grouping
  - Shipping cost calculation
  - Coupon application
```

#### Week 2: Order & Payment Services

**Monday-Tuesday: OrderService** (Template already created)
```csharp
File: src/Core/BL/Service/Order/OrderService.cs (Complete existing template)
Tests:
  - Create order from cart
  - Get order by ID
  - Get orders for customer
  - Cancel order
  - Get completion status
```

**Wednesday-Thursday: PaymentService**
```csharp
File: src/Core/BL/Service/Order/PaymentService.cs
Implements:
  - ProcessPaymentAsync(request)
  - GetPaymentStatusAsync(orderId)
  - ProcessRefundAsync(request)
  - VerifyPaymentAsync(orderId, transactionId)
  - GetOrderPaymentsAsync(orderId)

Tests:
  - Process payment via gateway
  - Get payment status
  - Verify payment
  - Process refund
```

**Friday: ShipmentService (Part 1)**
```csharp
File: src/Core/BL/Service/Order/ShipmentService.cs (Part 1)
Implements:
  - SplitOrderIntoShipmentsAsync(orderId)
  - GetShipmentByIdAsync(shipmentId)
  - GetShipmentByNumberAsync(shipmentNumber)

Tests:
  - Split order into shipments
  - Shipment grouping by vendor+warehouse
  - Get shipment details
```

### Phase 2: Advanced Services (Week 3)

**Monday-Wednesday: ShipmentService (Part 2) & FulfillmentService**
```csharp
File: src/Core/BL/Service/Order/ShipmentService.cs (Complete)
Implements:
  - GetOrderShipmentsAsync(orderId)
  - UpdateShipmentStatusAsync(shipmentId, status, location, notes)
  - AssignTrackingNumberAsync(shipmentId, trackingNumber, deliveryDate)
  - TrackShipmentAsync(trackingNumber)
  - GetVendorShipmentsAsync(vendorId, pageNumber, pageSize)

File: src/Core/BL/Service/Order/FulfillmentService.cs
Implements:
  - ProcessFBAShipmentAsync(shipmentId)
  - ProcessFBMShipmentAsync(shipmentId)
  - ReserveInventoryAsync(shipmentId)
  - ReleaseInventoryAsync(shipmentId)
  - DetermineFulfillmentTypeAsync(warehouseId)

Tests:
  - FBA fulfillment process
  - FBM fulfillment process
  - Inventory reservation
  - Tracking assignment
```

**Thursday-Friday: DeliveryService**
```csharp
File: src/Core/BL/Service/Order/DeliveryService.cs
Implements:
  - CompleteDeliveryAsync(shipmentId, confirmation)
  - ConfirmOrderCompletionAsync(orderId)
  - InitiateReturnAsync(shipmentId, reason)
  - ProcessReturnAsync(shipmentId, approved)
  - GetShipmentDeliveryInfoAsync(shipmentId)

Tests:
  - Complete delivery
  - Confirm order completion
  - Initiate return
  - Process return
  - Get delivery info
```

### Phase 3: Mapper Profiles (Week 4, Monday-Tuesday)

**OrderMappingProfile**
```csharp
File: src/Core/BL/Mapper/OrderMappingProfile.cs
Mappings:
  - TbOrder ? OrderDto
  - TbOrderDetail ? OrderDetailDto
  - TbOrderShipment ? OrderShipmentDto
  - TbOrderShipmentItem ? ShipmentItemDto
  - TbOrder ? OrderListItemDto
  - TbOrder ? OrderCreatedResponseDto
  - TbShipmentStatusHistory ? ShipmentStatusHistoryDto
```

**PaymentMappingProfile**
```csharp
File: src/Core/BL/Mapper/PaymentMappingProfile.cs
Mappings:
  - TbOrderPayment ? PaymentProcessedDto
  - TbOrderPayment ? PaymentStatusDto
```

**CartMappingProfile**
```csharp
File: src/Core/BL/Mapper/CartMappingProfile.cs
Mappings:
  - TbShoppingCartItem ? CartItemPreviewDto
  - TbShoppingCart ? CartSummaryDto
  - TbOrderShipment ? ExpectedShipmentDto
```

### Phase 4: API Controllers & Integration (Week 4, Wednesday-Friday)

Already created but ensure:
- ? CartController - Add comprehensive validation
- ? CheckoutController - Add shipment preview endpoint
- ? OrderController - Complete implementation
- ? PaymentController - Ensure gateway integration
- ? ShipmentController - Complete all endpoints
- ? DeliveryController - Add return endpoints

### Phase 5: Comprehensive Testing (Week 5)

**Unit Tests**
```
CartServiceTests.cs
CheckoutServiceTests.cs
OrderServiceTests.cs
PaymentServiceTests.cs
ShipmentServiceTests.cs
FulfillmentServiceTests.cs
DeliveryServiceTests.cs
```

**Integration Tests**
```
Full Order Flow Tests:
  1. Add to cart
  2. Prepare checkout
  3. Create order
  4. Split into shipments
  5. Process payment
  6. Process fulfillment
  7. Start shipping
  8. Complete delivery
  9. Request return
  10. Process return
```

**API Tests**
```
CartControllerTests.cs
CheckoutControllerTests.cs
OrderControllerTests.cs
PaymentControllerTests.cs
ShipmentControllerTests.cs
DeliveryControllerTests.cs
```

## Detailed Task Breakdown

### CartService Implementation Tasks

```
[ ] Create CartService.cs
[ ] Implement AddToCartAsync
    [ ] Get/Create active cart
    [ ] Check stock availability
    [ ] Add/update cart item
    [ ] Return updated cart
[ ] Implement RemoveFromCartAsync
[ ] Implement UpdateCartItemAsync
[ ] Implement ClearCartAsync
[ ] Implement GetCartSummaryAsync
[ ] Implement GetCartItemCountAsync
[ ] Create unit tests
[ ] Add Swagger documentation
```

### CheckoutService Implementation Tasks

```
[ ] Create CheckoutService.cs
[ ] Implement PrepareCheckoutAsync
    [ ] Validate cart
    [ ] Check stock
    [ ] Calculate totals
    [ ] Preview shipments
[ ] Implement PreviewShipmentsAsync
[ ] Implement ValidateCheckoutAsync
[ ] Implement CalculateShippingAsync
[ ] Implement ApplyCouponAsync
[ ] Create unit tests
[ ] Add Swagger documentation
```

### OrderService Implementation Tasks

```
[x] OrderService template created
[ ] Complete OrderService.cs implementation
[ ] Implement CreateOrderFromCartAsync
    [ ] Transaction support
    [ ] Offer details retrieval
    [ ] Cart disabling
[ ] Implement GetOrderByIdAsync
[ ] Implement GetOrderByNumberAsync
[ ] Implement GetCustomerOrdersAsync
[ ] Implement GetOrderWithShipmentsAsync
[ ] Implement GetOrderCompletionStatusAsync
[ ] Implement CancelOrderAsync
    [ ] Inventory release
    [ ] Refund processing
    [ ] Customer notification
[ ] Implement ValidateOrderAsync
[ ] Create unit tests
[ ] Add Swagger documentation
```

### PaymentService Implementation Tasks

```
[ ] Create PaymentService.cs
[ ] Implement ProcessPaymentAsync
    [ ] Payment gateway integration
    [ ] Transaction creation
    [ ] Order status update
    [ ] Shipment status update
[ ] Implement GetPaymentStatusAsync
[ ] Implement ProcessRefundAsync
    [ ] Validate refund eligibility
    [ ] Call gateway refund
    [ ] Update payment status
[ ] Implement VerifyPaymentAsync
[ ] Implement GetOrderPaymentsAsync
[ ] Payment gateway abstraction layer
[ ] Create unit tests
[ ] Add Swagger documentation
[ ] Error handling for gateway failures
```

### ShipmentService Implementation Tasks

```
[ ] Create ShipmentService.cs
[ ] Implement SplitOrderIntoShipmentsAsync
    [ ] Group by (VendorId, WarehouseId)
    [ ] Create shipment records
    [ ] Calculate shipping per group
    [ ] Generate tracking (if FBA)
[ ] Implement GetShipmentByIdAsync
[ ] Implement GetShipmentByNumberAsync
[ ] Implement GetOrderShipmentsAsync
[ ] Implement UpdateShipmentStatusAsync
    [ ] Validate status transition
    [ ] Add history entry
    [ ] Notify stakeholders
[ ] Implement AssignTrackingNumberAsync
    [ ] Generate tracking
    [ ] Update estimate delivery
[ ] Implement TrackShipmentAsync (Public endpoint)
[ ] Implement GetVendorShipmentsAsync
[ ] Create unit tests
[ ] Add Swagger documentation
```

### FulfillmentService Implementation Tasks

```
[ ] Create FulfillmentService.cs
[ ] Implement ProcessFBAShipmentAsync
    [ ] Reserve inventory
    [ ] Generate label
    [ ] Update status
    [ ] Add to fulfillment queue
[ ] Implement ProcessFBMShipmentAsync
    [ ] Notify vendor
    [ ] Update status
    [ ] Set deadline
[ ] Implement ReserveInventoryAsync
[ ] Implement ReleaseInventoryAsync
[ ] Implement DetermineFulfillmentTypeAsync
[ ] Create unit tests
[ ] Add Swagger documentation
```

### DeliveryService Implementation Tasks

```
[ ] Create DeliveryService.cs
[ ] Implement CompleteDeliveryAsync
    [ ] Validate shipment
    [ ] Update status
    [ ] Check order completion
    [ ] Update order if complete
    [ ] Send notifications
    [ ] Request review
[ ] Implement ConfirmOrderCompletionAsync
[ ] Implement InitiateReturnAsync
    [ ] Validate return window (30 days)
    [ ] Create return order
    [ ] Generate return label
    [ ] Notify customer
[ ] Implement ProcessReturnAsync
    [ ] Validate return
    [ ] Approve/reject
    [ ] Process refund if approved
    [ ] Restore inventory
[ ] Implement GetShipmentDeliveryInfoAsync
[ ] Create unit tests
[ ] Add Swagger documentation
```

## Resource Allocation

### Development Team

```
Senior Developer (1): Lead implementation, architecture review
  - Weeks 1-2: OrderService, PaymentService
  - Weeks 3-4: Mapper profiles, code review
  - Week 5: Testing, optimization

Mid-level Developer (1): Core services
  - Weeks 1-2: CartService, CheckoutService
  - Weeks 3-4: DeliveryService, FulfillmentService
  - Week 5: Integration tests

Junior Developer (1): Support services
  - Weeks 1-2: ShipmentService Part 1
  - Weeks 3-4: ShipmentService Part 2, refactor
  - Week 5: Unit tests

QA Engineer (1): Testing throughout
  - Ongoing: Unit test support
  - Week 4: Integration testing
  - Week 5: UAT preparation
```

## Success Criteria

### Code Quality
- ? All services follow Single Responsibility Principle
- ? Authorization checks on every endpoint
- ? Comprehensive error handling
- ? Logging on all operations
- ? Unit test coverage > 80%

### Functionality
- ? All 8 stages implemented
- ? Full order flow testable end-to-end
- ? Return/refund workflow functional
- ? Public tracking available
- ? Vendor shipment views functional

### Performance
- ? Order creation < 500ms
- ? Shipment split < 1000ms
- ? Tracking lookup < 200ms
- ? Order list with pagination < 500ms

### Documentation
- ? All services documented
- ? API endpoints in Swagger
- ? Integration guide created
- ? Error codes reference

## Timeline

```
Week 1 (Dec 2-6):     CartService, CheckoutService
Week 2 (Dec 9-13):    OrderService, PaymentService
Week 3 (Dec 16-20):   ShipmentService, FulfillmentService, DeliveryService
Week 4 (Dec 23-27):   Mapper profiles, Controllers, Integration
Week 5 (Dec 30-Jan 3): Testing, UAT, Deployment prep

Total: 5 weeks
Estimated Effort: 40 developer-days
```

## Risk Mitigation

| Risk | Probability | Impact | Mitigation |
|------|------------|--------|-----------|
| Payment gateway integration delays | Medium | High | Start early, use mock in dev |
| Inventory system incompatibility | Medium | High | Early integration testing |
| Performance issues with shipment split | Low | High | Index database, optimize queries |
| Bug in return workflow | Low | Medium | Comprehensive testing |

## Deployment Plan

### Staging Deployment (Week 5, Wed)
- Deploy all services
- Run integration tests
- Performance testing
- Security review

### UAT (Week 5, Thu-Fri)
- Test all scenarios
- Gather feedback
- Fix critical bugs

### Production Deployment (Following week)
- Deploy during off-peak hours
- Monitor error rates
- Have rollback plan ready

## Post-Implementation

### Monitoring
- Order creation success rate
- Payment processing success rate
- Shipment split accuracy
- Return processing time

### Metrics to Track
- Average order-to-delivery time
- Return rate
- Payment failure rate
- Customer satisfaction

### Future Enhancements
- Multi-currency support
- Subscription orders
- Advanced shipment routing
- ML-based delivery optimization

---

**Document Owner**: Development Team  
**Last Updated**: December 2025  
**Status**: Ready for Implementation
