using AutoMapper;
using BL.Contracts.IMapper;
using BL.Services.Merchandising.CouponCode;
using Common.Enumerations.Order;
using DAL.Contracts.Repositories.Merchandising;
using DAL.Contracts.UnitOfWork;
using DAL.ResultModels;
using Domains.Entities.Merchandising.CouponCode;
using Moq;
using Shared.DTOs.Order.CouponCode;

namespace UnitTests.Core.BL.Services.Merchandising
{
    /// <summary>
    /// Unit tests for CouponCodeService - Promo Type Specific Scenarios
    /// </summary>
    public class CouponCodeServiceTests_PromoTypes
    {
        private readonly Mock<ICouponCodeRepository> _mockRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IBaseMapper> _mockBaseMapper;
        private readonly CouponCodeService _service;
        private readonly Guid _testUserId = Guid.NewGuid();

        public CouponCodeServiceTests_PromoTypes()
        {
            _mockRepository = new Mock<ICouponCodeRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockBaseMapper = new Mock<IBaseMapper>();
            _service = new CouponCodeService(_mockRepository.Object, _mockUnitOfWork.Object, _mockBaseMapper.Object);
        }

        #region General Promo Type Tests

        [Fact]
        public async Task SaveAsync_GeneralPromoType_ShouldNotRequireScopes()
        {
            // Arrange
            var dto = CreateTestCouponDto(Guid.Empty, "GENERAL10");
            dto.PromoType = CouponCodeType.General;
            dto.ScopeItems = null;

            var entity = CreateTestCoupon(Guid.NewGuid(), "GENERAL10");
            var saveResult = new SaveResult { Success = true, Id = entity.Id };

            _mockRepository.Setup(r => r.IsCodeUniqueAsync("GENERAL10", null, default))
                .ReturnsAsync(true);
            _mockMapper.Setup(m => m.Map<TbCouponCode>(dto))
                .Returns(entity);
            _mockRepository.Setup(r => r.SaveAsync(It.IsAny<TbCouponCode>(), _testUserId, default))
                .ReturnsAsync(saveResult);
            _mockRepository.Setup(r => r.GetByIdAsync(entity.Id, default))
                .ReturnsAsync(entity);
            _mockMapper.Setup(m => m.Map<CouponCodeDto>(entity))
                .Returns(dto);

            // Act
            var result = await _service.SaveAsync(dto, _testUserId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(CouponCodeType.General, result.PromoType);
        }

        [Fact]
        public async Task SaveAsync_GeneralPromoType_WithMinimumProductPrice_ShouldSucceed()
        {
            // Arrange
            var dto = CreateTestCouponDto(Guid.Empty, "PRICEBASED");
            dto.PromoType = CouponCodeType.General;
            dto.MinimumProductPrice = 1000;

            var entity = CreateTestCoupon(Guid.NewGuid(), "PRICEBASED");
            entity.MinimumProductPrice = 1000;
            var saveResult = new SaveResult { Success = true, Id = entity.Id };

            _mockRepository.Setup(r => r.IsCodeUniqueAsync("PRICEBASED", null, default))
                .ReturnsAsync(true);
            _mockMapper.Setup(m => m.Map<TbCouponCode>(dto))
                .Returns(entity);
            _mockRepository.Setup(r => r.SaveAsync(It.IsAny<TbCouponCode>(), _testUserId, default))
                .ReturnsAsync(saveResult);
            _mockRepository.Setup(r => r.GetByIdAsync(entity.Id, default))
                .ReturnsAsync(entity);
            _mockMapper.Setup(m => m.Map<CouponCodeDto>(entity))
                .Returns(dto);

            // Act
            var result = await _service.SaveAsync(dto, _testUserId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1000, result.MinimumProductPrice);
        }

        #endregion

        #region CategoryBased Promo Type Tests

        [Fact]
        public async Task SaveAsync_CategoryBased_WithoutScopes_ShouldThrowException()
        {
            // Arrange
            var dto = CreateTestCouponDto(Guid.Empty, "CAT10");
            dto.PromoType = CouponCodeType.CategoryBased;
            dto.ScopeItems = null;

            _mockRepository.Setup(r => r.IsCodeUniqueAsync("CAT10", null, default))
                .ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.SaveAsync(dto, _testUserId));
            Assert.Equal("يجب تحديد العناصر المؤهلة للخصم", exception.Message);
        }

        [Fact]
        public async Task SaveAsync_CategoryBased_WithEmptyScopes_ShouldThrowException()
        {
            // Arrange
            var dto = CreateTestCouponDto(Guid.Empty, "CAT10");
            dto.PromoType = CouponCodeType.CategoryBased;
            dto.ScopeItems = new List<CouponScopeDto>();

            _mockRepository.Setup(r => r.IsCodeUniqueAsync("CAT10", null, default))
                .ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.SaveAsync(dto, _testUserId));
            Assert.Equal("يجب تحديد العناصر المؤهلة للخصم", exception.Message);
        }

