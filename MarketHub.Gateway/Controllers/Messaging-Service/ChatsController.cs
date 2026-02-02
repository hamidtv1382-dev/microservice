namespace MarketHub.Gateway.Controllers.Messaging_Service
{
    [ApiController]
    [Route("api/messaging-service/chats")]
    [Authorize]
    public class ChatsController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ChatsController> _logger;
        private const string MessagingServiceBaseUrl = "https://localhost:7236";

        public ChatsController(IHttpClientFactory httpClientFactory, ILogger<ChatsController> logger)
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
        public async Task<IActionResult> CreateChat([FromBody] object request)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.PostAsJsonAsync($"{MessagingServiceBaseUrl}/api/Chats", request);
                },
                "Create chat"
            );
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserChats(Guid userId)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.GetAsync($"{MessagingServiceBaseUrl}/api/Chats/user/{userId}");
                },
                "Get user chats"
            );
        }

        [HttpGet("{chatId}")]
        public async Task<IActionResult> GetChatById(Guid chatId)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.GetAsync($"{MessagingServiceBaseUrl}/api/Chats/{chatId}");
                },
                "Get chat by ID"
            );
        }
    }
}
