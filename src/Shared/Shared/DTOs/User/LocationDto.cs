using Shared.DTOs.Base;

namespace Shared.DTOs.User
{
    public class LocationDto : BaseDto
    {
        public Guid CountryId { get; set; }
        public string CountryName { get; set; } = null!;
        public string CountryCode { get; set; } = null!;
        public Guid StateId { get; set; }
        public string StateName { get; set; } = null!;
        public Guid CityId { get; set; }
        public string CityName { get; set; } = null!;
    }
}
