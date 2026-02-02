using Payment_Service.src._01_Domain.Services.Interfaces;

namespace Payment_Service.src._01_Domain.Core.Events.Handlers
{
    public class RefundProcessedEventHandler : IDomainEventHandler<RefundProcessedEvent>
    {
        private readonly ILoggingService _loggingService;

        public RefundProcessedEventHandler(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public Task Handle(RefundProcessedEvent domainEvent)
        {
            _loggingService.LogInformation($"Refund Processed Event: RefundId={domainEvent.RefundId}, PaymentId={domainEvent.PaymentId}");

            return Task.CompletedTask;
        }
    }
}
