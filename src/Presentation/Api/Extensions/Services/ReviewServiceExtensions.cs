using BL.Contracts.Service.Review;
using BL.Services.Review;

namespace Api.Extensions.Services
{
    public static class ReviewServiceExtensions
    {
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
