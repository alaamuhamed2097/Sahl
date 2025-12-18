//using BL.Service.ECommerce.Item;
//using Common.Enumerations.Fulfillment;
//using Common.Filters;
//using DAL.Contracts.Repositories;
//using DAL.Models.ItemSearch;
//using Domains.Procedures;
//using Domains.Views.Item;
//using Moq;
//using Serilog;

//namespace UnitTests.Core.BL.Services.ECommerce.Item
//{
//    public class ItemSearchServiceTests
//    {
//        private readonly Mock<IItemSearchRepository> _mockRepository;
//        private readonly Mock<ILogger> _mockLogger;
//        private readonly ItemSearchService _service;

//        public ItemSearchServiceTests()
//        {
//            _mockRepository = new Mock<IItemSearchRepository>();
//            _mockLogger = new Mock<ILogger>();
//            _service = new ItemSearchService(_mockRepository.Object, _mockLogger.Object);
//        }

//        #region Constructor Tests

//        [Fact]
//        public void Constructor_WithNullRepository_ThrowsArgumentNullException()
//        {
//            // Assert
//            Assert.Throws<ArgumentNullException>(() =>
//                new ItemSearchService(null, _mockLogger.Object));
//        }

//        [Fact]
//        public void Constructor_WithNullLogger_ThrowsArgumentNullException()
//        {
//            // Assert
//            Assert.Throws<ArgumentNullException>(() =>
//                new ItemSearchService(_mockRepository.Object, null));
//        }

//        #endregion

//        #region SearchItemsAsync Tests

//        [Fact]
//        public async Task SearchItemsAsync_WithNullFilter_ThrowsArgumentNullException()
//        {
//            // Act & Assert
//            await Assert.ThrowsAsync<ArgumentNullException>(() =>
//                _service.SearchItemsAsync(null));
//        }

//        [Fact]
//        public async Task SearchItemsAsync_WithValidFilter_ReturnsPagedResults()
//        {
//            // Arrange
//            var filter = new ItemFilterQuery
//            {
//                SearchTerm = "laptop",
//                PageNumber = 1,
//                PageSize = 20
//            };

//            var mockEntities = new List<SpSearchItemsMultiVendor>
//            {
//                CreateMockSearchEntity(Guid.NewGuid(), "Laptop 1", 1000m, 1500m),
//                CreateMockSearchEntity(Guid.NewGuid(), "Laptop 2", 2000m, 2500m)
//            };

//            _mockRepository
//                .Setup(r => r.SearchItemsAsync(It.IsAny<ItemFilterQuery>(), It.IsAny<CancellationToken>()))
//                .ReturnsAsync((mockEntities, 2));

//            // Act
//            var result = await _service.SearchItemsAsync(filter);

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal(2, result.TotalCount);
//            Assert.Equal(2, result.Items.Count);
//            Assert.Equal(1, result.PageNumber);
//            Assert.Equal(20, result.PageSize);
//            Assert.Equal(1, result.TotalPages);
//        }

//        [Fact]
//        public async Task SearchItemsAsync_WithInvalidPageNumber_SanitizesToOne()
//        {
//            // Arrange
//            var filter = new ItemFilterQuery
//            {
//                PageNumber = 0,
//                PageSize = 20
//            };

//            _mockRepository
//                .Setup(r => r.SearchItemsAsync(It.IsAny<ItemFilterQuery>(), It.IsAny<CancellationToken>()))
//                .ReturnsAsync((new List<SpSearchItemsMultiVendor>(), 0));

//            // Act
//            var result = await _service.SearchItemsAsync(filter);

//            // Assert
//            Assert.Equal(1, filter.PageNumber);
//        }

//        [Fact]
//        public async Task SearchItemsAsync_WithInvalidPageSize_SanitizesToTwenty()
//        {
//            // Arrange
//            var filter = new ItemFilterQuery
//            {
//                PageNumber = 1,
//                PageSize = 150
//            };

