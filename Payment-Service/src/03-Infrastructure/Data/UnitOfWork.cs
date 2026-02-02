using Payment_Service.src._01_Domain.Core.Interfaces.Repositories;
using Payment_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Payment_Service.src._03_Infrastructure.Repositories;

namespace Payment_Service.src._03_Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IPaymentRepository _paymentRepository;
        private IRefundRepository _refundRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IPaymentRepository Payments
        {
            get
            {
                if (_paymentRepository == null)
                    _paymentRepository = new PaymentRepository(_context);
                return _paymentRepository;
            }
        }

        public IRefundRepository Refunds
        {
            get
            {
                if (_refundRepository == null)
                    _refundRepository = new RefundRepository(_context);
                return _refundRepository;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
