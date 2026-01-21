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
    /// Unit tests for CouponCodeService - CRUD Operations
    /// </summary>
    public class CouponCodeServiceTests_CRUD
    {
        private readonly Mock<ICouponCodeRepository> _mockRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IBaseMapper> _mockBaseMapper;
        private readonly CouponCodeService _service;
        private readonly Guid _testUserId = Guid.NewGuid();

        public CouponCodeServiceTests_CRUD()
        {
            _mockRepository = new Mock<ICouponCodeRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockBaseMapper = new Mock<IBaseMapper>();
            _service = new CouponCodeService(_mockRepository.Object, _mockUnitOfWork.Object, _mockBaseMapper.Object);
        }

        #region GetByIdAsync Tests

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnCouponDto()
        {
            // Arrange
            var couponId = Guid.NewGuid();
            var coupon = CreateTestCoupon(couponId);
            var couponDto = CreateTestCouponDto(couponId);

            _mockRepository.Setup(r => r.GetByIdAsync(couponId, default))
                .ReturnsAsync(coupon);
            _mockMapper.Setup(m => m.Map<CouponCodeDto>(coupon))
                .Returns(couponDto);

            // Act
            var result = await _service.GetByIdAsync(couponId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(couponId, result.Id);
            Assert.Equal("TEST10", result.Code);
            _mockRepository.Verify(r => r.GetByIdAsync(couponId, default), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var couponId = Guid.NewGuid();
            _mockRepository.Setup(r => r.GetByIdAsync(couponId, default))
                .ReturnsAsync((TbCouponCode)null);

            // Act
            var result = await _service.GetByIdAsync(couponId);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(r => r.GetByIdAsync(couponId, default), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WithEmptyGuid_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(Guid.Empty, default))
                .ReturnsAsync((TbCouponCode)null);

            // Act
            var result = await _service.GetByIdAsync(Guid.Empty);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region GetAllAsync Tests

        [Fact]
        public async Task GetAllAsync_WithCoupons_ShouldReturnAllCoupons()
        {
            // Arrange
            var coupons = new List<TbCouponCode>
            {
                CreateTestCoupon(Guid.NewGuid(), "COUPON1"),
                CreateTestCoupon(Guid.NewGuid(), "COUPON2"),
                CreateTestCoupon(Guid.NewGuid(), "COUPON3")
            };

            var couponDtos = new List<CouponCodeDto>
            {
                CreateTestCouponDto(coupons[0].Id, "COUPON1"),
                CreateTestCouponDto(coupons[1].Id, "COUPON2"),
                CreateTestCouponDto(coupons[2].Id, "COUPON3")
            };

            _mockRepository.Setup(r => r.GetAllAsync(default))
                .ReturnsAsync(coupons);
            _mockMapper.Setup(m => m.Map<IEnumerable<CouponCodeDto>>(coupons))
                .Returns(couponDtos);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            _mockRepository.Verify(r => r.GetAllAsync(default), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WithNoCoupons_ShouldReturnEmptyList()
        {
            // Arrange
            var emptyCoupons = new List<TbCouponCode>();
            _mockRepository.Setup(r => r.GetAllAsync(default))
                .ReturnsAsync(emptyCoupons);
            _mockMapper.Setup(m => m.Map<IEnumerable<CouponCodeDto>>(emptyCoupons))
                .Returns(new List<CouponCodeDto>());

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region SaveAsync Tests - Create

        [Fact]
        public async Task SaveAsync_CreateNewCoupon_ShouldReturnCreatedCoupon()
        {
            // Arrange
            var dto = CreateTestCouponDto(Guid.Empty, "NEW10");
            var entity = CreateTestCoupon(Guid.NewGuid(), "NEW10");
            var saveResult = new SaveResult { Success = true, Id = entity.Id };

            _mockRepository.Setup(r => r.IsCodeUniqueAsync("NEW10", null, default))
                .ReturnsAsync(true);
            _mockMapper.Setup(m => m.Map<TbCouponCode>(dto))
                .Returns(entity);
            _mockRepository.Setup(r => r.SaveAsync(It.IsAny<TbCouponCode>(), _testUserId, default))
                .ReturnsAsync(saveResult);
            _mockRepository.Setup(r => r.GetByIdAsync(entity.Id, default))
                .ReturnsAsync(entity);
            _mockMapper.Setup(m => m.Map<CouponCodeDto>(entity))
                .Returns(CreateTestCouponDto(entity.Id, "NEW10"));

            // Act
            var result = await _service.SaveAsync(dto, _testUserId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("NEW10", result.Code);
            _mockRepository.Verify(r => r.IsCodeUniqueAsync("NEW10", null, default), Times.Once);
            _mockRepository.Verify(r => r.SaveAsync(It.IsAny<TbCouponCode>(), _testUserId, default), Times.Once);
        }

        [Fact]
        public async Task SaveAsync_CreateWithDuplicateCode_ShouldThrowException()
        {
            // Arrange
            var dto = CreateTestCouponDto(Guid.Empty, "DUPLICATE");
            _mockRepository.Setup(r => r.IsCodeUniqueAsync("DUPLICATE", null, default))
                .ReturnsAsync(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.SaveAsync(dto, _testUserId));
            Assert.Equal("رمز الكوبون مستخدم بالفعل", exception.Message);
        }

        [Fact]
        public async Task SaveAsync_CreateWithPercentageOver100_ShouldThrowException()
        {
            // Arrange
            var dto = CreateTestCouponDto(Guid.Empty, "INVALID");
            dto.DiscountType = DiscountType.Percentage;
            dto.DiscountValue = 150;

            _mockRepository.Setup(r => r.IsCodeUniqueAsync("INVALID", null, default))
                .ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.SaveAsync(dto, _testUserId));
            Assert.Equal("النسبة المئوية لا يمكن أن تتجاوز 100%", exception.Message);
        }

        [Fact]
        public async Task SaveAsync_CreateWithInvalidDates_ShouldThrowException()
        {
            // Arrange
            var dto = CreateTestCouponDto(Guid.Empty, "INVALID");
            dto.StartDate = DateTime.UtcNow.AddDays(10);
            dto.ExpiryDate = DateTime.UtcNow.AddDays(5);

            _mockRepository.Setup(r => r.IsCodeUniqueAsync("INVALID", null, default))
                .ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.SaveAsync(dto, _testUserId));
            Assert.Equal("تاريخ البدء يجب أن يكون قبل تاريخ الانتهاء", exception.Message);
        }

        [Fact]
        public async Task SaveAsync_CreateCategoryBasedWithoutScopes_ShouldThrowException()
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
        public async Task SaveAsync_CreateCoFundedWithoutVendor_ShouldThrowException()
        {
            // Arrange
            var dto = CreateTestCouponDto(Guid.Empty, "COFUND");
            dto.PlatformSharePercentage = 50;
            dto.VendorId = null;

            _mockRepository.Setup(r => r.IsCodeUniqueAsync("COFUND", null, default))
                .ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.SaveAsync(dto, _testUserId));
            Assert.Equal("يجب تحديد البائع للكوبونات المشتركة", exception.Message);
        }

        [Fact]
        public async Task SaveAsync_CreateWithScopes_ShouldAddScopesSuccessfully()
        {
            // Arrange
            var dto = CreateTestCouponDto(Guid.Empty, "SCOPED");
            dto.PromoType = CouponCodeType.CategoryBased;
            dto.ScopeItems = new List<CouponScopeDto>
            {
                new CouponScopeDto { ScopeType = CouponCodeScopeType.Category, ScopeId = Guid.NewGuid() }
            };

            var entity = CreateTestCoupon(Guid.NewGuid(), "SCOPED");
            var saveResult = new SaveResult { Success = true, Id = entity.Id };

            _mockRepository.Setup(r => r.IsCodeUniqueAsync("SCOPED", null, default))
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
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<TbCouponCode>(), default), Times.Once);
        }

        #endregion

        #region SaveAsync Tests - Update

        [Fact]
        public async Task SaveAsync_UpdateExistingCoupon_ShouldUpdateSuccessfully()
        {
            // Arrange
            var couponId = Guid.NewGuid();
            var dto = CreateTestCouponDto(couponId, "UPDATE10");
            dto.TitleAr = "عنوان محدث";
            dto.IsActive = false;

            var existingEntity = CreateTestCoupon(couponId, "UPDATE10");
            var updateResult = new SaveResult { Success = true, Id = couponId };

            _mockRepository.Setup(r => r.GetByIdAsync(couponId, default))
                .ReturnsAsync(existingEntity);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<TbCouponCode>(), _testUserId, default))
                .ReturnsAsync(updateResult);
            _mockMapper.Setup(m => m.Map<CouponCodeDto>(existingEntity))
                .Returns(dto);

            // Act
            var result = await _service.SaveAsync(dto, _testUserId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("عنوان محدث", result.TitleAr);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<TbCouponCode>(), _testUserId, default), Times.Once);
        }

        [Fact]
        public async Task SaveAsync_UpdateNonExistentCoupon_ShouldThrowException()
        {
            // Arrange
            var couponId = Guid.NewGuid();
            var dto = CreateTestCouponDto(couponId, "NOTFOUND");

            _mockRepository.Setup(r => r.GetByIdAsync(couponId, default))
                .ReturnsAsync((TbCouponCode)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.SaveAsync(dto, _testUserId));
        }

        #endregion

        #region DeleteAsync Tests

        [Fact]
        public async Task DeleteAsync_WithValidId_ShouldReturnTrue()
        {
            // Arrange
            var couponId = Guid.NewGuid();
            _mockRepository.Setup(r => r.SoftDeleteAsync(couponId, _testUserId, default))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteAsync(couponId, _testUserId);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(r => r.SoftDeleteAsync(couponId, _testUserId, default), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithInvalidId_ShouldReturnFalse()
        {
            // Arrange
            var couponId = Guid.NewGuid();
            _mockRepository.Setup(r => r.SoftDeleteAsync(couponId, _testUserId, default))
                .ReturnsAsync(false);

            // Act
            var result = await _service.DeleteAsync(couponId, _testUserId);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region Helper Methods

        private TbCouponCode CreateTestCoupon(Guid id, string code = "TEST10")
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

        private CouponCodeDto CreateTestCouponDto(Guid id, string code = "TEST10")
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