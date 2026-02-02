using Microsoft.AspNetCore.Http.HttpResults;
using Order_Service.src._01_Domain.Core.Common;
using Order_Service.src._01_Domain.Core.Enums;
using Order_Service.src._01_Domain.Core.ValueObjects;

namespace Order_Service.src._01_Domain.Core.Entities
{
    public class Payment : BaseEntity
    {
        public Guid Id { get; private set; }
        public Guid OrderId { get; private set; }
        public Money Amount { get; private set; }
        public PaymentStatus Status { get; private set; }
        public string PaymentGateway { get; private set; }
        public string? TransactionId { get; private set; }
        public DateTime? PaidAt { get; private set; }
        public string? FailureReason { get; private set; }
        public string? Payload { get; private set; }

        protected Payment() { }

        public Payment(Guid id, Guid orderId, Money amount, string paymentGateway)
        {
            Id = id;
            OrderId = orderId;
            Amount = amount;
            PaymentGateway = paymentGateway;
            Status = PaymentStatus.Pending;
            CreatedAt = DateTime.UtcNow;
        }

        public void Complete(string transactionId, string? payload = null)
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Payment is not in Pending status.");

            Status = PaymentStatus.Completed;
            TransactionId = transactionId;
            PaidAt = DateTime.UtcNow;
            Payload = payload;
            UpdateTimestamp();
        }

        public void Fail(string reason)
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Payment is not in Pending status.");

            Status = PaymentStatus.Failed;
            FailureReason = reason;
            UpdateTimestamp();
        }

        public void Cancel()
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Payment is not in Pending status.");

            Status = PaymentStatus.Cancelled;
            UpdateTimestamp();
        }
    }
}
