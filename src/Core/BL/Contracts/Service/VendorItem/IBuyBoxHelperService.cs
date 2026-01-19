namespace BL.Contracts.Service.VendorItem
{
    public interface IBuyBoxHelperService
    {
        Task RecalculateItemBuyBoxWinnersAsync(Guid itemId);
        Task RecalculateCombinationBuyBoxWinnerAsync(Guid itemCombinationId);
    }
}