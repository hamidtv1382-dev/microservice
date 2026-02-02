using Wallet_Service.src._01_Domain.Core.Entities;
using Wallet_Service.src._01_Domain.Core.Enums;
using Wallet_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Wallet_Service.src._01_Domain.Core.ValueObjects;
using Wallet_Service.src._01_Domain.Services.Interfaces;

namespace Wallet_Service.src._01_Domain.Services.Implementations
{
    public class WalletDomainService : IWalletDomainService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WalletDomainService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Wallet> CreateWalletAsync(Guid ownerId)
        {
            var wallet = new Wallet(ownerId, new Money(0, "IRR"), "System");
            await _unitOfWork.Wallets.AddAsync(wallet);
            await _unitOfWork.SaveChangesAsync();
            return wallet;
        }

        public async Task<bool> CreditWalletAsync(Guid walletId, Money amount, string referenceId)
        {
            var wallet = await _unitOfWork.Wallets.GetByIdAsync(walletId);
            if (wallet == null) return false;

            wallet.Credit(amount, referenceId);

            var transaction = new WalletTransaction(
                walletId: wallet.Id,
                type: TransactionType.Credit,
                amount: amount,
                referenceId: referenceId,
                description: "افزایش موجودی کیف پول",
                createdBy: "System"
            );

            await _unitOfWork.WalletTransactions.AddAsync(transaction);
            await _unitOfWork.Wallets.UpdateAsync(wallet);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DebitWalletAsync(Guid walletId, Money amount, string referenceId, string description)
        {
            var wallet = await _unitOfWork.Wallets.GetByIdAsync(walletId);
            if (wallet == null) return false;

            wallet.Debit(amount, referenceId, description);

            var transaction = new WalletTransaction(
                walletId: wallet.Id,
                type: TransactionType.Debit,
                amount: amount,
                referenceId: referenceId,
                description: description ?? "برداشت از موجودی",
                createdBy: "System"
            );

            await _unitOfWork.WalletTransactions.AddAsync(transaction);
            await _unitOfWork.Wallets.UpdateAsync(wallet);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
