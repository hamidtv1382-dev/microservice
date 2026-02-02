namespace Seller_Finance_Service.src._02_Application.Interfaces
{
    public interface IWalletService
    {
        Task<bool> AddFundsAsync(Guid walletId, decimal amount);
        Task<bool> DeductFundsAsync(Guid walletId, decimal amount);
    }
}
