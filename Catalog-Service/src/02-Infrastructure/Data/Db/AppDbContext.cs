using Catalog_Service.src._01_Domain.Core;
using Catalog_Service.src._01_Domain.Core.Entities;
using Catalog_Service.src._01_Domain.Core.Enums;
using Catalog_Service.src._01_Domain.Core.Primitives;
using Catalog_Service.src._02_Infrastructure.Configuration;
using Catalog_Service.src._02_Infrastructure.Data.Converters;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog_Service.src._02_Infrastructure.Data.Db
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet برای موجودیت‌های اصلی
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ImageResource> ImageResources { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<ProductReviewReply> ProductReviewReplies { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =======================================================
            // 1. Money as Value Object (Use Value Converter)
            // =======================================================
            var moneyConverter = new MoneyConverter();

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasConversion(moneyConverter);

            modelBuilder.Entity<Product>()
                .Property(p => p.OriginalPrice)
                .HasConversion(moneyConverter);

            modelBuilder.Entity<ProductVariant>()
                .Property(pv => pv.Price)
                .HasConversion(moneyConverter);

            // =======================================================
            // 2. Owned Types: Dimensions & Weight
            // =======================================================
            modelBuilder.Entity<Product>().OwnsOne(p => p.Dimensions, d =>
            {
                d.Property(p => p.Length).HasColumnName("Dimensions_Length");
                d.Property(p => p.Width).HasColumnName("Dimensions_Width");
                d.Property(p => p.Height).HasColumnName("Dimensions_Height");
                d.Property(p => p.Unit).HasColumnName("Dimensions_Unit");
            });

            modelBuilder.Entity<Product>().OwnsOne(p => p.Weight, w =>
            {
                w.Property(p => p.Value).HasColumnName("Weight_Value");
                w.Property(p => p.Unit).HasColumnName("Weight_Unit");
            });

            modelBuilder.Entity<ProductVariant>().OwnsOne(pv => pv.Dimensions, d =>
            {
                d.Property(p => p.Length).HasColumnName("Dimensions_Length");
                d.Property(p => p.Width).HasColumnName("Dimensions_Width");
                d.Property(p => p.Height).HasColumnName("Dimensions_Height");
                d.Property(p => p.Unit).HasColumnName("Dimensions_Unit");
            });

            modelBuilder.Entity<ProductVariant>().OwnsOne(pv => pv.Weight, w =>
            {
                w.Property(p => p.Value).HasColumnName("Weight_Value");
                w.Property(p => p.Unit).HasColumnName("Weight_Unit");
            });

            // =======================================================
            // 3. Apply IEntityTypeConfiguration
            // =======================================================
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // =======================================================
            // 4. Explicit Relationships
            // =======================================================

            // ProductAttribute
            modelBuilder.Entity<ProductAttribute>()
                .HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductAttribute>()
                .HasOne(x => x.ProductVariant)
                .WithMany()
                .HasForeignKey(x => x.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict);

            // =======================================================
            // 5. ImageResource (Polymorphic / Nullable FKs)
            // =======================================================

            // Explicitly define nullable shadow properties
            modelBuilder.Entity<ImageResource>()
                .Property<int?>("ProductId");

            modelBuilder.Entity<ImageResource>()
                .Property<int?>("ProductVariantId");

            modelBuilder.Entity<ImageResource>()
                .Property<int?>("ProductReviewId");

            // Set up relationships with Restrict
            modelBuilder.Entity<ImageResource>()
                .HasOne<Product>()
                .WithMany(p => p.Images)
                .HasForeignKey("ProductId")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ImageResource>()
                .HasOne<ProductVariant>()
                .WithMany()
                .HasForeignKey("ProductVariantId")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ImageResource>()
                .HasOne<ProductReview>()
                .WithMany()
                .HasForeignKey("ProductReviewId")
                .OnDelete(DeleteBehavior.Restrict);

            // =======================================================
            // 6. ProductReviewReply
            // =======================================================
            modelBuilder.Entity<ProductReviewReply>()
                .HasOne(x => x.ProductReview)
                .WithMany(x => x.Replies)
                .HasForeignKey(x => x.ProductReviewId)
                .OnDelete(DeleteBehavior.Restrict);

            // =======================================================
            // 7. Global Query Filters
            // =======================================================
            ConfigureGlobalQueryFilters(modelBuilder);
        }


        private void ConfigureGlobalQueryFilters(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<ImageResource>()
                .HasQueryFilter(i => !i.IsDeleted);

            modelBuilder.Entity<ProductAttribute>()
                .HasQueryFilter(pa => !pa.IsDeleted);

            modelBuilder.Entity<ProductReview>()
                 .HasQueryFilter(pr => !pr.IsDeleted);

            modelBuilder.Entity<ProductTag>()
                 .HasQueryFilter(pt => !pt.IsDeleted);

            // برای Brand و Category طبق نیازتان می‌توانید IsActive را فیلتر کنید
            // modelBuilder.Entity<Category>().HasQueryFilter(c => c.IsActive);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            await ProcessDomainEventsAsync(cancellationToken);
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries<Entity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Property("CreatedAt") != null && entry.Property("CreatedAt").CurrentValue == null)
                    {
                        entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    // برای Soft Delete هم UpdatedAt آپدیت شود
                    if (entry.Property("IsDeleted") != null && entry.Property("IsDeleted").IsModified && (bool)entry.Property("IsDeleted").CurrentValue == true)
                    {
                        entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                    }

                    if (entry.Property("UpdatedAt") != null && entry.Property("IsDeleted") == null)
                    {
                        entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                    }
                }
            }
        }

        private async Task ProcessDomainEventsAsync(CancellationToken cancellationToken)
        {
            var domainEntities = ChangeTracker.Entries<AggregateRoot>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
                .ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

            // ارسال رویدادها به Mediator یا MessageBus
            // foreach (var domainEvent in domainEvents)
            // {
            //     await _mediator.Publish(domainEvent, cancellationToken);
            // }
        }
    }
}