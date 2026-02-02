using Order_Service.src._01_Domain.Services.Interfaces;

namespace Order_Service.src._01_Domain.Services.Implementations
{
    public class LoggingService : ILoggingService
    {
        private readonly ILogger<LoggingService> _logger;

        public LoggingService(ILogger<LoggingService> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }

        public void LogError(string message, Exception exception, params object[] args)
        {
            _logger.LogError(exception, message, args);
        }
    }
}
