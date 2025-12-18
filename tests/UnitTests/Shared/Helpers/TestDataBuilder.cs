using Common.Filters;
using DAL.Models.ItemSearch;
using Domains.Procedures;
using Domains.Views.Item;
using Shared.DTOs.ECommerce.Item;

namespace UnitTests.Shared.Helpers
{
    /// <summary>
    /// Builder pattern for creating test data
    /// Location: tests/UnitTests/Shared/Helpers/TestDataBuilder.cs
    /// </summary>
    public static class TestDataBuilder
    {
        public static class ItemFilters
        {
            public static ItemFilterQuery Basic() => new ItemFilterQuery
            {
                PageNumber = 1,
                PageSize = 20,
                SortBy = "relevance"
            };

            public static ItemFilterQuery WithSearchTerm(string term) => new ItemFilterQuery
            {
                SearchTerm = term,
                PageNumber = 1,
                PageSize = 20
            };

            public static ItemFilterQuery WithPriceRange(decimal min, decimal max) => new ItemFilterQuery
            {
                MinPrice = min,
                MaxPrice = max,
                PageNumber = 1,
                PageSize = 20
            };

            public static ItemFilterQuery WithCategories(Guid categoryId) => new ItemFilterQuery
            {
                CategoryId = categoryId,
                PageNumber = 1,
                PageSize = 20
            };

            public static ItemFilterQuery Complex() => new ItemFilterQuery
            {
                SearchTerm = "laptop",
                CategoryId = Guid.NewGuid(),
                BrandId = Guid.NewGuid(),
                MinPrice = 500,
                MaxPrice = 5000,
                MinItemRating = 4.0m,
                InStockOnly = true,
                FreeShippingOnly = true,
                WithWarrantyOnly = true,
                AttributeValues = new Dictionary<Guid, List<Guid>>
                {
                    { Guid.NewGuid(), new List<Guid> { Guid.NewGuid() } }
                },
                PageNumber = 1,
                PageSize = 20,
                SortBy = "price_asc"
            };
        }

        public static class SearchEntities
        {
            public static SpSearchItemsMultiVendor Basic(Guid? itemId = null)
            {
                return new SpSearchItemsMultiVendor
                {
                    ItemId = itemId ?? Guid.NewGuid(),
                    TitleAr = "منتج تجريبي",
                    TitleEn = "Test Product",
                    ShortDescriptionAr = "وصف قصير",
                    ShortDescriptionEn = "Short description",
                    CategoryId = Guid.NewGuid(),
                    BrandId = Guid.NewGuid(),
                    ThumbnailImage = "test-image.jpg",
                    CreatedDateUtc = DateTime.UtcNow.AddDays(-7),
                    //MinPrice = 100,
                    //MaxPrice = 150,
                    //AvgPrice = 125,
                    //OffersCount = 3,
                    //FinalScore = 0.75,
                    //FastestDelivery = 2
                };
            }

            public static SpSearchItemsMultiVendor WithBestOffer(
                Guid? itemId = null,
                decimal salesPrice = 900,
                decimal originalPrice = 1000,
                bool isFreeShipping = true,
                bool isBuyBoxWinner = true)
            {
                var entity = Basic(itemId);
                var offerId = Guid.NewGuid();
                var vendorId = Guid.NewGuid();

                return entity;
            }


            public static List<SpSearchItemsMultiVendor> CreateList(int count)
            {
                var list = new List<SpSearchItemsMultiVendor>();
                for (int i = 0; i < count; i++)
                {
                    list.Add(Basic());
                }
                return list;
            }
        }

        public static class SearchFilters
        {
            public static DAL.Models.ItemSearch.SearchFilters Complete()
            {
                return new DAL.Models.ItemSearch.SearchFilters
                {
                    Categories = new List<CategoryFilter>
                    {
                        new CategoryFilter
                        {
                            Id = Guid.NewGuid(),
                            TitleAr = "إلكترونيات",
                            TitleEn = "Electronics",
                            ItemCount = 100
                        }
                    },
                    Brands = new List<BrandFilter>
                    {
                        new BrandFilter
                        {
                            Id = Guid.NewGuid(),
                            NameAr = "سامسونج",
                            NameEn = "Samsung",
                            ItemCount = 50
                        }
                    },
                    Vendors = new List<VendorFilter>
                    {
                        new VendorFilter
                        {
                            Id = Guid.NewGuid(),
                            StoreName = "Tech Store",
                            StoreNameAr = "متجر التقنية",
                            ItemCount = 25
                        }
                    },
                    PriceRange = new PriceRangeFilter
                    {
                        MinPrice = 50,
                        MaxPrice = 10000,
                        AvgPrice = 2500
                    },
                    Attributes = new List<AttributeFilter>
                    {
                        new AttributeFilter
                        {
                            AttributeId = Guid.NewGuid(),
                            NameAr = "اللون",
                            NameEn = "Color",
                            DisplayOrder = 1,
                            Values = new List<AttributeValueFilter>
                            {
                                new AttributeValueFilter
                                {
                                    ValueId = Guid.NewGuid(),
                                    ValueAr = "أحمر",
                                    ValueEn = "Red",
                                    ItemCount = 10
                                }
                            }
                        }
                    },
                    Conditions = new List<ConditionFilter>
                    {
                        new ConditionFilter
                        {
                            Id = Guid.NewGuid(),
                            NameAr = "جديد",
                            NameEn = "New",
                            ItemCount = 80
                        }
                    },
                    Features = new FeaturesFilter
                    {
                        FreeShippingCount = 40,
                        HasFreeShipping = 1,
                        WithWarrantyCount = 60,
                        HasWarranty = 1,
                        InStockCount = 75,
                        HasInStock = 1,
                        TotalItems = 100
                    }
                };
            }
        }

