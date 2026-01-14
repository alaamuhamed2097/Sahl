namespace Common.Enumerations.Merchandising
{
    /// <summary>
    /// Homepage block types - determines content source
    /// أنواع البلوكات في الصفحة الرئيسية - يحدد مصدر المحتوى
    /// </summary>
    public enum HomepageBlockType
    {
        /// <summary>
        /// Manually selected items
        /// منتجات مختارة يدوياً
        /// </summary>
        ManualItems = 1,

        /// <summary>
        /// Manually selected categories
        /// فئات مختارة يدوياً
        /// </summary>
        ManualCategories = 2,

        /// <summary>
        /// Campaign-based products
        /// منتجات من حملة تسويقية
        /// </summary>
        Campaign = 3,

        /// <summary>
        /// Dynamic rule-based content
        /// محتوى ديناميكي بناءً على قواعد
        /// </summary>
        Dynamic = 4,

        /// <summary>
        /// Personalized content for each user
        /// محتوى شخصي لكل مستخدم
        /// </summary>
        Personalized = 5
    }
}
