# OrderService: Testing Guide

## Overview
This guide provides test cases and scenarios for validating the fixed OrderService implementation.

---

## Unit Test Scenarios

### 1. ReserveStockForCartItems Tests

#### Test: Batch Loading Performance
```csharp
[Fact]
public async Task ReserveStockForCartItems_BatchLoadsAllPricings()
{
    // Arrange
    var cartItems = new List<CartItemDto>
    {
        new CartItemDto { OfferCombinationPricingId = pricingId1, Quantity = 1, ItemId = item1 },
        new CartItemDto { OfferCombinationPricingId = pricingId2, Quantity = 2, ItemId = item2 },
        new CartItemDto { OfferCombinationPricingId = pricingId3, Quantity = 1, ItemId = item3 }
    };

    using var logger = new QueryCountingLogger();

    // Act
    var result = await service.ReserveStockForCartItems(customerId, cartItems);

    // Assert
    Assert.Single(logger.Queries.Where(q => q.Contains("TbOfferCombinationPricing")));
    Assert.Equal(3, result.Count); // Mapping for 3 items
}
```

#### Test: Stock Validation
```csharp
[Fact]
public async Task ReserveStockForCartItems_ThrowsWhenInsufficientStock()
{
    // Arrange
    var pricing = new TbOfferCombinationPricing { Id = pricingId, AvailableQuantity = 2 };
    var cartItem = new CartItemDto { OfferCombinationPricingId = pricingId, Quantity = 5 };

    // Act & Assert
    await Assert.ThrowsAsync<InvalidOperationException>(
        () => service.ReserveStockForCartItems(customerId, new() { cartItem }));
}
```

#### Test: Returns Pricing-to-Offer Mapping
```csharp
[Fact]
public async Task ReserveStockForCartItems_ReturnsMappingOfPricingToOffers()
{
    // Arrange
    var pricing1 = new TbOfferCombinationPricing { Id = pricingId1, OfferId = offerId1 };
    var pricing2 = new TbOfferCombinationPricing { Id = pricingId2, OfferId = offerId1 };
    var pricing3 = new TbOfferCombinationPricing { Id = pricingId3, OfferId = offerId2 };

    var cartItems = new List<CartItemDto> { ... };

    // Act
    var result = await service.ReserveStockForCartItems(customerId, cartItems);

    // Assert
    Assert.Equal(offerId1, result[pricingId1]);
    Assert.Equal(offerId1, result[pricingId2]);
    Assert.Equal(offerId2, result[pricingId3]);
}
```

---

### 2. CreateOrderDetails Tests

#### Test: Batch Loads Pricings and Offers
```csharp
[Fact]
public async Task CreateOrderDetails_BatchLoadsAllData()
{
    // Arrange
    var pricingToOfferMap = new Dictionary<Guid, Guid>
    {
        { pricingId1, offerId1 },
        { pricingId2, offerId1 },
        { pricingId3, offerId2 }
    };

    var cartItems = new List<CartItemDto> { /* 10 items */ };
    var orderDetails = new List<TbOrderDetail> { /* created earlier */ };

    using var logger = new QueryCountingLogger();

    // Act
    var result = await service.CreateOrderDetails(orderId, cartItems, userId, pricingToOfferMap);

    // Assert
    // Should have exactly 2 SELECT queries: 1 for pricings, 1 for offers
    var selectQueries = logger.Queries.Where(q => q.StartsWith("SELECT")).Count();
    Assert.Equal(2, selectQueries);
    Assert.Equal(10, result.Count);
}
```

#### Test: Sets Correct Vendor IDs
```csharp
[Fact]
public async Task CreateOrderDetails_SetsCorrectVendorIdsFromOffers()
{
    // Arrange
    var offer1 = new TbOffer { Id = offerId1, VendorId = vendorIdA };
    var offer2 = new TbOffer { Id = offerId2, VendorId = vendorIdB };

    var pricingToOfferMap = new Dictionary<Guid, Guid>
    {
        { pricingId1, offerId1 },
        { pricingId2, offerId2 }
    };

    var cartItems = new List<CartItemDto>
    {
        new CartItemDto { ItemId = item1, OfferCombinationPricingId = pricingId1 },
        new CartItemDto { ItemId = item2, OfferCombinationPricingId = pricingId2 }
    };

    // Act
    var result = await service.CreateOrderDetails(orderId, cartItems, userId, pricingToOfferMap);

    // Assert
    Assert.Equal(vendorIdA, result[0].VendorId);
    Assert.Equal(vendorIdB, result[1].VendorId);
}
```

---

### 3. CreateOrderShipmentsFromDetails Tests

