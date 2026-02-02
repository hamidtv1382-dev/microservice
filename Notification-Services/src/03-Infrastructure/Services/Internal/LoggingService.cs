namespace Notification_Services.src._03_Infrastructure.Services.Internal
{
    public class LoggingService
    {
        public void LogInformation(string message)
        {
            Console.WriteLine($"[INFO] {DateTime.UtcNow}: {message}");
        }

        public void LogWarning(string message)
        {
            Console.WriteLine($"[WARN] {DateTime.UtcNow}: {message}");
        }

        public void LogError(string message, Exception exception = null)
        {
            Console.WriteLine($"[ERROR] {DateTime.UtcNow}: {message}");
            if (exception != null)
            {
                Console.WriteLine(exception.StackTrace);
            }
        }
    }
}
