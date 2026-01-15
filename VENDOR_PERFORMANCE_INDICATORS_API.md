# Vendor Performance Indicators APIs - Documentation

## Overview

Comprehensive APIs for **Vendor Performance Indicators (KPIs)** that measure and analyze key metrics critical to vendor success. These include conversion rate, average order value, return rate, order preparation time, and buy box win rate.

## API Endpoints

### 1. GET `/api/v1.0/vendor/performance-indicators/all`
**Get All Performance Indicators**

Returns comprehensive KPI metrics dashboard combining all performance indicators.

**Query Parameters:**
- `period` (optional, default: "CurrentMonth")
  - Valid values: `CurrentMonth`, `CurrentWeek`, `CurrentYear`, `Last30Days`

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Data retrieved",
  "data": {
    "vendorId": "550e8400-e29b-41d4-a716-446655440000",
    "conversionRate": {
      "totalProductViews": 1500,
      "totalOrders": 45,
      "conversionRatePercentage": 3.0,
      "percentageChange": 5.2,
      "period": "CurrentMonth"
    },
    "averageOrderValue": {
      "totalRevenue": 4500.00,
      "totalOrders": 45,
      "averageOrderValue": 100.00,
      "currencyCode": "USD",
      "maxOrderValue": 500.00,
      "minOrderValue": 25.00,
      "percentageChange": 8.5,
      "period": "CurrentMonth"
    },
    "returnRate": {
      "totalDeliveredOrders": 42,
      "totalReturnedOrders": 3,
      "returnRatePercentage": 7.14,
      "refundRequests": 3,
      "refundsProcessed": 3,
      "totalRefundAmount": 150.00,
      "currencyCode": "USD",
      "percentageChange": -2.1,
      "period": "CurrentMonth"
    },
    "orderPreparationTime": {
      "averagePreparationTimeHours": 12.5,
      "totalOrdersAnalyzed": 40,
      "minimumPreparationTimeHours": 2,
      "maximumPreparationTimeHours": 22,
      "ordersWithinSLA": 38,
      "slaCompliancePercentage": 95,
      "delayedOrders": 2,
      "percentageChange": 1.5,
      "period": "CurrentMonth"
    },
    "buyBoxWinRate": {
      "totalProductsOffered": 25,
      "buyBoxWins": 23,
      "buyBoxWinRatePercentage": 92,
      "averageCompetitors": 3,
      "topProducts": [
        {
          "productId": "550e8400-e29b-41d4-a716-446655440001",
          "productName": "Premium Laptop",
          "buyBoxWins": 10,
          "isCurrentBuyBoxHolder": true,
          "currentPrice": 999.99,
          "averageCompetitorPrice": 1050.00,
          "priceDifference": -50.01,
          "vendorRating": 4.5
        }
      ],
      "competitionFactors": {
        "priceCompetitivenessScore": 75,
        "ratingCompetitivenessScore": 85,
        "shippingCompetitivenessScore": 80,
        "overallBuyBoxEligibilityScore": 80,
        "recommendations": [
          "Maintain competitive pricing",
          "Improve shipping speed",
          "Maintain high product quality"
        ]
      },
      "percentageChange": 3.2,
      "period": "CurrentMonth"
    },
    "overallHealthScore": 82.5,
    "healthStatus": "Excellent",
    "calculatedAt": "2024-01-15T14:45:00Z",
    "reportPeriod": "CurrentMonth"
  }
}
```

---

### 2. GET `/api/v1.0/vendor/performance-indicators/conversion-rate`
**Get Conversion Rate Metric**

Measures the percentage of product views that result in orders.

**Query Parameters:**
- `period` (optional, default: "CurrentMonth")

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Data retrieved",
  "data": {
    "totalProductViews": 1500,
    "totalOrders": 45,
    "conversionRatePercentage": 3.0,
    "percentageChange": 5.2,
    "period": "CurrentMonth"
  }
}
```

**Interpretation:**
- High conversion rate indicates effective product listings and pricing
- Track trends to identify seasonal patterns
- Compare against industry benchmarks

---

### 3. GET `/api/v1.0/vendor/performance-indicators/average-order-value`
**Get Average Order Value (AOV) Metric**

Measures average spending per order including min/max values.

**Query Parameters:**
- `period` (optional, default: "CurrentMonth")

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Data retrieved",
  "data": {
    "totalRevenue": 4500.00,
    "totalOrders": 45,
    "averageOrderValue": 100.00,
    "currencyCode": "USD",
    "maxOrderValue": 500.00,
    "minOrderValue": 25.00,
    "percentageChange": 8.5,
    "period": "CurrentMonth"
  }
}
```

**Interpretation:**
- Higher AOV indicates better pricing strategy and upselling
- Min/Max values show customer spending range
- Positive percentage change shows growing customer value

---

### 4. GET `/api/v1.0/vendor/performance-indicators/return-rate`
**Get Return Rate Metric**

Measures percentage of delivered orders that are returned.

**Query Parameters:**
- `period` (optional, default: "CurrentMonth")

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Data retrieved",
  "data": {
    "totalDeliveredOrders": 42,
    "totalReturnedOrders": 3,
    "returnRatePercentage": 7.14,
    "refundRequests": 3,
    "refundsProcessed": 3,
    "totalRefundAmount": 150.00,
    "currencyCode": "USD",
    "percentageChange": -2.1,
    "period": "CurrentMonth"
  }
}
```

