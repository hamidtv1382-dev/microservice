using Microsoft.EntityFrameworkCore;
using Seller_Finance_Service.src._01_Domain.Core.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Seller_Finance_Service.src._03_Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<SellerAccount> SellerAccounts { get; set; }
        public DbSet<SellerTransaction> SellerTransactions { get; set; }
        public DbSet<SellerPayout> SellerPayouts { get; set; }
        public DbSet<SellerHold> SellerHolds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
