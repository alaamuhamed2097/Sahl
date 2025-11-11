# ????? ????? ???? ??????? - ???? ??????

## ?? ????? ???: CityService ????? ????? ????

???? ??? ????? ?? ???? ???????? ??? ????? ??? ??????? ?????? ??:
**?????**: `src\Core\BL\Service\Location\CityService.cs`

### ???????:

1. ???? ????? `src\Core\BL\Service\Location\CityService.cs`
2. ???? ?? method `GetPage`
3. ??? ??? ????? **???** ????? `var entitiesList = _baseRepository.GetPage(...)`

```csharp
// Create ordering function based on SortBy and SortDirection
Func<IQueryable<TbCity>, IOrderedQueryable<TbCity>> orderBy = null;

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
```

4. ???? ??????? `GetPage` ????? ??????? `orderBy`:

**???**:
```csharp
var entitiesList = _baseRepository.GetPage(
    criteriaModel.PageNumber,
    criteriaModel.PageSize,
    filter);
```

**???**:
```csharp
var entitiesList = _baseRepository.GetPage(
    criteriaModel.PageNumber,
    criteriaModel.PageSize,
    filter,
    orderBy);
```

---

## ?? ??????? ???????? ???????

### 1. Currencies (???????)

#### Backend: `src\Core\BL\Service\Currency\CurrencyService.cs`

??? ?? method `GetPage`:

```csharp
Func<IQueryable<TbCurrency>, IOrderedQueryable<TbCurrency>> orderBy = null;

if (!string.IsNullOrWhiteSpace(criteriaModel.SortBy))
{
    var sortBy = criteriaModel.SortBy.ToLower();
    var isDescending = criteriaModel.SortDirection?.ToLower() == "desc";

    orderBy = query =>
    {
        return sortBy switch
        {
            "code" => isDescending ? query.OrderByDescending(x => x.Code) : query.OrderBy(x => x.Code),
            "name" => isDescending ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
            "exchangerate" => isDescending ? query.OrderByDescending(x => x.ExchangeRate) : query.OrderBy(x => x.ExchangeRate),
            "createddateutc" => isDescending ? query.OrderByDescending(x => x.CreatedDateUtc) : query.OrderBy(x => x.CreatedDateUtc),
            _ => query.OrderBy(x => x.Code)
        };
    };
}
```

#### Frontend: `src\Presentation\Dashboard\Pages\Currency\Index.razor`

1. ??? CSS ?? ????? ?????:
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
```

2. ???? ???? ??????:
```razor
<thead>
    <tr>
        <th class="cursor-pointer" @onclick='() => SortByColumn("Code")'>
            Code
            <i class="@GetSortIconClass("Code") ms-1"></i>
        </th>
        <th class="cursor-pointer" @onclick='() => SortByColumn("Name")'>
            Name
            <i class="@GetSortIconClass("Name") ms-1"></i>
        </th>
        <th class="cursor-pointer" @onclick='() => SortByColumn("ExchangeRate")'>
            Exchange Rate
            <i class="@GetSortIconClass("ExchangeRate") ms-1"></i>
        </th>
        <th>Actions</th>
    </tr>
</thead>
```

---

### 2. Orders (???????)

#### Backend: `src\Core\BL\Service\Order\OrderService.cs`

```csharp
Func<IQueryable<TbOrder>, IOrderedQueryable<TbOrder>> orderBy = null;

