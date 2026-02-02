using Payment_Service.src._01_Domain.Core.Interfaces.Repositories;

namespace Payment_Service.src._01_Domain.Core.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IPaymentRepository Payments { get; }
        IRefundRepository Refunds { get; }

        Task<int> SaveChangesAsync();
    }
}
