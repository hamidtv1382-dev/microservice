using Media_Service.src._01_Domain.Services.Interfaces;
using Media_Service.src._02_Application.Interfaces;

namespace Media_Service.src._03._Infrastructure.Services.External
{
    public class BrandCatalogClient : IBrandCatalogService
    {
        private readonly HttpClient _httpClient;
        private readonly ILoggingService _logger;

        public BrandCatalogClient(HttpClient httpClient, ILoggingService logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<BrandCatalogInfo> GetBrandByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/catalog/brands/{id}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Failed to fetch brand {id}. Status: {response.StatusCode}");
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<BrandCatalogInfo>();
            return result;
        }
    }
}