        [Fact]
        public async Task SaveAsync_CategoryBased_WithValidScopes_ShouldSucceed()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var dto = CreateTestCouponDto(Guid.Empty, "CAT10");
            dto.PromoType = CouponCodeType.CategoryBased;
            dto.ScopeItems = new List<CouponScopeDto>
            {
                new CouponScopeDto { ScopeType = CouponCodeScopeType.Category, ScopeId = categoryId }
            };

            var entity = CreateTestCoupon(Guid.NewGuid(), "CAT10");
            var saveResult = new SaveResult { Success = true, Id = entity.Id };

            _mockRepository.Setup(r => r.IsCodeUniqueAsync("CAT10", null, default))
                .ReturnsAsync(true);
            _mockMapper.Setup(m => m.Map<TbCouponCode>(dto))
                .Returns(entity);
            _mockRepository.Setup(r => r.SaveAsync(It.IsAny<TbCouponCode>(), _testUserId, default))
                .ReturnsAsync(saveResult);
            _mockRepository.Setup(r => r.GetByIdAsync(entity.Id, default))
                .ReturnsAsync(entity);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<TbCouponCode>(), default))
                .ReturnsAsync(entity);
            _mockMapper.Setup(m => m.Map<CouponCodeDto>(entity))
                .Returns(dto);

            // Act
            var result = await _service.SaveAsync(dto, _testUserId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(CouponCodeType.CategoryBased, result.PromoType);
        }

        [Fact]
        public async Task SaveAsync_CategoryBased_WithMultipleCategories_ShouldSucceed()
        {
            // Arrange
            var dto = CreateTestCouponDto(Guid.Empty, "MULTICAT");
            dto.PromoType = CouponCodeType.CategoryBased;
            dto.ScopeItems = new List<CouponScopeDto>
            {
                new CouponScopeDto { ScopeType = CouponCodeScopeType.Category, ScopeId = Guid.NewGuid() },
                new CouponScopeDto { ScopeType = CouponCodeScopeType.Category, ScopeId = Guid.NewGuid() },
                new CouponScopeDto { ScopeType = CouponCodeScopeType.Category, ScopeId = Guid.NewGuid() }
            };

            var entity = CreateTestCoupon(Guid.NewGuid(), "MULTICAT");
            var saveResult = new SaveResult { Success = true, Id = entity.Id };

            _mockRepository.Setup(r => r.IsCodeUniqueAsync("MULTICAT", null, default))
                .ReturnsAsync(true);
            _mockMapper.Setup(m => m.Map<TbCouponCode>(dto))
                .Returns(entity);
            _mockRepository.Setup(r => r.SaveAsync(It.IsAny<TbCouponCode>(), _testUserId, default))
                .ReturnsAsync(saveResult);
            _mockRepository.Setup(r => r.GetByIdAsync(entity.Id, default))
                .ReturnsAsync(entity);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<TbCouponCode>(), default))
                .ReturnsAsync(entity);
            _mockMapper.Setup(m => m.Map<CouponCodeDto>(entity))
                .Returns(dto);

            // Act
            var result = await _service.SaveAsync(dto, _testUserId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.ScopeItems.Count);
        }

        #endregion

        #region VendorBased Promo Type Tests

        [Fact]
        public async Task SaveAsync_VendorBased_WithoutScopes_ShouldThrowException()
        {
            // Arrange
            var dto = CreateTestCouponDto(Guid.Empty, "VENDOR10");
            dto.PromoType = CouponCodeType.VendorBased;
            dto.VendorId = Guid.NewGuid();
            dto.ScopeItems = null;

            _mockRepository.Setup(r => r.IsCodeUniqueAsync("VENDOR10", null, default))
                .ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.SaveAsync(dto, _testUserId));
            Assert.Equal("يجب تحديد العناصر المؤهلة للخصم", exception.Message);
        }

        [Fact]
        public async Task SaveAsync_VendorBased_WithValidScopes_ShouldSucceed()
        {
            // Arrange
            var vendorId = Guid.NewGuid();
            var dto = CreateTestCouponDto(Guid.Empty, "VENDOR10");
            dto.PromoType = CouponCodeType.VendorBased;
            dto.VendorId = vendorId;
            dto.ScopeItems = new List<CouponScopeDto>
            {
                new CouponScopeDto { ScopeType = CouponCodeScopeType.Item, ScopeId = Guid.NewGuid() }
            };

            var entity = CreateTestCoupon(Guid.NewGuid(), "VENDOR10");
            entity.VendorId = vendorId;
            var saveResult = new SaveResult { Success = true, Id = entity.Id };

            _mockRepository.Setup(r => r.IsCodeUniqueAsync("VENDOR10", null, default))
                .ReturnsAsync(true);
            _mockMapper.Setup(m => m.Map<TbCouponCode>(dto))
                .Returns(entity);
            _mockRepository.Setup(r => r.SaveAsync(It.IsAny<TbCouponCode>(), _testUserId, default))
                .ReturnsAsync(saveResult);
            _mockRepository.Setup(r => r.GetByIdAsync(entity.Id, default))
                .ReturnsAsync(entity);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<TbCouponCode>(), default))
                .ReturnsAsync(entity);
            _mockMapper.Setup(m => m.Map<CouponCodeDto>(entity))
                .Returns(dto);

            // Act
            var result = await _service.SaveAsync(dto, _testUserId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(CouponCodeType.VendorBased, result.PromoType);
            Assert.Equal(vendorId, result.VendorId);
        }

        #endregion

        #region Co-Funded Tests

        [Fact]
        public async Task SaveAsync_CoFunded_WithVendorId_ShouldSucceed()
        {
            // Arrange
            var vendorId = Guid.NewGuid();
            var dto = CreateTestCouponDto(Guid.Empty, "COFUND");
            dto.VendorId = vendorId;
            dto.PlatformSharePercentage = 60;

            var entity = CreateTestCoupon(Guid.NewGuid(), "COFUND");
            entity.VendorId = vendorId;
            entity.PlatformSharePercentage = 60;
            var saveResult = new SaveResult { Success = true, Id = entity.Id };

            _mockRepository.Setup(r => r.IsCodeUniqueAsync("COFUND", null, default))
                .ReturnsAsync(true);
            _mockMapper.Setup(m => m.Map<TbCouponCode>(dto))
                .Returns(entity);
            _mockRepository.Setup(r => r.SaveAsync(It.IsAny<TbCouponCode>(), _testUserId, default))
                .ReturnsAsync(saveResult);
            _mockRepository.Setup(r => r.GetByIdAsync(entity.Id, default))
                .ReturnsAsync(entity);
            _mockMapper.Setup(m => m.Map<CouponCodeDto>(entity))
                .Returns(dto);

            // Act
            var result = await _service.SaveAsync(dto, _testUserId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(60, result.PlatformSharePercentage);
            Assert.Equal(vendorId, result.VendorId);
        }

        [Fact]
        public async Task SaveAsync_CoFunded_WithoutVendorId_ShouldThrowException()
        {
            // Arrange
            var dto = CreateTestCouponDto(Guid.Empty, "COFUND");
            dto.VendorId = null;
            dto.PlatformSharePercentage = 60;

            _mockRepository.Setup(r => r.IsCodeUniqueAsync("COFUND", null, default))
                .ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.SaveAsync(dto, _testUserId));
            Assert.Equal("يجب تحديد البائع للكوبونات المشتركة", exception.Message);
        }

        [Fact]
        public async Task SaveAsync_CoFunded_WithZeroPlatformShare_ShouldNotRequireVendor()
        {
            // Arrange
            var dto = CreateTestCouponDto(Guid.Empty, "ZEROFUND");
            dto.VendorId = null;
            dto.PlatformSharePercentage = 0;

            var entity = CreateTestCoupon(Guid.NewGuid(), "ZEROFUND");
            var saveResult = new SaveResult { Success = true, Id = entity.Id };

            _mockRepository.Setup(r => r.IsCodeUniqueAsync("ZEROFUND", null, default))
                .ReturnsAsync(true);
            _mockMapper.Setup(m => m.Map<TbCouponCode>(dto))
                .Returns(entity);
            _mockRepository.Setup(r => r.SaveAsync(It.IsAny<TbCouponCode>(), _testUserId, default))
                .ReturnsAsync(saveResult);
            _mockRepository.Setup(r => r.GetByIdAsync(entity.Id, default))
                .ReturnsAsync(entity);
            _mockMapper.Setup(m => m.Map<CouponCodeDto>(entity))
                .Returns(dto);

            // Act
            var result = await _service.SaveAsync(dto, _testUserId);

            // Assert
            Assert.NotNull(result);
        }

        #endregion

        #region Helper Methods

        private TbCouponCode CreateTestCoupon(Guid id, string code)
        {
            return new TbCouponCode
            {
                Id = id,
                Code = code,
                TitleAr = "كوبون تجريبي",
                TitleEn = "Test Coupon",
                PromoType = CouponCodeType.General,
                DiscountType = DiscountType.Percentage,
                DiscountValue = 10,
                IsActive = true,
                IsDeleted = false,
                CreatedDateUtc = DateTime.UtcNow,
                CouponScopes = new List<TbCouponCodeScope>()
            };
        }

        private CouponCodeDto CreateTestCouponDto(Guid id, string code)
        {
            return new CouponCodeDto
            {
                Id = id,
                Code = code,
                TitleAr = "كوبون تجريبي",
                TitleEn = "Test Coupon",
                PromoType = CouponCodeType.General,
                DiscountType = DiscountType.Percentage,
                DiscountValue = 10,
                IsActive = true
            };
        }

        #endregion
    }
}