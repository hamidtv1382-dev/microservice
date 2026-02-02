using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order_Service.src._01_Domain.Core.Entities;
using Order_Service.src._01_Domain.Core.Enums;

namespace Order_Service.src._03_Infrastructure.Data.Configurations
{
    public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.ToTable("Discounts");

            builder.HasKey(d => d.Id);
            builder.Property(d => d.Id).ValueGeneratedNever();

            builder.Property(d => d.Code).IsRequired().HasMaxLength(50);
            builder.Property(d => d.Description).HasMaxLength(500);

            builder.Property(d => d.Type)
                .HasConversion(t => t.ToString(),
                              t => (DiscountType)Enum.Parse(typeof(DiscountType), t));

            builder.Property(d => d.Value).HasColumnType("decimal(18,0)");
            builder.Property(d => d.UsageLimit).IsRequired();
            builder.Property(d => d.TimesUsed).IsRequired();
            builder.Property(d => d.IsActive).IsRequired();

            builder.Ignore(d => d.DomainEvents);

            // Value Objects Mapping
            builder.OwnsOne(d => d.MinimumOrderAmount, navigationBuilder =>
            {
                navigationBuilder.Property(m => m.Value).HasColumnName("MinimumOrderAmountValue").HasColumnType("decimal(18,0)");
                navigationBuilder.Property(m => m.Currency).HasColumnName("MinimumOrderAmountCurrency").HasMaxLength(3).HasDefaultValue("IRR");
            });
        }
    }
}