namespace Seller_Finance_Service.src._01_Domain.Services.Interfaces
{
    public interface ILoggingService
    {
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(string message, System.Exception? exception = null);
    }
}
