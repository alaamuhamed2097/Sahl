# Vendor Performance Indicators APIs - Quick Reference

## Base URL
```
https://api.example.com/api/v1.0/vendor/performance-indicators
```

## Authentication
All endpoints require:
```
Authorization: Bearer <JWT_TOKEN>
```

## Endpoints Summary

### 1. All Performance Indicators
```
GET /all?period=CurrentMonth
```
**Returns:** Complete KPI dashboard with all 5 metrics + health score

**Example Response:**
```json
{
  "data": {
    "vendorId": "550e8400-e29b-41d4-a716-446655440000",
    "conversionRate": { ... },
    "averageOrderValue": { ... },
    "returnRate": { ... },
    "orderPreparationTime": { ... },
    "buyBoxWinRate": { ... },
    "overallHealthScore": 82.5,
    "healthStatus": "Excellent"
  }
}
```

### 2. Conversion Rate
```
GET /conversion-rate?period=CurrentMonth
```
**Key Metrics:**
- TotalProductViews: long
- TotalOrders: int
- ConversionRatePercentage: decimal
- PercentageChange: decimal?

### 3. Average Order Value
```
GET /average-order-value?period=CurrentMonth
```
**Key Metrics:**
- TotalRevenue: decimal
- TotalOrders: int
- AverageOrderValue: decimal
- MaxOrderValue / MinOrderValue: decimal
- CurrencyCode: string

### 4. Return Rate
```
GET /return-rate?period=CurrentMonth
```
**Key Metrics:**
- TotalDeliveredOrders: int
- TotalReturnedOrders: int
- ReturnRatePercentage: decimal
- RefundRequests / RefundsProcessed: int
- TotalRefundAmount: decimal

### 5. Preparation Time
```
GET /preparation-time?period=CurrentMonth
```
**Key Metrics:**
- AveragePreparationTimeHours: decimal
- TotalOrdersAnalyzed: int
- SLACompliancePercentage: decimal
- OrdersWithinSLA / DelayedOrders: int

### 6. Buy Box Win Rate
```
GET /buybox-win-rate?topProducts=10&period=CurrentMonth
```
**Key Metrics:**
- BuyBoxWinRatePercentage: decimal
- TotalProductsOffered / BuyBoxWins: int
- TopProducts: List<BuyBoxProductDto>
- CompetitionFactors: Detailed scoring

### 7. Historical Trends
```
GET /trend/ConversionRate?months=6
GET /trend/AOV?months=12
GET /trend/ReturnRate?months=6
GET /trend/PreparationTime?months=6
GET /trend/BuyBoxWinRate?months=6
```
**Returns:**
- KPIType: string
- CurrentValue: decimal
- MonthlyValues: List<KPIMonthlyValueDto>
- Trend: "Improving" | "Declining" | "Stable"

## Query Parameters

| Parameter | Type | Default | Range | Description |
|-----------|------|---------|-------|-------------|
| period | string | CurrentMonth | CurrentMonth, CurrentWeek, CurrentYear, Last30Days | Time period for analysis |
| topProducts | int | 10 | 1-50 | For buybox endpoint only |
| months | int | 6 | 1-24 | For trend endpoint only |

## Response Codes

| Code | Meaning | Common Reasons |
|------|---------|----------------|
| 200 | Success | Data retrieved successfully |
| 400 | Bad Request | Invalid parameters |
| 401 | Unauthorized | Missing/invalid token, not Vendor role |
| 500 | Server Error | Processing failure |

## cURL Examples

### Get All KPIs
```bash
curl -X GET "https://api.example.com/api/v1.0/vendor/performance-indicators/all?period=CurrentMonth" \
  -H "Authorization: Bearer eyJhbGc..." \
  -H "Content-Type: application/json"
```

### Get Conversion Rate
```bash
curl -X GET "https://api.example.com/api/v1.0/vendor/performance-indicators/conversion-rate?period=Last30Days" \
  -H "Authorization: Bearer eyJhbGc..."
```

### Get 12-Month AOV Trend
```bash
curl -X GET "https://api.example.com/api/v1.0/vendor/performance-indicators/trend/AOV?months=12" \
  -H "Authorization: Bearer eyJhbGc..."
```

### Get Top 20 Buy Box Products
```bash
curl -X GET "https://api.example.com/api/v1.0/vendor/performance-indicators/buybox-win-rate?topProducts=20&period=CurrentMonth" \
  -H "Authorization: Bearer eyJhbGc..."
```

## C# Client Example

```csharp
using System.Net.Http.Headers;
using System.Text.Json;

var client = new HttpClient();
client.DefaultRequestHeaders.Authorization = 
    new AuthenticationHeaderValue("Bearer", token);

var response = await client.GetAsync(
    "https://api.example.com/api/v1.0/vendor/performance-indicators/all?period=CurrentMonth");

if (response.IsSuccessStatusCode)
{
    var json = await response.Content.ReadAsStringAsync();
    var result = JsonSerializer.Deserialize<ResponseModel<VendorPerformanceIndicatorsDto>>(json);
    var indicators = result.Data;
    
    Console.WriteLine($"Health Score: {indicators.OverallHealthScore}");
    Console.WriteLine($"Status: {indicators.HealthStatus}");
}
```

## JavaScript/TypeScript Example

```javascript
const response = await fetch(
    '/api/v1.0/vendor/performance-indicators/all?period=CurrentMonth',
    {
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        }
    }
);

if (response.ok) {
    const result = await response.json();
    const indicators = result.data;
    
    console.log(`Conversion Rate: ${indicators.conversionRate.conversionRatePercentage}%`);
    console.log(`Avg Order Value: $${indicators.averageOrderValue.averageOrderValue}`);
    console.log(`Return Rate: ${indicators.returnRate.returnRatePercentage}%`);
    console.log(`Health: ${indicators.healthStatus}`);
}
```

## Important Notes

1. **Vendor ID**: Automatically extracted from JWT token claims
2. **Time Zones**: All timestamps are UTC
3. **Currency**: Each response includes currency code (default: USD)
4. **Caching**: Responses not cached; always current data
5. **Permissions**: Only vendors can access their own metrics

## KPI Interpretation Guide

### Conversion Rate
- **Good:** 3%+ (benchmark 2-5%)
- **Improving:** Positive % change month-over-month
- **Action:** Optimize product descriptions, improve pricing

### Average Order Value
- **Growing AOV:** Positive % change indicates healthy business
- **High Max Value:** Indicates successful high-ticket sales
- **Action:** Cross-sell, bundle, volume discounts

### Return Rate
- **Good:** < 10%
- **Bad:** > 15%
- **Improving:** Negative % change (fewer returns)
- **Action:** Review product quality, descriptions accuracy

### Preparation Time
- **SLA Compliant:** > 90% within 24 hours
- **Improving:** Lower average hours month-over-month
- **Action:** Optimize warehouse, increase staffing during peaks

### Buy Box Win Rate
- **Excellent:** > 90%
- **Good:** 70-90%
- **Fair:** 50-70%
- **Action:** Follow recommendations in CompetitionFactors

## Health Score Breakdown

Excellent (80+) = Well-balanced strong performance across all metrics
Good (60-79) = Solid performance with some areas for improvement
Fair (40-59) = Multiple metrics need attention
Poor (0-39) = Critical issues across most metrics

## Support & Troubleshooting

- **No data returned?** Check if you have orders in the selected period
- **Unauthorized?** Verify JWT token includes Vendor role
- **Wrong metrics?** Ensure period parameter is spelled correctly
- **Performance slow?** Reduce month range in trend queries

---

Last Updated: 2024
API Version: 1.0
