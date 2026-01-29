using Common.Enumerations.FieldType;
using Domains.Entities.WithdrawalMethods;
using Microsoft.EntityFrameworkCore;

namespace DAL.ApplicationContext
{
    public static class WithdrawalMethodSeedData
    {
        // Seed Withdrawal Method IDs
        private static readonly Guid paypalId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        private static readonly Guid bankTransferId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        private static readonly Guid vodafoneCashId = Guid.Parse("33333333-3333-3333-3333-333333333333");
        private static readonly Guid instapayId = Guid.Parse("44444444-4444-4444-4444-444444444444");
        private static readonly Guid wiseId = Guid.Parse("55555555-5555-5555-5555-555555555555");

        public static async Task SeedWithdrawalMethodsAsync(ApplicationDbContext context)
        {
            try
            {
                Console.WriteLine("[SeedWithdrawalMethodsAsync] Starting...");

                // Check if withdrawal methods already exist
                if (await context.Set<TbWithdrawalMethod>().AnyAsync())
                {
                    Console.WriteLine("[SeedWithdrawalMethodsAsync] Withdrawal methods already exist, skipping...");
                    return;
                }

                // Seed Withdrawal Methods
                var withdrawalMethods = new List<TbWithdrawalMethod>
                {
                    new TbWithdrawalMethod
                    {
                        Id = paypalId,
                        TitleAr = "باي بال",
                        TitleEn = "PayPal",
                        ImagePath = "/images/withdrawal-methods/paypal.png",
                        CreatedBy = Guid.Empty,
                        CreatedDateUtc = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new TbWithdrawalMethod
                    {
                        Id = bankTransferId,
                        TitleAr = "تحويل بنكي",
                        TitleEn = "Bank Transfer",
                        ImagePath = "/images/withdrawal-methods/bank-transfer.png",
                        CreatedBy = Guid.Empty,
                        CreatedDateUtc = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new TbWithdrawalMethod
                    {
                        Id = vodafoneCashId,
                        TitleAr = "فودافون كاش",
                        TitleEn = "Vodafone Cash",
                        ImagePath = "/images/withdrawal-methods/vodafone-cash.png",
                        CreatedBy = Guid.Empty,
                        CreatedDateUtc = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new TbWithdrawalMethod
                    {
                        Id = instapayId,
                        TitleAr = "انستا باي",
                        TitleEn = "InstaPay",
                        ImagePath = "/images/withdrawal-methods/instapay.png",
                        CreatedBy = Guid.Empty,
                        CreatedDateUtc = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new TbWithdrawalMethod
                    {
                        Id = wiseId,
                        TitleAr = "وايز",
                        TitleEn = "Wise",
                        ImagePath = "/images/withdrawal-methods/wise.png",
                        CreatedBy = Guid.Empty,
                        CreatedDateUtc = DateTime.UtcNow,
                        IsDeleted = false
                    }
                };

                await context.Set<TbWithdrawalMethod>().AddRangeAsync(withdrawalMethods);
                Console.WriteLine("[SeedWithdrawalMethodsAsync] Added 5 withdrawal methods");

                // Seed Fields
                var fields = new List<TbField>();

                // PayPal Fields
                fields.Add(new TbField
                {
                    Id = Guid.Parse("a1111111-1111-1111-1111-111111111111"),
                    TitleAr = "البريد الإلكتروني",
                    TitleEn = "Email Address",
                    FieldType = FieldType.Email,
                    WithdrawalMethodId = paypalId,
                    CreatedBy = Guid.Empty,
                    CreatedDateUtc = DateTime.UtcNow,
                    IsDeleted = false
                });

                // Bank Transfer Fields
                fields.AddRange(new[]
                {
                    new TbField
                    {
                        Id = Guid.Parse("a2222222-2222-2222-2222-222222222221"),
                        TitleAr = "اسم البنك",
                        TitleEn = "Bank Name",
                        FieldType = FieldType.Text,
                        WithdrawalMethodId = bankTransferId,
                        CreatedBy = Guid.Empty,
                        CreatedDateUtc = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new TbField
                    {
                        Id = Guid.Parse("a2222222-2222-2222-2222-222222222222"),
                        TitleAr = "اسم صاحب الحساب",
                        TitleEn = "Account Holder Name",
                        FieldType = FieldType.Text,
                        WithdrawalMethodId = bankTransferId,
                        CreatedBy = Guid.Empty,
                        CreatedDateUtc = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new TbField
                    {
                        Id = Guid.Parse("a2222222-2222-2222-2222-222222222223"),
                        TitleAr = "رقم الحساب",
                        TitleEn = "Account Number",
                        FieldType = FieldType.IntegerNumber,
                        WithdrawalMethodId = bankTransferId,
                        CreatedBy = Guid.Empty,
                        CreatedDateUtc = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new TbField
                    {
                        Id = Guid.Parse("a2222222-2222-2222-2222-222222222224"),
                        TitleAr = "رقم الآيبان",
                        TitleEn = "IBAN",
                        FieldType = FieldType.Text,
                        WithdrawalMethodId = bankTransferId,
                        CreatedBy = Guid.Empty,
                        CreatedDateUtc = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new TbField
                    {
                        Id = Guid.Parse("a2222222-2222-2222-2222-222222222225"),
                        TitleAr = "رمز السويفت",
                        TitleEn = "SWIFT/BIC Code",
                        FieldType = FieldType.Text,
                        WithdrawalMethodId = bankTransferId,
                        CreatedBy = Guid.Empty,
                        CreatedDateUtc = DateTime.UtcNow,
                        IsDeleted = false
                    }
                });

                // Vodafone Cash Fields
                fields.AddRange(new[]
                {
                    new TbField
                    {
                        Id = Guid.Parse("a3333333-3333-3333-3333-333333333331"),
                        TitleAr = "رقم الموبايل",
                        TitleEn = "Mobile Number",
                        FieldType = FieldType.PhoneNumber,
                        WithdrawalMethodId = vodafoneCashId,
                        CreatedBy = Guid.Empty,
                        CreatedDateUtc = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new TbField
                    {
                        Id = Guid.Parse("a3333333-3333-3333-3333-333333333332"),
                        TitleAr = "الاسم",
                        TitleEn = "Name",
                        FieldType = FieldType.Text,
                        WithdrawalMethodId = vodafoneCashId,
                        CreatedBy = Guid.Empty,
                        CreatedDateUtc = DateTime.UtcNow,
                        IsDeleted = false
                    }
                });

                // InstaPay Fields
                fields.AddRange(new[]
                {
                    new TbField
                    {
                        Id = Guid.Parse("a4444444-4444-4444-4444-444444444441"),
                        TitleAr = "رقم الموبايل",
                        TitleEn = "Mobile Number",
                        FieldType = FieldType.PhoneNumber,
                        WithdrawalMethodId = instapayId,
                        CreatedBy = Guid.Empty,
                        CreatedDateUtc = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new TbField
                    {
                        Id = Guid.Parse("a4444444-4444-4444-4444-444444444442"),
                        TitleAr = "الاسم",
                        TitleEn = "Name",
                        FieldType = FieldType.Text,
                        WithdrawalMethodId = instapayId,
                        CreatedBy = Guid.Empty,
                        CreatedDateUtc = DateTime.UtcNow,
                        IsDeleted = false
                    }
                });

                // Wise Fields
                fields.AddRange(new[]
                {
                    new TbField
                    {
                        Id = Guid.Parse("a5555555-5555-5555-5555-555555555551"),
                        TitleAr = "البريد الإلكتروني",
                        TitleEn = "Email Address",
                        FieldType = FieldType.Email,
                        WithdrawalMethodId = wiseId,
                        CreatedBy = Guid.Empty,
                        CreatedDateUtc = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new TbField
                    {
                        Id = Guid.Parse("a5555555-5555-5555-5555-555555555552"),
                        TitleAr = "الاسم الكامل",
                        TitleEn = "Full Name",
                        FieldType = FieldType.Text,
                        WithdrawalMethodId = wiseId,
                        CreatedBy = Guid.Empty,
                        CreatedDateUtc = DateTime.UtcNow,
                        IsDeleted = false
                    }
                });

                await context.Set<TbField>().AddRangeAsync(fields);
                Console.WriteLine($"[SeedWithdrawalMethodsAsync] Added {fields.Count} fields");

                await context.SaveChangesAsync();
                Console.WriteLine("[SeedWithdrawalMethodsAsync] Completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SeedWithdrawalMethodsAsync] ERROR: {ex.Message}");
                Console.WriteLine($"[SeedWithdrawalMethodsAsync] Stack Trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}