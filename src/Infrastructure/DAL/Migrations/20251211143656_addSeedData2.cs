//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace DAL.Migrations
//{
//    public partial class addSeedData2 : Migration
//    {
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            // 2. TbCategories - Fixed required fields
//            migrationBuilder.Sql(@"
//                SET IDENTITY_INSERT [dbo].[TbCategories] ON;

//                INSERT INTO [dbo].[TbCategories] 
//                ([Id], [TitleAr], [TitleEn], [ParentId], [IsFinal], [IsHomeCategory], [IsRoot], [IsFeaturedCategory], 
//                 [IsMainCategory], [PriceRequired], [TreeViewSerial], [CreatedBy], [CreatedDateUtc], [IsDeleted],
//                 [PricingSystemId], [Icon], [ImageUrl], [UpdatedBy], [UpdatedDateUtc])
//                VALUES
//                ('fe567484-5ea7-487c-8956-1ddff6c4200c', N'الإلكترونيات', 'Electronics', NULL, 0, 1, 1, 1, 1, 1, N'001', 
//                 '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0,
//                 '11111111-1111-1111-1111-111111111111', N'icon-electronics', N'/images/categories/electronics.jpg', 
//                 '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
//                ('9a1654c7-3a37-4c38-a5f0-2f7898c9b794', N'الهواتف المحمولة', 'Mobile Phones', 'fe567484-5ea7-487c-8956-1ddff6c4200c', 1, 1, 0, 1, 0, 1, N'001.001', 
//                 '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0,
//                 '11111111-1111-1111-1111-111111111111', N'icon-mobile', N'/images/categories/mobile.jpg', 
//                 '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
//                ('c4a89d1e-7e9f-4c82-96f3-0d99c8e1a9a2', N'الحواسيب المحمولة', 'Laptops', 'fe567484-5ea7-487c-8956-1ddff6c4200c', 1, 1, 0, 1, 0, 1, N'001.002', 
//                 '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0,
//                 '11111111-1111-1111-1111-111111111111', N'icon-laptop', N'/images/categories/laptop.jpg', 
//                 '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
//                ('7b1e9f03-eb55-4d23-a7f0-c8c98f22b3f9', N'الأزياء', 'Fashion', NULL, 0, 1, 1, 1, 1, 1, N'002', 
//                 '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0,
//                 '11111111-1111-1111-1111-111111111111', N'icon-fashion', N'/images/categories/fashion.jpg', 
//                 '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
//                ('d3a0ce56-8f5c-4d8b-b7f1-e5c6c8f0e3a2', N'ملابس رجالية', 'Men''s Clothing', '7b1e9f03-eb55-4d23-a7f0-c8c98f22b3f9', 1, 1, 0, 1, 0, 1, N'002.001', 
//                 '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0,
//                 '11111111-1111-1111-1111-111111111111', N'icon-men', N'/images/categories/men.jpg', 
//                 '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
//                ('e5f6g7h8-9i0j-1k2l-3m4n-5o6p7q8r9s0t', N'الرياضة', 'Sports', NULL, 0, 1, 1, 1, 1, 1, N'003', 
//                 '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0,
//                 '11111111-1111-1111-1111-111111111111', N'icon-sports', N'/images/categories/sports.jpg', 
//                 '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
//                ('a1b2c3d4-e5f6-7g8h-9i0j-k1l2m3n4o5p6', N'مستلزمات رياضية', 'Sports Equipment', 'e5f6g7h8-9i0j-1k2l-3m4n-5o6p7q8r9s0t', 1, 1, 0, 1, 0, 1, N'003.001', 
//                 '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0,
//                 '11111111-1111-1111-1111-111111111111', N'icon-equipment', N'/images/categories/equipment.jpg', 
//                 '00000000-0000-0000-0000-000000000000', GETUTCDATE());

//                SET IDENTITY_INSERT [dbo].[TbCategories] OFF;
//            ");

//            // 3. TbBrands
//            migrationBuilder.Sql(@"
//                SET IDENTITY_INSERT [dbo].[TbBrands] ON;

//                INSERT INTO [dbo].[TbBrands] ([Id], [TitleAr], [TitleEn], [LogoPath], [IsActive], [CreatedBy], [CreatedDateUtc], [IsDeleted])
//                VALUES
//                ('bf480260-3292-4924-8459-c445d7e84964', N'نايكي', 'Nike', '/images/brands/nike.png', 1, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0),
//                ('e2d5c47a-9e5b-4c58-b2d7-2a5b8e9c3f6d', N'أبل', 'Apple', '/images/brands/apple.png', 1, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0),
//                ('a1c3f5e7-8b9d-4e6f-9a0b-2c4d6e8f0a1c', N'سامسونج', 'Samsung', '/images/brands/samsung.png', 1, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0),
//                ('d4e6f8a0-2c4b-4e6d-8f0a-1c3e5a7b9d1f', N'أديداس', 'Adidas', '/images/brands/adidas.png', 1, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0),
//                ('f1e2d3c4-b5a6-9788-8910-7f6e5d4c3b2a', N'سوني', 'Sony', '/images/brands/sony.png', 1, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0);

//                SET IDENTITY_INSERT [dbo].[TbBrands] OFF;
//            ");

