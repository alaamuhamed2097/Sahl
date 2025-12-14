namespace Common.Enumerations.Order
{
    /// <summary>
    /// Order progress status stored on TbOrder.OrderStatus column
    /// Represents the complete lifecycle of an order through 8 stages
    /// </summary>
    public enum OrderProgressStatus
    {
        // Stage 1-3: Order Creation
        Pending = 0,          // Initial state after cart checkout
        PaymentPending = 1,   // Awaiting payment processing
        Processing = 2,       // Payment completed, preparing shipments
        
        // Stage 4-6: Fulfillment
        Confirmed = 3,        // Order confirmed and split into shipments
        Fulfilling = 4,       // Shipments being prepared (FBA/FBM processing)
        
        // Stage 7-8: Shipping & Delivery
        Shipped = 5,          // All or partial shipments shipped
        InTransit = 6,        // Shipments in transit
        OutForDelivery = 7,   // Shipments out for delivery
        Delivered = 8,        // All shipments delivered
        Completed = 9,        // Order fully completed and confirmed
        
        // Terminal States
        Cancelled = 10,       // Order cancelled before shipping
        PaymentFailed = 11,   // Payment processing failed
        ReturnInitiated = 12, // Return process started
        Returned = 13,        // Product(s) returned
        Failed = 14           // Order failed for other reasons
    }
}
