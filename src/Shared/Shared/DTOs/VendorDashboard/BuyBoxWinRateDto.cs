namespace Shared.DTOs.VendorDashboard;

/// <summary>
/// DTO for Buy Box Win Rate metric
/// </summary>
public class BuyBoxWinRateDto
{
    /// <summary>
    /// Total number of unique products offered by vendor
    /// </summary>
    public int TotalProductsOffered { get; set; }

    /// <summary>
    /// Number of products where vendor won the buy box
    /// </summary>
    public int BuyBoxWins { get; set; }

    /// <summary>
    /// Buy box win rate percentage (Wins / Total * 100)
    /// </summary>
    public decimal BuyBoxWinRatePercentage { get; set; }

    /// <summary>
    /// Number of competing sellers on winning products
    /// </summary>
    public int AverageCompetitors { get; set; }

    /// <summary>
    /// List of top products by buy box wins
    /// </summary>
    public List<BuyBoxProductDto> TopProducts { get; set; } = new List<BuyBoxProductDto>();

    /// <summary>
    /// Factors affecting buy box (prices, ratings, shipping time, etc.)
    /// </summary>
    public BuyBoxCompetitionFactorsDto CompetitionFactors { get; set; } = new();

    /// <summary>
    /// Percentage change from previous period
    /// </summary>
    public decimal? PercentageChange { get; set; }

    /// <summary>
    /// Time period this metric covers
    /// </summary>
    public string Period { get; set; } = "Current Month";
}

/// <summary>
/// Details of a product with buy box wins
/// </summary>
public class BuyBoxProductDto
{
    /// <summary>
    /// Product ID
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Product name
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Number of times won the buy box (in current period)
    /// </summary>
    public int BuyBoxWins { get; set; }

    /// <summary>
    /// Current buy box holder status
    /// </summary>
    public bool IsCurrentBuyBoxHolder { get; set; }

    /// <summary>
    /// Vendor's current price for this product
    /// </summary>
    public decimal CurrentPrice { get; set; }

    /// <summary>
    /// Average competitor price
    /// </summary>
    public decimal AverageCompetitorPrice { get; set; }

    /// <summary>
    /// Price difference from average competitor
    /// </summary>
    public decimal PriceDifference { get; set; }

    /// <summary>
    /// Vendor's current rating for this product
    /// </summary>
    public decimal VendorRating { get; set; }
}

/// <summary>
/// Factors that affect buy box eligibility
/// </summary>
public class BuyBoxCompetitionFactorsDto
{
    /// <summary>
    /// Average price competitiveness score (0-100)
    /// </summary>
    public decimal PriceCompetitivenessScore { get; set; }

    /// <summary>
    /// Average rating competitiveness score (0-100)
    /// </summary>
    public decimal RatingCompetitivenessScore { get; set; }

    /// <summary>
    /// Average shipping time competitiveness score (0-100)
    /// </summary>
    public decimal ShippingCompetitivenessScore { get; set; }

    /// <summary>
    /// Overall buy box eligibility score (0-100)
    /// </summary>
    public decimal OverallBuyBoxEligibilityScore { get; set; }

    /// <summary>
    /// Recommendations to improve buy box win rate
    /// </summary>
    public List<string> Recommendations { get; set; } = new List<string>();
}
