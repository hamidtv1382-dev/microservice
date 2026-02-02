using Microsoft.EntityFrameworkCore;
using Seller_Finance_Service.src._01_Domain.Core.Entities;
using Seller_Finance_Service.src._01_Domain.Core.Interfaces.Repositories;
using Seller_Finance_Service.src._03_Infrastructure.Data;

namespace Seller_Finance_Service.src._03_Infrastructure.Repositories
{
    public class SellerAccountRepository : ISellerAccountRepository
    {
        private readonly AppDbContext _context;

        public SellerAccountRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SellerAccount?> GetByIdAsync(Guid id)
        {
            return await _context.SellerAccounts.FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
        }

        public async Task<SellerAccount?> GetBySellerIdAsync(Guid sellerId)
        {
            return await _context.SellerAccounts.FirstOrDefaultAsync(s => s.SellerId == sellerId && !s.IsDeleted);
        }

        public async Task AddAsync(SellerAccount account)
        {
            await _context.SellerAccounts.AddAsync(account);
        }

        public async Task UpdateAsync(SellerAccount account)
        {
            _context.SellerAccounts.Update(account);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(SellerAccount account)
        {
            account.MarkAsDeleted();
            _context.SellerAccounts.Update(account);
            await Task.CompletedTask;
        }
    }
}
