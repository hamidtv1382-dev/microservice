using Microsoft.EntityFrameworkCore;
using Payment_Service.src._01_Domain.Core.Entities;
using Payment_Service.src._01_Domain.Core.Enums;
using Payment_Service.src._01_Domain.Core.Interfaces.Repositories;
using Payment_Service.src._03_Infrastructure.Data;

namespace Payment_Service.src._03_Infrastructure.Repositories
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
            return await _context.Refunds.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
        }

        public async Task<IEnumerable<Refund>> GetByPaymentIdAsync(Guid paymentId)
        {
            return await _context.Refunds.Where(r => r.PaymentId == paymentId && !r.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<Refund>> GetByStatusAsync(RefundStatus status)
        {
            return await _context.Refunds.Where(r => r.Status == status && !r.IsDeleted).ToListAsync();
        }

        public async Task AddAsync(Refund refund)
        {
            await _context.Refunds.AddAsync(refund);
        }

        public async Task UpdateAsync(Refund refund)
        {
            _context.Refunds.Update(refund);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Refund refund)
        {
            refund.MarkAsDeleted();
            _context.Refunds.Update(refund);
            await Task.CompletedTask;
        }
    }
}
