using DAL.Models.ItemSearch;
using Domains.Procedures;

namespace DAL.Contracts.Repositories;

public interface IItemDetailsRepository 
{
    Task<SpGetItemDetails> GetItemDetailsAsync(Guid itemCombinationId);
    Task<SpGetItemDetails> GetCombinationByAttributesAsync(List<AttributeSelection> selectedAttributes);
}