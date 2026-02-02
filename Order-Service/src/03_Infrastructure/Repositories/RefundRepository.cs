using Microsoft.EntityFrameworkCore;
using Order_Service.src._01_Domain.Core.Entities;
using Order_Service.src._01_Domain.Core.Interfaces.Repositories;
using Order_Service.src._03_Infrastructure.Data;

namespace Order_Service.src._03_Infrastructure.Repositories
{
    public class RefundRepository : IRefundRepository
    {
        private readonly AppDbContext _context;

        public RefundRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Refund?> GetByIdAsync(Guid id)
        {
            return await _context.Refunds.FindAsync(id);
        }

        public async Task<Refund?> GetByOrderIdAsync(Guid orderId)
        {
            return await _context.Refunds
                .FirstOrDefaultAsync(r => r.OrderId == orderId);
        }

        public async Task<IEnumerable<Refund>> GetByPaymentIdAsync(Guid paymentId)
        {
            return await _context.Refunds
                .Where(r => r.PaymentId == paymentId)
                .ToListAsync();
        }

        public async Task AddAsync(Refund refund)
        {
            await _context.Refunds.AddAsync(refund);
        }

        public void Update(Refund refund)
        {
            _context.Refunds.Update(refund);
        }

        public void Delete(Refund refund)
        {
            _context.Refunds.Remove(refund);
        }
    }
}
