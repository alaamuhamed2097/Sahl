namespace Common.Enumerations.Shipping
{
    /// <summary>
    /// Shipment status tracking throughout the delivery lifecycle
    /// </summary>
    public enum ShipmentStatus
    {
        /// <summary>
        /// Order placed, awaiting warehouse processing
        /// الطلب مستلم وفي انتظار المعالجة
        /// </summary>
        PendingProcessing = 1,

        /// <summary>
        /// Order is being packed and prepared in warehouse
        /// جاري تجهيز وتعبئة الطلب في المخزن
        /// </summary>
        PreparingForShipment = 2,

        /// <summary>
        /// Package handed over to shipping carrier/courier
        /// تم تسليم الشحنة لشركة الشحن
        /// </summary>
        PickedUpByCarrier = 3,

        /// <summary>
        /// Package is on its way to customer location
        /// الشحنة في الطريق للعميل
        /// </summary>
        InTransitToCustomer = 4,

        /// <summary>
        /// Package successfully delivered to customer
        /// تم التسليم بنجاح للعميل
        /// </summary>
        DeliveredToCustomer = 5,

        /// <summary>
        /// Package returned to sender/warehouse
        /// تم إرجاع الشحنة للمرسل
        /// </summary>
        ReturnedToSender = 6,

        /// <summary>
        /// Order cancelled by the customer
        /// تم الإلغاء من قبل العميل
        /// </summary>
        CancelledByCustomer = 7,

        /// <summary>
        /// Order cancelled by the marketplace/admin
        /// تم الإلغاء من قبل المتجر
        /// </summary>
        CancelledByMarketplace = 8,

        /// <summary>
        /// Delivery attempt failed (customer not available, wrong address, etc.)
        /// فشل محاولة التسليم
        /// </summary>
        DeliveryAttemptFailed = 9
    }
}