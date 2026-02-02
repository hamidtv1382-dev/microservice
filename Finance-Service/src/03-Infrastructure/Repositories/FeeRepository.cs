using Finance_Service.src._01_Domain.Core.Entities;
using Finance_Service.src._01_Domain.Core.Interfaces.Repositories;
using Finance_Service.src._03_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Finance_Service.src._03_Infrastructure.Repositories
{
    public class FeeRepository : IFeeRepository
    {
        private readonly AppDbContext _context;

        public FeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Fee?> GetByIdAsync(Guid id)
        {
            return await _context.Fees.FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted);
        }

        public async Task<IEnumerable<Fee>> GetByOrderIdAsync(Guid orderId)
        {
            return await _context.Fees.Where(f => f.OrderId == orderId && !f.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<Fee>> GetBySellerIdAsync(Guid sellerId)
        {
            return await _context.Fees.Where(f => f.SellerId == sellerId && !f.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<Fee>> GetUnpaidFeesAsync()
        {
            return await _context.Fees.Where(f => !f.IsPaid && !f.IsDeleted).ToListAsync();
        }

        public async Task AddAsync(Fee fee)
        {
            await _context.Fees.AddAsync(fee);
        }

        public async Task UpdateAsync(Fee fee)
        {
            _context.Fees.Update(fee);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Fee fee)
        {
            fee.MarkAsDeleted();
            _context.Fees.Update(fee);
            await Task.CompletedTask;
        }
    }
}
