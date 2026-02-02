using Wallet_Service.src._01_Domain.Services.Interfaces;

namespace Wallet_Service.src._01_Domain.Core.Events.Handlers
{
    public class WalletCreditedEventHandler : IDomainEventHandler<WalletCreditedEvent>
    {
        private readonly ILoggingService _loggingService;

        public WalletCreditedEventHandler(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public Task Handle(WalletCreditedEvent domainEvent)
        {
            _loggingService.LogInformation($"Wallet Credited Event: WalletId={domainEvent.WalletId}, Amount={domainEvent.Amount}");
            return Task.CompletedTask;
        }
    }
}
