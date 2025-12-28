using Domains.Entities.Warehouse;
using Shared.DTOs.Warehouse;

namespace BL.Mapper;

public partial class MappingProfile
{
    private void ConfigureWarehouseAndInventoryMappings()
    {
        // Warehouse
        CreateMap<TbWarehouse, WarehouseDto>()
            .ReverseMap();
    }
}
