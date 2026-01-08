using Common.Filters;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories.Customer;
using DAL.Exceptions;
using DAL.Models;
using Domains.Entities.Catalog.Item.ItemAttributes;
using Domains.Entities.Customer;
using Domains.Entities.ECommerceSystem.Customer;
using Domains.Procedures;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;

namespace DAL.Repositories.Customer
{
    /// <summary>
    /// Repository for Wishlist operations
    /// All operations include customer validation for security
    /// </summary>
    public class CustomerRecommendedItemsRepository : Repository<SpGetCustomerRecommendedItems>, ICustomerRecommendedItemsRepository
    {
        public CustomerRecommendedItemsRepository(ApplicationDbContext dbContext, ILogger logger) : base(dbContext, logger)
        {
        }
        /// <summary>
        /// Get recommended items for a customer based on various criteria
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<AdvancedPagedResult<SpGetCustomerRecommendedItems>> GetCustomerRecommendedItemsAsync(CustomerFilterQuery filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            ValidateCriteria(filter);

            // Prepare SQL parameters matching the stored procedure signature
            var parameters = new[]
            {
                new SqlParameter("@CustomerId", (object)filter.CustomerId ?? DBNull.Value),
                new SqlParameter("@SearchTerm", filter.SearchTerm),
                new SqlParameter("@SortBy", filter.SortBy ?? "relevance"),
                new SqlParameter("@SortDirection", filter.SortDirection ?? "asc"),
                new SqlParameter("@PageNumber", filter.PageNumber),
                new SqlParameter("@PageSize", filter.PageSize)
            };

            var results = (await ExecuteStoredProcedureAsync<SpGetCustomerRecommendedItems>("SpGetCustomerRecommendedItems", default, parameters))
                            .ToList();

            int totalRecords = results.Any() ? results[0].TotalRecords : 0;

            // Return using the same pattern as the original code
            return new AdvancedPagedResult<SpGetCustomerRecommendedItems>
            {
                Items = results,
                TotalRecords = totalRecords,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)filter.PageSize)
            };
        }
        private void ValidateCriteria(BaseSearchCriteriaModel filter)
        {
            if (filter.PageNumber < 1)
                filter.PageNumber = 1;

            if (filter.PageSize < 1 || filter.PageSize > 100)
                filter.PageSize = 20;

            var validSorts = new[] { "relevance", "price", "rating", "newest" };
            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                filter.SortBy = filter.SortBy.ToLower();
                if (!validSorts.Contains(filter.SortBy))
                {
                    _logger.Warning("Invalid sort value: {SortBy}, defaulting to 'relevance'", filter.SortBy);
                    filter.SortBy = "relevance";
                }
            }
            else
            {
                filter.SortBy = "relevance";
            }

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                filter.SearchTerm = filter.SearchTerm.Trim();
                if (filter.SearchTerm.Length > 255)
                {
                    filter.SearchTerm = filter.SearchTerm.Substring(0, 255);
                }
            }
        }
    }
}