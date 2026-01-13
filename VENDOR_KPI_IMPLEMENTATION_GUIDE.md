# Vendor Performance Indicators (KPIs) - Implementation Guide

## Overview
Complete API implementation for Vendor Performance Indicators tracking conversion rate, average order value, return rate, order preparation time, and buy box win rate with historical trend analysis.

## Files Created

### 1. DTOs (Data Transfer Objects)
Location: `src/Shared/Shared/DTOs/VendorDashboard/`

- **ConversionRateDto.cs** - Conversion rate metric DTO
  - `TotalProductViews`: Estimated product views
  - `TotalOrders`: Number of orders placed
  - `ConversionRatePercentage`: Calculated conversion %
  - `PercentageChange`: Change from previous period
  - `Period`: Time period covered

- **AverageOrderValueDto.cs** - Average order value metric DTO
  - `TotalRevenue`: Total revenue from orders
  - `TotalOrders`: Number of orders
  - `AverageOrderValue`: Calculated AOV
  - `MaxOrderValue` / `MinOrderValue`: Price range
  - `CurrencyCode`: Currency of values
  - `PercentageChange`: Trend comparison

- **ReturnRateDto.cs** - Return rate metric DTO
  - `TotalDeliveredOrders`: Delivered order count
  - `TotalReturnedOrders`: Returned order count
  - `ReturnRatePercentage`: Return %
  - `RefundRequests` / `RefundsProcessed`: Refund details
  - `TotalRefundAmount`: Total refund value

- **OrderPreparationTimeDto.cs** - Preparation time metric DTO
  - `AveragePreparationTimeHours`: Avg preparation hours
  - `TotalOrdersAnalyzed`: Orders in calculation
  - `MinimumPreparationTimeHours` / `MaximumPreparationTimeHours`: Range
  - `OrdersWithinSLA`: Orders meeting 24-hour SLA
  - `SLACompliancePercentage`: SLA % compliance
  - `DelayedOrders`: Orders beyond SLA

- **BuyBoxWinRateDto.cs** - Buy box win rate metric DTO
  - `TotalProductsOffered`: Total unique products
  - `BuyBoxWins`: Products winning buy box
  - `BuyBoxWinRatePercentage`: Win rate %
  - `TopProducts`: List of top performing products
  - `CompetitionFactors`: Price, rating, shipping scores
  - `Recommendations`: Improvement suggestions

- **VendorPerformanceIndicatorsDto.cs** - Comprehensive KPI dashboard
  - Contains all 5 KPI metrics
  - `OverallHealthScore`: Weighted score (0-100)
  - `HealthStatus`: Status badge (Excellent/Good/Fair/Poor)
  - `CalculatedAt`: Timestamp
  - `ReportPeriod`: Period covered

### 2. Service Interface
Location: `src/Core/BL/Contracts/Service/VendorDashboard/`

- **IVendorPerformanceIndicatorsService.cs** - Service contract
  - `GetAllPerformanceIndicatorsAsync()` - Get all KPIs
  - `GetConversionRateAsync()` - Conversion rate specific
  - `GetAverageOrderValueAsync()` - AOV specific
  - `GetReturnRateAsync()` - Return rate specific
  - `GetOrderPreparationTimeAsync()` - Prep time specific
  - `GetBuyBoxWinRateAsync()` - Buy box specific
  - `GetKPITrendAsync()` - Historical trend data

### 3. Service Implementation
Location: `src/Core/BL/Services/VendorDashboard/`

- **VendorPerformanceIndicatorsService.cs** - Core business logic
  - Calculates all KPI metrics from order data
  - Supports multiple time periods (Week, Month, Year, 30 Days)
  - Compares with previous period for trend analysis
  - Generates health score based on weighted metrics
  - Tracks historical trends over multiple months
  - Error handling and logging throughout

### 4. API Controller
Location: `src/Presentation/Api/Controllers/v1/VendorDashboard/`

- **VendorPerformanceIndicatorsController.cs** - REST endpoints
  - `GET /all` - All performance indicators
  - `GET /conversion-rate` - Conversion rate
  - `GET /average-order-value` - Average order value
  - `GET /return-rate` - Return rate
  - `GET /preparation-time` - Order preparation time
  - `GET /buybox-win-rate` - Buy box win rate
  - `GET /trend/{kpiType}` - Historical trends
  - Full validation and error handling
  - Swagger/OpenAPI documentation
  - Role-based authorization (Vendor)

### 5. Service Extensions
Location: `src/Presentation/Api/Extensions/Services/`

- **VendorPerformanceIndicatorsServiceExtensions.cs** - DI registration
  - Extension method for easy service registration
  - Used in `ECommerceExtensions.cs`

### 6. Documentation
- **VENDOR_PERFORMANCE_INDICATORS_API.md** - Complete API documentation
  - All endpoints with examples
  - Response models
  - KPI definitions
  - Health score calculation
  - Integration examples

## Setup Instructions

### 1. Database Considerations
No new tables required. Service uses existing tables:
- `TbOrder` - Order data
- `TbOrderDetail` - Order line items
- `TbOrderShipment` - Shipment tracking

### 2. Dependency Injection Configuration
Already configured in `src/Presentation/Api/Extensions/ECommerceExtensions.cs`:
```csharp
services.AddScoped<IVendorPerformanceIndicatorsService, VendorPerformanceIndicatorsService>();
```

### 3. Authorization
- All endpoints require `Vendor` role authorization
- User ID extracted from claims: `GuidUserId` (from base controller)
- 401 Unauthorized response if not authenticated/authorized

