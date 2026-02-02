namespace Finance_Service.src._03_Infrastructure.Services.Internal.Resilience
{
    public class CircuitBreakerPolicyHandler
    {
        private Exception _lastException;
        private DateTime _lastFailureTime;
        private int _failureCount;
        private readonly int _failureThreshold;
        private readonly TimeSpan _durationOfBreak;

        public CircuitBreakerPolicyHandler(int failureThreshold = 5, int durationOfBreakSeconds = 60)
        {
            _failureThreshold = failureThreshold;
            _durationOfBreak = TimeSpan.FromSeconds(durationOfBreakSeconds);
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
        {
            if (IsOpen())
            {
                throw new InvalidOperationException("Circuit breaker is currently OPEN. Calls are blocked.");
            }

            try
            {
                var result = await action();
                Reset();
                return result;
            }
            catch (Exception ex)
            {
                RecordFailure(ex);
                throw;
            }
        }

        private bool IsOpen()
        {
            if (_failureCount >= _failureThreshold)
            {
                if (DateTime.UtcNow - _lastFailureTime < _durationOfBreak)
                {
                    return true;
                }
                Reset();
            }
            return false;
        }

        private void RecordFailure(Exception ex)
        {
            _lastException = ex;
            _lastFailureTime = DateTime.UtcNow;
            _failureCount++;
        }

        private void Reset()
        {
            _failureCount = 0;
            _lastException = null;
        }
    }
}
