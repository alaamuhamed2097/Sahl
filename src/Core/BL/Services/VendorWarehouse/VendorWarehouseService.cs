using BL.Contracts.IMapper;
using BL.Contracts.Service.VendorWarehouse;
using BL.Contracts.Service.Warehouse;
using BL.Extensions;
using BL.Services.Base;
using Common.Filters;
using DAL.Contracts.Repositories;
using DAL.Models;
using DAL.Repositories;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Entities.Warehouse;
using Microsoft.AspNetCore.Identity;
using Resources;
using Serilog;
using Shared.DTOs.Vendor;
using Shared.DTOs.Warehouse;
using Shared.GeneralModels.SearchCriteriaModels;
using System.Linq.Expressions;

namespace BL.Services.Warehouse;

public class VendorWarehouseService : IVendorWarehouseService
{
    private readonly ITableRepository<TbWarehouse> _warehouseRepository;
    private readonly ITableRepository<TbVendor> _vendorRepository;
    private readonly ILogger _logger;
    private readonly IBaseMapper _mapper;

    public VendorWarehouseService(
        ITableRepository<TbWarehouse> warehouseRepository,
        ILogger logger,
        IBaseMapper mapper,
        ITableRepository<TbVendor> vendorRepository)
    {
        _warehouseRepository = warehouseRepository;
        _logger = logger;
        _mapper = mapper;
        _vendorRepository = vendorRepository;
    }

    public async Task<IEnumerable<WarehouseDto>> GetVendorAvailableWarehousesByUserIdAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentNullException(nameof(userId));

        var vendor = (await _vendorRepository
            .GetAsync(x => x.UserId == userId && !x.IsDeleted))
            .FirstOrDefault()
            ?? throw new KeyNotFoundException("Vendor not found");

        var warehouses = await GetVendorWarehousesInternalAsync(vendor.Id);

        return _mapper.MapList<TbWarehouse, WarehouseDto>(warehouses);
    }

    public async Task<IEnumerable<WarehouseDto>> GetVendorAvailableWarehousesByVendorIdAsync(Guid vendorId)
    {
        if (vendorId == Guid.Empty)
            throw new ArgumentException("Invalid vendorId");

        var warehouses = await GetVendorWarehousesInternalAsync(vendorId);

        return _mapper.MapList<TbWarehouse, WarehouseDto>(warehouses);
    }

    public async Task<WarehouseDto> GetMarketWarehousesAsync()
    {
        var warehouses = await _warehouseRepository
            .FindAsync(x => !x.IsDeleted && x.IsDefaultPlatformWarehouse);

        return _mapper.MapModel<TbWarehouse, WarehouseDto>(warehouses);
    }

    // helper method
    private async Task<List<TbWarehouse>> GetVendorWarehousesInternalAsync(Guid vendorId)
    {
        var warehouses = (await _warehouseRepository.GetAsync(x =>
            !x.IsDeleted &&
            x.IsActive &&
            x.VendorId == vendorId)).ToList();

        var defaultWarehouse = (await _warehouseRepository.GetAsync(x =>
            !x.IsDeleted &&
            x.IsActive &&
            x.IsDefaultPlatformWarehouse))
            .FirstOrDefault();

        if (defaultWarehouse != null &&
            !warehouses.Any(w => w.Id == defaultWarehouse.Id))
        {
            warehouses.Add(defaultWarehouse);
        }

        return warehouses;
    }

}
