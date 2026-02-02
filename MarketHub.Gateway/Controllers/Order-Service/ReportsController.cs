using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketHub.Gateway.Controllers.Order_Service
{
    [ApiController]
    [Route("api/reports")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ReportsController> _logger;
        private const string OrderServiceBaseUrl = "https://localhost:7226";

        public ReportsController(IHttpClientFactory httpClientFactory, ILogger<ReportsController> logger)
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

        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders([FromQuery] object query)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    // Forward query parameters manually or via HttpRequestMessage if complex
                    return client.GetAsync($"{OrderServiceBaseUrl}/api/Reports/orders?{Request.QueryString}");
                },
                "Get orders report"
            );
        }

        [HttpGet("seller-orders")]
        public async Task<IActionResult> GetSellerOrders([FromQuery] object query)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    // Forward query parameters manually or via HttpRequestMessage if complex
                    return client.GetAsync($"{OrderServiceBaseUrl}/api/Reports/seller-orders?{Request.QueryString}");
                },
                "Get seller orders report"
            );
        }
    }
}
