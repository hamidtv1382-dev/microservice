using Seller_Finance_Service.src._01_Domain.Services.Interfaces;

namespace Seller_Finance_Service.src._01_Domain.Services.Implementations
{
    public class LoggingService : ILoggingService
    {
        public void LogInformation(string message)
        {
            Console.WriteLine($"[INFO] {DateTime.UtcNow}: {message}");
        }

        public void LogWarning(string message)
        {
            Console.WriteLine($"[WARN] {DateTime.UtcNow}: {message}");
        }

        public void LogError(string message, System.Exception? exception = null)
        {
            Console.WriteLine($"[ERROR] {DateTime.UtcNow}: {message}");
            if (exception != null)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