//            // 4. AspNetUsers (Roles and Users)
//            migrationBuilder.Sql(@"
//                -- إضافة الأدوار
//                INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
//                VALUES 
//                (N'02b804e9-1144-42d8-bc0a-686d419bc40a', N'Customer', N'CUSTOMER', N'0cfc5c37-d2e5-41a0-9cc2-65dc1759add6'),
//                (N'0c0203ef-78fd-4e63-96b1-98b3555d04c3', N'Vendor', N'VENDOR', N'0735d525-5825-4448-bcfb-f0f7e48622c1'),
//                (N'dd391c0d-4b88-44c8-ab7a-5b27f2ac9152', N'Admin', N'ADMIN', N'325ead53-0d36-414a-886e-5b4ad964b8be');

//                -- إضافة مستخدمين
//                INSERT INTO [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [ProfileImagePath], [PhoneCode], [NormalizedPhone], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [UserName], [NormalizedUserName])
//                VALUES
//                ('d37f6313-3de6-48ed-aab7-2423b84ec7d7', 'أحمد', 'محمد', '/images/profiles/default.png', '+966', '966500000000', 'customer@example.com', 'CUSTOMER@EXAMPLE.COM', 1, 'AQAAAAEAACcQAAAAEK9v8QjQXcDq5XZ6qKdGjv5K7LxXhXh3NQZx+8Z+8w9XQg==', NEWID(), NEWID(), '500000000', 1, 0, NULL, 1, 0, 'customer', 'CUSTOMER'),
//                ('9223ad7c-f56b-4a05-9174-8ea3840bb7bc', 'فهد', 'السعودي', '/images/profiles/vendor.png', '+966', '966511111111', 'vendor@example.com', 'VENDOR@EXAMPLE.COM', 1, 'AQAAAAEAACcQAAAAEK9v8QjQXcDq5XZ6qKdGjv5K7LxXhXh3NQZx+8Z+8w9XQg==', NEWID(), NEWID(), '511111111', 1, 0, NULL, 1, 0, 'vendor1', 'VENDOR1'),
//                ('e168fe35-586e-4948-b833-35fffe1014dd', 'خالد', 'الإدارة', '/images/profiles/admin.png', '+966', '966522222222', 'admin@example.com', 'ADMIN@EXAMPLE.COM', 1, 'AQAAAAEAACcQAAAAEK9v8QjQXcDq5XZ6qKdGjv5K7LxXhXh3NQZx+8Z+8w9XQg==', NEWID(), NEWID(), '522222222', 1, 0, NULL, 1, 0, 'admin', 'ADMIN');

//                -- ربط المستخدمين بالأدوار
//                INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId])
//                VALUES
//                ('d37f6313-3de6-48ed-aab7-2423b84ec7d7', '02b804e9-1144-42d8-bc0a-686d419bc40a'),
//                ('9223ad7c-f56b-4a05-9174-8ea3840bb7bc', '0c0203ef-78fd-4e63-96b1-98b3555d04c3'),
//                ('e168fe35-586e-4948-b833-35fffe1014dd', 'dd391c0d-4b88-44c8-ab7a-5b27f2ac9152');
//            ");

//            // 5. TbVendors
//            migrationBuilder.Sql(@"
//                SET IDENTITY_INSERT [dbo].[TbVendors] ON;

//                INSERT INTO [dbo].[TbVendors] ([Id], [NameEn], [NameAr], [Email], [PhoneNumber], [IsActive], [IsVerified], [IsPrimeVendor], [Rating], [CreatedBy], [CreatedDateUtc], [IsDeleted])
//                VALUES
//                ('a8a68f35-64f1-4567-95a8-811e9b3981f0', 'Fashion Hub Co.', N'شركة مودا للبيع', 'vendor1@example.com', '+966500000001', 1, 1, 1, 4.8, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0),
//                ('b7c9d1e3-5f8a-4c6b-9d0e-1f2a3b4c5d6e', 'Tech World', N'عالم التقنية', 'vendor2@example.com', '+966500000002', 1, 1, 0, 4.5, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0),
//                ('c8d0e2f4-6a9b-5d7c-0e1f-2a3b4c5d6e7f', 'Mobile Experts', N'خبراء الهواتف', 'vendor3@example.com', '+966500000003', 1, 1, 1, 4.7, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0),
//                ('d9e0f1g2-3h4i-5j6k-7l8m-9n0o1p2q3r4s', 'Sports Gear', N'معدات رياضية', 'vendor4@example.com', '+966500000004', 1, 1, 0, 4.3, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0);

//                SET IDENTITY_INSERT [dbo].[TbVendors] OFF;
//            ");

//            // 6. TbWarehouses
//            migrationBuilder.Sql(@"
//                SET IDENTITY_INSERT [dbo].[TbWarehouses] ON;

//                INSERT INTO [dbo].[TbWarehouses] ([Id], [TitleAr], [TitleEn], [Address], [PhoneNumber], [PhoneCode], [IsDefaultPlatformWarehouse], [VendorId], [IsActive], [CreatedBy], [CreatedDateUtc], [IsDeleted])
//                VALUES
//                ('1f2a3b4c-5d6e-7f8a-9b0c-1d2e3f4a5b6c', N'المستودع الرئيسي', 'Main Warehouse', 'الرياض، المملكة العربية السعودية', '+966112345678', '+966', 1, NULL, 1, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0),
//                ('2e3f4a5b-6c7d-8e9f-0a1b-2c3d4e5f6a7b', N'مستودع الرياض', 'Riyadh Warehouse', 'حي العليا، الرياض', '+966118765432', '+966', 0, 'a8a68f35-64f1-4567-95a8-811e9b3981f0', 1, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0),
//                ('3d4e5f6a-7b8c-9d0e-1f2a-3b4c5d6e7f8a', N'مستودع جدة', 'Jeddah Warehouse', 'حي الشرفية، جدة', '+966123456789', '+966', 0, 'b7c9d1e3-5f8a-4c6b-9d0e-1f2a3b4c5d6e', 1, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0),
//                ('4e5f6g7h-8i9j-0k1l-2m3n-4o5p6q7r8s9t', N'مستودع الرياض الرياضي', 'Riyadh Sports Warehouse', 'حي الملز، الرياض', '+966112223333', '+966', 0, 'd9e0f1g2-3h4i-5j6k-7l8m-9n0o1p2q3r4s', 1, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0);

