using Common.Filters;
using Dashboard.Constants;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Warehouse;
using Dashboard.Models.pagintion;
using Resources;
using Shared.DTOs.Vendor;
using Shared.DTOs.Warehouse;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Dashboard.Services.Warehouse
{
    public class VendorWarehouseService : IVendorWarehouseService
    {
        private readonly IApiService _apiService;

        public VendorWarehouseService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ResponseModel<WarehouseDto>> GetMarketWarehouseAsync(string userId)
        {
            try
            {
                return await _apiService.GetAsync<WarehouseDto>(ApiEndpoints.VendorWarehouse.GetMarketWarehouse);
            }
            catch (Exception ex)
            {
                return new ResponseModel<WarehouseDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
