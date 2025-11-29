//using BL.Contracts.IMapper;
//using BL.Contracts.Service.CouponCode;
//using BL.Contracts.Service.ECommerce.Item;
//using DAL.Contracts.UnitOfWork;
//using Serilog;

//namespace BL.Service.CouponCode
//{
//    // Thin adapter to maintain backward-compatible namespace "BL.Service.CouponCode"
//    // Delegates to the implementation under BL.Service.PromoCode
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
