//using BL.Contracts.IMapper;
//using BL.Contracts.Service.CouponCode;
//using BL.Contracts.Service.ECommerce.Item;
//using DAL.Contracts.UnitOfWork;
//using Serilog;

//namespace BL.Services.CouponCode
//{
//    // Thin adapter to maintain backward-compatible namespace "BL.Services.CouponCode"
//    // Delegates to the implementation under BL.Services.PromoCode
//    public class CouponCodeService : PromoCode.CouponCodeService, ICouponCodeService
//    {
//        public CouponCodeService(
//            IUnitOfWork unitOfWork,
//            IItemService itemService,
//            IBaseMapper mapper,
//            ILogger logger)
//            : base(unitOfWork, itemService, mapper, logger)
//        {
//        }
//    }
//}
