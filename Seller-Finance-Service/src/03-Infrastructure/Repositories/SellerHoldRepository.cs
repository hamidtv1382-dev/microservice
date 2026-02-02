using Microsoft.EntityFrameworkCore;
using Seller_Finance_Service.src._01_Domain.Core.Entities;
using Seller_Finance_Service.src._01_Domain.Core.Interfaces.Repositories;
using Seller_Finance_Service.src._03_Infrastructure.Data;

namespace Seller_Finance_Service.src._03_Infrastructure.Repositories
{
    public class SellerHoldRepository : ISellerHoldRepository
    {
        private readonly AppDbContext _context;

        public SellerHoldRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SellerHold?> GetByIdAsync(Guid id)
        {
            return await _context.SellerHolds.FirstOrDefaultAsync(h => h.Id == id && !h.IsDeleted);
        }

        public async Task<IEnumerable<SellerHold>> GetActiveHoldsBySellerAccountIdAsync(Guid accountId)
        {
            return await _context.SellerHolds
                .Where(h => h.SellerAccountId == accountId && !h.IsReleased && !h.IsDeleted)
                .ToListAsync();
        }

        public async Task AddAsync(SellerHold hold)
        {
            await _context.SellerHolds.AddAsync(hold);
        }

        public async Task UpdateAsync(SellerHold hold)
        {
            _context.SellerHolds.Update(hold);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(SellerHold hold)
        {
            hold.MarkAsDeleted();
            _context.SellerHolds.Update(hold);
            await Task.CompletedTask;
        }
    }
}
