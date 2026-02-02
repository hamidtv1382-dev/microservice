using Seller_Finance_Service.src._01_Domain.Services.Interfaces;

namespace Seller_Finance_Service.src._01_Domain.Core.Events.Handlers
{
    public class SellerPayoutRequestedEventHandler : IDomainEventHandler<SellerPayoutRequestedEvent>
    {
        private readonly ILoggingService _loggingService;

        public SellerPayoutRequestedEventHandler(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public Task Handle(SellerPayoutRequestedEvent domainEvent)
        {
            _loggingService.LogInformation($"Seller Payout Requested: Seller={domainEvent.SellerAccountId}, PayoutId={domainEvent.PayoutId}");
            return Task.CompletedTask;
        }
    }
}
