using Seller_Finance_Service.src._01_Domain.Core.ValueObjects;

namespace Seller_Finance_Service.src._02_Application.Interfaces
{
    public interface IFinanceService
    {
        // This interface connects to the Global Finance Service to move money to seller's bank
        Task<bool> RequestSettlementCreationAsync(Guid sellerId, BankAccountInfo bankInfo, Money amount);
    }
}
