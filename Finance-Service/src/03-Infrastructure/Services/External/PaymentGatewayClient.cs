using Finance_Service.src._01_Domain.Core.ValueObjects;
using Finance_Service.src._02_Application.Interfaces;

namespace Finance_Service.src._03_Infrastructure.Services.External
{
    public class PaymentGatewayClient : IExternalPaymentService
    {
        public async Task<bool> TransferToBankAccountAsync(Money amount, string accountInfo)
        {
            // Mock implementation for bank transfer
            await Task.Delay(100);

            // Simulate success
            if (string.IsNullOrWhiteSpace(accountInfo)) return false;

            Console.WriteLine($"Transferring {amount.Amount} {amount.Currency} to account {accountInfo}");

            return true;
        }
    }
}
