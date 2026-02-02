using Order_Service.src._01_Domain.Core.Interfaces.Repositories;

namespace Order_Service.src._01_Domain.Core.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IOrderRepository Orders { get; }
        IBasketRepository Baskets { get; }
        IDiscountRepository Discounts { get; }
        IPaymentRepository Payments { get; }
        IRefundRepository Refunds { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();
    }
}