//            _mockRepository
//                .Setup(r => r.SearchItemsAsync(It.IsAny<ItemFilterQuery>(), It.IsAny<CancellationToken>()))
//                .ReturnsAsync((new List<SpSearchItemsMultiVendor>(), 0));

//            // Act
//            await _service.SearchItemsAsync(filter);

//            // Assert
//            Assert.Equal(20, filter.PageSize);
//        }

//        [Fact]
//        public async Task SearchItemsAsync_WithMinPriceGreaterThanMaxPrice_ThrowsArgumentException()
//        {
//            // Arrange
//            var filter = new ItemFilterQuery
//            {
//                PageNumber = 1,
//                PageSize = 20,
//                MinPrice = 1000,
//                MaxPrice = 500
//            };

//            // Act & Assert
//            await Assert.ThrowsAsync<ArgumentException>(() =>
//                _service.SearchItemsAsync(filter));
//        }

//        [Fact]
//        public async Task SearchItemsAsync_WithInvalidSortBy_DefaultsToRelevance()
//        {
//            // Arrange
//            var filter = new ItemFilterQuery
//            {
//                PageNumber = 1,
//                PageSize = 20,
//                SortBy = "invalid_sort"
//            };

//            _mockRepository
//                .Setup(r => r.SearchItemsAsync(It.IsAny<ItemFilterQuery>(), It.IsAny<CancellationToken>()))
//                .ReturnsAsync((new List<SpSearchItemsMultiVendor>(), 0));

//            // Act
//            await _service.SearchItemsAsync(filter);

//            // Assert
//            Assert.Equal("relevance", filter.SortBy);
//        }

//        [Fact]
//        public async Task SearchItemsAsync_WithBestOfferData_ParsesCorrectly()
//        {
//            // Arrange
//            var itemId = Guid.NewGuid();
//            var offerId = Guid.NewGuid();
//            var vendorId = Guid.NewGuid();

//            var filter = new ItemFilterQuery { PageNumber = 1, PageSize = 20 };

//            var entity = CreateMockSearchEntity(itemId, "Test Item", 900m, 1000m);
//            //entity.BestOfferDataRaw = $"{offerId}|{vendorId}|900.00|1000.00|10|1|3|1|1";

//            _mockRepository
//                .Setup(r => r.SearchItemsAsync(It.IsAny<ItemFilterQuery>(), It.IsAny<CancellationToken>()))
//                .ReturnsAsync((new List<SpSearchItemsMultiVendor> { entity }, 1));

//            // Act
//            var result = await _service.SearchItemsAsync(filter);

//            // Assert
//            var item = result.Items.First();
//            Assert.NotNull(item.BestOffer);
//            Assert.Equal(offerId, item.BestOffer.OfferId);
//            Assert.Equal(vendorId, item.BestOffer.VendorId);
//            Assert.Equal(900m, item.BestOffer.SalesPrice);
//            Assert.Equal(1000m, item.BestOffer.OriginalPrice);
//            Assert.Equal(10, item.BestOffer.AvailableQuantity);
//            Assert.True(item.BestOffer.IsFreeShipping);
//            Assert.Equal(3, item.BestOffer.EstimatedDeliveryDays);
//            Assert.True(item.BestOffer.IsBuyBoxWinner);
//            Assert.Equal(FulfillmentType.Marketplace, item.BestOffer.FulfillmentType);
//            Assert.Equal(10, item.BestOffer.DiscountPercentage);
//        }

//        [Fact]
//        public async Task SearchItemsAsync_EnrichesBadgesCorrectly()
//        {
//            // Arrange
//            var filter = new ItemFilterQuery { PageNumber = 1, PageSize = 20 };

//            var entity = CreateMockSearchEntity(Guid.NewGuid(), "Test", 800m, 1000m);

//            _mockRepository
//                .Setup(r => r.SearchItemsAsync(It.IsAny<ItemFilterQuery>(), It.IsAny<CancellationToken>()))
//                .ReturnsAsync((new List<SpSearchItemsMultiVendor> { entity }, 1));

