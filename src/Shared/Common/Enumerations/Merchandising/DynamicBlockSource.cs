namespace Common.Enumerations.Merchandising
{
    /// <summary>
    /// Dynamic block data sources
    /// مصادر البيانات للبلوكات الديناميكية
    /// </summary>
    public enum DynamicBlockSource
    {
        /// <summary>
        /// Best selling products (based on sales)
        /// الأكثر مبيعاً (بناءً على المبيعات)
        /// </summary>
        BestSellers = 1,

        /// <summary>
        /// New arrivals (based on creation date)
        /// الوافدات الجديدة (بناءً على تاريخ الإضافة)
        /// </summary>
        NewArrivals = 2,

        /// <summary>
        /// Top rated products (based on ratings)
        /// الأعلى تقييماً (بناءً على التقييمات)
        /// </summary>
        TopRated = 3,

        /// <summary>
        /// Trending products (based on views)
        /// المنتجات الرائجة (بناءً على المشاهدات)
        /// </summary>
        Trending = 4,

        /// <summary>
        /// Most wishlisted products
        /// الأكثر إضافة للمفضلة
        /// </summary>
        MostWishlisted = 5
    }
}