//                SET IDENTITY_INSERT [dbo].[TbWarehouses] OFF;
//            ");

//            // 7. TbItems - Fixed column names (TbBrandId, TbCategoryId) and added missing required columns
//            migrationBuilder.Sql(@"
//                SET IDENTITY_INSERT [dbo].[TbItems] ON;

//                INSERT INTO [dbo].[TbItems] 
//                ([Id], [TitleAr], [TitleEn], [ShortDescriptionAr], [ShortDescriptionEn], [DescriptionAr], [DescriptionEn], 
//                 [TbCategoryId], [TbBrandId], [Sku], [Barcode], [IsActive], [IsFeatured], [IsOnSale], [IsNew], 
//                 [CreatedDateUtc], [IsDeleted], [CreatedBy], [UnitId], [HasCombinations])
//                VALUES
//                ('6c587394-618e-4d5d-b764-0453a262e249', N'آيفون 15 برو', 'iPhone 15 Pro', N'أحدث هاتف من أبل مع كاميرا متطورة', 'Latest iPhone with advanced camera', N'آيفون 15 برو بذاكرة 256 جيجابايت، معالج A17 Pro، وكاميرا ثلاثية', 'iPhone 15 Pro with 256GB storage, A17 Pro chip, and triple camera system', 
//                 '9a1654c7-3a37-4c38-a5f0-2f7898c9b794', 'e2d5c47a-9e5b-4c58-b2d7-2a5b8e9c3f6d', 'IP15P-256-SLV', '194252123456', 1, 1, 0, 1, 
//                 GETUTCDATE(), 0, '00000000-0000-0000-0000-000000000000', 
//                 1, 1),  -- UnitId=1 (Piece), HasCombinations=1 (true)
//                ('3ea4bc7a-8888-4dab-ab84-d5febdb37fd3', N'حذاء نايكي إير ماكس', 'Nike Air Max Shoe', N'حذاء رياضي مريح مع تقنية الهواء', 'Comfortable sports shoe with air technology', N'حذاء نايكي إير ماكس للرجال، متوفر بألوان متعددة، مثالي للرياضة والأنشطة اليومية', 'Nike Air Max men''s shoe, available in multiple colors, perfect for sports and daily activities', 
//                 'd3a0ce56-8f5c-4d8b-b7f1-e5c6c8f0e3a2', 'bf480260-3292-4924-8459-c445d7e84964', 'NK-AM-45-US', '1928374650123', 1, 1, 1, 0, 
//                 GETUTCDATE(), 0, '00000000-0000-0000-0000-000000000000', 
//                 1, 1),  -- UnitId=1 (Piece), HasCombinations=1 (true)
//                ('9b8c7d6e-5f4a-3b2c-1d0e-9f8a7b6c5d4e', N'تابلت سامسونج جالكسي', 'Samsung Galaxy Tab', N'تابلت بشاشة كبيرة ومعالج قوي', 'Tablet with large screen and powerful processor', N'تابلت سامسونج جالكسي مع شاشة 11 بوصة، ذاكرة 128 جيجابايت، ومناسب للعمل والترفيه', 'Samsung Galaxy Tab with 11-inch display, 128GB storage, ideal for work and entertainment', 
//                 'fe567484-5ea7-487c-8956-1ddff6c4200c', 'a1c3f5e7-8b9d-4e6f-9a0b-2c4d6e8f0a1c', 'SGT-11-128-BLK', '4567890123456', 1, 0, 0, 1, 
//                 GETUTCDATE(), 0, '00000000-0000-0000-0000-000000000000', 
//                 1, 0),  -- UnitId=1 (Piece), HasCombinations=0 (false)
//                ('a7b8c9d0-e1f2-3g4h-5i6j-7k8l9m0n1o2p', N'كرة قدم أديداس', 'Adidas Soccer Ball', N'كرة قدم رسمية من أديداس', 'Official Adidas soccer ball', N'كرة قدم أديداس بحجم قياسي، مناسبة للمباريات الرسمية والتدريب', 'Standard size Adidas soccer ball, suitable for official matches and training', 
//                 'a1b2c3d4-e5f6-7g8h-9i0j-k1l2m3n4o5p6', 'd4e6f8a0-2c4b-4e6d-8f0a-1c3e5a7b9d1f', 'AD-SB-STD', '9876543210987', 1, 1, 0, 0, 
//                 GETUTCDATE(), 0, '00000000-0000-0000-0000-000000000000', 
//                 1, 0);  -- UnitId=1 (Piece), HasCombinations=0 (false)

//                SET IDENTITY_INSERT [dbo].[TbItems] OFF;
//            ");

//            // 8. TbItemCombinations
//            migrationBuilder.Sql(@"
//                SET IDENTITY_INSERT [dbo].[TbItemCombinations] ON;

