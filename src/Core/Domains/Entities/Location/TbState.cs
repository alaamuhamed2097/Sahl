namespace Domains.Entities.Location
{
    public class TbState : BaseEntity
    {
        public string TitleAr { get; set; } = null!;
        public string TitleEn { get; set; } = null!;

        public Guid CountryId { get; set; }
        public TbCountry Country { get; set; } = null!;

        public ICollection<TbCity> Cities { get; set; } = new HashSet<TbCity>();
    }
}
