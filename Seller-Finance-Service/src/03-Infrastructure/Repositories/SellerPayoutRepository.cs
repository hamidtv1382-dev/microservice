using Microsoft.EntityFrameworkCore;
using Seller_Finance_Service.src._01_Domain.Core.Entities;
using Seller_Finance_Service.src._01_Domain.Core.Enums;
using Seller_Finance_Service.src._01_Domain.Core.Interfaces.Repositories;
using Seller_Finance_Service.src._03_Infrastructure.Data;

namespace Seller_Finance_Service.src._03_Infrastructure.Repositories
{
    public class SellerPayoutRepository : ISellerPayoutRepository
    {
        private readonly AppDbContext _context;

        public SellerPayoutRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SellerPayout?> GetByIdAsync(Guid id)
        {
            return await _context.SellerPayouts.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task<IEnumerable<SellerPayout>> GetBySellerAccountIdAsync(Guid accountId)
        {
            return await _context.SellerPayouts
                .Where(p => p.SellerAccountId == accountId && !p.IsDeleted)
                .OrderByDescending(p => p.RequestedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<SellerPayout>> GetRequestedPayoutsAsync()
        {
            return await _context.SellerPayouts
                .Where(p => p.Status == PayoutStatus.Requested && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task AddAsync(SellerPayout payout)
        {
            await _context.SellerPayouts.AddAsync(payout);
        }

        public async Task UpdateAsync(SellerPayout payout)
        {
            _context.SellerPayouts.Update(payout);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(SellerPayout payout)
        {
            payout.MarkAsDeleted();
            _context.SellerPayouts.Update(payout);
            await Task.CompletedTask;
        }
    }
}
