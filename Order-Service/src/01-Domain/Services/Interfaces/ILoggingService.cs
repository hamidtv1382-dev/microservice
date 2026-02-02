namespace Order_Service.src._01_Domain.Services.Interfaces
{
    public interface ILoggingService
    {
        void LogInformation(string message, params object[] args);
        void LogWarning(string message, params object[] args);
        void LogError(string message, Exception exception, params object[] args);
    }
}
