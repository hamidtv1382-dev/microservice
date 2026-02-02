using Order_Service.src._01_Domain.Core.Common;
using Order_Service.src._01_Domain.Core.Enums;
using Order_Service.src._01_Domain.Core.ValueObjects;

namespace Order_Service.src._01_Domain.Core.Entities
{
    public class Refund : BaseEntity
    {
        public Guid Id { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid PaymentId { get; private set; }
        public Money Amount { get; private set; }
        public string Reason { get; private set; }
        public PaymentStatus Status { get; private set; }
        public DateTime? ProcessedAt { get; private set; }
        public string? TransactionId { get; private set; }
        public string? FailureReason { get; private set; }

        protected Refund() { }

        public Refund(Guid id, Guid orderId, Guid paymentId, Money amount, string reason)
        {
            Id = id;
            OrderId = orderId;
            PaymentId = paymentId;
            Amount = amount;
            Reason = reason;
            Status = PaymentStatus.Pending;
        }

        public void Process(string transactionId)
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Refund is not in Pending status.");

            Status = PaymentStatus.Completed;
            TransactionId = transactionId;
            ProcessedAt = DateTime.UtcNow;
            UpdateTimestamp();
        }

        public void Fail(string reason)
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Refund is not in Pending status.");

            Status = PaymentStatus.Failed;
            FailureReason = reason;
            UpdateTimestamp();
        }
    }
}
