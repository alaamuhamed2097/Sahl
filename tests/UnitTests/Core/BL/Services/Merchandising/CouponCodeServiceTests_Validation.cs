using AutoMapper;
using BL.Services.Merchandising.CouponCode;
using Common.Enumerations.Order;
using DAL.Contracts.Repositories.Merchandising;
using DAL.Contracts.UnitOfWork;
using Domains.Entities.Merchandising.CouponCode;
using Moq;

namespace UnitTests.Core.BL.Services.Merchandising
{
    /// <summary>
    /// Unit tests for CouponCodeService - Validation Scenarios
    /// </summary>
    public class CouponCodeServiceTests_Validation
    {
        private readonly Mock<ICouponCodeRepository> _mockRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CouponCodeService _service;
        private readonly string _testUserId = Guid.NewGuid().ToString();

        public CouponCodeServiceTests_Validation()
        {
            _mockRepository = new Mock<ICouponCodeRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _service = new CouponCodeService(_mockRepository.Object, _mockUnitOfWork.Object, _mockMapper.Object);
        }

        #region ValidateCouponCodeAsync - Success Scenarios

        [Fact]
        public async Task ValidateCouponCodeAsync_WithValidCoupon_ShouldReturnSuccess()
        {
            // Arrange
            var coupon = CreateValidCoupon();
            _mockRepository.Setup(r => r.GetByCodeAsync("VALID10", default))
                .ReturnsAsync(coupon);

            // Act
            var result = await _service.ValidateCouponCodeAsync("VALID10", _testUserId);

            // Assert
            Assert.True(result.IsValid);
            Assert.Equal("الكوبون صالح", result.Message);
            Assert.Equal(coupon.Id, result.CouponId);
            Assert.Equal(coupon.DiscountType, result.DiscountType);
            Assert.Equal(coupon.DiscountValue, result.DiscountValue);
        }

        [Fact]
        public async Task ValidateCouponCodeAsync_WithPercentageDiscount_ShouldReturnCorrectType()
        {
            // Arrange
            var coupon = CreateValidCoupon();
            coupon.DiscountType = DiscountType.Percentage;
            coupon.DiscountValue = 20;

            _mockRepository.Setup(r => r.GetByCodeAsync("PERCENT20", default))
                .ReturnsAsync(coupon);

            // Act
            var result = await _service.ValidateCouponCodeAsync("PERCENT20", _testUserId);

            // Assert
            Assert.True(result.IsValid);
            Assert.Equal(DiscountType.Percentage, result.DiscountType);
            Assert.Equal(20, result.DiscountValue);
        }

        [Fact]
        public async Task ValidateCouponCodeAsync_WithFixedDiscount_ShouldReturnCorrectType()
        {
            // Arrange
            var coupon = CreateValidCoupon();
            coupon.DiscountType = DiscountType.FixedAmount;
            coupon.DiscountValue = 50;

            _mockRepository.Setup(r => r.GetByCodeAsync("FIXED50", default))
                .ReturnsAsync(coupon);

            // Act
            var result = await _service.ValidateCouponCodeAsync("FIXED50", _testUserId);

            // Assert
            Assert.True(result.IsValid);
            Assert.Equal(DiscountType.FixedAmount, result.DiscountType);
            Assert.Equal(50, result.DiscountValue);
        }

        #endregion

        #region ValidateCouponCodeAsync - Failure Scenarios

        [Fact]
        public async Task ValidateCouponCodeAsync_WithNonExistentCode_ShouldReturnInvalid()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByCodeAsync("NOTFOUND", default))
                .ReturnsAsync((TbCouponCode)null);

