using BL.Contracts.Service.Base;
using DAL.Models;
using Domins.Entities.Location;
using Shared.DTOs.Location;
using Shared.GeneralModels.SearchCriteriaModels;

namespace BL.Contracts.Service.Location
{
    public interface ICountryService : IBaseService<TbCountry, CountryDto>
    {
        PaginatedDataModel<CountryDto> GetPage(BaseSearchCriteriaModel criteriaModel);
    }
}
