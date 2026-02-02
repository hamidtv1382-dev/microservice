using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketHub.Gateway.Controllers.Order_Service
{
    [ApiController]
    [Route("api/baskets")]
    [Authorize]
    public class BasketsController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BasketsController> _logger;
        private const string OrderServiceBaseUrl = "https://localhost:7226";

        public BasketsController(IHttpClientFactory httpClientFactory, ILogger<BasketsController> logger)
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
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("{OperationName} failed: {Error}", operationName, errorContent);
                    return StatusCode((int)response.StatusCode, new { Message = $"{operationName} failed.", Details = errorContent });
                }

                // Handle NoContent specifically
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return NoContent();
                }

                var successResponse = await response.Content.ReadFromJsonAsync<object>();
                return Ok(successResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during {OperationName}", operationName);
                return StatusCode(500, new { Message = $"An error occurred during {operationName}." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.GetAsync($"{OrderServiceBaseUrl}/api/Baskets");
                },
                "Get basket"
            );
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItem([FromBody] object request)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.PostAsJsonAsync($"{OrderServiceBaseUrl}/api/Baskets/items", request);
                },
                "Add item to basket"
            );
        }

        [HttpPut("items")]
        public async Task<IActionResult> UpdateItem([FromBody] object request)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.PutAsJsonAsync($"{OrderServiceBaseUrl}/api/Baskets/items", request);
                },
                "Update item in basket"
            );
        }

        [HttpDelete("items/{productId}")]
        public async Task<IActionResult> RemoveItem(int productId)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.DeleteAsync($"{OrderServiceBaseUrl}/api/Baskets/items/{productId}");
                },
                "Remove item from basket"
            );
        }

        [HttpPost("apply-discount")]
        public async Task<IActionResult> ApplyDiscount([FromBody] object request)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.PostAsJsonAsync($"{OrderServiceBaseUrl}/api/Baskets/apply-discount", request);
                },
                "Apply discount"
            );
        }

        [HttpDelete]
        public async Task<IActionResult> ClearBasket()
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.DeleteAsync($"{OrderServiceBaseUrl}/api/Baskets");
                },
                "Clear basket"
            );
        }
    }
}
