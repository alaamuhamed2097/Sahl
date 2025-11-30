namespace Common.Enumerations.Fulfillment
{
    /// <summary>
    /// Fulfillment fee types
    /// </summary>
    public enum FulfillmentFeeType
    {
        /// <summary>
        /// Storage fee
        /// </summary>
        Storage = 1,

        /// <summary>
        /// Picking and packing fee
        /// </summary>
        PickingPacking = 2,

        /// <summary>
        /// Shipping fee
        /// </summary>
        Shipping = 3,

        /// <summary>
        /// Handling fee
        /// </summary>
        Handling = 4,

        /// <summary>
        /// Returns processing fee
        /// </summary>
        ReturnsProcessing = 5,

        /// <summary>
        /// Long-term storage fee
        /// </summary>
        LongTermStorage = 6,

        /// <summary>
        /// Removal/Disposal fee
        /// </summary>
        RemovalDisposal = 7,

        /// <summary>
        /// Special handling fee
        /// </summary>
        SpecialHandling = 8
    }

    /// <summary>
    /// FBM Shipment status
    /// </summary>
    public enum FBMShipmentStatus
    {
        /// <summary>
        /// Order received, pending processing
        /// </summary>
        Pending = 1,

        /// <summary>
        /// Being prepared for shipment
        /// </summary>
        Processing = 2,

        /// <summary>
        /// Ready for pickup
        /// </summary>
        ReadyForPickup = 3,

        /// <summary>
        /// Picked up by courier
        /// </summary>
        PickedUp = 4,

        /// <summary>
        /// In transit to customer
        /// </summary>
        InTransit = 5,

        /// <summary>
        /// Out for delivery
        /// </summary>
        OutForDelivery = 6,

        /// <summary>
        /// Successfully delivered
        /// </summary>
        Delivered = 7,

        /// <summary>
        /// Delivery failed
        /// </summary>
        DeliveryFailed = 8,

        /// <summary>
        /// Returned to warehouse
        /// </summary>
        ReturnedToWarehouse = 9,

        /// <summary>
        /// Cancelled
        /// </summary>
        Cancelled = 10
    }
}
