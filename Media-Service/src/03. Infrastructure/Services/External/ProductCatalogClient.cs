using Media_Service.src._01_Domain.Services.Interfaces;
using Media_Service.src._02_Application.Interfaces;

namespace Media_Service.src._03._Infrastructure.Services.External
{
    public class ProductCatalogClient : IProductCatalogService
    {
        private readonly HttpClient _httpClient;
        private readonly ILoggingService _logger;

        public ProductCatalogClient(HttpClient httpClient, ILoggingService logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ProductCatalogInfo> GetProductByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/catalog/products/{id}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Failed to fetch product {id}. Status: {response.StatusCode}");
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<ProductCatalogInfo>();
            return result;
        }
    }
}
