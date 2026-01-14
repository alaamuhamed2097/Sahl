using Dashboard.Contracts.General;
using Dashboard.Contracts.Review;

namespace Dashboard.Services.Review
{
	public class VendorReviewService : IVendorReviewService
	{
		private readonly IApiService _apiService;

		public VendorReviewService(IApiService apiService)
		{
			_apiService = apiService;
		}
	}
}
