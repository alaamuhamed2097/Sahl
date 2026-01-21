# Implementation Plan - Order Details & Shipment Status View

## Completed Tasks
- [x] **Backend Integration**
    - Created `IShipmentService` interface in Dashboard contracts.
    - Implemented `ShipmentService` in Dashboard services to fetch data from API.
    - Updated `ShipmentDto` to include `StatusHistory` list.
    - Updated `ApiEndpoints` with new shipment endpoints.
    - Registered `ShipmentService` in Dependency Injection.
    - **CRITIAL UPDATE**: Added `Include(s => s.StatusHistory)` to `ShipmentRepository.GetOrderShipmentsAsync` query.
    - **CRITICAL UPDATE**: Configured AutoMapper profiles for `TbOrderShipment` <-> `ShipmentDto` and `TbShipmentStatusHistory` <-> `ShipmentStatusHistoryDto`.

- [x] **Frontend Implementation (Order Details Page)**
    - Injected `IShipmentService` into `Details.razor.cs`.
    - Implemented `LoadShipments` method to fetch data asynchronously.
    - Added comprehensive Shipment Section to `Details.razor`:
        - **Header**: Shipment Number and Badge with Localized Status.
        - **Info Card**: Vendor Name, Shipping Company, Tracking Number (with color-coded visual hierarchy).
        - **Items Table**: Product image, name, quantity, unit price, and subtotal.
        - **Timeline**: Vertical visual timeline showing status history with location, dates, and notes.
        - **Delivery Info**: Estimated and Actual delivery dates.
    - Implemented `GetLocalizedStatus` helper to display Arabic status names properly.

## Verification
- Built the solution successfully (`Exit code: 0`).
- Verified code structure matches existing project patterns (Repositories, Services, DTOs).
- Ensured localization support for keys and status enums.

## Components Created/Modified
- `src/Presentation/Dashboard/Pages/Sales/Orders/Details.razor` (UI)
- `src/Presentation/Dashboard/Pages/Sales/Orders/Details.razor.cs` (Logic)
- `src/Presentation/Dashboard/Services/Order/ShipmentService.cs` (Service)
- `src/Presentation/Dashboard/Contracts/Order/IShipmentService.cs` (Interface)
- `src/Presentation/Dashboard/Constants/ApiEndpoints.cs` (Constants)
- `src/Core/BL/Mapper/Order/OrderMappingProfile.cs` (Mapping)
- `src/Infrastructure/DAL/Repositories/Order/ShipmentRepository.cs` (Data Access)
- `src/Shared/Shared/DTOs/Order/Fulfillment/Shipment/ShipmentDto.cs` (Data Model)
