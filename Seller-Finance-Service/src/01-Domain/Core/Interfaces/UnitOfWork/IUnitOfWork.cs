using Seller_Finance_Service.src._01_Domain.Core.Interfaces.Repositories;

namespace Seller_Finance_Service.src._01_Domain.Core.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ISellerAccountRepository SellerAccounts { get; }
        ISellerTransactionRepository SellerTransactions { get; }
        ISellerPayoutRepository SellerPayouts { get; }
        ISellerHoldRepository SellerHolds { get; }

        Task<int> SaveChangesAsync();
    }
}
