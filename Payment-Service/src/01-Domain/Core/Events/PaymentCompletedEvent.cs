using Payment_Service.src._01_Domain.Core.Entities;

namespace Payment_Service.src._01_Domain.Core.Events
{
    public class PaymentCompletedEvent : IDomainEvent
    {
        public Guid PaymentId { get; }
        public Guid OrderId { get; }
        public decimal Amount { get; }
        public DateTime OccurredOn { get; }

        public PaymentCompletedEvent(Payment payment)
        {
            PaymentId = payment.Id;
            OrderId = payment.OrderId;
            Amount = payment.Amount.Amount;
            OccurredOn = DateTime.UtcNow;
        }
    }

    public interface IDomainEvent
    {
    }
}
