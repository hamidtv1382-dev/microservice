namespace MarketHub.Gateway.Controllers.Seller_Finance_Service
{
    [ApiController]
    [Route("api/seller-finance-service/accounts")]
    [Authorize]
    public class SellerAccountsController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<SellerAccountsController> _logger;
        private const string SellerFinanceServiceBaseUrl = "https://localhost:7004";

        public SellerAccountsController(IHttpClientFactory httpClientFactory, ILogger<SellerAccountsController> logger)
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

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] object request)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.PostAsJsonAsync($"{SellerFinanceServiceBaseUrl}/api/SellerAccounts", request);
                },
                "Create seller account"
            );
        }

        [HttpGet("{sellerId}")]
        public async Task<IActionResult> GetAccount(Guid sellerId)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.GetAsync($"{SellerFinanceServiceBaseUrl}/api/SellerAccounts/{sellerId}");
                },
                "Get seller account"
            );
        }

        [HttpPut("bank-info")]
        public async Task<IActionResult> UpdateBankInfo([FromBody] object request)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.PutAsJsonAsync($"{SellerFinanceServiceBaseUrl}/api/SellerAccounts/bank-info", request);
                },
                "Update bank info"
            );
        }

        [HttpPatch("{sellerId}/activate")]
        public async Task<IActionResult> ActivateAccount(Guid sellerId)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.PatchAsync($"{SellerFinanceServiceBaseUrl}/api/SellerAccounts/{sellerId}/activate", null);
                },
                "Activate seller account"
            );
        }

        [HttpPatch("{sellerId}/deactivate")]
        public async Task<IActionResult> DeactivateAccount(Guid sellerId)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.PatchAsync($"{SellerFinanceServiceBaseUrl}/api/SellerAccounts/{sellerId}/deactivate", null);
                },
                "Deactivate seller account"
            );
        }
    }
}
