using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketHub.Gateway.Controllers.Catalog_Service.Admin
{
    [ApiController]
    [Route("api/admin/productattributes")]
    [Authorize]
    public class AdminProductAttributeController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AdminProductAttributeController> _logger;
        private const string CatalogServiceBaseUrl = "https://localhost:7070";

        public AdminProductAttributeController(IHttpClientFactory httpClientFactory, ILogger<AdminProductAttributeController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        private void AddAuthorizationHeader(HttpClient client)
        {
            var authHeader = Request.Headers.Authorization.FirstOrDefault();
            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        private async Task<IActionResult> ForwardRequest(Func<Task<HttpResponseMessage>> requestAction, string operationName)
        {
            try
            {
                var response = await requestAction();
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("{OperationName} failed: {Error}", operationName, content);
                    return StatusCode((int)response.StatusCode, content);
                }

                return Content(content, response.Content.Headers.ContentType?.ToString() ?? "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during {OperationName}", operationName);
                return StatusCode(500, $"An error occurred during {operationName}: {ex.Message}");
            }
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetProductAttributes(int productId)
        {
            return await ForwardRequest(
                () =>
                {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.GetAsync($"{CatalogServiceBaseUrl}/api/admin/AdminProductAttribute/product/{productId}");
                },
                "Get product attributes"
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductAttribute(int id)
        {
            return await ForwardRequest(
                () =>
                {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.GetAsync($"{CatalogServiceBaseUrl}/api/admin/AdminProductAttribute/{id}");
                },
                "Get product attribute by ID"
            );
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductAttribute()
        {
            return await ForwardRequest(
                async () =>
                {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    var body = await Request.ReadFromJsonAsync<object>();
                    return await client.PostAsJsonAsync($"{CatalogServiceBaseUrl}/api/admin/AdminProductAttribute", body);
                },
                "Create product attribute"
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductAttribute(int id)
        {
            return await ForwardRequest(
                async () =>
                {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    var body = await Request.ReadFromJsonAsync<object>();
                    return await client.PutAsJsonAsync($"{CatalogServiceBaseUrl}/api/admin/AdminProductAttribute/{id}", body);
                },
                "Update product attribute"
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAttribute(int id)
        {
            return await ForwardRequest(
                () =>
                {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.DeleteAsync($"{CatalogServiceBaseUrl}/api/admin/AdminProductAttribute/{id}");
                },
                "Delete product attribute"
            );
        }
    }
}