//                INSERT INTO [dbo].[TbItemCombinations] 
//                ([Id], [ItemId], [Barcode], [SKU], [IsDefault], [BasePrice], [CreatedBy], [CreatedDateUtc], [UpdatedBy], [UpdatedDateUtc], [IsDeleted])
//                VALUES
//                ('c3f4d5e6-7a8b-9c0d-1e2f-3a4b5c6d7e8f', '6c587394-618e-4d5d-b764-0453a262e249', '194252123456', 'IP15P-256-SLV', 1, 48999.00, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), NULL, NULL, 0),
//                ('d4e5f6g7-8h9i-0j1k-2l3m-4n5o6p7q8r9s', '3ea4bc7a-8888-4dab-ab84-d5febdb37fd3', '1928374650123', 'NK-AM-45-US', 1, 3499.00, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), NULL, NULL, 0),
//                ('e5f6g7h8-9i0j-1k2l-3m4n-5o6p7q8r9s0t', '9b8c7d6e-5f4a-3b2c-1d0e-9f8a7b6c5d4e', '4567890123456', 'SGT-11-128-BLK', 1, 2199.00, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), NULL, NULL, 0),
//                ('f6g7h8i9-j0k1-l2m3-n4o5-p6q7r8s9t0u1', 'a7b8c9d0-e1f2-3g4h-5i6j-7k8l9m0n1o2p', '9876543210987', 'AD-SB-STD', 1, 199.00, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), NULL, NULL, 0);

//                SET IDENTITY_INSERT [dbo].[TbItemCombinations] OFF;
//            ");

//            // 9. TbItemImages
//            migrationBuilder.Sql(@"
//                SET IDENTITY_INSERT [dbo].[TbItemImages] ON;

//                INSERT INTO [dbo].[TbItemImages] ([Id], [ItemId], [ImageUrl], [DisplayOrder], [IsPrimary], [CreatedBy], [CreatedDateUtc], [IsDeleted])
//                VALUES
//                ('1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d', '6c587394-618e-4d5d-b764-0453a262e249', '/images/items/iphone15pro-1.jpg', 1, 1, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0),
//                ('2b3c4d5e-6f7a-8b9c-0d1e-2f3a4b5c6d7e', '6c587394-618e-4d5d-b764-0453a262e249', '/images/items/iphone15pro-2.jpg', 2, 0, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0),
//                ('3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f', '3ea4bc7a-8888-4dab-ab84-d5febdb37fd3', '/images/items/nikeairmax-1.jpg', 1, 1, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0),
//                ('4d5e6f7a-8b9c-0d1e-2f3a-4b5c6d7e8f9a', '9b8c7d6e-5f4a-3b2c-1d0e-9f8a7b6c5d4e', '/images/items/samsungtab-1.jpg', 1, 1, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0),
//                ('5e6f7g8h-9i0j-1k2l-3m4n-5o6p7q8r9s0t', 'a7b8c9d0-e1f2-3g4h-5i6j-7k8l9m0n1o2p', '/images/items/adidassoccerball-1.jpg', 1, 1, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0);

//                SET IDENTITY_INSERT [dbo].[TbItemImages] OFF;
//            ");

//            // 10. TbOffers
//            migrationBuilder.Sql(@"
//                SET IDENTITY_INSERT [dbo].[TbOffers] ON;

//                INSERT INTO [dbo].[TbOffers] ([Id], [ItemId], [VendorId], [WarehouseId], [VisibilityScope], [IsBuyBoxWinner], [StockStatus], [AvailabilityMessage], [CreatedDateUtc], [IsDeleted], [CreatedBy])
//                VALUES
//                ('8986f278-8db2-446a-9e69-965db00e41d3', '6c587394-618e-4d5d-b764-0453a262e249', 'c8d0e2f4-6a9b-5d7c-0e1f-2a3b4c5d6e7f', '3d4e5f6a-7b8c-9d0e-1f2a-3b4c5d6e7f8a', 1, 1, 1, N'متوفر للشحن خلال 24 ساعة', GETUTCDATE(), 0, '00000000-0000-0000-0000-000000000000'),
//                ('7dcde4fc-24ff-41f5-8475-2d14a3f8e65e', '3ea4bc7a-8888-4dab-ab84-d5febdb37fd3', 'a8a68f35-64f1-4567-95a8-811e9b3981f0', '2e3f4a5b-6c7d-8e9f-0a1b-2c3d4e5f6a7b', 1, 1, 1, N'متوفر للشحن خلال 48 ساعة', GETUTCDATE(), 0, '00000000-0000-0000-0000-000000000000'),
//                ('5e6f7a8b-9c0d-1e2f-3a4b-5c6d7e8f9a0b', '9b8c7d6e-5f4a-3b2c-1d0e-9f8a7b6c5d4e', 'b7c9d1e3-5f8a-4c6b-9d0e-1f2a3b4c5d6e', '3d4e5f6a-7b8c-9d0e-1f2a-3b4c5d6e7f8a', 1, 0, 1, N'متوفر للشحن خلال 72 ساعة', GETUTCDATE(), 0, '00000000-0000-0000-0000-000000000000'),
//                ('f1g2h3i4-j5k6-l7m8-n9o0-p1q2r3s4t5u6', 'a7b8c9d0-e1f2-3g4h-5i6j-7k8l9m0n1o2p', 'd9e0f1g2-3h4i-5j6k-7l8m-9n0o1p2q3r4s', '4e5f6g7h-8i9j-0k1l-2m3n-4o5p6q7r8s9t', 1, 0, 1, N'متوفر للشحن خلال 48 ساعة', GETUTCDATE(), 0, '00000000-0000-0000-0000-000000000000');

//                SET IDENTITY_INSERT [dbo].[TbOffers] OFF;
//            ");

