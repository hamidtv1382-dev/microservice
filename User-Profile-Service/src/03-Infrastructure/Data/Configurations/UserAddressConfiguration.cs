using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User_Profile_Service.src._01_Domain.Core.Aggregates.UserProfile;

namespace User_Profile_Service.src._03_Infrastructure.Data.Configurations
{
    public class UserAddressConfiguration : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.ToTable("UserAddresses");

            builder.HasKey(a => a.Id);

            builder.Property<Guid>("UserProfileId").IsRequired();

            builder.Property(a => a.Type).HasConversion<int>().IsRequired();
            builder.Property(a => a.Title).HasMaxLength(100);
            builder.Property(a => a.IsDefault).IsRequired();

            // نگاشت مستقیم پراپرتی‌های Address
            builder.Property(a => a.Street).HasMaxLength(255).IsRequired();
            builder.Property(a => a.City).HasMaxLength(100).IsRequired();
            builder.Property(a => a.State).HasMaxLength(100);
            builder.Property(a => a.PostalCode).HasMaxLength(20).IsRequired();
            builder.Property(a => a.Country).HasMaxLength(100).IsRequired();

            builder.HasOne<UserProfile>()
                .WithMany()
                .HasForeignKey("UserProfileId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
