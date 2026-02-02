using Order_Service.src._01_Domain.Core.Entities;
using Order_Service.src._01_Domain.Core.ValueObjects;

namespace Order_Service.src._01_Domain.Core.Events
{
    public class OrderCancelledEvent : IDomainEvent
    {
        public Guid OrderId { get; }
        public Guid BuyerId { get; }
        public string Reason { get; }
        public DateTime OccurredOn { get; }

        public OrderCancelledEvent(Guid orderId, Guid buyerId, string reason)
        {
            OrderId = orderId;
            BuyerId = buyerId;
            Reason = reason;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