if (!string.IsNullOrWhiteSpace(criteriaModel.SortBy))
{
    var sortBy = criteriaModel.SortBy.ToLower();
    var isDescending = criteriaModel.SortDirection?.ToLower() == "desc";

    orderBy = query =>
    {
        return sortBy switch
        {
            "ordernumber" => isDescending ? query.OrderByDescending(x => x.OrderNumber) : query.OrderBy(x => x.OrderNumber),
            "customername" => isDescending ? query.OrderByDescending(x => x.CustomerName) : query.OrderBy(x => x.CustomerName),
            "orderdate" => isDescending ? query.OrderByDescending(x => x.OrderDate) : query.OrderBy(x => x.OrderDate),
            "totalprice" => isDescending ? query.OrderByDescending(x => x.TotalPrice) : query.OrderBy(x => x.TotalPrice),
            "orderstatus" => isDescending ? query.OrderByDescending(x => x.OrderStatus) : query.OrderBy(x => x.OrderStatus),
            _ => query.OrderByDescending(x => x.OrderDate) // Default: newest first
        };
    };
}
```

#### Frontend: `src\Presentation\Dashboard\Pages\Sales\Orders\Index.razor`

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

<!-- ?? ?????? -->
<thead>
    <tr>
        <th class="cursor-pointer" @onclick='() => SortByColumn("OrderNumber")'>
            Order #
            <i class="@GetSortIconClass("OrderNumber") ms-1"></i>
        </th>
        <th class="cursor-pointer" @onclick='() => SortByColumn("CustomerName")'>
            Customer
            <i class="@GetSortIconClass("CustomerName") ms-1"></i>
        </th>
        <th class="cursor-pointer" @onclick='() => SortByColumn("OrderDate")'>
            Date
            <i class="@GetSortIconClass("OrderDate") ms-1"></i>
        </th>
        <th class="cursor-pointer" @onclick='() => SortByColumn("TotalPrice")'>
            Total
            <i class="@GetSortIconClass("TotalPrice") ms-1"></i>
        </th>
        <th class="cursor-pointer" @onclick='() => SortByColumn("OrderStatus")'>
            Status
            <i class="@GetSortIconClass("OrderStatus") ms-1"></i>
        </th>
        <th>Actions</th>
    </tr>
</thead>
```

---

### 3. Pages (??????? ???????)

#### Backend: `src\Core\BL\Service\CMS\PageService.cs`

```csharp
Func<IQueryable<TbPage>, IOrderedQueryable<TbPage>> orderBy = null;

if (!string.IsNullOrWhiteSpace(criteriaModel.SortBy))
{
    var sortBy = criteriaModel.SortBy.ToLower();
    var isDescending = criteriaModel.SortDirection?.ToLower() == "desc";

    orderBy = query =>
    {
        return sortBy switch
        {
            "title" => isDescending ? query.OrderByDescending(x => x.Title) : query.OrderBy(x => x.Title),
            "slug" => isDescending ? query.OrderByDescending(x => x.Slug) : query.OrderBy(x => x.Slug),
            "createddateutc" => isDescending ? query.OrderByDescending(x => x.CreatedDateUtc) : query.OrderBy(x => x.CreatedDateUtc),
            _ => query.OrderBy(x => x.Title)
        };
    };
}
```

#### Frontend: `src\Presentation\Dashboard\Pages\Content\Pages\Index.razor`

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

<thead>
    <tr>
        <th class="cursor-pointer" @onclick='() => SortByColumn("Title")'>
            Title
            <i class="@GetSortIconClass("Title") ms-1"></i>
        </th>
        <th class="cursor-pointer" @onclick='() => SortByColumn("Slug")'>
            URL Slug
            <i class="@GetSortIconClass("Slug") ms-1"></i>
        </th>
        <th class="cursor-pointer" @onclick='() => SortByColumn("CreatedDateUtc")'>
            Created Date
            <i class="@GetSortIconClass("CreatedDateUtc") ms-1"></i>
        </th>
        <th>Actions</th>
    </tr>
</thead>
```

---

### 4. Administrators (????????)

#### Backend: `src\Core\BL\Service\User\AdminService.cs`

```csharp
Func<IQueryable<TbUser>, IOrderedQueryable<TbUser>> orderBy = null;

if (!string.IsNullOrWhiteSpace(criteriaModel.SortBy))
{
    var sortBy = criteriaModel.SortBy.ToLower();
    var isDescending = criteriaModel.SortDirection?.ToLower() == "desc";

    orderBy = query =>
    {
        return sortBy switch
        {
            "username" => isDescending ? query.OrderByDescending(x => x.UserName) : query.OrderBy(x => x.UserName),
            "email" => isDescending ? query.OrderByDescending(x => x.Email) : query.OrderBy(x => x.Email),
            "name" => isDescending ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
            "userstate" => isDescending ? query.OrderByDescending(x => x.UserState) : query.OrderBy(x => x.UserState),
            "createddateutc" => isDescending ? query.OrderByDescending(x => x.CreatedDateUtc) : query.OrderBy(x => x.CreatedDateUtc),
            _ => query.OrderBy(x => x.UserName)
        };
    };
}
```

#### Frontend: `src\Presentation\Dashboard\Pages\UserManagement\Administrators\Index.razor`

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

<thead>
    <tr>
        <th class="cursor-pointer" @onclick='() => SortByColumn("UserName")'>
            Username
            <i class="@GetSortIconClass("UserName") ms-1"></i>
        </th>
        <th class="cursor-pointer" @onclick='() => SortByColumn("Email")'>
            Email
            <i class="@GetSortIconClass("Email") ms-1"></i>
        </th>
        <th class="cursor-pointer" @onclick='() => SortByColumn("Name")'>
            Full Name
            <i class="@GetSortIconClass("Name") ms-1"></i>
        </th>
        <th class="cursor-pointer" @onclick='() => SortByColumn("UserState")'>
            Status
            <i class="@GetSortIconClass("UserState") ms-1"></i>
        </th>
        <th>Actions</th>
    </tr>
</thead>
```