#### Test: Creates Correct Number of Shipments
```csharp
[Fact]
public async Task CreateOrderShipmentsFromDetails_GroupsByVendorAndWarehouse()
{
    // Arrange
    var orderDetails = new List<TbOrderDetail>
    {
        new TbOrderDetail { VendorId = vendorA, WarehouseId = warehouse1 },
        new TbOrderDetail { VendorId = vendorA, WarehouseId = warehouse1 },
        new TbOrderDetail { VendorId = vendorB, WarehouseId = warehouse2 },
        new TbOrderDetail { VendorId = vendorC, WarehouseId = warehouse1 }
    };

    var pricingToOfferMap = new Dictionary<Guid, Guid>
    {
        { pricing1, offer1 },
        { pricing2, offer1 },
        { pricing3, offer2 },
        { pricing4, offer3 }
    };

    // Act
    var result = await service.CreateOrderShipmentsFromDetails(orderId, orderDetails, pricingToOfferMap, userId);

    // Assert
    Assert.Equal(3, result.Count); // 3 unique (vendor, warehouse) combinations
    Assert.Contains(result, s => s.VendorId == vendorA && s.WarehouseId == warehouse1);
    Assert.Contains(result, s => s.VendorId == vendorB && s.WarehouseId == warehouse2);
    Assert.Contains(result, s => s.VendorId == vendorC && s.WarehouseId == warehouse1);
}
```

#### Test: Calculates Correct Estimated Delivery Dates
```csharp
[Fact]
public async Task CreateOrderShipmentsFromDetails_UsesMaximumEstimatedDays()
{
    // Arrange
    var offer1 = new TbOffer { Id = offerId1, HandlingTimeInDays = 2 };
    var offer2 = new TbOffer { Id = offerId2, HandlingTimeInDays = 5 };

    var shipping1 = new TbShippingDetail { OfferId = offerId1, MaximumEstimatedDays = 3 };
    var shipping2 = new TbShippingDetail { OfferId = offerId2, MaximumEstimatedDays = 2 };

    // Offer1: 2 + 3 = 5 days
    // Offer2: 5 + 2 = 7 days
    // Expected: max(5, 7) = 7 days

    var orderDetails = new List<TbOrderDetail>
    {
        new TbOrderDetail { VendorId = vendorA, WarehouseId = warehouse1, OfferCombinationPricingId = pricing1 },
        new TbOrderDetail { VendorId = vendorA, WarehouseId = warehouse1, OfferCombinationPricingId = pricing2 }
    };

    var pricingToOfferMap = new Dictionary<Guid, Guid>
    {
        { pricing1, offerId1 },
        { pricing2, offerId2 }
    };

    // Act
    var result = await service.CreateOrderShipmentsFromDetails(orderId, orderDetails, pricingToOfferMap, userId);

    // Assert
    var shipment = result.First();
    var expectedDate = DateTime.UtcNow.AddDays(7);
    Assert.NotNull(shipment.EstimatedDeliveryDate);
    Assert.Equal(expectedDate.Date, shipment.EstimatedDeliveryDate.Value.Date);
}
```

#### Test: Batch Loads Offers and Shipping Details
```csharp
[Fact]
public async Task CreateOrderShipmentsFromDetails_BatchLoadsOffers()
{
    // Arrange
    var orderDetails = new List<TbOrderDetail> { /* many items */ };
    var pricingToOfferMap = new Dictionary<Guid, Guid> { /* many mappings */ };

    using var logger = new QueryCountingLogger();

    // Act
    var result = await service.CreateOrderShipmentsFromDetails(orderId, orderDetails, pricingToOfferMap, userId);

    // Assert
    var selectQueries = logger.Queries.Where(q => q.StartsWith("SELECT")).Count();
    // Should have 2: one for offers, one for shipping details
    Assert.Equal(2, selectQueries);
}
```

---

### 4. CreateOrderFromCartAsync Integration Tests

#### Test: Single Vendor Order
```csharp
[Fact]
public async Task CreateOrderFromCartAsync_SingleVendor_CreatesOrderSuccessfully()
{
    // Arrange
    var customerId = Guid.NewGuid().ToString();
    var cartItems = new List<CartItemDto>
    {
        new CartItemDto { ItemId = item1, OfferCombinationPricingId = pricing1, Quantity = 2 },
        new CartItemDto { ItemId = item2, OfferCombinationPricingId = pricing2, Quantity = 1 }
    };

    var request = new CreateOrderFromCartRequest
    {
        DeliveryAddressId = addressId,
        PaymentMethodId = paymentMethodId
    };

    // Act
    var result = await service.CreateOrderFromCartAsync(customerId, request);

    // Assert
    Assert.NotNull(result);
    Assert.NotEqual(Guid.Empty, result.OrderId);
    Assert.Equal(3, result.SubTotal + result.TaxAmount + result.ShippingAmount); // Math check
}
```

