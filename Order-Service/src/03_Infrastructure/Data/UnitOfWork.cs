using Order_Service.src._01_Domain.Core.Interfaces.Repositories;
using Order_Service.src._01_Domain.Core.Interfaces.UnitOfWork;

namespace Order_Service.src._03_Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private bool _disposed;

        public IOrderRepository Orders { get; }
        public IBasketRepository Baskets { get; }
        public IDiscountRepository Discounts { get; }
        public IPaymentRepository Payments { get; }
        public IRefundRepository Refunds { get; }

        public UnitOfWork(AppDbContext context,
                          IOrderRepository orderRepository,
                          IBasketRepository basketRepository,
                          IDiscountRepository discountRepository,
                          IPaymentRepository paymentRepository,
                          IRefundRepository refundRepository)
        {
            _context = context;
            Orders = orderRepository;
            Baskets = basketRepository;
            Discounts = discountRepository;
            Payments = paymentRepository;
            Refunds = refundRepository;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