### 4. API Versioning
- API Version: 1.0
- Route: `api/v{version:apiVersion}/vendor/performance-indicators`
- Swagger documentation included

## Usage Examples

### Get All Indicators
```
GET /api/v1.0/vendor/performance-indicators/all?period=CurrentMonth
Authorization: Bearer <token>
```

### Get Specific Metric
```
GET /api/v1.0/vendor/performance-indicators/conversion-rate?period=Last30Days
Authorization: Bearer <token>
```

### Get Historical Trend
```
GET /api/v1.0/vendor/performance-indicators/trend/AOV?months=12
Authorization: Bearer <token>
```

### Get Buy Box Analysis
```
GET /api/v1.0/vendor/performance-indicators/buybox-win-rate?topProducts=20&period=CurrentMonth
Authorization: Bearer <token>
```

## KPI Calculations

### Conversion Rate
- **Formula:** (Total Orders / Total Product Views) × 100
- **Views Estimation:** Orders × 33
- **Target:** 2-5% (benchmark)

### Average Order Value (AOV)
- **Formula:** Total Revenue / Total Orders
- **Impact:** Profitability indicator
- **Trends:** Week-over-week/month-over-month

### Return Rate
- **Formula:** (Returned Orders / Delivered Orders) × 100
- **Benchmark:** < 10% (industry: 15-30%)
- **Tracks:** Refund requests and amounts

### Order Preparation Time
- **Formula:** Average (Shipment Time - Order Time)
- **SLA:** 24 hours standard
- **Compliance:** % of orders within SLA
- **Estimation:** 12 hours if shipment data unavailable

### Buy Box Win Rate
- **Formula:** (Products Winning Buy Box / Total Products) × 100
- **Factors:** Price competitiveness, rating, shipping speed
- **Target:** > 90%
- **Insights:** Top products and competitor analysis

## Health Score Calculation

Weighted formula combining all KPIs:
- Conversion Rate: 20 points max (2% of score)
- Average Order Value: 20 points (baseline)
- Return Rate: 20 points max (lower returns = higher)
- Preparation Time: 20 points (SLA compliance / 5)
- Buy Box Win Rate: 20 points max

**Status Levels:**
- Excellent: 80-100
- Good: 60-79
- Fair: 40-59
- Poor: 0-39

## Period Support
- `CurrentMonth` - First day of current month to now
- `CurrentWeek` - Start of week to now
- `CurrentYear` - January 1st to now
- `Last30Days` - Last 30 days from today

## Error Handling

### Validation Errors (400)
- Invalid period values
- Invalid KPI type
- Invalid topProducts range
- Invalid months range

### Authentication Errors (401)
- Missing authorization token
- Invalid user ID
- Insufficient permissions

### Server Errors (500)
- Database connection issues
- Calculation errors
- Internal service failures

## Testing Checklist

- [ ] All endpoints return 200 with valid data
- [ ] Period parameter filtering works correctly
- [ ] Percentage change calculation is accurate
- [ ] Health score is properly calculated
- [ ] 401 returned for unauthorized access
- [ ] 400 returned for invalid parameters
- [ ] Trend data shows 6+ months of history
- [ ] Buy box top products ranked correctly
- [ ] SLA compliance calculation accurate
- [ ] Response times < 2 seconds

## Performance Considerations

1. **Query Optimization:** Service uses in-memory LINQ for calculations
   - For large datasets, consider pagination
   - Cache results for 15-30 minutes

2. **Database Calls:** Single call to get all orders per vendor per period
   - Filter by vendor ID in order details
   - Future: Add database indexes on VendorId, CreatedDateUtc

3. **Calculation Complexity:** O(n) for each metric
   - Trend data: O(n * m) where m = months
   - Consider async processing for large vendors

## Future Enhancements

1. **Caching Layer**
   - Cache KPI results for 30 minutes
   - Invalidate on order status changes

2. **Analytics Database**
   - Dedicated table for KPI snapshots
   - Faster trend queries

3. **Advanced Metrics**
   - Customer lifetime value (CLV)
   - Product-specific KPIs
   - Geographic performance analysis

4. **Alerts & Notifications**
   - Alert when metrics drop below threshold
   - Weekly/monthly KPI reports

5. **Comparative Analysis**
   - Peer comparison (vs other vendors)
   - Category benchmarks

6. **Export Functionality**
   - PDF report generation
   - Excel export with charts

## Troubleshooting

### No Data Returned
- Check if vendor has any orders in the period
- Verify vendor ID is correct
- Ensure orders have CreatedDateUtc set

### Incorrect Calculations
- Verify order status enumeration values
- Check date range logic for the period
- Ensure order details have VendorId set

### Performance Issues
- Reduce month range in trend queries
- Add database indexes on VendorId, CreatedDateUtc
- Consider caching for frequently requested metrics

## API Monitoring

### Key Metrics to Track
- Average response time per endpoint
- Error rate (4xx, 5xx responses)
- Most requested KPI type
- Period parameters distribution

### Logging
- All exceptions logged with context
- Request/response logging in production
- Performance metrics tracking

## Build & Deployment

### Prerequisites
- .NET 10 SDK
- Visual Studio 2022+ or VS Code
- SQL Server (for order data)

### Build Command
```bash
dotnet build
```

### Publish Command
```bash
dotnet publish -c Release
```

### No Database Migrations Required
All tables already exist in the system.

## Summary

This implementation provides comprehensive vendor performance analytics through 5 key metrics with trend analysis, enabling vendors to monitor their business health and identify improvement opportunities. The service integrates seamlessly with the existing vendor dashboard infrastructure and provides actionable insights through calculated health scores and competitive analysis.