#### Test: Multi-Vendor Order
```csharp
[Fact]
public async Task CreateOrderFromCartAsync_MultiVendor_CreatesCorrectShipments()
{
    // Arrange
    var customerId = Guid.NewGuid().ToString();
    var cartItems = new List<CartItemDto>
    {
        new CartItemDto { ItemId = item1, OfferCombinationPricingId = pricing1, Quantity = 1 }, // Vendor A
        new CartItemDto { ItemId = item2, OfferCombinationPricingId = pricing2, Quantity = 1 }, // Vendor B
        new CartItemDto { ItemId = item3, OfferCombinationPricingId = pricing3, Quantity = 2 }  // Vendor A
    };

    // Act
    var result = await service.CreateOrderFromCartAsync(customerId, new CreateOrderFromCartRequest { ... });

    // Assert
    Assert.NotNull(result);
    
    // Verify 2 shipments created (one per vendor)
    var shipments = db.TbOrderShipment
        .Where(s => s.OrderId == result.OrderId)
        .ToList();
    Assert.Equal(2, shipments.Count);
}
```

#### Test: Cart Cleared After Order
```csharp
[Fact]
public async Task CreateOrderFromCartAsync_AfterSuccess_ClearsCart()
{
    // Arrange
    var customerId = Guid.NewGuid().ToString();
    var cartSummary = await cartService.GetCartSummaryAsync(customerId);
    Assert.NotEmpty(cartSummary.Items); // Cart has items

    // Act
    var result = await service.CreateOrderFromCartAsync(customerId, request);

    // Assert
    var cartAfter = await cartService.GetCartSummaryAsync(customerId);
    Assert.Empty(cartAfter.Items); // Cart is empty
}
```

#### Test: Transaction Rollback on Failure
```csharp
[Fact]
public async Task CreateOrderFromCartAsync_OnStockError_RollsBackTransaction()
{
    // Arrange
    var cartItems = new List<CartItemDto>
    {
        new CartItemDto { OfferCombinationPricingId = pricingWithNoStock, Quantity = 100 }
    };

    // Act & Assert
    await Assert.ThrowsAsync<InvalidOperationException>(
        () => service.CreateOrderFromCartAsync(customerId, request));

    // Verify no order was created
    var order = db.TbOrder.FirstOrDefault(o => o.UserId == customerId);
    Assert.Null(order);
}
```

#### Test: Consistent User IDs in Audit Trail
```csharp
[Fact]
public async Task CreateOrderFromCartAsync_SetsConsistentUserIds()
{
    // Arrange
    var customerId = Guid.NewGuid().ToString();
    var customerIdGuid = Guid.Parse(customerId);

    // Act
    var result = await service.CreateOrderFromCartAsync(customerId, request);

    // Assert
    var order = db.TbOrder.Find(result.OrderId);
    Assert.Equal(customerIdGuid, order.CreatedBy);

    var orderDetails = db.TbOrderDetail.Where(od => od.OrderId == result.OrderId);
    foreach (var detail in orderDetails)
        Assert.Equal(customerIdGuid, detail.CreatedBy);

    var payment = db.TbOrderPayment.First(p => p.OrderId == result.OrderId);
    Assert.Equal(customerIdGuid, payment.CreatedBy); // Changed from Guid.Empty

    var shipments = db.TbOrderShipment.Where(s => s.OrderId == result.OrderId);
    foreach (var shipment in shipments)
        Assert.Equal(customerIdGuid, shipment.CreatedBy); // Changed from Guid.Empty
}
```

---

## Performance Tests

### Test: Query Count Verification
```csharp
[Fact]
public async Task CreateOrderFromCartAsync_OptimizedQueryCount()
{
    // Arrange
    var cartItems = new List<CartItemDto> { /* 10 items */ };
    using var logger = new QueryCountingLogger();

    // Act
    await service.CreateOrderFromCartAsync(customerId, request);

    // Assert
    // Expected: ~5 queries (not 20+)
    // - 1 currency lookup
    // - 1 pricing batch load
    // - 1 offer batch load
    // - 1 shipping detail batch load
    // - 1 shipment insert
    Assert.InRange(logger.SelectQueries.Count, 3, 6);
}
```

### Test: Guid Parsing Optimization
```csharp
[Fact]
public async Task CreateOrderFromCartAsync_ParsesGuidOnce()
{
    // Arrange
    var customerId = "550e8400-e29b-41d4-a716-446655440000";
    using var logger = new MethodCallLogger();

    // Act
    await service.CreateOrderFromCartAsync(customerId, request);

    // Assert
    var guidParseCalls = logger.GetMethodCalls(typeof(Guid), nameof(Guid.Parse));
    Assert.Single(guidParseCalls); // Only one parse call
}
```

