namespace Messaging_Service.src._03_Infrastructure.Caching
{
    public class RedisCacheService : ICacheService
    {
        public async Task<T> GetAsync<T>(string key)
        {
            await Task.CompletedTask;
            return default;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            await Task.CompletedTask;
        }

        public async Task RemoveAsync(string key)
        {
            await Task.CompletedTask;
        }
    }
}
