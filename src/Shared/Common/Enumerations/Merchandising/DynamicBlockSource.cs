namespace Common.Enumerations.Merchandising
{
    // بدل ما نعمل enum لكل نوع، نعمل enum للـ source
    public enum DynamicBlockSource
    {
        BestSellers = 1,        // من المبيعات
        NewArrivals = 2,        // من تاريخ الإضافة
        TopRated = 3,           // من التقييمات
        Trending = 4,           // من الـ Views
        MostWishlisted = 5     // من الـ Wishlist
    }
}