//            // 11. TbOfferCombinationPricing
//            migrationBuilder.Sql(@"
//                SET IDENTITY_INSERT [dbo].[TbOfferCombinationPricing] ON;

//                INSERT INTO [dbo].[TbOfferCombinationPricing] ([Id], [OfferId], [SalesPrice], [AvailableQuantity], [IsDefault], [CreatedBy], [CreatedDateUtc], [IsDeleted])
//                VALUES
//                ('afc52f33-dc24-4aff-93a1-93f866156415', '8986f278-8db2-446a-9e69-965db00e41d3', 48999.00, 15, 1, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0),
//                ('b0c1d2e3-f4a5-6b7c-8d9e-0f1a2b3c4d5e', '7dcde4fc-24ff-41f5-8475-2d14a3f8e65e', 3499.00, 50, 1, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0),
//                ('c1d2e3f4-a5b6-7c8d-9e0f-1a2b3c4d5e6f', '5e6f7a8b-9c0d-1e2f-3a4b-5c6d7e8f9a0b', 2199.00, 30, 1, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0),
//                ('d2e3f4g5-h6i7-j8k9-l0m1-n2o3p4q5r6s7', 'f1g2h3i4-j5k6-l7m8-n9o0-p1q2r3s4t5u6', 199.00, 100, 1, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0);

//                SET IDENTITY_INSERT [dbo].[TbOfferCombinationPricing] OFF;
//            ");

//            // 12. TbCountries
//            migrationBuilder.Sql(@"
//                SET IDENTITY_INSERT [dbo].[TbCountries] ON;

//                INSERT INTO [dbo].[TbCountries] ([Id], [NameAr], [NameEn], [Code], [PhoneCode], [CurrencyId], [IsActive], [CreatedBy], [CreatedDateUtc], [IsDeleted])
//                VALUES
//                ('c31ad1a9-ccbe-439e-9b91-5273cfa0f75a', N'المملكة العربية السعودية', 'Saudi Arabia', 'SA', '+966', NULL, 1, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0),
//                ('d4e5f6g7-h8i9-j0k1-l2m3-n4o5p6q7r8s9', N'الإمارات العربية المتحدة', 'United Arab Emirates', 'AE', '+971', NULL, 1, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0);

//                SET IDENTITY_INSERT [dbo].[TbCountries] OFF;
//            ");

//            // 13. TbCurrencies
//            migrationBuilder.Sql(@"
//                SET IDENTITY_INSERT [dbo].[TbCurrencies] ON;

//                INSERT INTO [dbo].[TbCurrencies] ([Id], [Code], [NameEn], [NameAr], [Symbol], [ExchangeRate], [IsBaseCurrency], [IsActive], [CountryCode], [CreatedBy], [CreatedDateUtc], [IsDeleted])
//                VALUES
//                ('fec27aae-b2d0-49f5-ab84-aad0d77a4905', 'SAR', 'Saudi Riyal', N'ريال سعودي', 'ر.س', 1.000000, 1, 1, 'SAU', '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0),
//                ('aa258e5e-386f-4e60-8b80-b052e1899d7b', 'USD', 'US Dollar', N'دولار أمريكي', '$', 3.750000, 0, 1, 'USA', '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0),
//                ('ba1adff7-6218-4e69-88d9-e7536c91c0dc', 'AED', 'UAE Dirham', N'درهم إماراتي', 'د.إ', 1.020000, 0, 1, 'ARE', '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0);

//                SET IDENTITY_INSERT [dbo].[TbCurrencies] OFF;

//                -- Update countries with currency IDs
//                UPDATE [dbo].[TbCountries] SET [CurrencyId] = 'fec27aae-b2d0-49f5-ab84-aad0d77a4905' WHERE [Id] = 'c31ad1a9-ccbe-439e-9b91-5273cfa0f75a';
//                UPDATE [dbo].[TbCountries] SET [CurrencyId] = 'ba1adff7-6218-4e69-88d9-e7536c91c0dc' WHERE [Id] = 'd4e5f6g7-h8i9-j0k1-l2m3-n4o5p6q7r8s9';
//            ");

//            // 14. TbStates
//            migrationBuilder.Sql(@"
//                SET IDENTITY_INSERT [dbo].[TbStates] ON;

//                INSERT INTO [dbo].[TbStates] ([Id], [TitleAr], [TitleEn], [CountryId], [CreatedBy], [CreatedDateUtc], [IsDeleted])
//                VALUES
//                ('008b8b51-c40c-497c-ae18-9422db642124', N'الرياض', 'Riyadh', 'c31ad1a9-ccbe-439e-9b91-5273cfa0f75a', '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0),
//                ('1a2b3c4d-5e6f-7g8h-9i0j-k1l2m3n4o5p6', N'جدة', 'Jeddah', 'c31ad1a9-ccbe-439e-9b91-5273cfa0f75a', '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0),
//                ('2b3c4d5e-6f7g-8h9i-0j1k-l2m3n4o5p6q7', N'أبوظبي', 'Abu Dhabi', 'd4e5f6g7-h8i9-j0k1-l2m3-n4o5p6q7r8s9', '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0);

//                SET IDENTITY_INSERT [dbo].[TbStates] OFF;
//            ");

//            // 15. TbCities
//            migrationBuilder.Sql(@"
//                SET IDENTITY_INSERT [dbo].[TbCities] ON;

