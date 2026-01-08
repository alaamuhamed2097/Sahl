using Common.Enumerations.Fulfillment;

namespace BL.Contracts.Service.Order.Fulfillment;

public interface IFulfillmentService
{
    Task ProcessFulfillmentByMarketplaceShipmentAsync(
        Guid shipmentId,
        CancellationToken cancellationToken = default);
    Task ProcessFulfillmentBySellerShipmentAsync(
        Guid shipmentId,
        CancellationToken cancellationToken = default);
    Task<bool> ReserveInventoryAsync(
        Guid shipmentId,
        CancellationToken cancellationToken = default);
    Task<bool> ReleaseInventoryAsync(
        Guid shipmentId,
        CancellationToken cancellationToken = default);
    Task<FulfillmentType> DetermineFulfillmentTypeAsync(
        Guid warehouseId,
        CancellationToken cancellationToken = default);
}