---

## Edge Cases & Error Scenarios

### Test: Missing Pricing Record
```csharp
[Fact]
public async Task CreateOrderFromCartAsync_ThrowsWhenPricingNotFound()
{
    // Arrange
    var cartItem = new CartItemDto
    {
        OfferCombinationPricingId = Guid.NewGuid() // Non-existent
    };

    // Act & Assert
    await Assert.ThrowsAsync<InvalidOperationException>(
        () => service.CreateOrderFromCartAsync(customerId, request));
}
```

### Test: Missing Currency
```csharp
[Fact]
public async Task CreateOrderFromCartAsync_ThrowsWhenCurrencyNotFound()
{
    // Arrange
    db.TbCurrency.RemoveRange(db.TbCurrency);
    db.SaveChanges();

    // Act & Assert
    await Assert.ThrowsAsync<NotFoundException>(
        () => service.CreateOrderFromCartAsync(customerId, request));
}
```

### Test: Empty Cart
```csharp
[Fact]
public async Task CreateOrderFromCartAsync_ThrowsWhenCartEmpty()
{
    // Arrange
    var cartSummary = new CartSummaryDto { Items = new() };

    // Act & Assert
    await Assert.ThrowsAsync<InvalidOperationException>(
        () => service.CreateOrderFromCartAsync(customerId, request));
}
```

---

## Regression Tests

### Test: Ensures No N+1 Queries
```csharp
[Theory]
[InlineData(5)]
[InlineData(10)]
[InlineData(20)]
[InlineData(50)]
public async Task CreateOrderFromCartAsync_NoNPlusOneQueries(int itemCount)
{
    // Arrange
    var cartItems = Enumerable.Range(0, itemCount)
        .Select(i => new CartItemDto { ... })
        .ToList();

    using var logger = new QueryCountingLogger();

    // Act
    await service.CreateOrderFromCartAsync(customerId, request);

    // Assert
    // Query count should NOT increase linearly with item count
    var expectedMaxQueries = 10; // Constant, regardless of item count
    Assert.True(logger.SelectQueries.Count <= expectedMaxQueries,
        $"Query count ({logger.SelectQueries.Count}) exceeded expected maximum ({expectedMaxQueries})");
}
```

---

## Test Data Setup

### Helper: Create Test Offers and Pricings
```csharp
private (List<CartItemDto> items, Dictionary<Guid, Guid> mapping) CreateTestCart(
    int vendorCount, 
    int itemsPerVendor)
{
    var items = new List<CartItemDto>();
    var mapping = new Dictionary<Guid, Guid>();

    for (int v = 0; v < vendorCount; v++)
    {
        var vendor = new TbVendor { /* ... */ };
        db.TbVendor.Add(vendor);

        for (int i = 0; i < itemsPerVendor; i++)
        {
            var offer = new TbOffer { VendorId = vendor.Id, HandlingTimeInDays = v + 2 };
            db.TbOffer.Add(offer);

            var pricing = new TbOfferCombinationPricing
            {
                OfferId = offer.Id,
                AvailableQuantity = 100
            };
            db.TbOfferCombinationPricing.Add(pricing);

            items.Add(new CartItemDto
            {
                OfferCombinationPricingId = pricing.Id,
                Quantity = 1
            });

            mapping[pricing.Id] = offer.Id;
        }
    }

    db.SaveChanges();
    return (items, mapping);
}
```

---

## CI/CD Integration

### GitHub Actions Example
```yaml
name: OrderService Tests

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    services:
      sqlserver:
        image: mcr.microsoft.com/mssql/server:2019-latest
        env:
          ACCEPT_EULA: Y
          SA_PASSWORD: MyPassword123

    steps:
      - uses: actions/checkout@v2
      
      - name: Setup .NET
        uses: actions/setup-dotnet@v1
        with:
          dotnet-version: '10.0.x'
      
      - name: Run Unit Tests
        run: dotnet test tests/UnitTests/ -f net10.0
      
      - name: Run Integration Tests
        run: dotnet test IntegrationTests/ -f net10.0
      
      - name: Check Query Performance
        run: dotnet test IntegrationTests/Performance/ -f net10.0
```

---

## Success Criteria

? All unit tests pass  
? All integration tests pass  
? No N+1 queries detected  
? Multi-vendor orders create correct shipments  
? Audit trail shows correct user IDs  
? Transaction rollback works correctly  
? Cart clearing happens after transaction commit  
? Performance improves by 4x+  