//                INSERT INTO [dbo].[TbCities] ([Id], [TitleAr], [TitleEn], [StateId], [CreatedBy], [CreatedDateUtc], [IsDeleted])
//                VALUES
//                ('3c4d5e6f-7g8h-9i0j-k1l2-m3n4o5p6q7r8', N'الرياض', 'Riyadh City', '008b8b51-c40c-497c-ae18-9422db642124', '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0),
//                ('4d5e6f7g-8h9i-0j1k-l2m3-n4o5p6q7r8s9', N'جدة الشمالية', 'North Jeddah', '1a2b3c4d-5e6f-7g8h-9i0j-k1l2m3n4o5p6', '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0),
//                ('5e6f7g8h-9i0j-k1l2-m3n4-o5p6q7r8s9t0', N'أبوظبي العاصمة', 'Abu Dhabi Capital', '2b3c4d5e-6f7g-8h9i-0j1k-l2m3n4o5p6q7', '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0);

//                SET IDENTITY_INSERT [dbo].[TbCities] OFF;
//            ");

//            // 16. TbShippingDetails
//            migrationBuilder.Sql(@"
//                SET IDENTITY_INSERT [dbo].[TbShippingDetails] ON;

//                INSERT INTO [dbo].[TbShippingDetails] ([Id], [OfferId], [CityId], [ShippingCost], [ShippingMethod], [MinimumEstimatedDays], [MaximumEstimatedDays], [CreatedBy], [CreatedDateUtc], [IsDeleted])
//                VALUES
//                ('d2e3f4a5-b6c7-8d9e-0f1a-2b3c4d5e6f7a', '8986f278-8db2-446a-9e69-965db00e41d3', '3c4d5e6f-7g8h-9i0j-k1l2-m3n4o5p6q7r8', 0.00, 1, 1, 3, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0),
//                ('e3f4a5b6-c7d8-9e0f-1a2b-3c4d5e6f7a8b', '7dcde4fc-24ff-41f5-8475-2d14a3f8e65e', '4d5e6f7g-8h9i-0j1k-l2m3-n4o5p6q7r8s9', 25.00, 2, 2, 5, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0),
//                ('f4a5b6c7-d8e9-0f1a-2b3c-4d5e6f7a8b9c', '5e6f7a8b-9c0d-1e2f-3a4b-5c6d7e8f9a0b', '4d5e6f7g-8h9i-0j1k-l2m3-n4o5p6q7r8s9', 35.00, 2, 2, 4, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0),
//                ('g5h6i7j8-k9l0-m1n2-o3p4-q5r6s7t8u9v0', 'f1g2h3i4-j5k6-l7m8-n9o0-p1q2r3s4t5u6', '5e6f7g8h-9i0j-k1l2-m3n4-o5p6q7r8s9t0', 20.00, 2, 2, 3, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0);

//                SET IDENTITY_INSERT [dbo].[TbShippingDetails] OFF;
//            ");

//            // 17. TbSettings
//            migrationBuilder.Sql(@"
//                SET IDENTITY_INSERT [dbo].[TbSettings] ON;

//                INSERT INTO [dbo].[TbSettings] ([Id], [StoreName], [StoreDescription], [StoreEmail], [StorePhone], [StoreAddress], [DefaultCurrencyId], [TaxRate], [ShippingAmount], [OrderExtraCost], [MinOrderAmount], [MaxOrderAmount], [OrderTaxPercentage], [CreatedBy], [CreatedDateUtc], [IsDeleted])
//                VALUES
//                ('e1f2g3h4-i5j6-k7l8-m9n0-o1p2q3r4s5t6', N'متجر سهل', 'Sahl Store', 'info@sahlstore.com', '+966112345678', 'الرياض، المملكة العربية السعودية', 'fec27aae-b2d0-49f5-ab84-aad0d77a4905', 15.00, 30.00, 5.00, 100.00, 100000.00, 15.00, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0);

//                SET IDENTITY_INSERT [dbo].[TbSettings] OFF;
//            ");

//            // 18. TbCouponCodes
//            migrationBuilder.Sql(@"
//                SET IDENTITY_INSERT [dbo].[TbCouponCodes] ON;

//                INSERT INTO [dbo].[TbCouponCodes] ([Id], [TitleAR], [TitleEN], [Code], [Value], [StartDateUTC], [EndDateUTC], [UsageLimit], [UsageCount], [CouponCodeType], [CreatedBy], [CreatedDateUtc], [IsDeleted])
//                VALUES
//                ('h1i2j3k4-l5m6-n7o8-p9q0-r1s2t3u4v5w6', N'خصم العيد', 'Eid Discount', 'EID2025', 20.00, GETUTCDATE(), DATEADD(MONTH, 1, GETUTCDATE()), 100, 0, 1, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0),
//                ('i2j3k4l5-m6n7-o8p9-q0r1-s2t3u4v5w6x7', N'خصم الموبايل', 'Mobile Discount', 'MOBILE10', 10.00, GETUTCDATE(), DATEADD(MONTH, 2, GETUTCDATE()), 50, 0, 1, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0);

//                SET IDENTITY_INSERT [dbo].[TbCouponCodes] OFF;
//            ");

//            // 19. TbCustomerAddresses
//            migrationBuilder.Sql(@"
//                SET IDENTITY_INSERT [dbo].[TbCustomerAddresses] ON;

//                INSERT INTO [dbo].[TbCustomerAddresses] ([Id], [UserId], [FirstName], [LastName], [Phone], [AddressLine1], [AddressLine2], [CityId], [PostalCode], [IsDefault], [AddressType], [Notes], [CreatedBy], [CreatedDateUtc], [IsDeleted])
//                VALUES
//                ('6f7g8h9i-0j1k-l2m3-n4o5-p6q7r8s9t0u1', 'd37f6313-3de6-48ed-aab7-2423b84ec7d7', 'أحمد', 'محمد', '+966500000000', 'طريق الملك فهد', 'حي العليا', '3c4d5e6f-7g8h-9i0j-k1l2-m3n4o5p6q7r8', '12345', 1, 1, N'الشقة رقم 10', '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0);

