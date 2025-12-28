using Domains.Entities.ECommerceSystem.Customer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Configurations
{
	public class CustomerConfiguration : IEntityTypeConfiguration<TbCustomer>
	{
		public void Configure(EntityTypeBuilder<TbCustomer> builder)
		{
			builder.ToTable("TbCustomer");

			builder.HasKey(x => x.Id);

			builder.Property(x => x.UserId)
				   .HasMaxLength(450);
		}
	}
}
