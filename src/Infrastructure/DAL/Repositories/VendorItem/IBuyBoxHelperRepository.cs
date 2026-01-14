
namespace DAL.Repositories.Offer
{
    public interface IBuyBoxHelperRepository
    {
        /// <summary>
        /// Recalculate Buy Box winners for all item combinations of a given item
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RecalculateBuyBoxWinnersByItemIdAsync(
    Guid itemId,
    CancellationToken cancellationToken = default);

        /// <summary>
        /// Recalculate the Buy Box winner for a specific item combination
        /// </summary>
        /// <param name="itemCombinationId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RecalculateBuyBoxWinnerByItemCombinationIdAsync(Guid itemCombinationId, CancellationToken cancellationToken = default);
    }
}