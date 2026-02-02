using Wallet_Service.src._01_Domain.Core.Interfaces.Repositories;

namespace Wallet_Service.src._01_Domain.Core.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IWalletRepository Wallets { get; }
        IWalletTransactionRepository WalletTransactions { get; }

        Task<int> SaveChangesAsync();
    }
}
