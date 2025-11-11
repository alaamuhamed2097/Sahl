# Statistics Components Documentation

## Overview
This directory contains reusable statistics widgets for the dashboard with lazy loading support.

## Components

### LazyComponent
**Location**: `Dashboard/Components/LazyComponent.razor`

A wrapper component that provides lazy loading functionality with a loading state.

**Usage**:
```razor
<LazyComponent LoadingText="Loading your data...">
    <YourComponent />
</LazyComponent>
```

**Parameters**:
- `ChildContent` (RenderFragment): The component to be lazy loaded
- `LoadingText` (string): Custom loading message (default: "Loading...")

---

### AdminStatisticsWidget
**Location**: `Dashboard/Components/Statistics/AdminStatisticsWidget.razor`

Dashboard widget showing admin-level statistics and metrics.

**Features**:
- Total Orders count
- Total Revenue
- Total Products
- Total Users
- Sales Overview chart
- Recent Orders activity

**Authorization**: Requires Admin role

**Usage**:
```razor
<LazyComponent>
    <AdminStatisticsWidget />
</LazyComponent>
```

---

### VendorStatisticsWidget
**Location**: `Dashboard/Components/Statistics/VendorStatisticsWidget.razor`

Dashboard widget for vendor-specific statistics and performance metrics.

**Features**:
- My Orders count
- My Earnings
- Pending Orders
- Completed Orders
- Sales Performance chart
- Quick Stats (progress indicators)
- Recent Activity timeline

**Authorization**: Requires Vendor role

**Usage**:
```razor
<LazyComponent>
    <VendorStatisticsWidget />
</LazyComponent>
```

---

## Styling

All components use the dashboard styling defined in:
- `wwwroot/assets/css/pages/dashboard.css`
- `wwwroot/css/home-lazy-loading.css`

## Charts

Charts are initialized using the existing `statistic.js` file which provides Flot.js integration.

## TODO

### API Integration
Both statistics widgets currently use mock data. Update the `OnInitializedAsync` methods to call actual services:

```csharp
// Example for AdminStatisticsWidget
protected override async Task OnInitializedAsync()
{
    TotalOrders = await AdminStatisticsService.GetTotalOrdersAsync();
    TotalRevenue = await AdminStatisticsService.GetTotalRevenueAsync();
    TotalProducts = await AdminStatisticsService.GetTotalProductsAsync();
    TotalUsers = await AdminStatisticsService.GetTotalUsersAsync();
    
    isLoading = false;
}
```

### Localization
Replace hardcoded strings with resource file references:
```razor
<!-- Current -->
<div class="stat-widget-label">Total Orders</div>

<!-- Should be -->
<div class="stat-widget-label">@ECommerceResources.TotalOrders</div>
```

### Real-time Updates
Consider implementing SignalR for real-time statistics updates.
