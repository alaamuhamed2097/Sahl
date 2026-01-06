namespace Common.Enumerations.Order
{
    public enum RefundStatus
    {
        Open = 1,             // Refund request created, pending action
        UnderReview = 2,         // Being reviewed by admin/vendor
        NeedMoreInfo = 3,      // More information needed from customer
        InfoApproved = 4,        // Customer provided info, approved
        ItemShippedBack = 5,     // Customer shipped item back
        ItemReceived = 6,        // Vendor received returned item
        Inspecting = 7,          // Item being inspected for condition
        Approved = 8,            // Refund approved, awaiting item
        Rejected = 9,            // Refund request denied
        Refunded = 10,            // Money refunded to customer
        Closed = 11            // Refund process completed
    }
}