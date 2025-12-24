namespace Common.Enumerations.Merchandising
{
    public enum HomepageBlockType
    {
        // 1. Manual - Admin يختار المنتجات يدوي
        ManualItems = 1,

        // 2. Campaign - من حملة معينة
        Campaign = 2,

        // 3. Dynamic - System يجيب تلقائي
        Dynamic = 3,

        // 4. Personalized - لكل User
        Personalized = 4,

        // 5. CategoryShowcase - عرض فئات
        ManualCategories = 5
    }
}
