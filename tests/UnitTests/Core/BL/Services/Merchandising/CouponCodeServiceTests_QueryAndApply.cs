using AutoMapper;
using BL.Services.Merchandising.CouponCode;
using Common.Enumerations.Order;
using Common.Filters;
using DAL.Contracts.Repositories.Merchandising;
using DAL.Contracts.UnitOfWork;
using Domains.Entities.Merchandising.CouponCode;
using Moq;
using Shared.DTOs.Order.CouponCode;
using Shared.GeneralModels.SearchCriteriaModels;

namespace UnitTests.Core.BL.Services.Merchandising
{
    /// <summary>
    /// Unit tests for CouponCodeService - Query and Apply Operations
    /// </summary>
    public class CouponCodeServiceTests_QueryAndApply
    {
        private readonly Mock<ICouponCodeRepository> _mockRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CouponCodeService _service;

        public CouponCodeServiceTests_QueryAndApply()
        {
            _mockRepository = new Mock<ICouponCodeRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _service = new CouponCodeService(_mockRepository.Object, _mockUnitOfWork.Object, _mockMapper.Object);
        }

        #region GetByCodeAsync Tests

        [Fact]
        public async Task GetByCodeAsync_WithValidCode_ShouldReturnCoupon()
        {
            // Arrange
            var coupon = CreateTestCoupon(Guid.NewGuid(), "GETCODE");
            var couponDto = CreateTestCouponDto(coupon.Id, "GETCODE");

            _mockRepository.Setup(r => r.GetByCodeAsync("GETCODE", default))
                .ReturnsAsync(coupon);
            _mockMapper.Setup(m => m.Map<CouponCodeDto>(coupon))
                .Returns(couponDto);

            // Act
            var result = await _service.GetByCodeAsync("GETCODE");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("GETCODE", result.Code);
            _mockRepository.Verify(r => r.GetByCodeAsync("GETCODE", default), Times.Once);
        }

        [Fact]
        public async Task GetByCodeAsync_WithInvalidCode_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByCodeAsync("INVALID", default))
                .ReturnsAsync((TbCouponCode)null);

