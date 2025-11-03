namespace Domins.Views.UserNotification
{
    public class VwUserNotification
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public string TimeAgo { get; set; }
    }
}
