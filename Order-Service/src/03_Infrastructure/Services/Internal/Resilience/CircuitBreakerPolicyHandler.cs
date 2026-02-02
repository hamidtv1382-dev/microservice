using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;

namespace Order_Service.src._03_Infrastructure.Services.Internal.Resilience
{
    public static class CircuitBreakerPolicyHandler
    {
        public static AsyncCircuitBreakerPolicy GetCircuitBreakerPolicy(ILogger logger, int exceptionsAllowedBeforeBreaking = 5, int durationOfBreakSeconds = 30)
        {
            return Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: exceptionsAllowedBeforeBreaking,
                    durationOfBreak: TimeSpan.FromSeconds(durationOfBreakSeconds),
                    onBreak: (exception, breakDelay) =>
                    {
                        logger.LogError("Circuit broken due to: {Message}. Delay: {Delay}s", exception.Message, breakDelay.TotalSeconds);
                    },
                    onReset: () =>
                    {
                        logger.LogInformation("Circuit reset.");
                    });
        }
    }
}