//                SET IDENTITY_INSERT [dbo].[TbCustomerAddresses] OFF;
//            ");

//            // 20. TbPaymentMethods
//            migrationBuilder.Sql(@"
//                SET IDENTITY_INSERT [dbo].[TbPaymentMethods] ON;

//                INSERT INTO [dbo].[TbPaymentMethods] ([Id], [TitleAr], [TitleEn], [DescriptionAr], [DescriptionEn], [PaymentGateway], [IsActive], [ProviderDetails], [CreatedBy], [CreatedDateUtc], [IsDeleted])
//                VALUES
//                ('7g8h9i0j-k1l2-m3n4-o5p6-q7r8s9t0u1v2', N'الدفع عند الاستلام', 'Cash on Delivery', N'ادفع عند استلام الطلب', 'Pay when you receive your order', 'COD', 1, NULL, '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0);

//                SET IDENTITY_INSERT [dbo].[TbPaymentMethods] OFF;
//            ");

//            // 21. TbPlatformTreasuries
//            migrationBuilder.Sql(@"
//                SET IDENTITY_INSERT [dbo].[TbPlatformTreasuries] ON;

//                INSERT INTO [dbo].[TbPlatformTreasuries] ([Id], [TotalBalance], [CustomerWalletsTotal], [VendorWalletsTotal], [PendingCommissions], [CollectedCommissions], [PendingPayouts], [ProcessedPayouts], [TotalRevenue], [TotalCommissions], [TotalRefunds], [TotalPayouts], [LastUpdatedUtc], [CurrencyId], [LastReconciliationDate], [Notes], [CreatedBy], [CreatedDateUtc], [IsDeleted])
//                VALUES
//                ('9i0j1k2l-m3n4-o5p6-q7r8-s9t0u1v2w3x4', 100000.00, 50000.00, 30000.00, 5000.00, 10000.00, 3000.00, 7000.00, 15000.00, 2000.00, 500.00, 7000.00, GETUTCDATE(), 'fec27aae-b2d0-49f5-ab84-aad0d77a4905', GETUTCDATE(), N'الرصيد الأولي للمنصة', '00000000-0000-0000-0000-000000000000', GETUTCDATE(), 0);

//                SET IDENTITY_INSERT [dbo].[TbPlatformTreasuries] OFF;
//            ");
//        }