**Interpretation:**
- Lower return rate is better (industry avg: 15-30%)
- Track refund reasons to improve product quality
- Negative percentage change is positive trend

---

### 5. GET `/api/v1.0/vendor/performance-indicators/preparation-time`
**Get Order Preparation Time Metric**

Measures time from order placement to shipment.

**Query Parameters:**
- `period` (optional, default: "CurrentMonth")

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Data retrieved",
  "data": {
    "averagePreparationTimeHours": 12.5,
    "totalOrdersAnalyzed": 40,
    "minimumPreparationTimeHours": 2,
    "maximumPreparationTimeHours": 22,
    "ordersWithinSLA": 38,
    "slaCompliancePercentage": 95,
    "delayedOrders": 2,
    "percentageChange": 1.5,
    "period": "CurrentMonth"
  }
}
```

**Interpretation:**
- Lower preparation time improves customer satisfaction
- SLA Compliance % (24hr standard) is critical metric
- Track trends to optimize warehouse operations

---

### 6. GET `/api/v1.0/vendor/performance-indicators/buybox-win-rate`
**Get Buy Box Win Rate Metric**

Measures percentage of vendor's products winning the buy box.

**Query Parameters:**
- `topProducts` (optional, default: 10, max: 50) - Number of top products to return
- `period` (optional, default: "CurrentMonth")

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Data retrieved",
  "data": {
    "totalProductsOffered": 25,
    "buyBoxWins": 23,
    "buyBoxWinRatePercentage": 92,
    "averageCompetitors": 3,
    "topProducts": [
      {
        "productId": "550e8400-e29b-41d4-a716-446655440001",
        "productName": "Premium Laptop",
        "buyBoxWins": 10,
        "isCurrentBuyBoxHolder": true,
        "currentPrice": 999.99,
        "averageCompetitorPrice": 1050.00,
        "priceDifference": -50.01,
        "vendorRating": 4.5
      },
      {
        "productId": "550e8400-e29b-41d4-a716-446655440002",
        "productName": "Wireless Mouse",
        "buyBoxWins": 8,
        "isCurrentBuyBoxHolder": false,
        "currentPrice": 29.99,
        "averageCompetitorPrice": 28.50,
        "priceDifference": 1.49,
        "vendorRating": 4.2
      }
    ],
    "competitionFactors": {
      "priceCompetitivenessScore": 75,
      "ratingCompetitivenessScore": 85,
      "shippingCompetitivenessScore": 80,
      "overallBuyBoxEligibilityScore": 80,
      "recommendations": [
        "Maintain competitive pricing",
        "Improve shipping speed",
        "Maintain high product quality"
      ]
    },
    "percentageChange": 3.2,
    "period": "CurrentMonth"
  }
}
```

**Interpretation:**
- Buy box wins directly impact sales volume
- Analysis shows competitive positioning
- Recommendations guide improvement strategies

---

### 7. GET `/api/v1.0/vendor/performance-indicators/trend/{kpiType}`
**Get KPI Historical Trend**

Returns historical trend data for specific KPI over multiple months.

**Parameters:**
- `kpiType` (required in path) - Type of KPI to track
  - Valid values: `ConversionRate`, `AOV`, `ReturnRate`, `PreparationTime`, `BuyBoxWinRate`
- `months` (optional, default: 6, max: 24) - Number of months to retrieve

