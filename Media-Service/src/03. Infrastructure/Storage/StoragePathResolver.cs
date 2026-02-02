using Media_Service.src._03._Infrastructure.Caching;

namespace Media_Service.src._03._Infrastructure.Storage
{
    public class StoragePathResolver : IStoragePathResolver
    {
        private readonly IConfiguration _configuration;
        private readonly ICacheService _cacheService;

        public StoragePathResolver(IConfiguration configuration, ICacheService cacheService)
        {
            _configuration = configuration;
            _cacheService = cacheService;
        }

        public async Task<string> GetRootPathAsync()
        {
            const string cacheKey = "MediaService:RootPath";

            var cachedPath = await _cacheService.GetAsync<string>(cacheKey);
            if (!string.IsNullOrEmpty(cachedPath))
            {
                return cachedPath;
            }

            var rootPath = _configuration["Storage:RootPath"] ?? "wwwroot/media";

            await _cacheService.SetAsync(cacheKey, rootPath, TimeSpan.FromHours(1));

            return rootPath;
        }
    }
}
