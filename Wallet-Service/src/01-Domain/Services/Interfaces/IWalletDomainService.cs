using Wallet_Service.src._01_Domain.Core.Entities;
using Wallet_Service.src._01_Domain.Core.ValueObjects;

namespace Wallet_Service.src._01_Domain.Services.Interfaces
{
    public interface IWalletDomainService
    {
        Task<Wallet> CreateWalletAsync(Guid ownerId);
        Task<bool> CreditWalletAsync(Guid walletId, Money amount, string referenceId);
        Task<bool> DebitWalletAsync(Guid walletId, Money amount, string referenceId, string description);
    }
}
