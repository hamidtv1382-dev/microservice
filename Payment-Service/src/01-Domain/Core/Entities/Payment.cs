using Payment_Service.src._01_Domain.Core.Common;
using Payment_Service.src._01_Domain.Core.Enums;
using Payment_Service.src._01_Domain.Core.ValueObjects;

namespace Payment_Service.src._01_Domain.Core.Entities
{
    public class Payment : BaseEntity
    {
        public Guid OrderId { get; private set; }
        public Money Amount { get; private set; }
        public PaymentMethod Method { get; private set; }
        public PaymentStatus Status { get; private set; }
        public TransactionNumber TransactionNumber { get; private set; }
        public string? ExternalTransactionId { get; private set; }
        public string? FailureReason { get; private set; }
        public DateTime? PaidAt { get; private set; }
        public AuditInfo AuditInfo { get; private set; }

        private Payment() { }

        public Payment(Guid orderId, Money amount, PaymentMethod method, TransactionNumber transactionNumber, string? createdBy)
        {
            Id = Guid.NewGuid();
            OrderId = orderId;
            Amount = amount;
            Method = method;
            TransactionNumber = transactionNumber;
            Status = PaymentStatus.Pending;
            AuditInfo = new AuditInfo(createdBy);
        }

        public void CompletePayment(string externalTransactionId)
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Payment is not in a valid state to be completed.");

            Status = PaymentStatus.Completed;
            ExternalTransactionId = externalTransactionId;
            PaidAt = DateTime.UtcNow;
            SetUpdatedAt();
        }

        public void FailPayment(string reason)
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Payment is not in a valid state to be failed.");

            Status = PaymentStatus.Failed;
            FailureReason = reason;
            SetUpdatedAt();
        }

        public void CancelPayment()
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Payment is not in a valid state to be cancelled.");

            Status = PaymentStatus.Cancelled;
            SetUpdatedAt();
        }
    }
}
