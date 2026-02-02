using Seller_Finance_Service.src._01_Domain.Services.Interfaces;

namespace Seller_Finance_Service.src._01_Domain.Core.Events.Handlers
{
    public class SellerEarningRecordedEventHandler : IDomainEventHandler<SellerEarningRecordedEvent>
    {
        private readonly ILoggingService _loggingService;

        public SellerEarningRecordedEventHandler(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public Task Handle(SellerEarningRecordedEvent domainEvent)
        {
            _loggingService.LogInformation($"Seller Earning Recorded: Seller={domainEvent.SellerAccountId}, Order={domainEvent.OrderId}, Amount={domainEvent.Amount}");
            return Task.CompletedTask;
        }
    }
}
