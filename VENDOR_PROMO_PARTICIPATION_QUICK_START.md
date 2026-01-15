# Quick Start Guide: Vendor Promo Code Participation API

## Overview
This guide helps you quickly get started with the Vendor Promo Code Participation API.

## Prerequisites
- Valid JWT authentication token for Vendor role
- Base API URL (e.g., `https://api.example.com`)

## Quick Start Examples

### 1. Submit a Participation Request

#### Using cURL
```bash
curl -X POST "https://api.example.com/api/v1/vendorpromocodeparticipation/submit" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "promoCodeId": "550e8400-e29b-41d4-a716-446655440000",
    "descriptionEn": "Interested in participating in this summer sale promo",
    "descriptionAr": "???? ????????? ?? ??? ????? ??????",
    "notes": "Store is ready with inventory"
  }'
```

#### Using JavaScript/Fetch
```javascript
const submitRequest = async (promoCodeId, token) => {
  const response = await fetch(
    'https://api.example.com/api/v1/vendorpromocodeparticipation/submit',
    {
      method: 'POST',
      headers: {
        'Authorization': `Bearer ${token}`,
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        promoCodeId: promoCodeId,
        descriptionEn: 'Interested in this promo code',
        descriptionAr: '???? ???? ????? ????????'
      })
    }
  );
  
  const data = await response.json();
  return data;
};

// Usage
submitRequest('550e8400-e29b-41d4-a716-446655440000', 'YOUR_TOKEN')
  .then(result => console.log(result));
```

#### Using C# HttpClient
```csharp
using (var httpClient = new HttpClient())
{
    httpClient.DefaultRequestHeaders.Authorization = 
        new AuthenticationHeaderValue("Bearer", token);
    
    var request = new CreateVendorPromoCodeParticipationRequestDto
    {
        PromoCodeId = Guid.Parse("550e8400-e29b-41d4-a716-446655440000"),
        DescriptionEn = "Interested in this promo code",
        DescriptionAr = "???? ???? ????? ????????"
    };
    
    var json = JsonSerializer.Serialize(request);
    var content = new StringContent(json, Encoding.UTF8, "application/json");
    
    var response = await httpClient.PostAsync(
        "https://api.example.com/api/v1/vendorpromocodeparticipation/submit",
        content
    );
    
    var responseContent = await response.Content.ReadAsStringAsync();
    var result = JsonSerializer.Deserialize<ResponseModel<VendorPromoCodeParticipationRequestDto>>(responseContent);
}
```

### 2. List Your Participation Requests

#### Using cURL
```bash
curl -X POST "https://api.example.com/api/v1/vendorpromocodeparticipation/list" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "pageNumber": 1,
    "pageSize": 10,
    "searchTerm": "summer"
  }'
```

#### Using JavaScript/Fetch
```javascript
const listRequests = async (pageNumber = 1, pageSize = 10, searchTerm = '', token) => {
  const response = await fetch(
    'https://api.example.com/api/v1/vendorpromocodeparticipation/list',
    {
      method: 'POST',
      headers: {
        'Authorization': `Bearer ${token}`,
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        pageNumber: pageNumber,
        pageSize: pageSize,
        searchTerm: searchTerm
      })
    }
  );
  
  const data = await response.json();
  return data;
};

// Usage - Get first page
listRequests(1, 10, '', 'YOUR_TOKEN')
  .then(result => {
    console.log(`Total requests: ${result.data.totalRecords}`);
    console.log('Requests:', result.data.items);
  });

// Usage - Search for "summer"
listRequests(1, 10, 'summer', 'YOUR_TOKEN')
  .then(result => console.log(result.data.items));
```

### 3. Get Request Details

#### Using cURL
```bash
curl -X GET "https://api.example.com/api/v1/vendorpromocodeparticipation/660e8400-e29b-41d4-a716-446655440000" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

#### Using JavaScript/Fetch
```javascript
const getRequest = async (requestId, token) => {
  const response = await fetch(
    `https://api.example.com/api/v1/vendorpromocodeparticipation/${requestId}`,
    {
      method: 'GET',
      headers: {
        'Authorization': `Bearer ${token}`
      }
    }
  );
  
  const data = await response.json();
  return data;
};

// Usage
getRequest('660e8400-e29b-41d4-a716-446655440000', 'YOUR_TOKEN')
  .then(result => {
    if (result.success) {
      console.log('Request Status:', result.data.statusName);
      console.log('Promo Code:', result.data.promoCodeValue);
      console.log('Submitted:', result.data.submittedAt);
    } else {
      console.error('Error:', result.message);
    }
  });
```

### 4. Cancel a Participation Request

#### Using cURL
```bash
curl -X DELETE "https://api.example.com/api/v1/vendorpromocodeparticipation/660e8400-e29b-41d4-a716-446655440000" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

#### Using JavaScript/Fetch
```javascript
const cancelRequest = async (requestId, token) => {
  const response = await fetch(
    `https://api.example.com/api/v1/vendorpromocodeparticipation/${requestId}`,
    {
      method: 'DELETE',
      headers: {
        'Authorization': `Bearer ${token}`
      }
    }
  );
  
  const data = await response.json();
  return data;
};

// Usage
cancelRequest('660e8400-e29b-41d4-a716-446655440000', 'YOUR_TOKEN')
  .then(result => {
    if (result.success) {
      console.log('Request cancelled successfully');
    } else {
      console.error('Error:', result.message);
    }
  });
