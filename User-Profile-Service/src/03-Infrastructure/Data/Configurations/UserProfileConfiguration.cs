using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User_Profile_Service.src._01_Domain.Core.Aggregates.UserProfile;

namespace User_Profile_Service.src._03_Infrastructure.Data.Configurations
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.ToTable("UserProfiles");

            builder.HasKey(u => u.Id);
            builder.Property(u => u.UserId).IsRequired();
            builder.Property(u => u.Gender).HasConversion<int>();
            builder.Property(u => u.Status).HasConversion<int>();

            // FullName Mapping
            builder.OwnsOne(u => u.Name, name =>
            {
                name.Property(n => n.FirstName).HasColumnName("FirstName").HasMaxLength(100).IsRequired();
                name.Property(n => n.LastName).HasColumnName("LastName").HasMaxLength(100).IsRequired();
            });

            // UserContactInfo Mapping (Flattened to columns)
            builder.OwnsOne(u => u.ContactInfo, contact =>
            {
                // Map Email
                contact.Property(c => c.Email).HasColumnName("Email").HasMaxLength(255).IsRequired();

                // Map Phone
                contact.Property(c => c.PhoneNumber).HasColumnName("PhoneNumber").HasMaxLength(20);
                contact.Property(c => c.CountryCode).HasColumnName("PhoneCountryCode").HasMaxLength(5);
            });

            // UserAvatar Mapping
            builder.OwnsOne(u => u.Avatar, avatar =>
            {
                avatar.Property(a => a.Url).HasColumnName("AvatarUrl").HasMaxLength(500);
                avatar.Property(a => a.LastUpdatedAt);
            });

            // BirthDate Mapping
            builder.OwnsOne(u => u.BirthDate, birthDate =>
            {
                birthDate.Property(b => b.Date).HasColumnName("BirthDate");
            });

            // UserPreference Mapping
            builder.OwnsOne(u => u.Preferences, prefs =>
            {
                prefs.Property(p => p.LanguageCode).HasColumnName("LanguageCode").HasMaxLength(10).IsRequired();
                prefs.Property(p => p.CurrencyCode).HasColumnName("CurrencyCode").HasMaxLength(5).IsRequired();
                prefs.Property(p => p.NotificationsEnabled).HasColumnName("NotificationsEnabled").IsRequired();
                prefs.Property(p => p.Theme).HasColumnName("Theme").HasMaxLength(20).IsRequired();
            });

            // Addresses Navigation
            builder.Metadata.FindNavigation(nameof(UserProfile.Addresses))!.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
