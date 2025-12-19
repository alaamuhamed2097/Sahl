using DAL.Models.ItemSearch;
using Domains.Procedures;

namespace DAL.Contracts.Repositories;

public interface IItemDetailsRepository
{
    Task<SpGetItemDetails> GetItemDetailsAsync(Guid itemId);
    Task<SpGetAvailableOptionsForSelection> GetCombinationByAttributesAsync(Guid itemId, List<AttributeSelection> selectedAttributes);
}