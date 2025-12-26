using Common.Enumerations.Review;
using Domains.Entities.ECommerceSystem.Review;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
	//public class ReviewReportConfiguration : IEntityTypeConfiguration<TbReviewReport>
	//{
	//    public void Configure(EntityTypeBuilder<TbReviewReport> entity)
	//    {
	//        entity.HasKey(e => e.Id);

	//        entity.Property(e => e.Id)
	//            .HasDefaultValueSql("NEWID()");

	//        //entity.Property(e => e.ReportId)
	//        //    .IsRequired();

	//        entity.Property(e => e.ItemReviewId)
	//            .IsRequired();

	//        entity.Property(e => e.CustomerId)
	//            .IsRequired();

	//        entity.Property(e => e.Reason)
	//            .IsRequired()
	//            .HasMaxLength(1);

	//        entity.Property(e => e.Details)
	//            .HasColumnType("nvarchar(max)");

	//        entity.Property(e => e.Status)
	//            .IsRequired()
	//            .HasConversion<int>();

	//        entity.Property(e => e.IsDeleted)
	//            .HasDefaultValue(1);

	//        entity.Property(e => e.CreatedDateUtc)
	//            .HasColumnType("datetime2(2)")
	//            .HasDefaultValueSql("GETUTCDATE()");

	//        entity.Property(e => e.UpdatedDateUtc)
	//            .HasColumnType("datetime2(2)");

	//        // Indexes
	//        entity.HasIndex(e => e.IsDeleted);
	//        entity.HasIndex(e => e.ItemReviewId);
	//        entity.HasIndex(e => e.CustomerId);
	//        entity.HasIndex(e => e.Status);


	//        // Relationships
	//        entity.HasOne(e => e.ItemReview)
	//            .WithMany(r => r.ReviewReports)
	//            .HasForeignKey(e => e.ItemReviewId)
	//            .OnDelete(DeleteBehavior.Restrict);
	//    }
	//}

	public class ReviewReportConfiguration : IEntityTypeConfiguration<TbReviewReport>
	{
		public void Configure(EntityTypeBuilder<TbReviewReport> entity)
		{
			
			entity.HasKey(e => e.Id);

			entity.Property(e => e.Id)
				  .HasDefaultValueSql("NEWID()");

			
			entity.Property(e => e.ItemReviewId)
				  .IsRequired();

			entity.Property(e => e.CustomerId)
				  .IsRequired();

			
			entity.Property(e => e.Reason)
				  .IsRequired()
				  .HasConversion<int>(); 
			

			entity.Property(e => e.Details)
				  .HasMaxLength(1000) 
				  .IsUnicode(true);

			
			entity.Property(e => e.Status)
				  .IsRequired()
				  .HasConversion<int>()
				  .HasDefaultValue(ReviewReportStatus.Pending); 

			
			entity.Property(e => e.IsDeleted)
				  .HasDefaultValue(false); 

			
			entity.Property(e => e.CreatedBy)
				  .IsRequired();

			entity.Property(e => e.CreatedDateUtc)
				  .HasColumnType("datetime2(2)")
				  .HasDefaultValueSql("GETUTCDATE()");

			entity.Property(e => e.UpdatedDateUtc)
				  .HasColumnType("datetime2(2)");

			
			entity.HasIndex(e => e.IsDeleted);
			entity.HasIndex(e => e.ItemReviewId);
			entity.HasIndex(e => e.CustomerId);
			entity.HasIndex(e => e.Status);
			entity.HasIndex(e => e.Reason); 

			
			entity.HasIndex(e => new { e.CustomerId, e.ItemReviewId })
				  .IsUnique()
				  .HasFilter("[IsDeleted] = 0") 
				  .HasDatabaseName("IX_TbReviewReports_CustomerId_ItemReviewId_Unique");

			
			entity.HasOne(e => e.ItemReview)
				  .WithMany(r => r.ReviewReports)
				  .HasForeignKey(e => e.ItemReviewId)
				  .OnDelete(DeleteBehavior.Restrict);

			entity.HasOne(e => e.Customer)
				  .WithMany() 
				  .HasForeignKey(e => e.CustomerId)
				  .OnDelete(DeleteBehavior.Cascade); 
		}
	}
}
