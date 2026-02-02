using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketHub.Gateway.Controllers.Order_Service
{
    [ApiController]
    [Route("api/admin/discounts")]
    [Authorize]
    public class AdminDiscountsController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AdminDiscountsController> _logger;
        private const string OrderServiceBaseUrl = "https://localhost:7226";

        public AdminDiscountsController(IHttpClientFactory httpClientFactory, ILogger<AdminDiscountsController> logger)
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
        public async Task<IActionResult> GetAllDiscounts()
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.GetAsync($"{OrderServiceBaseUrl}/api/admin/AdminDiscounts");
                },
                "Get all discounts"
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDiscountById(Guid id)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.GetAsync($"{OrderServiceBaseUrl}/api/admin/AdminDiscounts/{id}");
                },
                "Get discount by ID"
            );
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiscount([FromBody] object request)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.PostAsJsonAsync($"{OrderServiceBaseUrl}/api/admin/AdminDiscounts", request);
                },
                "Create discount"
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDiscount(Guid id, [FromBody] object request)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.PutAsJsonAsync($"{OrderServiceBaseUrl}/api/admin/AdminDiscounts/{id}", request);
                },
                "Update discount"
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiscount(Guid id)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.DeleteAsync($"{OrderServiceBaseUrl}/api/admin/AdminDiscounts/{id}");
                },
                "Delete discount"
            );
        }
    }
}
