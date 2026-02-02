using Wallet_Service.src._01_Domain.Core.Entities;

namespace Wallet_Service.src._01_Domain.Core.Interfaces.Repositories
{
    public interface IWalletTransactionRepository
    {
        Task<WalletTransaction?> GetByIdAsync(Guid id);
        Task<IEnumerable<WalletTransaction>> GetByWalletIdAsync(Guid walletId);
        Task AddAsync(WalletTransaction transaction);
        Task UpdateAsync(WalletTransaction transaction);
        Task DeleteAsync(WalletTransaction transaction);
    }
}
