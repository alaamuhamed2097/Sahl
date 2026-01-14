namespace Common.Enumerations.Merchandising
{
    /// <summary>
    /// Personalization data sources
    /// مصادر البيانات للمحتوى الشخصي
    /// </summary>
    public enum PersonalizationSource
    {
        /// <summary>
        /// Based on user's view history
        /// بناءً على سجل المشاهدات
        /// </summary>
        ViewHistory = 1,

        /// <summary>
        /// Based on user's purchase history
        /// بناءً على سجل المشتريات
        /// </summary>
        PurchaseHistory = 2,

        /// <summary>
        /// Recently viewed products
        /// المنتجات المشاهدة مؤخراً
        /// </summary>
        RecentlyViewed = 3,

        /// <summary>
        /// Products from user's wishlist
        /// المنتجات من قائمة المفضلة
        /// </summary>
        Wishlist = 4
    }
}
