using Wallet_Service.src._01_Domain.Core.Entities;
using Wallet_Service.src._01_Domain.Core.Enums;
using Wallet_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Wallet_Service.src._01_Domain.Core.ValueObjects;
using Wallet_Service.src._01_Domain.Services.Interfaces;

namespace Wallet_Service.src._01_Domain.Services.Implementations
{
    public class TransactionDomainService : ITransactionDomainService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionDomainService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<WalletTransaction> RecordTransactionAsync(Guid walletId, TransactionType type, Money amount, string referenceId, string description)
        {
            var transaction = new WalletTransaction(walletId, type, amount, referenceId, description, "System");
            await _unitOfWork.WalletTransactions.AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync();
            return transaction;
        }
    }
}
