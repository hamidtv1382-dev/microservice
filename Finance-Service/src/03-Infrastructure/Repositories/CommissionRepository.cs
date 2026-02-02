using Finance_Service.src._01_Domain.Core.Entities;
using Finance_Service.src._01_Domain.Core.Interfaces.Repositories;
using Finance_Service.src._03_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Finance_Service.src._03_Infrastructure.Repositories
{
    public class CommissionRepository : ICommissionRepository
    {
        private readonly AppDbContext _context;

        public CommissionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Commission?> GetByIdAsync(Guid id)
        {
            return await _context.Commissions.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }

        public async Task<IEnumerable<Commission>> GetByOrderIdAsync(Guid orderId)
        {
            return await _context.Commissions.Where(c => c.OrderId == orderId && !c.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<Commission>> GetBySellerIdAsync(Guid sellerId)
        {
            return await _context.Commissions.Where(c => c.SellerId == sellerId && !c.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<Commission>> GetUnsettledCommissionsAsync()
        {
            return await _context.Commissions.Where(c => !c.IsSettled && !c.IsDeleted).ToListAsync();
        }

        public async Task AddAsync(Commission commission)
        {
            await _context.Commissions.AddAsync(commission);
        }

        public async Task UpdateAsync(Commission commission)
        {
            _context.Commissions.Update(commission);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Commission commission)
        {
            commission.MarkAsDeleted();
            _context.Commissions.Update(commission);
            await Task.CompletedTask;
        }
    }
}
