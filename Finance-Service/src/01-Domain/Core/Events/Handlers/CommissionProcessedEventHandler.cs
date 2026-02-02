using Finance_Service.src._01_Domain.Services.Interfaces;

namespace Finance_Service.src._01_Domain.Core.Events.Handlers
{
    public class CommissionProcessedEventHandler : IDomainEventHandler<CommissionProcessedEvent>
    {
        private readonly ILoggingService _loggingService;

        public CommissionProcessedEventHandler(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public Task Handle(CommissionProcessedEvent domainEvent)
        {
            _loggingService.LogInformation($"Commission Processed Event: CommissionId={domainEvent.CommissionId}, OrderId={domainEvent.OrderId}, SellerId={domainEvent.SellerId}");
            return Task.CompletedTask;
        }
    }
}
