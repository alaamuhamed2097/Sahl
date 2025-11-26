using Domains.Entities.Warehouse;
using Domains.Entities.Inventory;
using Shared.DTOs.Warehouse;
using Shared.DTOs.Inventory;

namespace BL.Mapper
{
    public partial class MappingProfile
    {
        private void ConfigureWarehouseAndInventoryMappings()
        {
            // Warehouse
            CreateMap<TbWarehouse, WarehouseDto>()
                .ReverseMap();

            // Inventory
            CreateMap<TbMoitem, MoitemDto>()
                .ReverseMap();

            CreateMap<TbMortem, MortemDto>()
                .ReverseMap();

            CreateMap<TbMovitemsdetail, MovitemsdetailDto>()
                .ReverseMap();
        }
    }
}
