# Vendor Dashboard APIs - Quick Reference Guide

## Authentication
All endpoints require:
- JWT Bearer token in Authorization header
- Vendor role in the JWT claims
- User ID in the JWT subject claim

Example header:
```
Authorization: Bearer <JWT_TOKEN>
x-language: en (optional, for localization)
```

## Base URL
```
GET /api/v1.0/vendor/dashboard/{endpoint}
```

## Endpoints Summary

### 1. Dashboard Summary (Complete KPI Overview)
```
GET /api/v1.0/vendor/dashboard/summary
```

**Response Example:**
```json
{
  "success": true,
  "message": "Data retrieved",
  "data": {
    "vendorId": "550e8400-e29b-41d4-a716-446655440000",
    "dailySales": {
      "totalSales": 5250.50,
      "orderCount": 12,
      "currencyCode": "USD",
      "percentageChange": 15.5
    },
    "newOrders": {
      "totalNewOrders": 8,
      "pendingOrders": 3,
      "processingOrders": 4,
      "readyForShipmentOrders": 1,
      "percentageChange": 8.3
    },
    "bestSellingProducts": [
      {
        "productId": "550e8400-e29b-41d4-a716-446655440001",
        "productName": "Premium Laptop",
        "sku": "LAP-001",
        "quantitySold": 25,
        "revenue": 37500.00,
        "currencyCode": "USD",
        "averageRating": 4.5,
        "imageUrl": "/images/laptop.jpg"
      }
    ],
    "latestReviews": [
      {
        "reviewId": "550e8400-e29b-41d4-a716-446655440002",
        "itemId": "550e8400-e29b-41d4-a716-446655440001",
        "itemName": "Premium Laptop",
        "customerName": "550e8400-e29b-41d4-a716-446655440003",
        "rating": 5,
        "comment": "Great product, fast shipping!",
        "reviewDate": "2024-01-15T10:30:00Z",
        "status": "Approved"
      }
    ],
    "overallRating": 4.6,
    "totalProductsSold": 156,
    "totalReviews": 42,
    "generatedAt": "2024-01-15T14:45:00Z"
  }
}
```

### 2. Daily Sales Metrics
```
GET /api/v1.0/vendor/dashboard/daily-sales
```

**Response Example:**
```json
{
  "success": true,
  "message": "Data retrieved",
  "data": {
    "totalSales": 2150.75,
    "orderCount": 7,
    "currencyCode": "USD",
    "percentageChange": 12.5
  }
}
```

### 3. New Orders Status
```
GET /api/v1.0/vendor/dashboard/new-orders
```

**Response Example:**
```json
{
  "success": true,
  "message": "Data retrieved",
  "data": {
    "totalNewOrders": 8,
    "pendingOrders": 2,
    "processingOrders": 5,
    "readyForShipmentOrders": 1,
    "percentageChange": 6.7
  }
}
```

### 4. Best-Selling Products
```
GET /api/v1.0/vendor/dashboard/best-selling-products?limit=5
```

**Query Parameters:**
- `limit` (optional, default: 10, max: 100) - Number of products to return

**Response Example:**
```json
{
  "success": true,
  "message": "Data retrieved",
  "data": [
    {
      "productId": "550e8400-e29b-41d4-a716-446655440001",
      "productName": "Premium Laptop",
      "sku": "LAP-001",
      "quantitySold": 25,
      "revenue": 37500.00,
      "currencyCode": "USD",
      "averageRating": 4.5,
      "imageUrl": "/images/laptop.jpg"
    },
    {
      "productId": "550e8400-e29b-41d4-a716-446655440004",
      "productName": "Wireless Mouse",
      "sku": "MOU-002",
      "quantitySold": 85,
      "revenue": 2125.00,
      "currencyCode": "USD",
      "averageRating": 4.2,
      "imageUrl": "/images/mouse.jpg"
    }
  ]
}
```

