using DAL.Models.ItemSearch;
using Domains.Procedures;

namespace DAL.Contracts.Repositories.Catalog.Item;

public interface IItemDetailsRepository
{
    Task<SpGetItemDetails> GetItemDetailsAsync(
            Guid itemCombinationId,
            CancellationToken cancellationToken = default);
    Task<SpGetItemDetails> GetCombinationByAttributesAsync(
            List<AttributeSelection> selectedAttributes,
            CancellationToken cancellationToken = default);
}