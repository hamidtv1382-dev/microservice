namespace MarketHub.Gateway.Controllers.Messaging_Service
{
    [ApiController]
    [Route("api/messaging-service/messages")]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<MessagesController> _logger;
        private const string MessagingServiceBaseUrl = "https://localhost:7236";

        public MessagesController(IHttpClientFactory httpClientFactory, ILogger<MessagesController> logger)
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
        public async Task<IActionResult> SendMessage([FromBody] object request)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.PostAsJsonAsync($"{MessagingServiceBaseUrl}/api/Messages", request);
                },
                "Send message"
            );
        }

        [HttpPost("{messageId}/approve")]
        public async Task<IActionResult> ApproveMessage(Guid messageId)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.PostAsync($"{MessagingServiceBaseUrl}/api/Messages/{messageId}/approve", null);
                },
                "Approve message"
            );
        }

        [HttpGet("chat/{chatId}")]
        public async Task<IActionResult> GetChatMessages(Guid chatId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    // Forwarding query parameters from the incoming request to the downstream service
                    return client.GetAsync($"{MessagingServiceBaseUrl}/api/Messages/chat/{chatId}?page={page}&pageSize={pageSize}");
                },
                "Get chat messages"
            );
        }
    }
}
