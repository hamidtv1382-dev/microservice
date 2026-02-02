using Payment_Service.src._01_Domain.Core.Common;
using Payment_Service.src._01_Domain.Core.Enums;
using Payment_Service.src._01_Domain.Core.ValueObjects;

namespace Payment_Service.src._01_Domain.Core.Entities
{
    public class Transaction : BaseEntity
    {
        public Guid PaymentId { get; private set; }
        public Money Amount { get; private set; }
        public TransactionType Type { get; private set; }
        public TransactionNumber TransactionNumber { get; private set; }
        public string? GatewayResponse { get; private set; }
        public DateTime ProcessedAt { get; private set; }
        public AuditInfo AuditInfo { get; private set; }

        private Transaction() { }

        public Transaction(Guid paymentId, Money amount, TransactionType type, TransactionNumber transactionNumber, string? createdBy)
        {
            Id = Guid.NewGuid();
            PaymentId = paymentId;
            Amount = amount;
            Type = type;
            TransactionNumber = transactionNumber;
            ProcessedAt = DateTime.UtcNow;
            AuditInfo = new AuditInfo(createdBy);
        }
    }
}
