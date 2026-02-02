using Payment_Service.src._01_Domain.Services.Interfaces;

namespace Payment_Service.src._01_Domain.Core.Events.Handlers
{

    public class PaymentCompletedEventHandler : IDomainEventHandler<PaymentCompletedEvent>
    {
        private readonly ILoggingService _loggingService;

        public PaymentCompletedEventHandler(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public Task Handle(PaymentCompletedEvent domainEvent)
        {
            _loggingService.LogInformation($"Payment Completed Event: PaymentId={domainEvent.PaymentId}, OrderId={domainEvent.OrderId}");

            return Task.CompletedTask;
        }
    }
}
