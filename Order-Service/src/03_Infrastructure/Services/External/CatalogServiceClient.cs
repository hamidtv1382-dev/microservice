using System.Net.Http.Json;
using Polly;
using Polly.Retry;

namespace Order_Service.src._03_Infrastructure.Services.External
{
    public class CatalogServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
        private readonly ILogger<CatalogServiceClient> _logger;

        public CatalogServiceClient(HttpClient httpClient, ILogger<CatalogServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _retryPolicy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (outcome, timeSpan, retryCount, context) =>
                    {
                        _logger.LogWarning("Retry {Count} for Catalog Service after {Delay}s due to: {Error}",
                            retryCount, timeSpan.TotalSeconds, outcome.Exception?.Message);
                    });
        }

        public async Task<CatalogProductDto?> GetProductByIdAsync(int productId)
        {
            var response = await _retryPolicy.ExecuteAsync(() =>
                _httpClient.GetAsync($"/api/public/products/{productId}"));

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Catalog Service returned {StatusCode} for ProductId {ProductId}",
                    response.StatusCode, productId);
                return null;
            }

            var product = await response.Content.ReadFromJsonAsync<CatalogServiceProductResponse>();

            if (product == null) return null;

            return new CatalogProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                SellerId = product.CreatedByUserId,
                CategoryId = product.CategoryId
            };
        }

        public async Task<bool> ValidateProductAsync(int productId)
        {
            var response = await _retryPolicy.ExecuteAsync(() =>
                _httpClient.GetAsync($"/api/public/products/{productId}"));

            return response.IsSuccessStatusCode;
        }

        public class CatalogServiceProductResponse
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string? ImageUrl { get; set; }
            public string Sku { get; set; }
            public string CreatedByUserId { get; set; }
            public int CategoryId { get; set; }
        }

        public class CatalogProductDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string? ImageUrl { get; set; }
            public string SellerId { get; set; }
            public int CategoryId { get; set; }
        }
    }
}