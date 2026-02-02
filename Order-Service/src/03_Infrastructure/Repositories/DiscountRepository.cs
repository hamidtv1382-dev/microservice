using Microsoft.EntityFrameworkCore;
using Order_Service.src._01_Domain.Core.Entities;
using Order_Service.src._01_Domain.Core.Interfaces.Repositories;
using Order_Service.src._03_Infrastructure.Data;

namespace Order_Service.src._03_Infrastructure.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly AppDbContext _context;

        public DiscountRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Discount?> GetByIdAsync(Guid id)
        {
            return await _context.Discounts.FindAsync(id);
        }

        public async Task<Discount?> GetByCodeAsync(string code)
        {
            return await _context.Discounts
                .FirstOrDefaultAsync(d => d.Code == code);
        }

        public async Task<IEnumerable<Discount>> GetAllActiveAsync()
        {
            return await _context.Discounts
                .Where(d => d.IsActive && !d.IsDeleted)
                .ToListAsync();
        }

        public async Task AddAsync(Discount discount)
        {
            await _context.Discounts.AddAsync(discount);
        }

        public void Update(Discount discount)
        {
            _context.Discounts.Update(discount);
        }

        public void Delete(Discount discount)
        {
            discount.MarkAsDeleted();
            _context.Discounts.Update(discount);
        }
    }
}
