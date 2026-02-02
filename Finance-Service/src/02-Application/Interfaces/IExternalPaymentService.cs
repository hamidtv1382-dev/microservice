using Finance_Service.src._01_Domain.Core.ValueObjects;

namespace Finance_Service.src._02_Application.Interfaces
{
    public interface IExternalPaymentService
    {
        Task<bool> TransferToBankAccountAsync(Money amount, string accountInfo);
    }
}
