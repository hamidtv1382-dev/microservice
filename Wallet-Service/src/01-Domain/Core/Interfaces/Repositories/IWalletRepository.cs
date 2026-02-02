using Wallet_Service.src._01_Domain.Core.Entities;

namespace Wallet_Service.src._01_Domain.Core.Interfaces.Repositories
{
    public interface IWalletRepository
    {
        Task<Wallet?> GetByIdAsync(Guid id);
        Task<Wallet?> GetByOwnerIdAsync(Guid ownerId);
        Task AddAsync(Wallet wallet);
        Task UpdateAsync(Wallet wallet);
        Task DeleteAsync(Wallet wallet);
    }
}
