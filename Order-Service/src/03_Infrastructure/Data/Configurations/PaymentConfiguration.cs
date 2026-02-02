using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order_Service.src._01_Domain.Core.Entities;
using Order_Service.src._01_Domain.Core.Enums;

namespace Order_Service.src._03_Infrastructure.Data.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedNever();

            builder.Property(p => p.OrderId).IsRequired();
            builder.Property(p => p.PaymentGateway).IsRequired().HasMaxLength(100);
            builder.Property(p => p.TransactionId).HasMaxLength(100);
            builder.Property(p => p.PaidAt).IsRequired(false);
            builder.Property(p => p.FailureReason).HasMaxLength(500);
            builder.Property(p => p.Payload).HasMaxLength(2000);

            builder.Property(p => p.Status)
                .HasConversion(s => s.ToString(),
                              s => (PaymentStatus)Enum.Parse(typeof(PaymentStatus), s));

            builder.Ignore(p => p.DomainEvents);

            // Value Objects Mapping
            builder.OwnsOne(p => p.Amount, navigationBuilder =>
            {
                navigationBuilder.Property(m => m.Value).HasColumnName("AmountValue").HasColumnType("decimal(18,0)");
                navigationBuilder.Property(m => m.Currency).HasColumnName("AmountCurrency").HasMaxLength(3).HasDefaultValue("IRR");
            });
        }
    }
}