**Example:** `GET /api/v1.0/vendor/performance-indicators/trend/ConversionRate?months=12`

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Data retrieved",
  "data": {
    "kpiType": "ConversionRate",
    "currentValue": 3.0,
    "monthlyValues": [
      {
        "month": "2023-07",
        "value": 2.1
      },
      {
        "month": "2023-08",
        "value": 2.3
      },
      {
        "month": "2023-09",
        "value": 2.5
      },
      {
        "month": "2023-10",
        "value": 2.8
      },
      {
        "month": "2023-11",
        "value": 2.9
      },
      {
        "month": "2023-12",
        "value": 3.0
      }
    ],
    "averageValue": 2.6,
    "maxValue": 3.0,
    "minValue": 2.1,
    "trend": "Improving"
  }
}
```

**Trend Directions:**
- `Improving` - Value increasing over time (positive trend)
- `Declining` - Value decreasing over time (negative trend)
- `Stable` - Value relatively unchanged

---

## Error Responses

### 400 Bad Request
```json
{
  "success": false,
  "message": "Invalid period. Valid values: CurrentMonth, CurrentWeek, CurrentYear, Last30Days"
}
```

### 401 Unauthorized
```json
{
  "success": false,
  "message": "Invalid user ID"
}
```

### 500 Internal Server Error
```json
{
  "success": false,
  "message": "An error occurred while retrieving performance indicators"
}
```

---

## KPI Definitions

### Conversion Rate
**Definition:** Percentage of product views that result in orders
- **Formula:** (Total Orders / Total Product Views) × 100
- **Target:** 2-5% (industry benchmark)
- **Healthy Range:** 3%+

### Average Order Value (AOV)
**Definition:** Average amount spent per order
- **Formula:** Total Revenue / Total Orders
- **Impact:** Key metric for profitability
- **Optimization:** Cross-selling, upselling strategies

### Return Rate
**Definition:** Percentage of delivered orders that are returned
- **Formula:** (Returned Orders / Delivered Orders) × 100
- **Target:** < 10% (industry benchmark: 15-30%)
- **Drivers:** Product quality, description accuracy

### Order Preparation Time
**Definition:** Average hours from order placement to shipment
- **Formula:** Average (Shipment Time - Order Time)
- **SLA:** 24 hours (standard benchmark)
- **Compliance:** % of orders within SLA
- **Impact:** Customer satisfaction, competitive advantage

### Buy Box Win Rate
**Definition:** Percentage of vendor's products where they won the buy box
- **Formula:** (Products Winning Buy Box / Total Products Offered) × 100
- **Factors:** Price, rating, shipping speed, seller performance
- **Target:** > 90%

---

## Health Score Calculation

Overall Health Score (0-100) is calculated using weighted metrics:
- Conversion Rate: 20 points max
- Average Order Value: 20 points
- Return Rate: 20 points max (lower returns = higher score)
- Order Preparation Time: 20 points max (SLA compliance)
- Buy Box Win Rate: 20 points max

**Health Status:**
- **Excellent:** 80-100 points
- **Good:** 60-79 points
- **Fair:** 40-59 points
- **Poor:** 0-39 points

---

## Usage Examples

### Get All Indicators for Current Month
```bash
curl -X GET \
  "https://api.example.com/api/v1.0/vendor/performance-indicators/all?period=CurrentMonth" \
  -H "Authorization: Bearer <JWT_TOKEN>"
```

### Get Conversion Rate for Last 30 Days
```bash
curl -X GET \
  "https://api.example.com/api/v1.0/vendor/performance-indicators/conversion-rate?period=Last30Days" \
  -H "Authorization: Bearer <JWT_TOKEN>"
```

### Get 12-Month Trend for Average Order Value
```bash
curl -X GET \
  "https://api.example.com/api/v1.0/vendor/performance-indicators/trend/AOV?months=12" \
  -H "Authorization: Bearer <JWT_TOKEN>"
```

### Get Top 20 Buy Box Winners
```bash
curl -X GET \
  "https://api.example.com/api/v1.0/vendor/performance-indicators/buybox-win-rate?topProducts=20" \
  -H "Authorization: Bearer <JWT_TOKEN>"
```

---

## Data Freshness & Updates

- **Calculation Frequency:** Near real-time (calculated on request)
- **Data Latency:** 0-5 minutes
- **Historical Data:** Up to 24 months retained
- **Aggregation:** Order-level data aggregated by date

---

## Performance Tips

1. **Improve Conversion Rate:**
   - Optimize product descriptions
   - Improve product photography
   - Simplify checkout process

2. **Increase Average Order Value:**
   - Implement product bundling
   - Create volume discounts
   - Recommend complementary products

3. **Reduce Return Rate:**
   - Accurate product descriptions
   - High-quality images
   - Clear sizing/specification info

4. **Improve Preparation Time:**
   - Optimize warehouse operations
   - Implement automated picking
   - Allocate more staff during peak

5. **Win More Buy Boxes:**
   - Competitive pricing strategy
   - Maintain high seller rating
   - Fast shipping times
   - Strong order fulfillment

---

## Integration Guide

### In Blazor Component
```csharp
@inject HttpClient Http

private VendorPerformanceIndicatorsDto indicators;

protected override async Task OnInitializedAsync()
{
    var response = await Http.GetAsync(
        "/api/v1.0/vendor/performance-indicators/all?period=CurrentMonth");
    
    if (response.IsSuccessStatusCode)
    {
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ResponseModel<VendorPerformanceIndicatorsDto>>(json);
        indicators = result.Data;
    }
}
```

### In JavaScript
```javascript
const getPerformanceIndicators = async (period = 'CurrentMonth') => {
    const response = await fetch(
        `/api/v1.0/vendor/performance-indicators/all?period=${period}`,
        {
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            }
        }
    );
    return await response.json();
};
```

---

## Rate Limiting
(To be implemented based on requirements)

## Version History
- **v1.0** (Current) - Initial release with 5 KPI metrics