//            // Act
//            var result = await _service.SearchItemsAsync(filter);

//            // Assert
//            var item = result.Items.First();
//            Assert.Contains("مميز", item.Badges);
//            Assert.Contains("6 عروض", item.Badges);
//            Assert.Contains("خصم 20%", item.Badges);
//            Assert.Contains("الأفضل مبيعاً", item.Badges);
//            Assert.Contains("شحن مجاني", item.Badges);
//        }

//        [Fact]
//        public async Task SearchItemsAsync_CalculatesTotalPagesCorrectly()
//        {
//            // Arrange
//            var filter = new ItemFilterQuery { PageNumber = 1, PageSize = 10 };

//            _mockRepository
//                .Setup(r => r.SearchItemsAsync(It.IsAny<ItemFilterQuery>(), It.IsAny<CancellationToken>()))
//                .ReturnsAsync((new List<SpSearchItemsMultiVendor>(), 25));

//            // Act
//            var result = await _service.SearchItemsAsync(filter);

//            // Assert
//            Assert.Equal(25, result.TotalCount);
//            Assert.Equal(3, result.TotalPages);
//        }

//        #endregion

//        #region GetAvailableFiltersAsync Tests

//        [Fact]
//        public async Task GetAvailableFiltersAsync_WithNullQuery_ThrowsArgumentNullException()
//        {
//            // Act & Assert
//            await Assert.ThrowsAsync<ArgumentNullException>(() =>
//                _service.GetAvailableFiltersAsync(null));
//        }

//        [Fact]
//        public async Task GetAvailableFiltersAsync_WithNoSearchParameters_ThrowsArgumentNullException()
//        {
//            // Arrange
//            var query = new AvailableFiltersQuery();

//            // Act & Assert
//            await Assert.ThrowsAsync<ArgumentNullException>(() =>
//                _service.GetAvailableFiltersAsync(query));
//        }

//        [Fact]
//        public async Task GetAvailableFiltersAsync_WithValidQuery_ReturnsFilters()
//        {
//            // Arrange
//            var query = new AvailableFiltersQuery { SearchTerm = "laptop" };

//            var mockFilters = new SearchFilters
//            {
//                Categories = new List<CategoryFilter>
//                {
//                    new CategoryFilter { Id = Guid.NewGuid(), TitleAr = "إلكترونيات", TitleEn = "Electronics", ItemCount = 10 }
//                },
//                Brands = new List<BrandFilter>
//                {
//                    new BrandFilter { Id = Guid.NewGuid(), NameAr = "ديل", NameEn = "Dell", ItemCount = 5 }
//                },
//                Vendors = new List<VendorFilter>
//                {
//                    new VendorFilter { Id = Guid.NewGuid(), StoreName = "Tech Store", StoreNameAr = "متجر التقنية", ItemCount = 3 }
//                },
//                PriceRange = new PriceRangeFilter { MinPrice = 100, MaxPrice = 5000, AvgPrice = 2500 },
//                Attributes = new List<AttributeFilter>
//                {
//                    new AttributeFilter
//                    {
//                        AttributeId = Guid.NewGuid(),
//                        NameAr = "اللون",
//                        NameEn = "Color",
//                        DisplayOrder = 1,
//                        Values = new List<AttributeValueFilter>
//                        {
//                            new AttributeValueFilter { ValueId = Guid.NewGuid(), ValueAr = "أسود", ValueEn = "Black", ItemCount = 5 }
//                        }
//                    }
//                },
//                Conditions = new List<ConditionFilter>
//                {
//                    new ConditionFilter { Id = Guid.NewGuid(), NameAr = "جديد", NameEn = "New", ItemCount = 8 }
//                },
//                Features = new FeaturesFilter
//                {
//                    FreeShippingCount = 5,
//                    HasFreeShipping = 1,
//                    WithWarrantyCount = 8,
//                    HasWarranty = 1,
//                    InStockCount = 7,
//                    HasInStock = 1,
//                    TotalItems = 10
//                }
//            };

