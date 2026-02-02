using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order_Service.src._01_Domain.Core.Entities;
using Order_Service.src._01_Domain.Core.Enums;

namespace Order_Service.src._03_Infrastructure.Data.Configurations
{
    public class RefundConfiguration : IEntityTypeConfiguration<Refund>
    {
        public void Configure(EntityTypeBuilder<Refund> builder)
        {
            builder.ToTable("Refunds");

            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).ValueGeneratedNever();

            builder.Property(r => r.OrderId).IsRequired();
            builder.Property(r => r.PaymentId).IsRequired();
            builder.Property(r => r.Reason).IsRequired().HasMaxLength(500);
            builder.Property(r => r.TransactionId).HasMaxLength(100);
            builder.Property(r => r.ProcessedAt).IsRequired(false);
            builder.Property(r => r.FailureReason).HasMaxLength(500);

            builder.Property(r => r.Status)
                .HasConversion(s => s.ToString(),
                              s => (PaymentStatus)Enum.Parse(typeof(PaymentStatus), s));

            builder.Ignore(r => r.DomainEvents);

            // Value Objects Mapping
            builder.OwnsOne(r => r.Amount, navigationBuilder =>
            {
                navigationBuilder.Property(m => m.Value).HasColumnName("AmountValue").HasColumnType("decimal(18,0)");
                navigationBuilder.Property(m => m.Currency).HasColumnName("AmountCurrency").HasMaxLength(3).HasDefaultValue("IRR");
            });
        }
    }
}