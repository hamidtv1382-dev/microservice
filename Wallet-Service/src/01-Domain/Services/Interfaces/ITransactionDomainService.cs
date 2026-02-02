using System;
using Wallet_Service.src._01_Domain.Core.Entities;
using Wallet_Service.src._01_Domain.Core.Enums;
using Wallet_Service.src._01_Domain.Core.ValueObjects;

namespace Wallet_Service.src._01_Domain.Services.Interfaces
{
    public interface ITransactionDomainService
    {
        Task<WalletTransaction> RecordTransactionAsync(Guid walletId, TransactionType type, Money amount, string referenceId, string description);
    }
}
