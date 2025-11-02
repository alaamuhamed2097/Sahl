using Dashboard.Services.General;

namespace Dashboard.Contracts.General
{
    public interface ICountryPhoneCodeService
    {
        public IEnumerable<CountryInfo> GetAllCountries(string language = null);

        public CountryInfo? GetCountryByCode(string code, string language = null);

        public CountryInfo? GetCountryByIso(string iso, string language = null);

        public IEnumerable<CountryInfo> SearchCountries(string searchTerm, string language = null);

        public IEnumerable<CountryInfo> GetCountriesByRegion(string region, string language = null);

        public IEnumerable<CountryInfo> GetPopularCountries(string language = null);

        public IEnumerable<CountryInfo> GetFallbackCountries();
    }

}