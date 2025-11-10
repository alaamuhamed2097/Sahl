# Column Sorting Feature Implementation Guide

## Overview
This guide explains how to implement column sorting in your index pages. The feature allows users to sort table data by clicking on column headers, with visual indicators showing the current sort column and direction.

## Architecture

### Backend Changes

#### 1. BaseSearchCriteriaModel
Added two properties to support sorting:
```csharp
public string? SortBy { get; set; }  // Column name to sort by
public string SortDirection { get; set; } = "asc";  // "asc" or "desc"
```

#### 2. Service Layer (Example: UnitService)
Update the `GetPage` method to handle sorting:

```csharp
public PaginatedDataModel<UnitDto> GetPage(BaseSearchCriteriaModel criteriaModel)
{
    // ...existing validation code...

    // Create ordering function
    Func<IQueryable<VwUnitWithConversionsUnits>, IOrderedQueryable<VwUnitWithConversionsUnits>> orderBy = null;
    
    if (!string.IsNullOrWhiteSpace(criteriaModel.SortBy))
    {
        var sortBy = criteriaModel.SortBy.ToLower();
        var isDescending = criteriaModel.SortDirection?.ToLower() == "desc";

        orderBy = query =>
        {
            return sortBy switch
            {
                "titlear" => isDescending ? query.OrderByDescending(x => x.TitleAr) : query.OrderBy(x => x.TitleAr),
                "titleen" => isDescending ? query.OrderByDescending(x => x.TitleEn) : query.OrderBy(x => x.TitleEn),
                "title" => isDescending ? query.OrderByDescending(x => x.TitleAr) : query.OrderBy(x => x.TitleAr),
                "createddateutc" => isDescending ? query.OrderByDescending(x => x.CreatedDateUtc) : query.OrderBy(x => x.CreatedDateUtc),
                _ => query.OrderBy(x => x.TitleAr) // Default sorting
            };
        };
    }

    var entitiesList = _repository.GetPage(
        criteriaModel.PageNumber,
        criteriaModel.PageSize,
        filter,
        orderBy);  // Pass orderBy to repository
    
    // ...rest of method...
}
```

#### 3. SearchService (Frontend)
Updated to include sort parameters in API calls:

```csharp
public async Task<ResponseModel<PaginatedDataModel<T>>> SearchAsync(BaseSearchCriteriaModel model, string Endpoint)
{
    var queryString = $"PageNumber={model.PageNumber}&PageSize={model.PageSize}&SearchTerm={model.SearchTerm}";
    
    // Add sorting parameters if provided
    if (!string.IsNullOrWhiteSpace(model.SortBy))
    {
        queryString += $"&SortBy={model.SortBy}&SortDirection={model.SortDirection}";
    }
    
    string url = $"{Endpoint}?{queryString}";
    return await _apiService.GetAsync<PaginatedDataModel<T>>(url);
}
```

### Frontend Changes

#### 1. BaseListPage
Added two methods to handle sorting:

```csharp
/// <summary>
/// Handle column sorting
/// </summary>
protected virtual async Task SortByColumn(string columnName)
{
    if (searchModel.SortBy == columnName)
    {
        // Toggle sort direction if same column
        searchModel.SortDirection = searchModel.SortDirection == "asc" ? "desc" : "asc";
    }
    else
    {
        // New column, default to ascending
        searchModel.SortBy = columnName;
        searchModel.SortDirection = "asc";
    }

    // Reset to first page when sorting changes
    currentPage = 1;
    searchModel.PageNumber = 1;
    await Search();
}

/// <summary>
/// Get CSS class for sort icon
/// </summary>
protected string GetSortIconClass(string columnName)
{
    if (searchModel.SortBy != columnName)
        return "fas fa-sort text-muted";  // Neutral sort icon

    return searchModel.SortDirection == "asc" 
        ? "fas fa-sort-up text-primary"   // Ascending icon
        : "fas fa-sort-down text-primary"; // Descending icon
}
```

#### 2. Index Page (Razor Component)
Update table headers to be sortable:

```razor
<style>
    .cursor-pointer {
        cursor: pointer;
        user-select: none;
    }

    .cursor-pointer:hover {
        background-color: rgba(0, 0, 0, 0.05);
    }
</style>

<table class="table table-hover mb-0">
    <thead class="bg-light">
        <tr>
            <!-- Sortable column header -->
            <th class="text-center align-middle cursor-pointer" @onclick="() => SortByColumn(\"Title\")">
                @ECommerceResources.Title
                <i class="@GetSortIconClass(\"Title\") ms-1"></i>
            </th>
            
            <!-- Non-sortable column -->
            <th class="text-center align-middle">@ECommerceResources.Actions</th>
        </tr>
    </thead>
    <tbody>
        <!-- Table rows -->
    </tbody>
</table>
```

## Implementation Steps for New Pages

