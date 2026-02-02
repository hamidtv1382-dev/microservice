using Wallet_Service.src._01_Domain.Core.Common;
using Wallet_Service.src._01_Domain.Core.Enums;
using Wallet_Service.src._01_Domain.Core.ValueObjects;

namespace Wallet_Service.src._01_Domain.Core.Entities
{
    public class Wallet : BaseEntity
    {
        public Guid OwnerId { get; private set; }
        public Money Balance { get; private set; }
        public bool IsActive { get; private set; }
        public AuditInfo AuditInfo { get; private set; }

        private readonly List<WalletTransaction> _transactions = new List<WalletTransaction>();
        public IReadOnlyCollection<WalletTransaction> Transactions => _transactions.AsReadOnly();

        private Wallet() { }

        public Wallet(Guid ownerId, Money initialBalance, string createdBy)
        {
            Id = Guid.NewGuid();
            OwnerId = ownerId;
            Balance = initialBalance;
            IsActive = true;
            AuditInfo = new AuditInfo(createdBy);
        }

        public void Credit(Money amount, string referenceId)
        {
            if (!IsActive) throw new InvalidOperationException("Wallet is not active.");
            Balance = Balance.Add(amount);
            SetUpdatedAt();

            var transaction = new WalletTransaction(Id, TransactionType.Credit, amount, referenceId, "Wallet Credit", "System");
            _transactions.Add(transaction);
        }

        public void Debit(Money amount, string referenceId, string description)
        {
            if (!IsActive) throw new InvalidOperationException("Wallet is not active.");
            if (Balance.Amount < amount.Amount) throw new InvalidOperationException("Insufficient balance.");

            Balance = Balance.Subtract(amount);
            SetUpdatedAt();

            var transaction = new WalletTransaction(Id, TransactionType.Debit, amount, referenceId, description, "System");
            _transactions.Add(transaction);
        }

        public void SetActiveStatus(bool status)
        {
            IsActive = status;
            SetUpdatedAt();
        }
    }
}