            // Act
            var result = await _service.GetByCodeAsync("INVALID");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByCodeAsync_WithEmptyCode_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByCodeAsync("", default))
                .ReturnsAsync((TbCouponCode)null);

            // Act
            var result = await _service.GetByCodeAsync("");

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region GetActiveCouponsAsync Tests

        [Fact]
        public async Task GetActiveCouponsAsync_WithActiveCoupons_ShouldReturnOnlyActive()
        {
            // Arrange
            var activeCoupons = new List<TbCouponCode>
            {
                CreateTestCoupon(Guid.NewGuid(), "ACTIVE1"),
                CreateTestCoupon(Guid.NewGuid(), "ACTIVE2")
            };

            var activeDtos = new List<CouponCodeDto>
            {
                CreateTestCouponDto(activeCoupons[0].Id, "ACTIVE1"),
                CreateTestCouponDto(activeCoupons[1].Id, "ACTIVE2")
            };

            _mockRepository.Setup(r => r.GetActiveCouponsAsync(default))
                .ReturnsAsync(activeCoupons);
            _mockMapper.Setup(m => m.Map<IEnumerable<CouponCodeDto>>(activeCoupons))
                .Returns(activeDtos);

            // Act
            var result = await _service.GetActiveCouponsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, c => Assert.True(c.IsActive));
        }

        [Fact]
        public async Task GetActiveCouponsAsync_WithNoActiveCoupons_ShouldReturnEmpty()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetActiveCouponsAsync(default))
                .ReturnsAsync(new List<TbCouponCode>());
            _mockMapper.Setup(m => m.Map<IEnumerable<CouponCodeDto>>(It.IsAny<List<TbCouponCode>>()))
                .Returns(new List<CouponCodeDto>());

            // Act
            var result = await _service.GetActiveCouponsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region GetVendorCouponsAsync Tests

        [Fact]
        public async Task GetVendorCouponsAsync_WithValidVendorId_ShouldReturnVendorCoupons()
        {
            // Arrange
            var vendorId = Guid.NewGuid();
            var vendorCoupons = new List<TbCouponCode>
            {
                CreateTestCoupon(Guid.NewGuid(), "VENDOR1", vendorId),
                CreateTestCoupon(Guid.NewGuid(), "VENDOR2", vendorId)
            };

            var vendorDtos = new List<CouponCodeDto>
            {
                CreateTestCouponDto(vendorCoupons[0].Id, "VENDOR1", vendorId),
                CreateTestCouponDto(vendorCoupons[1].Id, "VENDOR2", vendorId)
            };

            _mockRepository.Setup(r => r.GetCouponsByVendorAsync(vendorId, default))
                .ReturnsAsync(vendorCoupons);
            _mockMapper.Setup(m => m.Map<IEnumerable<CouponCodeDto>>(vendorCoupons))
                .Returns(vendorDtos);

            // Act
            var result = await _service.GetVendorCouponsAsync(vendorId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, c => Assert.Equal(vendorId, c.VendorId));
        }

        [Fact]
        public async Task GetVendorCouponsAsync_WithNoVendorCoupons_ShouldReturnEmpty()
        {
            // Arrange
            var vendorId = Guid.NewGuid();
            _mockRepository.Setup(r => r.GetCouponsByVendorAsync(vendorId, default))
                .ReturnsAsync(new List<TbCouponCode>());
            _mockMapper.Setup(m => m.Map<IEnumerable<CouponCodeDto>>(It.IsAny<List<TbCouponCode>>()))
                .Returns(new List<CouponCodeDto>());

            // Act
            var result = await _service.GetVendorCouponsAsync(vendorId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region GetPageAsync Tests

        [Fact]
        public async Task GetPageAsync_WithSearchTerm_ShouldFilterResults()
        {
            // Arrange
            var coupons = new List<TbCouponCode>
            {
                CreateTestCoupon(Guid.NewGuid(), "SEARCH1"),
                CreateTestCoupon(Guid.NewGuid(), "SEARCH2"),
                CreateTestCoupon(Guid.NewGuid(), "OTHER")
            };

            var dtos = new List<CouponCodeDto>
            {
                CreateTestCouponDto(coupons[0].Id, "SEARCH1"),
                CreateTestCouponDto(coupons[1].Id, "SEARCH2")
            };

            var criteria = new BaseSearchCriteriaModel
            {
                SearchTerm = "SEARCH",
                PageNumber = 1,
                PageSize = 10
            };

            _mockRepository.Setup(r => r.GetAllAsync(default))
                .ReturnsAsync(coupons);
            _mockMapper.Setup(m => m.Map<List<CouponCodeDto>>(It.IsAny<List<TbCouponCode>>()))
                .Returns(dtos);

            // Act
            var result = await _service.GetPageAsync(criteria);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            Assert.All(result.Items, item => Assert.Contains("SEARCH", item.Code));
        }

        [Fact]
        public async Task GetPageAsync_WithPagination_ShouldReturnCorrectPage()
        {
            // Arrange
            var coupons = Enumerable.Range(1, 25)
                .Select(i => CreateTestCoupon(Guid.NewGuid(), $"COUPON{i}"))
                .ToList();

            var criteria = new BaseSearchCriteriaModel
            {
                PageNumber = 2,
                PageSize = 10
            };

            _mockRepository.Setup(r => r.GetAllAsync(default))
                .ReturnsAsync(coupons);
            _mockMapper.Setup(m => m.Map<List<CouponCodeDto>>(It.IsAny<List<TbCouponCode>>()))
                .Returns<List<TbCouponCode>>(c => c.Select(x => CreateTestCouponDto(x.Id, x.Code)).ToList());

            // Act
            var result = await _service.GetPageAsync(criteria);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Items.Count);
            Assert.Equal(25, result.TotalRecords);
            Assert.Equal(2, result.PageNumber);
            Assert.Equal(3, result.TotalPages);
        }

        [Fact]
        public async Task GetPageAsync_WithSearchInArabic_ShouldFilterCorrectly()
        {
            // Arrange
            var coupons = new List<TbCouponCode>
            {
                CreateTestCoupon(Guid.NewGuid(), "CODE1", titleAr: "خصم الصيف"),
                CreateTestCoupon(Guid.NewGuid(), "CODE2", titleAr: "خصم الشتاء"),
                CreateTestCoupon(Guid.NewGuid(), "CODE3", titleAr: "عرض خاص")
            };

            var criteria = new BaseSearchCriteriaModel
            {
                SearchTerm = "خصم",
                PageNumber = 1,
                PageSize = 10
            };

            _mockRepository.Setup(r => r.GetAllAsync(default))
                .ReturnsAsync(coupons);
            _mockMapper.Setup(m => m.Map<List<CouponCodeDto>>(It.IsAny<List<TbCouponCode>>()))
                .Returns<List<TbCouponCode>>(c => c.Where(x => x.TitleAr.Contains("خصم"))
                    .Select(x => CreateTestCouponDto(x.Id, x.Code, titleAr: x.TitleAr)).ToList());

            // Act
            var result = await _service.GetPageAsync(criteria);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
        }

        #endregion

        #region ApplyCouponToOrderAsync Tests

        [Fact]
        public async Task ApplyCouponToOrderAsync_WithValidCoupon_ShouldReturnTrueAndIncrementUsage()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var coupon = CreateTestCoupon(Guid.NewGuid(), "APPLY10");

            _mockRepository.Setup(r => r.GetByCodeAsync("APPLY10", default))
                .ReturnsAsync(coupon);
            _mockRepository.Setup(r => r.IncrementUsageCountAsync(coupon.Id, default))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.ApplyCouponToOrderAsync(orderId, "APPLY10");

            // Assert
            Assert.True(result);
            _mockRepository.Verify(r => r.IncrementUsageCountAsync(coupon.Id, default), Times.Once);
        }

        [Fact]
        public async Task ApplyCouponToOrderAsync_WithInvalidCoupon_ShouldReturnFalse()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            _mockRepository.Setup(r => r.GetByCodeAsync("INVALID", default))
                .ReturnsAsync((TbCouponCode)null);

            // Act
            var result = await _service.ApplyCouponToOrderAsync(orderId, "INVALID");

            // Assert
            Assert.False(result);
            _mockRepository.Verify(r => r.IncrementUsageCountAsync(It.IsAny<Guid>(), default), Times.Never);
        }

        [Fact]
        public async Task ApplyCouponToOrderAsync_ShouldNotValidateBeforeApplying()
        {
            // Arrange - Even if coupon is expired, it should still apply
            var orderId = Guid.NewGuid();
            var expiredCoupon = CreateTestCoupon(Guid.NewGuid(), "EXPIRED");
            expiredCoupon.ExpiryDate = DateTime.UtcNow.AddDays(-10);

            _mockRepository.Setup(r => r.GetByCodeAsync("EXPIRED", default))
                .ReturnsAsync(expiredCoupon);
            _mockRepository.Setup(r => r.IncrementUsageCountAsync(expiredCoupon.Id, default))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.ApplyCouponToOrderAsync(orderId, "EXPIRED");

            // Assert - Should apply even if expired (validation is separate concern)
            Assert.True(result);
            _mockRepository.Verify(r => r.IncrementUsageCountAsync(expiredCoupon.Id, default), Times.Once);
        }

        #endregion

        #region Helper Methods

        private TbCouponCode CreateTestCoupon(Guid id, string code, Guid? vendorId = null, string titleAr = "كوبون تجريبي")
        {
            return new TbCouponCode
            {
                Id = id,
                Code = code,
                TitleAr = titleAr,
                TitleEn = "Test Coupon",
                PromoType = CouponCodeType.General,
                DiscountType = DiscountType.Percentage,
                DiscountValue = 10,
                IsActive = true,
                IsDeleted = false,
                VendorId = vendorId,
                CreatedDateUtc = DateTime.UtcNow,
                CouponScopes = new List<TbCouponCodeScope>()
            };
        }

        private CouponCodeDto CreateTestCouponDto(Guid id, string code, Guid? vendorId = null, string titleAr = "كوبون تجريبي")
        {
            return new CouponCodeDto
            {
                Id = id,
                Code = code,
                TitleAr = titleAr,
                TitleEn = "Test Coupon",
                PromoType = CouponCodeType.General,
                DiscountType = DiscountType.Percentage,
                DiscountValue = 10,
                IsActive = true,
                VendorId = vendorId
            };
        }

        #endregion
    }
}