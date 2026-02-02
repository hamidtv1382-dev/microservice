using System;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace Order_Service.src._03_Infrastructure.Services.Internal.Resilience
{
    public static class RetryPolicyHandler
    {
        public static IAsyncPolicy<T> GetRetryPolicy<T>(ILogger logger, int retryCount = 3)
        {
            return Policy<T>
                .Handle<Exception>() // مشخص کردن نوع Exception که باید Retry شود
                .WaitAndRetryAsync(
                    retryCount: retryCount,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (exception, timeSpan, retryNumber, context) =>
                    {
                        // exception یک DelegateResult<T> است
                        logger.LogWarning(
                            "Retry {RetryNumber} after {TimeSpan}s due to: {Message}",
                            retryNumber,
                            timeSpan.TotalSeconds,
                            exception.Exception.Message);
                    });
        }
    }
}
