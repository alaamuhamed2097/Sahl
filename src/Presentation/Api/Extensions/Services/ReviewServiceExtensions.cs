using BL.Contracts.Service.Review;
using BL.Services.Review;

namespace Api.Extensions.Services
{
    /// <summary>
    /// Extension methods for registering review and rating services.
    /// </summary>
    public static class ReviewServiceExtensions
    {
        /// <summary>
        /// Adds item review, vendor review, and review voting services.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
        public static IServiceCollection AddReviewServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Review Services
            services.AddScoped<IItemReviewService, ItemReviewService>();
            services.AddScoped<IReviewReportService, ReviewReportService>();
            services.AddScoped<IReviewVoteService, ReviewVoteService>();
            services.AddScoped<IVendorReviewService, VendorReviewService>();

            return services;
        }
    }
}
