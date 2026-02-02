namespace Order_Service.src._01_Domain.Core.Events.Handlers
{
    public class OrderCancelledEventHandler : IDomainEventHandler<OrderCancelledEvent>
    {
        public Task Handle(OrderCancelledEvent domainEvent)
        {
            // Logic to handle order cancelled event (e.g., Restock Inventory, Send Notification, Release Discount)
            return Task.CompletedTask;
        }
    }
}