```

## Common Response Codes

### Success Responses

#### 200 OK - Request Successful
```json
{
  "success": true,
  "message": "Participation request submitted successfully",
  "data": { /* response data */ }
}
```

### Error Responses

#### 400 Bad Request - Invalid Input
```json
{
  "success": false,
  "message": "Promo code not found",
  "data": null
}
```

#### 401 Unauthorized - Missing Token
```json
{
  "success": false,
  "message": "Unauthorized",
  "data": null
}
```

#### 404 Not Found - Request Not Found
```json
{
  "success": false,
  "message": "Request not found",
  "data": null
}
```

#### 409 Conflict - Duplicate Request
```json
{
  "success": false,
  "message": "Vendor already has a pending or approved request for this promo code",
  "data": null
}
```

## Request Status Values

| Status | Value | Description |
|--------|-------|-------------|
| Pending | 0 | Awaiting admin review |
| Approved | 1 | Admin approved the request |
| Rejected | 2 | Admin rejected the request |
| Withdrawn | 3 | Vendor withdrew the request |
| Archived | 4 | Request archived |

## Workflows

### Complete Participation Request Flow

```
1. Vendor finds desired promo code
   ?
2. Vendor submits participation request (POST /submit)
   ?
3. Request created with Status=Pending
   ?
4. Vendor can view requests (POST /list, GET /{id})
   ?
5. Admin reviews request
   ?
6. Status changes to Approved or Rejected
   ?
7. If still Pending, vendor can cancel (DELETE /{id})
```

### Search and Filter

```javascript
// Get all requests
listRequests(1, 10, '', token);

// Search for summer promo
listRequests(1, 10, 'summer', token);

// Get next page
listRequests(2, 10, 'summer', token);

// Change page size
listRequests(1, 20, '', token);
```

## Best Practices

### 1. Error Handling
Always check the `success` property before using `data`:

```javascript
const response = await listRequests(1, 10, '', token);
if (response.success) {
  // Use response.data safely
  console.log(response.data.items);
} else {
  // Handle error
  console.error('API Error:', response.message);
}
```

### 2. Token Management
Keep tokens secure and refresh before expiration:

```javascript
// Don't do this
const token = 'hardcoded_token';

// Do this instead
const token = getTokenFromSecureStorage();
if (isTokenExpiring()) {
  token = await refreshToken();
}
```

### 3. Pagination
Handle pagination properly for large datasets:

```javascript
const allRequests = [];
let pageNumber = 1;
let hasMore = true;

while (hasMore) {
  const response = await listRequests(pageNumber, 10, '', token);
  allRequests.push(...response.data.items);
  hasMore = pageNumber < response.data.totalPages;
  pageNumber++;
}
```

### 4. Search Implementation
Combine search with pagination:

```javascript
async function searchRequests(term, token) {
  const response = await listRequests(1, 10, term, token);
  return {
    items: response.data.items,
    totalResults: response.data.totalRecords,
    pageCount: response.data.totalPages
  };
}
```

## Troubleshooting

### Issue: 401 Unauthorized
**Solution**: Ensure your token is valid and hasn't expired
```javascript
// Check token
console.log('Token:', token);
// Refresh token
token = await getNewToken(credentials);
```

### Issue: 404 Not Found
**Solution**: Verify the request ID exists
```javascript
// First list requests to get valid IDs
const list = await listRequests(1, 10, '', token);
const requestId = list.data.items[0].id; // Use valid ID
```

### Issue: 409 Conflict
**Solution**: Check existing requests before submitting
```javascript
// Search for existing requests for this promo code
const existing = await listRequests(1, 10, 'PROMO_CODE', token);
if (existing.data.totalRecords > 0) {
  console.log('Request already exists');
} else {
  // Safe to submit new request
  await submitRequest(promoCodeId, token);
}
```

### Issue: 400 Bad Request
**Solution**: Validate input before submitting
```javascript
// Validate promo code ID format
if (!isValidGuid(promoCodeId)) {
  console.error('Invalid promo code ID');
  return;
}
// Ensure required fields
if (!promoCodeId) {
  console.error('PromoCodeId is required');
  return;
}
```

## Integration with Your Application

### React Component Example
```typescript
import React, { useState } from 'react';

const VendorPromoParticipation: React.FC = () => {
  const [requests, setRequests] = useState([]);
  const [loading, setLoading] = useState(false);
  const [token] = useState(getToken());

  const handleSubmitRequest = async (promoCodeId: string) => {
    setLoading(true);
    try {
      const response = await fetch(
        'https://api.example.com/api/v1/vendorpromocodeparticipation/submit',
        {
          method: 'POST',
          headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
          },
          body: JSON.stringify({
            promoCodeId: promoCodeId,
            descriptionEn: 'Interested in this promo'
          })
        }
      );
      const data = await response.json();
      if (data.success) {
        alert('Request submitted successfully');
        loadRequests();
      } else {
        alert(`Error: ${data.message}`);
      }
    } finally {
      setLoading(false);
    }
  };

  const loadRequests = async () => {
    setLoading(true);
    try {
      const response = await fetch(
        'https://api.example.com/api/v1/vendorpromocodeparticipation/list',
        {
          method: 'POST',
          headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
          },
          body: JSON.stringify({ pageNumber: 1, pageSize: 10 })
        }
      );
      const data = await response.json();
      if (data.success) {
        setRequests(data.data.items);
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <button onClick={loadRequests} disabled={loading}>
        Load Requests
      </button>
      {/* Display requests */}
    </div>
  );
};

export default VendorPromoParticipation;
```

## Next Steps

1. ? Test the API with your vendor account
2. ? Integrate into your dashboard/application
3. ? Implement proper error handling
4. ? Add loading states and user feedback
5. ? Set up request status monitoring
6. ? Implement search and filtering UI

## Support

For API documentation, see: `README_VENDOR_PROMO_PARTICIPATION.md`

For implementation details, see: `VENDOR_PROMO_PARTICIPATION_IMPLEMENTATION.md`
