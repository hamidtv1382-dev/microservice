using Finance_Service.src._01_Domain.Services.Interfaces;

namespace Finance_Service.src._01_Domain.Core.Events.Handlers
{
    public class FeeAppliedEventHandler : IDomainEventHandler<FeeAppliedEvent>
    {
        private readonly ILoggingService _loggingService;

        public FeeAppliedEventHandler(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public Task Handle(FeeAppliedEvent domainEvent)
        {
            _loggingService.LogInformation($"Fee Applied Event: FeeId={domainEvent.FeeId}, OrderId={domainEvent.OrderId}, Amount={domainEvent.Amount}");
            return Task.CompletedTask;
        }
    }
}