        public static class BestPrices
        {
            public static VwItemBestPrice Create(
                Guid? itemId = null,
                decimal bestPrice = 1000,
                int totalStock = 50,
                int totalOffers = 5,
                decimal buyBoxRatio = 0.85m)
            {
                return new VwItemBestPrice
                {
                    ItemId = itemId ?? Guid.NewGuid(),
                    BestPrice = bestPrice,
                    TotalStock = totalStock,
                    TotalOffers = totalOffers,
                    BuyBoxRatio = buyBoxRatio
                };
            }

            public static List<VwItemBestPrice> CreateList(int count)
            {
                var list = new List<VwItemBestPrice>();
                for (int i = 0; i < count; i++)
                {
                    list.Add(Create(
                        bestPrice: 1000 + (i * 100),
                        totalStock: 50 - (i * 5),
                        totalOffers: 5 + i
                    ));
                }
                return list;
            }
        }
    }

    /// <summary>
    /// Assertion helpers for testing
    /// </summary>
    public static class AssertionHelpers
    {
        public static void AssertValidPagedResult<T>(
            T pagedResult,
            int expectedItemCount,
            int expectedTotalCount,
            int expectedPageNumber,
            int expectedPageSize) where T : class
        {
            Assert.NotNull(pagedResult);

            var itemsProperty = pagedResult.GetType().GetProperty("Items");
            var items = itemsProperty?.GetValue(pagedResult) as ICollection<object>;
            Assert.Equal(expectedItemCount, items?.Count ?? 0);

            var totalCountProperty = pagedResult.GetType().GetProperty("TotalCount");
            Assert.Equal(expectedTotalCount, (int)(totalCountProperty?.GetValue(pagedResult) ?? 0));

            var pageNumberProperty = pagedResult.GetType().GetProperty("PageNumber");
            Assert.Equal(expectedPageNumber, (int)(pageNumberProperty?.GetValue(pagedResult) ?? 0));

            var pageSizeProperty = pagedResult.GetType().GetProperty("PageSize");
            Assert.Equal(expectedPageSize, (int)(pageSizeProperty?.GetValue(pagedResult) ?? 0));
        }

        public static void AssertBestOfferDetails(
            BestOfferDetailsDto offer,
            decimal expectedSalesPrice,
            decimal expectedOriginalPrice,
            bool expectedIsFreeShipping,
            bool expectedIsBuyBoxWinner)
        {
            Assert.NotNull(offer);
            Assert.Equal(expectedSalesPrice, offer.SalesPrice);
            Assert.Equal(expectedOriginalPrice, offer.OriginalPrice);
            Assert.Equal(expectedIsFreeShipping, offer.IsFreeShipping);
            Assert.Equal(expectedIsBuyBoxWinner, offer.IsBuyBoxWinner);

            if (expectedOriginalPrice > expectedSalesPrice)
            {
                var expectedDiscount = Math.Round(
                    ((expectedOriginalPrice - expectedSalesPrice) / expectedOriginalPrice) * 100, 2);
                Assert.Equal(expectedDiscount, offer.DiscountPercentage);
            }
        }
    }

    /// <summary>
    /// Mock data generator for testing
    /// </summary>
    public static class MockDataGenerator
    {
        private static readonly Random _random = new Random();

        public static string GenerateRandomSearchTerm()
        {
            var terms = new[] { "laptop", "phone", "tablet", "watch", "headphones" };
            return terms[_random.Next(terms.Length)];
        }

        public static decimal GenerateRandomPrice(decimal min = 100, decimal max = 10000)
        {
            return Math.Round((decimal)(_random.NextDouble() * (double)(max - min)) + min, 2);
        }

        public static int GenerateRandomStock(int min = 0, int max = 100)
        {
            return _random.Next(min, max);
        }

        public static double GenerateRandomScore()
        {
            return Math.Round(_random.NextDouble(), 2);
        }

        public static DateTime GenerateRandomCreatedDate(int daysBack = 365)
        {
            return DateTime.UtcNow.AddDays(-_random.Next(1, daysBack));
        }
    }
}