//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            // التراجع عن البيانات المضافة بشكل آمن - حذف السجلات المحددة فقط
//            migrationBuilder.Sql(@"
//                DELETE FROM [dbo].[TbPlatformTreasuries] WHERE [Id] = '9i0j1k2l-m3n4-o5p6-q7r8-s9t0u1v2w3x4';
//                DELETE FROM [dbo].[TbPaymentMethods] WHERE [Id] = '7g8h9i0j-k1l2-m3n4-o5p6-q7r8s9t0u1v2';
//                DELETE FROM [dbo].[TbCustomerAddresses] WHERE [Id] = '6f7g8h9i-0j1k-l2m3-n4o5-p6q7r8s9t0u1';
//                DELETE FROM [dbo].[TbCouponCodes] WHERE [Id] IN ('h1i2j3k4-l5m6-n7o8-p9q0-r1s2t3u4v5w6', 'i2j3k4l5-m6n7-o8p9-q0r1-s2t3u4v5w6x7');
//                DELETE FROM [dbo].[TbSettings] WHERE [Id] = 'e1f2g3h4-i5j6-k7l8-m9n0-o1p2q3r4s5t6';
//                DELETE FROM [dbo].[TbShippingDetails] WHERE [Id] IN ('d2e3f4a5-b6c7-8d9e-0f1a-2b3c4d5e6f7a', 'e3f4a5b6-c7d8-9e0f-1a2b-3c4d5e6f7a8b', 'f4a5b6c7-d8e9-0f1a-2b3c-4d5e6f7a8b9c', 'g5h6i7j8-k9l0-m1n2-o3p4-q5r6s7t8u9v0');
//                DELETE FROM [dbo].[TbOfferCombinationPricing] WHERE [Id] IN ('afc52f33-dc24-4aff-93a1-93f866156415', 'b0c1d2e3-f4a5-6b7c-8d9e-0f1a2b3c4d5e', 'c1d2e3f4-a5b6-7c8d-9e0f-1a2b3c4d5e6f', 'd2e3f4g5-h6i7-j8k9-l0m1-n2o3p4q5r6s7');
//                DELETE FROM [dbo].[TbOffers] WHERE [Id] IN ('8986f278-8db2-446a-9e69-965db00e41d3', '7dcde4fc-24ff-41f5-8475-2d14a3f8e65e', '5e6f7a8b-9c0d-1e2f-3a4b-5c6d7e8f9a0b', 'f1g2h3i4-j5k6-l7m8-n9o0-p1q2r3s4t5u6');
//                DELETE FROM [dbo].[TbItemImages] WHERE [Id] IN ('1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d', '2b3c4d5e-6f7a-8b9c-0d1e-2f3a4b5c6d7e', '3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f', '4d5e6f7a-8b9c-0d1e-2f3a-4b5c6d7e8f9a', '5e6f7g8h-9i0j-1k2l-3m4n-5o6p7q8r9s0t');
//                DELETE FROM [dbo].[TbItemCombinations] WHERE [Id] IN ('c3f4d5e6-7a8b-9c0d-1e2f-3a4b5c6d7e8f', 'd4e5f6g7-8h9i-0j1k-2l3m-4n5o6p7q8r9s', 'e5f6g7h8-9i0j-1k2l-3m4n-5o6p7q8r9s0t', 'f6g7h8i9-j0k1-l2m3-n4o5-p6q7r8s9t0u1');
//                DELETE FROM [dbo].[TbItems] WHERE [Id] IN ('6c587394-618e-4d5d-b764-0453a262e249', '3ea4bc7a-8888-4dab-ab84-d5febdb37fd3', '9b8c7d6e-5f4a-3b2c-1d0e-9f8a7b6c5d4e', 'a7b8c9d0-e1f2-3g4h-5i6j-7k8l9m0n1o2p');
//                DELETE FROM [dbo].[TbWarehouses] WHERE [Id] IN ('1f2a3b4c-5d6e-7f8a-9b0c-1d2e3f4a5b6c', '2e3f4a5b-6c7d-8e9f-0a1b-2c3d4e5f6a7b', '3d4e5f6a-7b8c-9d0e-1f2a-3b4c5d6e7f8a', '4e5f6g7h-8i9j-0k1l-2m3n-4o5p6q7r8s9t');
//                DELETE FROM [dbo].[TbVendors] WHERE [Id] IN ('a8a68f35-64f1-4567-95a8-811e9b3981f0', 'b7c9d1e3-5f8a-4c6b-9d0e-1f2a3b4c5d6e', 'c8d0e2f4-6a9b-5d7c-0e1f-2a3b4c5d6e7f', 'd9e0f1g2-3h4i-5j6k-7l8m-9n0o1p2q3r4s');
//                DELETE FROM [dbo].[TbCities] WHERE [Id] IN ('3c4d5e6f-7g8h-9i0j-k1l2-m3n4o5p6q7r8', '4d5e6f7g-8h9i-0j1k-l2m3-n4o5p6q7r8s9', '5e6f7g8h-9i0j-k1l2-m3n4-o5p6q7r8s9t0');
//                DELETE FROM [dbo].[TbStates] WHERE [Id] IN ('008b8b51-c40c-497c-ae18-9422db642124', '1a2b3c4d-5e6f-7g8h-9i0j-k1l2m3n4o5p6', '2b3c4d5e-6f7g-8h9i-0j1k-l2m3n4o5p6q7');
//                DELETE FROM [dbo].[TbCountries] WHERE [Id] IN ('c31ad1a9-ccbe-439e-9b91-5273cfa0f75a', 'd4e5f6g7-h8i9-j0k1-l2m3-n4o5p6q7r8s9');
//                DELETE FROM [dbo].[TbCurrencies] WHERE [Id] IN ('fec27aae-b2d0-49f5-ab84-aad0d77a4905', 'aa258e5e-386f-4e60-8b80-b052e1899d7b', 'ba1adff7-6218-4e69-88d9-e7536c91c0dc');
//                DELETE FROM [dbo].[TbBrands] WHERE [Id] IN ('bf480260-3292-4924-8459-c445d7e84964', 'e2d5c47a-9e5b-4c58-b2d7-2a5b8e9c3f6d', 'a1c3f5e7-8b9d-4e6f-9a0b-2c4d6e8f0a1c', 'd4e6f8a0-2c4b-4e6d-8f0a-1c3e5a7b9d1f', 'f1e2d3c4-b5a6-9788-8910-7f6e5d4c3b2a');

//                -- حذف المستخدمين والأدوار
//                DELETE FROM [dbo].[AspNetUserRoles] 
//                WHERE [UserId] IN ('d37f6313-3de6-48ed-aab7-2423b84ec7d7', '9223ad7c-f56b-4a05-9174-8ea3840bb7bc', 'e168fe35-586e-4948-b833-35fffe1014dd')
//                OR [RoleId] IN ('02b804e9-1144-42d8-bc0a-686d419bc40a', '0c0203ef-78fd-4e63-96b1-98b3555d04c3', 'dd391c0d-4b88-44c8-ab7a-5b27f2ac9152');

//                DELETE FROM [dbo].[AspNetUsers] 
//                WHERE [Id] IN ('d37f6313-3de6-48ed-aab7-2423b84ec7d7', '9223ad7c-f56b-4a05-9174-8ea3840bb7bc', 'e168fe35-586e-4948-b833-35fffe1014dd');

//                DELETE FROM [dbo].[AspNetRoles] 
//                WHERE [Id] IN ('02b804e9-1144-42d8-bc0a-686d419bc40a', '0c0203ef-78fd-4e63-96b1-98b3555d04c3', 'dd391c0d-4b88-44c8-ab7a-5b27f2ac9152');

//                DELETE FROM [dbo].[TbCategories] 
//                WHERE [Id] IN (
//                    'fe567484-5ea7-487c-8956-1ddff6c4200c', '9a1654c7-3a37-4c38-a5f0-2f7898c9b794', 'c4a89d1e-7e9f-4c82-96f3-0d99c8e1a9a2',
//                    '7b1e9f03-eb55-4d23-a7f0-c8c98f22b3f9', 'd3a0ce56-8f5c-4d8b-b7f1-e5c6c8f0e3a2', 'e5f6g7h8-9i0j-1k2l-3m4n-5o6p7q8r9s0t',
//                    'a1b2c3d4-e5f6-7g8h-9i0j-k1l2m3n4o5p6'
//                );                
//            ");
//        }
//    }
//}