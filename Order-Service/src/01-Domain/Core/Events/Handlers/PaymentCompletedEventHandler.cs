namespace Order_Service.src._01_Domain.Core.Events.Handlers
{
    public class PaymentCompletedEventHandler : IDomainEventHandler<PaymentCompletedEvent>
    {
        public Task Handle(PaymentCompletedEvent domainEvent)
        {
            // Logic to handle payment completed event (e.g., Update Order Status, Notify Finance, Trigger Fulfillment)
            return Task.CompletedTask;
        }
    }
}
