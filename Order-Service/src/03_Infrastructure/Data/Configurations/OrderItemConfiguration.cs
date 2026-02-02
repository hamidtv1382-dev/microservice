using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order_Service.src._01_Domain.Core.Entities;

namespace Order_Service.src._03_Infrastructure.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            builder.HasKey(oi => oi.Id);
            builder.Property(oi => oi.Id).ValueGeneratedNever();

            builder.Property(oi => oi.ProductId).IsRequired();
            builder.Property(oi => oi.ProductName).IsRequired().HasMaxLength(250);
            builder.Property(oi => oi.ImageUrl).HasMaxLength(500);
            builder.Property(oi => oi.SellerId).IsRequired().HasMaxLength(450);
            builder.Property(oi => oi.Quantity).IsRequired();

            builder.Ignore(oi => oi.TotalPrice);

            // Value Objects Mapping
            builder.OwnsOne(oi => oi.UnitPrice, navigationBuilder =>
            {
                navigationBuilder.Property(m => m.Value).HasColumnName("UnitPriceValue").HasColumnType("decimal(18,0)");
                navigationBuilder.Property(m => m.Currency).HasColumnName("UnitPriceCurrency").HasMaxLength(3).HasDefaultValue("IRR");
            });
        }
    }
}