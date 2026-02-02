using Microsoft.EntityFrameworkCore;
using Payment_Service.src._01_Domain.Core.Entities;
using Payment_Service.src._01_Domain.Core.Enums;
using Payment_Service.src._01_Domain.Core.Interfaces.Repositories;
using Payment_Service.src._03_Infrastructure.Data;

namespace Payment_Service.src._03_Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;

        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Payment?> GetByIdAsync(Guid id)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task<Payment?> GetByOrderIdAsync(Guid orderId)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId && !p.IsDeleted);
        }

        public async Task<Payment?> GetByTransactionNumberAsync(string transactionNumber)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.TransactionNumber.Value == transactionNumber && !p.IsDeleted);
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _context.Payments.Where(p => !p.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status)
        {
            return await _context.Payments.Where(p => p.Status == status && !p.IsDeleted).ToListAsync();
        }

        public async Task AddAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
        }

        public async Task UpdateAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Payment payment)
        {
            payment.MarkAsDeleted();
            _context.Payments.Update(payment);
            await Task.CompletedTask;
        }
    }
}