### 5. Latest Customer Reviews
```
GET /api/v1.0/vendor/dashboard/latest-reviews?limit=5
```

**Query Parameters:**
- `limit` (optional, default: 5, max: 50) - Number of reviews to return

**Response Example:**
```json
{
  "success": true,
  "message": "Data retrieved",
  "data": [
    {
      "reviewId": "550e8400-e29b-41d4-a716-446655440002",
      "itemId": "550e8400-e29b-41d4-a716-446655440001",
      "itemName": "Premium Laptop",
      "customerName": "550e8400-e29b-41d4-a716-446655440003",
      "rating": 5,
      "comment": "Great product, fast shipping!",
      "reviewDate": "2024-01-15T10:30:00Z",
      "status": "Approved"
    },
    {
      "reviewId": "550e8400-e29b-41d4-a716-446655440005",
      "itemId": null,
      "itemName": "Vendor Review",
      "customerName": "550e8400-e29b-41d4-a716-446655440006",
      "rating": 4,
      "comment": "Good service and quality products",
      "reviewDate": "2024-01-14T15:20:00Z",
      "status": "Approved"
    }
  ]
}
```

## Error Responses

### 401 Unauthorized
```json
{
  "success": false,
  "message": "Invalid user ID"
}
```

### 400 Bad Request
```json
{
  "success": false,
  "message": "Limit must be between 1 and 100"
}
```

### 500 Internal Server Error
```json
{
  "success": false,
  "message": "An error occurred while retrieving the dashboard summary"
}
```

## Usage Examples

### cURL
```bash
# Get dashboard summary
curl -X GET \
  "https://api.example.com/api/v1.0/vendor/dashboard/summary" \
  -H "Authorization: Bearer <TOKEN>" \
  -H "x-language: en"

# Get best-selling products (top 5)
curl -X GET \
  "https://api.example.com/api/v1.0/vendor/dashboard/best-selling-products?limit=5" \
  -H "Authorization: Bearer <TOKEN>"
```

### JavaScript (Fetch)
```javascript
const headers = {
  'Authorization': `Bearer ${token}`,
  'x-language': 'en'
};

// Get dashboard summary
fetch('https://api.example.com/api/v1.0/vendor/dashboard/summary', {
  method: 'GET',
  headers: headers
})
.then(response => response.json())
.then(data => console.log(data));

// Get best-selling products
fetch('https://api.example.com/api/v1.0/vendor/dashboard/best-selling-products?limit=5', {
  method: 'GET',
  headers: headers
})
.then(response => response.json())
.then(data => console.log(data.data));
```

### JavaScript (Axios)
```javascript
const apiClient = axios.create({
  baseURL: 'https://api.example.com',
  headers: {
    'Authorization': `Bearer ${token}`,
    'x-language': 'en'
  }
});

// Get dashboard summary
apiClient.get('/api/v1.0/vendor/dashboard/summary')
  .then(response => console.log(response.data))
  .catch(error => console.error(error));

// Get best-selling products with limit
apiClient.get('/api/v1.0/vendor/dashboard/best-selling-products', {
  params: { limit: 5 }
})
  .then(response => console.log(response.data.data))
  .catch(error => console.error(error));
```

## Response Status Codes

| Status | Meaning |
|--------|---------|
| 200 | Successfully retrieved data |
| 400 | Bad request (invalid parameters) |
| 401 | Unauthorized (invalid/missing token) |
| 403 | Forbidden (insufficient permissions) |
| 500 | Server error |

## Rate Limiting
(To be implemented based on requirements)

## Caching Strategy
(Can be implemented to improve performance)

## Data Freshness
- Daily sales: Near real-time (calculated on each request)
- New orders: Real-time
- Best-selling products: Updated on each request
- Latest reviews: Real-time

## Notes
- All timestamps are in UTC (ISO 8601 format)
- All monetary values use the currency code specified
- Product names default to English title (TitleEn)
- Customer names currently show customer IDs (can be enhanced)
