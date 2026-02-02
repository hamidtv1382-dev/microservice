using Seller_Finance_Service.src._01_Domain.Services.Interfaces;

namespace Seller_Finance_Service.src._01_Domain.Core.Events.Handlers
{
    public class SellerBalanceReleasedEventHandler : IDomainEventHandler<SellerBalanceReleasedEvent>
    {
        private readonly ILoggingService _loggingService;

        public SellerBalanceReleasedEventHandler(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public Task Handle(SellerBalanceReleasedEvent domainEvent)
        {
            _loggingService.LogInformation($"Seller Balance Released: Seller={domainEvent.SellerAccountId}, Order={domainEvent.OrderId}");
            return Task.CompletedTask;
        }
    }
}
