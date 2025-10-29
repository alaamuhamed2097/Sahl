using Dashboard.Services.General;

namespace Dashboard.Contracts.General
{
    public interface ICountryPhoneCodeService
    {
        IEnumerable<CountryInfo> GetAllCountries(string language = null);
        IEnumerable<CountryInfo> GetCountriesByRegion(string region, string language = null);
        CountryInfo? GetCountryByCode(string code, string language = null);
        CountryInfo? GetCountryByIso(string iso, string language = null);
        IEnumerable<CountryInfo> GetPopularCountries(string language = null);
        IEnumerable<CountryInfo> SearchCountries(string searchTerm, string language = null);
    }
}