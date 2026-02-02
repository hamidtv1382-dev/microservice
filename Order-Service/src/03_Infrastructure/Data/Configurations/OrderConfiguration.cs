using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order_Service.src._01_Domain.Core.Entities;
using Order_Service.src._01_Domain.Core.Enums;

namespace Order_Service.src._03_Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).ValueGeneratedNever();
            builder.Property(o => o.BuyerId).IsRequired();

            builder.Property(o => o.Status)
                .HasConversion(p => p.ToString(),
                              p => (OrderStatus)Enum.Parse(typeof(OrderStatus), p));

            builder.Property(o => o.Description).HasMaxLength(500);
            builder.Property(o => o.OrderDate).IsRequired(false);

            builder.HasMany(o => o.Items)
                .WithOne()
                .HasForeignKey("OrderId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Ignore(o => o.DomainEvents);

            // Value Objects Mapping
            builder.OwnsOne(o => o.OrderNumber, navigationBuilder =>
            {
                navigationBuilder.Property(n => n.Value).HasColumnName("OrderNumber").HasMaxLength(50);
            });

            builder.OwnsOne(o => o.ShippingAddress, navigationBuilder =>
            {
                navigationBuilder.Property(a => a.FirstName).HasMaxLength(100);
                navigationBuilder.Property(a => a.LastName).HasMaxLength(100);
                navigationBuilder.Property(a => a.PhoneNumber).HasMaxLength(20);
                navigationBuilder.Property(a => a.Country).HasMaxLength(100);
                navigationBuilder.Property(a => a.City).HasMaxLength(100);
                navigationBuilder.Property(a => a.State).HasMaxLength(100);
                navigationBuilder.Property(a => a.Street).HasMaxLength(255);
                navigationBuilder.Property(a => a.ZipCode).HasMaxLength(20);
                navigationBuilder.Property(a => a.BuildingNumber).HasMaxLength(50);
                navigationBuilder.Property(a => a.ApartmentNumber).HasMaxLength(50);
            });

            builder.OwnsOne(o => o.TotalAmount, navigationBuilder =>
            {
                navigationBuilder.Property(m => m.Value).HasColumnName("TotalAmountValue").HasColumnType("decimal(18,0)");
                navigationBuilder.Property(m => m.Currency).HasColumnName("TotalAmountCurrency").HasMaxLength(3).HasDefaultValue("IRR");
            });

            builder.OwnsOne(o => o.DiscountAmount, navigationBuilder =>
            {
                navigationBuilder.Property(m => m.Value).HasColumnName("DiscountAmountValue").HasColumnType("decimal(18,0)");
                navigationBuilder.Property(m => m.Currency).HasColumnName("DiscountAmountCurrency").HasMaxLength(3).HasDefaultValue("IRR");
            });

            builder.OwnsOne(o => o.FinalAmount, navigationBuilder =>
            {
                navigationBuilder.Property(m => m.Value).HasColumnName("FinalAmountValue").HasColumnType("decimal(18,0)");
                navigationBuilder.Property(m => m.Currency).HasColumnName("FinalAmountCurrency").HasMaxLength(3).HasDefaultValue("IRR");
            });
        }
    }
}