using Domains.Entities.Base;

namespace Domins.Entities.Location
{
    public class TbCountry : BaseEntity
    {
        public string TitleAr { get; set; } = null!;
        public string TitleEn { get; set; } = null!;

        public ICollection<TbState> States { get; set; } = new HashSet<TbState>();
    }
}
