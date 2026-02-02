using Microsoft.EntityFrameworkCore;
using Review_Rating_Service.src._01_Domain.Core.Aggregates.Review;

namespace Review_Rating_Service.src._03_Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

          
        }
    }
}
