namespace Common.Enumerations.Order
{
    public enum CouponCodeType
    {
        General = 1,        // Default - يشمل (PriceBased, BrandBased, ProductGroup)
        CategoryBased = 2,
        VendorBased = 3,
        ItemBased = 4
    }
}
