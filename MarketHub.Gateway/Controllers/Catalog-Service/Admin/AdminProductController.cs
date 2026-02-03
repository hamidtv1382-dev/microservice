using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MarketHub.Gateway.Controllers.Catalog_Service.Admin
{
    [ApiController]
    [Route("api/admin/products")] // مسیر در Gateway
    [Authorize]
    public class AdminProductController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AdminProductController> _logger;
        private const string CatalogServiceBaseUrl = "https://localhost:7070";

        public AdminProductController(IHttpClientFactory httpClientFactory, ILogger<AdminProductController> logger)
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
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        private async Task<IActionResult> ForwardRequest(Func<Task<HttpResponseMessage>> requestAction, string operationName)
        {
            try
            {
                var response = await requestAction();
                var contentString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("{OperationName} failed: {Error}", operationName, contentString);
                    return StatusCode((int)response.StatusCode, new { Message = $"{operationName} failed.", Details = contentString });
                }

                // اگر محتوا خالی است
                if (string.IsNullOrWhiteSpace(contentString))
                    return StatusCode((int)response.StatusCode, new { });

                // اگر محتوا JSON نیست، همان متن را برگردان
                try
                {
                    var json = JsonSerializer.Deserialize<object>(contentString);
                    return StatusCode((int)response.StatusCode, json);
                }
                catch
                {
                    // متن ساده را بدون Deserialize برمی‌گردانیم
                    return StatusCode((int)response.StatusCode, contentString);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during {OperationName}", operationName);
                return StatusCode(500, new { Message = $"An error occurred during {operationName}." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var queryString = Request.QueryString.ToUriComponent();
            return await ForwardRequest(
                () =>
                {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    // اصلاح مسیر: از AdminProduct استفاده می‌کنیم
                    return client.GetAsync($"{CatalogServiceBaseUrl}/api/admin/AdminProduct{queryString}");
                },
                "Get all admin products"
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            return await ForwardRequest(
                () =>
                {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    // اصلاح مسیر
                    return client.GetAsync($"{CatalogServiceBaseUrl}/api/admin/AdminProduct/{id}");
                },
                "Get admin product by ID"
            );
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] object request)
        {
            return await ForwardRequest(
                () =>
                {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    // اصلاح مسیر
                    return client.PostAsJsonAsync($"{CatalogServiceBaseUrl}/api/admin/AdminProduct", request);
                },
                "Create product"
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] object request)
        {
            return await ForwardRequest(
                () =>
                {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    // اصلاح مسیر
                    return client.PutAsJsonAsync($"{CatalogServiceBaseUrl}/api/admin/AdminProduct/{id}", request);
                },
                "Update product"
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            return await ForwardRequest(
                () =>
                {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    // اصلاح مسیر
                    return client.DeleteAsync($"{CatalogServiceBaseUrl}/api/admin/AdminProduct/{id}");
                },
                "Delete product"
            );
        }

        [HttpPost("{id}/publish")]
        public async Task<IActionResult> PublishProduct(int id)
        {
            return await ForwardRequest(
                () =>
                {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    // اصلاح مسیر
                    return client.PostAsync($"{CatalogServiceBaseUrl}/api/admin/AdminProduct/{id}/publish", null);
                },
                "Publish product"
            );
        }

        [HttpPost("{id}/unpublish")]
        public async Task<IActionResult> UnpublishProduct(int id)
        {
            return await ForwardRequest(
                () =>
                {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    // اصلاح مسیر
                    return client.PostAsync($"{CatalogServiceBaseUrl}/api/admin/AdminProduct/{id}/unpublish", null);
                },
                "Unpublish product"
            );
        }

        [HttpPost("{id}/archive")]
        public async Task<IActionResult> ArchiveProduct(int id)
        {
            return await ForwardRequest(
                () =>
                {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    // اصلاح مسیر
                    return client.PostAsync($"{CatalogServiceBaseUrl}/api/admin/AdminProduct/{id}/archive", null);
                },
                "Archive product"
            );
        }

        [HttpPost("{id}/feature")]
        public async Task<IActionResult> SetAsFeatured(int id)
        {
            return await ForwardRequest(
                () =>
                {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    // اصلاح مسیر
                    return client.PostAsync($"{CatalogServiceBaseUrl}/api/admin/AdminProduct/{id}/feature", null);
                },
                "Set product as featured"
            );
        }

        [HttpDelete("{id}/feature")]
        public async Task<IActionResult> RemoveFromFeatured(int id)
        {
            return await ForwardRequest(
                () =>
                {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    // اصلاح مسیر
                    return client.DeleteAsync($"{CatalogServiceBaseUrl}/api/admin/AdminProduct/{id}/feature");
                },
                "Remove product from featured"
            );
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllForAdmin()
        {
            return await ForwardRequest(
                () =>
                {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.GetAsync($"{CatalogServiceBaseUrl}/api/admin/AdminProduct/all");
                },
                "Get all products for admin"
            );
        }

        [HttpPatch("{id}/approve")]
        public async Task<IActionResult> ApproveProduct(int id)
        {
            return await ForwardRequest(
                () =>
                {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.PatchAsync($"{CatalogServiceBaseUrl}/api/admin/AdminProduct/{id}/approve", null);
                },
                "Approve product"
            );
        }

        [HttpPatch("{id}/reject")]
        public async Task<IActionResult> RejectProduct(int id)
        {
            return await ForwardRequest(
                () =>
                {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.PatchAsync($"{CatalogServiceBaseUrl}/api/admin/AdminProduct/{id}/reject", null);
                },
                "Reject product"
            );
        }

        [HttpPatch("{id}/slug")]
        public async Task<IActionResult> UpdateSlug(int id, [FromBody] object request)
        {
            return await ForwardRequest(
                () =>
                {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.PatchAsJsonAsync($"{CatalogServiceBaseUrl}/api/admin/AdminProduct/{id}/slug", request);
                },
                "Update product slug"
            );
        }
    }
}
