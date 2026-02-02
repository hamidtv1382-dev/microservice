using Order_Service.src._01_Domain.Core.Entities;
using Order_Service.src._01_Domain.Core.ValueObjects;

namespace Order_Service.src._01_Domain.Core.Events
{
    public class OrderCreatedEvent : IDomainEvent
    {
        public Guid OrderId { get; }
        public Guid BuyerId { get; }
        public DateTime OccurredOn { get; }

        public OrderCreatedEvent(Guid orderId, Guid buyerId)
        {
            OrderId = orderId;
            BuyerId = buyerId;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
