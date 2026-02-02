namespace Order_Service.src._01_Domain.Core.Events.Handlers
{
    public class OrderCreatedEventHandler : IDomainEventHandler<OrderCreatedEvent>
    {
        public Task Handle(OrderCreatedEvent domainEvent)
        {
            // Logic to handle order created event (e.g., Send Notification to User, Email Seller, Update Statistics)
            // Implementation details depend on infrastructure services (Email, SMS, etc.)
            return Task.CompletedTask;
        }
    }
}
