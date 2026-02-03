using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketHub.Gateway.Controllers.Seller_Finance_Service
{
    [ApiController]
    [Route("api/seller-finance-service/balances")]
    [Authorize]
    public class SellerBalancesController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<SellerBalancesController> _logger;
        private const string SellerFinanceServiceBaseUrl = "https://localhost:7004";

        public SellerBalancesController(IHttpClientFactory httpClientFactory, ILogger<SellerBalancesController> logger)
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

        [HttpGet("{sellerId}")]
        public async Task<IActionResult> GetBalance(Guid sellerId)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.GetAsync($"{SellerFinanceServiceBaseUrl}/api/SellerBalances/{sellerId}");
                },
                "Get seller balance"
            );
        }

        [HttpPost("earning")]
        public async Task<IActionResult> RecordEarning([FromBody] object request)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.PostAsJsonAsync($"{SellerFinanceServiceBaseUrl}/api/SellerBalances/earning", request);
                },
                "Record earning"
            );
        }

        [HttpPost("release")]
        public async Task<IActionResult> ReleaseBalance([FromBody] object request)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.PostAsJsonAsync($"{SellerFinanceServiceBaseUrl}/api/SellerBalances/release", request);
                },
                "Release balance"
            );
        }

        [HttpPost("hold")]
        public async Task<IActionResult> HoldBalance(Guid sellerId, decimal amount, string reason, string description)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);

                    // Manually constructing query string for this specific endpoint signature
                    var queryString = $"?sellerId={sellerId}&amount={amount}&reason={reason}&description={description}";
                    return client.PostAsync($"{SellerFinanceServiceBaseUrl}/api/SellerBalances/hold{queryString}", null);
                },
                "Hold balance"
            );
        }
    }
}
