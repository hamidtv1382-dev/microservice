using Finance_Service.src._01_Domain.Core.Entities;
using Finance_Service.src._01_Domain.Core.Enums;
using Finance_Service.src._01_Domain.Core.Interfaces.Repositories;
using Finance_Service.src._03_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Finance_Service.src._03_Infrastructure.Repositories
{
    public class SettlementRepository : ISettlementRepository
    {
        private readonly AppDbContext _context;

        public SettlementRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Settlement?> GetByIdAsync(Guid id)
        {
            return await _context.Settlements.FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
        }

        public async Task<IEnumerable<Settlement>> GetBySellerIdAsync(Guid sellerId)
        {
            return await _context.Settlements.Where(s => s.SellerId == sellerId && !s.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<Settlement>> GetByStatusAsync(SettlementStatus status)
        {
            return await _context.Settlements.Where(s => s.Status == status && !s.IsDeleted).ToListAsync();
        }

        public async Task AddAsync(Settlement settlement)
        {
            await _context.Settlements.AddAsync(settlement);
        }

        public async Task UpdateAsync(Settlement settlement)
        {
            _context.Settlements.Update(settlement);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Settlement settlement)
        {
            settlement.MarkAsDeleted();
            _context.Settlements.Update(settlement);
            await Task.CompletedTask;
        }
    }
}