---

## ? ????? ??????

??? ????? ?? ????? ???? ??:

- [ ] **Backend**: ?? ????? ??? ??????? ?? Service
- [ ] **Backend**: ?? ????? `orderBy` ??? `GetPage`
- [ ] **Frontend**: ?? ????? CSS ??? cursor
- [ ] **Frontend**: ?? ????? `@onclick` ??? ??? headers
- [ ] **Frontend**: ?? ????? ??????? ???????
- [ ] **Build**: ??????? ???? ???? ?????
- [ ] **Test**: ?????? ??????? ????????
- [ ] **Test**: ?????? ??????? ????????
- [ ] **Test**: ?????? ????? ?????????

---

## ?? ????? ??????

### 1. Database Indexes
??? Indexes ??? ??????? ????????? ???????:

```sql
-- ???? ?????????
CREATE INDEX IX_TbState_TitleAr ON TbState(TitleAr);
CREATE INDEX IX_TbState_TitleEn ON TbState(TitleEn);
CREATE INDEX IX_TbState_CreatedDateUtc ON TbState(CreatedDateUtc);

-- ???? ???????
CREATE INDEX IX_TbOrder_OrderDate ON TbOrder(OrderDate);
CREATE INDEX IX_TbOrder_TotalPrice ON TbOrder(TotalPrice);
CREATE INDEX IX_TbOrder_OrderStatus ON TbOrder(OrderStatus);
```

### 2. Default Sorting
???? ????? ??????? ????? ??? ????:
- **Orders**: OrderDate DESC (?????? ?????)
- **Pages**: Title ASC (?????)
- **Users**: UserName ASC (?????)
- **Currencies**: Code ASC (?????)

### 3. Pagination + Sorting
??????? ???? ?? Pagination ????????? ?? ???? ???????? ??????.

---

## ?? ??????? ??????? ???????

### ???????: ????????? ?? ????
**????**: ???? ?? ????? Font Awesome ?? `index.html`
```html
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />
```

### ???????: ??????? ?? ????
**??????? ????????**:
1. ??? ????? ????? ??????? ??? Frontend ? Backend
2. ??? ????? `orderBy` ??? `GetPage` ?? Service
3. ???????? (Repository) ?? ???? ????? `orderBy`

**??????**:
- ???? Network tab ?? ???????
- ???? ?? ????? `SortBy` ? `SortDirection` ?? ??? Query String
- ?? Breakpoint ?? Service ???? ???? `criteriaModel.SortBy`

### ???????: ??? Compilation ?? Razor
**????**: ???? ?? ??????? ?????? ?????? ?????:
```razor
@onclick='() => SortByColumn("ColumnName")'
NOT: @onclick="() => SortByColumn(\"ColumnName\")"
```

---

## ?? ?????

??? ????? ?????:
1. ???? `docs/SORTING_FEATURE_GUIDE.md` ?????? ??????
2. ???? `docs/SORTING_QUICK_GUIDE_AR.md` ?????? ??????
3. ???? ??????? ???????? ??????:
   - Units
   - States
   - ShippingCompanies
   - Categories

---

## ? ??? ???????

??? ????? ???? ???????:
1. ?? ?????? Build ????: `dotnet build`
2. ????? ?? ???? ??? ???
3. ???? ?? ??? ??????? ?? ????? ????????
4. ???? ?????? ??? ?????? ?????
5. ??? Indexes ??? ??? ?????

**???????! ??** ????? ?? ????? ???? ??????? ??? ???? ????? Index ?? ???????.
