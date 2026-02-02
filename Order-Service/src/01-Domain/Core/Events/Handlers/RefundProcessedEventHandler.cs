namespace Order_Service.src._01_Domain.Core.Events.Handlers
{
    public class RefundProcessedEventHandler : IDomainEventHandler<RefundProcessedEvent>
    {
        public Task Handle(RefundProcessedEvent domainEvent)
        {
            // Logic to handle refund processed event (e.g., Notify User, Update Financial Records, Update Order Status)
            return Task.CompletedTask;
        }
    }
}
