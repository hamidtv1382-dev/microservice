namespace Order_Service.src._01_Domain.Services.Interfaces
{
    public interface INotificationService
    {
        Task SendOrderCreatedNotificationAsync(Guid userId, Guid orderId, string userEmail);
        Task SendOrderStatusChangedNotificationAsync(Guid userId, Guid orderId, string status, string userEmail);
        Task SendPaymentFailedNotificationAsync(Guid userId, Guid orderId, string reason, string userEmail);
    }
}
