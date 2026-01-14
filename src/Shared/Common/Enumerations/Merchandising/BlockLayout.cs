namespace Common.Enumerations.Merchandising
{
    /// <summary>
    /// Block layout types for visual presentation
    /// أنواع تخطيطات البلوك للعرض المرئي
    /// </summary>
    public enum BlockLayout
    {
        /// <summary>
        /// Carousel/Slider - horizontal scrolling
        /// كاروسيل - عرض أفقي متحرك
        /// </summary>
        Carousel = 1,

        /// <summary>
        /// Two rows layout - stacked vertically
        /// صفين - مرتبين عمودياً
        /// </summary>
        TwoRows = 2,

        /// <summary>
        /// Featured layout - large product card (1 item)
        /// عرض مميز - كارت كبير للمنتج (عنصر واحد)
        /// </summary>
        Featured = 3,

        /// <summary>
        /// Compact layout - 4 small items in 2x2 grid
        /// عرض مضغوط - 4 عناصر صغيرة في شبكة 2×2
        /// </summary>
        Compact = 4,

        /// <summary>
        /// Full width banner/hero
        /// بانر بعرض كامل
        /// </summary>
        FullWidth = 5
    }
}