            // Act
            var result = await _service.ValidateCouponCodeAsync("NOTFOUND", _testUserId);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("الكوبون غير موجود", result.Message);
        }

        [Fact]
        public async Task ValidateCouponCodeAsync_WithInactiveCoupon_ShouldReturnInvalid()
        {
            // Arrange
            var coupon = CreateValidCoupon();
            coupon.IsActive = false;

            _mockRepository.Setup(r => r.GetByCodeAsync("INACTIVE", default))
                .ReturnsAsync(coupon);

            // Act
            var result = await _service.ValidateCouponCodeAsync("INACTIVE", _testUserId);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("الكوبون غير مفعل", result.Message);
        }

        [Fact]
        public async Task ValidateCouponCodeAsync_WithDeletedCoupon_ShouldReturnInvalid()
        {
            // Arrange
            var coupon = CreateValidCoupon();
            coupon.IsDeleted = true;

            _mockRepository.Setup(r => r.GetByCodeAsync("DELETED", default))
                .ReturnsAsync(coupon);

            // Act
            var result = await _service.ValidateCouponCodeAsync("DELETED", _testUserId);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("الكوبون غير مفعل", result.Message);
        }

        [Fact]
        public async Task ValidateCouponCodeAsync_BeforeStartDate_ShouldReturnInvalid()
        {
            // Arrange
            var coupon = CreateValidCoupon();
            coupon.StartDate = DateTime.UtcNow.AddDays(5);

            _mockRepository.Setup(r => r.GetByCodeAsync("FUTURE", default))
                .ReturnsAsync(coupon);

            // Act
            var result = await _service.ValidateCouponCodeAsync("FUTURE", _testUserId);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("الكوبون لم يبدأ بعد", result.Message);
        }

        [Fact]
        public async Task ValidateCouponCodeAsync_AfterExpiryDate_ShouldReturnInvalid()
        {
            // Arrange
            var coupon = CreateValidCoupon();
            coupon.ExpiryDate = DateTime.UtcNow.AddDays(-5);

            _mockRepository.Setup(r => r.GetByCodeAsync("EXPIRED", default))
                .ReturnsAsync(coupon);

            // Act
            var result = await _service.ValidateCouponCodeAsync("EXPIRED", _testUserId);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("الكوبون منتهي الصلاحية", result.Message);
        }

        [Fact]
        public async Task ValidateCouponCodeAsync_UsageLimitReached_ShouldReturnInvalid()
        {
            // Arrange
            var coupon = CreateValidCoupon();
            coupon.UsageLimit = 100;
            coupon.UsageCount = 100;

            _mockRepository.Setup(r => r.GetByCodeAsync("MAXUSED", default))
                .ReturnsAsync(coupon);

            // Act
            var result = await _service.ValidateCouponCodeAsync("MAXUSED", _testUserId);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("تم استخدام الكوبون بالحد الأقصى", result.Message);
        }

        [Fact]
        public async Task ValidateCouponCodeAsync_UserLimitReached_ShouldReturnInvalid()
        {
            // Arrange
            var coupon = CreateValidCoupon();
            coupon.UsageLimitPerUser = 3;

            _mockRepository.Setup(r => r.GetByCodeAsync("USERLIMIT", default))
                .ReturnsAsync(coupon);
            _mockRepository.Setup(r => r.GetUserUsageCountAsync(coupon.Id, _testUserId, default))
                .ReturnsAsync(3);

            // Act
            var result = await _service.ValidateCouponCodeAsync("USERLIMIT", _testUserId);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("لقد تجاوزت الحد الأقصى لاستخدام هذا الكوبون", result.Message);
        }

        [Fact]
        public async Task ValidateCouponCodeAsync_FirstOrderOnlyWithExistingOrders_ShouldReturnInvalid()
        {
            // Arrange
            var coupon = CreateValidCoupon();
            coupon.IsFirstOrderOnly = true;

            _mockRepository.Setup(r => r.GetByCodeAsync("FIRSTONLY", default))
                .ReturnsAsync(coupon);
            _mockRepository.Setup(r => r.IsValidForUserAsync(coupon.Id, _testUserId, default))
                .ReturnsAsync(false);

            // Act
            var result = await _service.ValidateCouponCodeAsync("FIRSTONLY", _testUserId);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("هذا الكوبون متاح للطلب الأول فقط", result.Message);
        }

        #endregion

        #region ValidateCouponCodeAsync - Edge Cases

        [Fact]
        public async Task ValidateCouponCodeAsync_WithMaxDiscountAmount_ShouldIncludeInResult()
        {
            // Arrange
            var coupon = CreateValidCoupon();
            coupon.MaxDiscountAmount = 100;

            _mockRepository.Setup(r => r.GetByCodeAsync("MAXDISC", default))
                .ReturnsAsync(coupon);

            // Act
            var result = await _service.ValidateCouponCodeAsync("MAXDISC", _testUserId);

            // Assert
            Assert.True(result.IsValid);
            Assert.Equal(100, result.MaxDiscountAmount);
        }

        [Fact]
        public async Task ValidateCouponCodeAsync_WithMinimumOrderAmount_ShouldIncludeInResult()
        {
            // Arrange
            var coupon = CreateValidCoupon();
            coupon.MinimumOrderAmount = 500;

            _mockRepository.Setup(r => r.GetByCodeAsync("MINORDER", default))
                .ReturnsAsync(coupon);

            // Act
            var result = await _service.ValidateCouponCodeAsync("MINORDER", _testUserId);

            // Assert
            Assert.True(result.IsValid);
            Assert.Equal(500, result.MinimumOrderAmount);
        }

        [Fact]
        public async Task ValidateCouponCodeAsync_OnStartDate_ShouldBeValid()
        {
            // Arrange
            var coupon = CreateValidCoupon();
            coupon.StartDate = DateTime.UtcNow.AddMinutes(-1);

            _mockRepository.Setup(r => r.GetByCodeAsync("STARTTODAY", default))
                .ReturnsAsync(coupon);

            // Act
            var result = await _service.ValidateCouponCodeAsync("STARTTODAY", _testUserId);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task ValidateCouponCodeAsync_OnExpiryDate_ShouldBeValid()
        {
            // Arrange
            var coupon = CreateValidCoupon();
            coupon.ExpiryDate = DateTime.UtcNow.AddMinutes(1);

            _mockRepository.Setup(r => r.GetByCodeAsync("EXPIRETODAY", default))
                .ReturnsAsync(coupon);

            // Act
            var result = await _service.ValidateCouponCodeAsync("EXPIRETODAY", _testUserId);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task ValidateCouponCodeAsync_UsageLimitNotSet_ShouldBeValid()
        {
            // Arrange
            var coupon = CreateValidCoupon();
            coupon.UsageLimit = null;
            coupon.UsageCount = 999999;

            _mockRepository.Setup(r => r.GetByCodeAsync("NOLIMIT", default))
                .ReturnsAsync(coupon);

            // Act
            var result = await _service.ValidateCouponCodeAsync("NOLIMIT", _testUserId);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task ValidateCouponCodeAsync_UserLimitNotSet_ShouldBeValid()
        {
            // Arrange
            var coupon = CreateValidCoupon();
            coupon.UsageLimitPerUser = null;

            _mockRepository.Setup(r => r.GetByCodeAsync("NOUSERLIMIT", default))
                .ReturnsAsync(coupon);
            _mockRepository.Setup(r => r.GetUserUsageCountAsync(coupon.Id, _testUserId, default))
                .ReturnsAsync(999);

            // Act
            var result = await _service.ValidateCouponCodeAsync("NOUSERLIMIT", _testUserId);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task ValidateCouponCodeAsync_FirstOrderOnlyWithoutOrders_ShouldBeValid()
        {
            // Arrange
            var coupon = CreateValidCoupon();
            coupon.IsFirstOrderOnly = true;

            _mockRepository.Setup(r => r.GetByCodeAsync("FIRSTOK", default))
                .ReturnsAsync(coupon);
            _mockRepository.Setup(r => r.IsValidForUserAsync(coupon.Id, _testUserId, default))
                .ReturnsAsync(true);

            // Act
            var result = await _service.ValidateCouponCodeAsync("FIRSTOK", _testUserId);

            // Assert
            Assert.True(result.IsValid);
        }

        #endregion

        #region Helper Methods

        private TbCouponCode CreateValidCoupon()
        {
            return new TbCouponCode
            {
                Id = Guid.NewGuid(),
                Code = "VALID10",
                TitleAr = "كوبون صالح",
                TitleEn = "Valid Coupon",
                PromoType = CouponCodeType.General,
                DiscountType = DiscountType.Percentage,
                DiscountValue = 10,
                IsActive = true,
                IsDeleted = false,
                StartDate = DateTime.UtcNow.AddDays(-1),
                ExpiryDate = DateTime.UtcNow.AddDays(30),
                UsageLimit = 1000,
                UsageCount = 50,
                CreatedDateUtc = DateTime.UtcNow
            };
        }

        #endregion
    }
}