//            _mockRepository
//                .Setup(r => r.GetAvailableFiltersAsync(It.IsAny<AvailableFiltersQuery>()))
//                .ReturnsAsync(mockFilters);

//            // Act
//            var result = await _service.GetAvailableFiltersAsync(query);

//            // Assert
//            Assert.NotNull(result);
//            Assert.Single(result.Categories);
//            Assert.Single(result.Brands);
//            Assert.Single(result.Vendors);
//            Assert.Single(result.Attributes);
//            Assert.Single(result.Conditions);
//            Assert.Equal(100, result.PriceRange.MinPrice);
//            Assert.Equal(5000, result.PriceRange.MaxPrice);
//            Assert.True(result.Features.HasFreeShipping);
//            Assert.Equal(5, result.Features.FreeShippingCount);
//        }

//        #endregion

//        #region GetItemBestPricesAsync Tests

//        [Fact]
//        public async Task GetItemBestPricesAsync_WithNullItemIds_ReturnsEmptyList()
//        {
//            // Act
//            var result = await _service.GetItemBestPricesAsync(null);

//            // Assert
//            Assert.NotNull(result);
//            Assert.Empty(result);
//        }

//        [Fact]
//        public async Task GetItemBestPricesAsync_WithEmptyItemIds_ReturnsEmptyList()
//        {
//            // Act
//            var result = await _service.GetItemBestPricesAsync(new List<Guid>());

//            // Assert
//            Assert.NotNull(result);
//            Assert.Empty(result);
//        }

//        [Fact]
//        public async Task GetItemBestPricesAsync_WithValidItemIds_ReturnsPrices()
//        {
//            // Arrange
//            var itemId1 = Guid.NewGuid();
//            var itemId2 = Guid.NewGuid();
//            var itemIds = new List<Guid> { itemId1, itemId2 };

//            var mockPrices = new List<VwItemBestPrice>
//            {
//                new VwItemBestPrice
//                {
//                    ItemId = itemId1,
//                    BestPrice = 1000,
//                    TotalStock = 50,
//                    TotalOffers = 5,
//                    BuyBoxRatio = 0.85m
//                },
//                new VwItemBestPrice
//                {
//                    ItemId = itemId2,
//                    BestPrice = 2000,
//                    TotalStock = 30,
//                    TotalOffers = 3,
//                    BuyBoxRatio = 0.75m
//                }
//            };

//            _mockRepository
//                .Setup(r => r.GetItemBestPricesAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()))
//                .ReturnsAsync(mockPrices);

//            // Act
//            var result = await _service.GetItemBestPricesAsync(itemIds);

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal(2, result.Count);
//            Assert.Equal(1000, result[0].BestPrice);
//            Assert.Equal(0.85, result[0].BuyBoxRatio);
//        }

//        #endregion

//        #region Helper Methods

//        private SpSearchItemsMultiVendor CreateMockSearchEntity(
//            Guid itemId,
//            string title,
//            decimal minPrice,
//            decimal maxPrice)
//        {
//            return new SpSearchItemsMultiVendor
//            {
//                ItemId = itemId,
//                TitleAr = title + " AR",
//                TitleEn = title,
//                ShortDescriptionAr = "وصف قصير",
//                ShortDescriptionEn = "Short description",
//                CategoryId = Guid.NewGuid(),
//                BrandId = Guid.NewGuid(),
//                ThumbnailImage = "image.jpg",
//                CreatedDateUtc = DateTime.UtcNow,
//                //MinPrice = minPrice,
//                //MaxPrice = maxPrice,
//                //AvgPrice = (minPrice + maxPrice) / 2,
//                //OffersCount = 3,
//                //FinalScore = 0.75
//            };
//        }

//        #endregion
//    }
//}