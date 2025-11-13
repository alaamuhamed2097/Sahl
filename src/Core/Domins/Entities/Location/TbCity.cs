namespace Domins.Entities.Location
{
    public class TbCity : BaseEntity
    {
        public string TitleAr { get; set; } = null!;
        public string TitleEn { get; set; } = null!;

        public Guid StateId { get; set; }
        public TbState State { get; set; } = null!;
    }
}
