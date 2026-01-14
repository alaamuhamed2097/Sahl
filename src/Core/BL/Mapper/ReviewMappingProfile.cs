using Domains.Entities.ECommerceSystem.Customer;
using Domains.Entities.ECommerceSystem.Review;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Entities.Order;
using Shared.DTOs.Review;
using Shared.GeneralModels.SearchCriteriaModels;

namespace BL.Mapper
{
    public partial class MappingProfile
    {
       
        private void ConfigureReviewMappings()
        {
            CreateMap<TbItemReview, ItemReviewDto>().ReverseMap();
            CreateMap<TbSalesReview, SalesReviewDto>().ReverseMap();
            CreateMap<TbReviewVote, ReviewVoteDto>().ReverseMap();
            CreateMap<TbReviewReport, ReviewReportDto>().ReverseMap();
			//CreateMap<TbVendorReview, VendorReviewDto>().ReverseMap();
			CreateMap<VendorReviewSearchCriteriaModel, AdminVendorReviewSearchCriteriaModel>();

			CreateMap<TbVendorReview, VendorReviewDto>()
			// Customer mapping
			.ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.User.FirstName))
			.ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(src => src.Customer.User.Email))
			.ForMember(dest => dest.CustomerPhoneNumber, opt => opt.MapFrom(src => src.Customer.User.PhoneNumber))

			// Vendor mapping
			.ForMember(dest => dest.VendorName, opt => opt.MapFrom(src => src.Vendor.User.FirstName))
			.ForMember(dest => dest.VendorEmail, opt => opt.MapFrom(src => src.Vendor.User.Email))
			.ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Vendor.StoreName))

			// Order Detail mapping
			.ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.OrderDetail.Item.TitleEn))
			.ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.OrderDetail.Order.Number))
			.ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.OrderDetail.Quantity))
			.ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.OrderDetail.UnitPrice))
			.ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDetail.CreatedDateUtc));

			CreateMap<VendorReviewDto, TbVendorReview>()
				.ForMember(dest => dest.Customer, opt => opt.Ignore())
				.ForMember(dest => dest.Vendor, opt => opt.Ignore())
				.ForMember(dest => dest.OrderDetail, opt => opt.Ignore());
			//----cast show

			//CreateMap<TbCustomer, CustomerBasicDto>();
			//CreateMap<TbVendor, VendorBasicDto>();
			//CreateMap<TbOrderDetail, OrderDetailBasicDto>();
		}
    }
}
