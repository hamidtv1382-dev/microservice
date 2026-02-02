namespace Seller_Finance_Service.src._03_Infrastructure.Services.Internal.Resilience
{
    public class RetryPolicyHandler
    {
        public static async Task<T> ExecuteAsync<T>(Func<Task<T>> action, int maxRetries = 3, int delayMilliseconds = 200)
        {
            int retryCount = 0;
            while (true)
            {
                try
                {
                    return await action();
                }
                catch (Exception)
                {
                    retryCount++;
                    if (retryCount >= maxRetries) throw;
                    await Task.Delay(delayMilliseconds);
                }
            }
        }
    }
}
