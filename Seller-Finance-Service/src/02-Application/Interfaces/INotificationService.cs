namespace Seller_Finance_Service.src._02_Application.Interfaces
{
    public interface INotificationService
    {
        Task SendPayoutSuccessEmailAsync(Guid sellerId, decimal amount);
        Task SendPayoutFailureEmailAsync(Guid sellerId, string reason);
    }
}
