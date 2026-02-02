namespace MarketHub.Gateway.Controllers.Wallet_Service
{

    [ApiController]
    [Route("api/wallet-service/wallets")]
    [Authorize]
    public class WalletsController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<WalletsController> _logger;
        private const string WalletServiceBaseUrl = "https://localhost:7030";

        public WalletsController(IHttpClientFactory httpClientFactory, ILogger<WalletsController> logger)
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

        [HttpGet("balance/{ownerId}")]
        public async Task<IActionResult> GetBalance(Guid ownerId)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.GetAsync($"{WalletServiceBaseUrl}/api/Wallets/balance/{ownerId}");
                },
                "Get wallet balance"
            );
        }

        [HttpPost("add-funds")]
        public async Task<IActionResult> AddFunds([FromBody] object request)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.PostAsJsonAsync($"{WalletServiceBaseUrl}/api/Wallets/add-funds", request);
                },
                "Add funds"
            );
        }

        [HttpPost("deduct-funds")]
        public async Task<IActionResult> DeductFunds([FromBody] object request)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.PostAsJsonAsync($"{WalletServiceBaseUrl}/api/Wallets/deduct-funds", request);
                },
                "Deduct funds"
            );
        }

        [HttpPost("{userId}/deduct")]
        public async Task<IActionResult> DeductFundsByUserId(Guid userId, [FromBody] object request)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.PostAsJsonAsync($"{WalletServiceBaseUrl}/api/Wallets/{userId}/deduct", request);
                },
                "Deduct funds by user ID"
            );
        }
    }
}
