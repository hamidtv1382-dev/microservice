using Finance_Service.src._01_Domain.Services.Interfaces;

namespace Finance_Service.src._03_Infrastructure.Services.Internal
{
    public class LoggingService : ILoggingService
    {
        public void LogInformation(string message)
        {
            Console.WriteLine($"[INFRASTRUCTURE-LOG]: {message}");
        }

        public void LogWarning(string message)
        {
            Console.WriteLine($"[INFRASTRUCTURE-WARN]: {message}");
        }

        public void LogError(string message, System.Exception? exception = null)
        {
            Console.WriteLine($"[INFRASTRUCTURE-ERR]: {message}");
            if (exception != null) Console.WriteLine(exception.StackTrace);
        }
    }
}
