using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order_Service.src._01_Domain.Core.Entities;

namespace Order_Service.src._03_Infrastructure.Data.Configurations
{
    public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
    {
        public void Configure(EntityTypeBuilder<BasketItem> builder)
        {
            builder.ToTable("BasketItems");

            builder.HasKey(bi => bi.Id);
            builder.Property(bi => bi.Id).ValueGeneratedNever();

            builder.Property(bi => bi.ProductId).IsRequired();
            builder.Property(bi => bi.ProductName).IsRequired().HasMaxLength(250);
            builder.Property(bi => bi.ImageUrl).HasMaxLength(500);

            builder.Property(bi => bi.Quantity).IsRequired();

            builder.Ignore(bi => bi.TotalPrice);

            // Value Objects Mapping
            builder.OwnsOne(bi => bi.UnitPrice, navigationBuilder =>
            {
                navigationBuilder.Property(m => m.Value).HasColumnName("UnitPriceValue").HasColumnType("decimal(18,0)");
                navigationBuilder.Property(m => m.Currency).HasColumnName("UnitPriceCurrency").HasMaxLength(3).HasDefaultValue("IRR");
            });
        }
    }
}