### Step 1: Backend Service
Update your service's `GetPage` method to support sorting:

```csharp
// Add sorting logic
Func<IQueryable<YourEntity>, IOrderedQueryable<YourEntity>> orderBy = null;

if (!string.IsNullOrWhiteSpace(criteriaModel.SortBy))
{
    var sortBy = criteriaModel.SortBy.ToLower();
    var isDescending = criteriaModel.SortDirection?.ToLower() == "desc";

    orderBy = query =>
    {
        return sortBy switch
        {
            "columnname1" => isDescending ? query.OrderByDescending(x => x.Property1) : query.OrderBy(x => x.Property1),
            "columnname2" => isDescending ? query.OrderByDescending(x => x.Property2) : query.OrderBy(x => x.Property2),
            _ => query.OrderBy(x => x.DefaultProperty)
        };
    };
}

// Pass orderBy to repository
var result = _repository.GetPage(pageNumber, pageSize, filter, orderBy);
```

### Step 2: Frontend Index Page
1. Add CSS styles for sortable columns
2. Make column headers clickable with sort icons
3. Ensure the page inherits from `BaseListPage<T>`

```razor
@inherits BaseListPage<YourDto>

<style>
    .cursor-pointer {
        cursor: pointer;
        user-select: none;
    }
    .cursor-pointer:hover {
        background-color: rgba(0, 0, 0, 0.05);
    }
</style>

<th class="text-center cursor-pointer" @onclick="() => SortByColumn(\"ColumnName\")">
    @ResourceLabel
    <i class="@GetSortIconClass(\"ColumnName\") ms-1"></i>
</th>
```

## Sortable Column Names

Use lowercase for consistency. Common examples:

- `"title"` - General title field
- `"titlear"` - Arabic title
- `"titleen"` - English title  
- `"createddateutc"` - Creation date
- `"updateddateutc"` - Update date
- `"price"` - Price field
- `"quantity"` - Quantity field
- `"displayorder"` - Display order

## Visual Indicators

The sort icons provide clear visual feedback:

| State | Icon | Color |
|-------|------|-------|
| Not sorted | `fa-sort` | Gray (text-muted) |
| Ascending | `fa-sort-up` | Blue (text-primary) |
| Descending | `fa-sort-down` | Blue (text-primary) |

## Best Practices

1. **Column Names**: Use meaningful, lowercase column names that match your entity properties
2. **Default Sorting**: Always provide a default sorting option in your switch statement
3. **Performance**: Add database indexes on frequently sorted columns
4. **User Experience**: 
   - Make it obvious which columns are sortable (cursor pointer, hover effect)
   - Show clear visual feedback for the current sort state
   - Reset to page 1 when sort changes
5. **Consistency**: Use the same column names across frontend and backend

## Example: Multiple Sortable Columns

```razor
<thead class="bg-light">
    <tr>
        <th class="text-center cursor-pointer" @onclick="() => SortByColumn(\"Title\")">
            @ECommerceResources.Title
            <i class="@GetSortIconClass(\"Title\") ms-1"></i>
        </th>
        <th class="text-center cursor-pointer" @onclick="() => SortByColumn(\"CreatedDateUtc\")">
            @FormResources.CreationDate
            <i class="@GetSortIconClass(\"CreatedDateUtc\") ms-1"></i>
        </th>
        <th class="text-center cursor-pointer" @onclick="() => SortByColumn(\"Price\")">
            @FormResources.Price
            <i class="@GetSortIconClass(\"Price\") ms-1"></i>
        </th>
        <th class="text-center">@ECommerceResources.Actions</th>
    </tr>
</thead>
```

## Troubleshooting

### Sort Not Working
- Check that `SortBy` parameter matches entity property names (case-insensitive)
- Verify the backend service includes sorting logic
- Ensure `SearchService` passes sort parameters in query string

### Icons Not Showing
- Verify Font Awesome is loaded
- Check CSS classes are correct (`fas fa-sort`, `fa-sort-up`, `fa-sort-down`)
- Ensure `GetSortIconClass` method is called correctly

### Performance Issues
- Add database indexes on sorted columns
- Consider caching for frequently accessed sorted data
- Use projection/select to limit data retrieved

## Testing

1. **Click column header** - Should sort ascending
2. **Click same header again** - Should toggle to descending  
3. **Click different header** - Should change sort column and reset to ascending
4. **Change page** - Sort order should persist
5. **Search** - Sort order should persist
6. **Change page size** - Sort order should persist

## Summary

The sorting feature is now fully integrated across all layers:
- ? Backend: Services handle sorting logic with LINQ OrderBy/OrderByDescending
- ? API: Controllers pass sort parameters from query string
- ? Frontend: BaseListPage provides reusable sorting methods
- ? UI: Visual indicators show current sort state
- ? Responsive: Works with pagination and search

Apply these patterns to any new index pages that need sorting functionality.
