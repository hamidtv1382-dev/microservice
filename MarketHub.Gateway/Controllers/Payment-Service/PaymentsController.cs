using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketHub.Gateway.Controllers.Payment_Service
{
    [ApiController]
    [Route("api/payments-service/payments")]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<PaymentsController> _logger;
        private const string PaymentServiceBaseUrl = "https://localhost:7116";

        public PaymentsController(IHttpClientFactory httpClientFactory, ILogger<PaymentsController> logger)
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
        public async Task<IActionResult> MakePayment([FromBody] object request)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.PostAsJsonAsync($"{PaymentServiceBaseUrl}/api/Payments", request);
                },
                "Make payment"
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentStatus(Guid id)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.GetAsync($"{PaymentServiceBaseUrl}/api/Payments/{id}");
                },
                "Get payment status"
            );
        }

        [HttpPost("{id}/verify")]
        public async Task<IActionResult> VerifyPayment(Guid id)
        {
            return await ForwardRequest(
                () => {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    return client.PostAsync($"{PaymentServiceBaseUrl}/api/Payments/{id}/verify", null);
                },
                "Verify payment"
            );
        }
    }
}
