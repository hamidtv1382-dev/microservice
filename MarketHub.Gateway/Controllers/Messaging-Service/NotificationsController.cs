namespace MarketHub.Gateway.Controllers.Messaging_Service
{
    [ApiController]
    [Route("api/messaging-service/notifications")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<NotificationsController> _logger;
        private const string MessagingServiceBaseUrl = "https://localhost:7236";

        public NotificationsController(IHttpClientFactory httpClientFactory, ILogger<NotificationsController> logger)
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

        [HttpPost("sms")]
        public async Task<IActionResult> SendSms([FromBody] object request)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.PostAsJsonAsync($"{MessagingServiceBaseUrl}/api/Notifications/sms", request);
                },
                "Send SMS"
            );
        }

        [HttpPost("email")]
        public async Task<IActionResult> SendEmail([FromBody] object request)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.PostAsJsonAsync($"{MessagingServiceBaseUrl}/api/Notifications/email", request);
                },
                "Send Email"
            );
        }

        [HttpPost("push")]
        public async Task<IActionResult> SendPush([FromBody] object request)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.PostAsJsonAsync($"{MessagingServiceBaseUrl}/api/Notifications/push", request);
                },
                "Send Push Notification"
            );
        }
    }
}
