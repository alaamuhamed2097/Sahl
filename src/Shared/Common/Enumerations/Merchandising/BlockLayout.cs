namespace Common.Enumerations.Merchandising
{
    /// <summary>
    /// Block layout types for visual presentation
    /// </summary>
    public enum BlockLayout
    {
        /// <summary>
        /// Carousel/Slider - horizontal scrolling
        /// </summary>
        Carousel = 1,

        /// <summary>
        /// Two column layout
        /// </summary>
        TwoColumn = 2,

        /// <summary>
        /// Featured layout - large product cards 1 item per card
        /// عرض مميز - كروت كبيرة للمنتجات المختارة
        /// </summary>
        Featured = 3,

        /// <summary>
        /// Compact layout - smaller product cards 4 items in card
        /// عرض مضغوط - كروت صغيرة جدًا - أكبر عدد منتجات في مساحة أقل
        /// </summary>
        Compact = 4,

        /// <summary>
        /// Full width banner/hero
        /// </summary>
        FullWidth = 5
    }
}
