namespace MarketHub.Gateway.Controllers.Finance_Service
{
    [ApiController]
    [Route("api/finance-service/fees")]
    [Authorize]
    public class FeesController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<FeesController> _logger;
        private const string FinanceServiceBaseUrl = "https://localhost:7120";

        public FeesController(IHttpClientFactory httpClientFactory, ILogger<FeesController> logger)
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

        [HttpPost]
        public async Task<IActionResult> ApplyFee([FromBody] object request)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.PostAsJsonAsync($"{FinanceServiceBaseUrl}/api/Fees", request);
                },
                "Apply fee"
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeeById(Guid id)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.GetAsync($"{FinanceServiceBaseUrl}/api/Fees/{id}");
                },
                "Get fee by ID"
            );
        }

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetFeesByOrderId(Guid orderId)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.GetAsync($"{FinanceServiceBaseUrl}/api/Fees/order/{orderId}");
                },
                "Get fees by order ID"
            );
        }
    }
}
