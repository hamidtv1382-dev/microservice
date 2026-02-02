using Wallet_Service.src._01_Domain.Services.Interfaces;

namespace Wallet_Service.src._01_Domain.Core.Events.Handlers
{
    public class WalletDebitedEventHandler : IDomainEventHandler<WalletDebitedEvent>
    {
        private readonly ILoggingService _loggingService;

        public WalletDebitedEventHandler(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public Task Handle(WalletDebitedEvent domainEvent)
        {
            _loggingService.LogInformation($"Wallet Debited Event: WalletId={domainEvent.WalletId}, Amount={domainEvent.Amount}");
            return Task.CompletedTask;
        }
    }
}
