using Payment_Service.src._01_Domain.Core.Entities;

namespace Payment_Service.src._01_Domain.Core.Events
{
    public class RefundProcessedEvent : IDomainEvent
    {
        public Guid RefundId { get; }
        public Guid PaymentId { get; }
        public decimal Amount { get; }
        public DateTime OccurredOn { get; }

        public RefundProcessedEvent(Refund refund)
        {
            RefundId = refund.Id;
            PaymentId = refund.PaymentId;
            Amount = refund.Amount.Amount;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
