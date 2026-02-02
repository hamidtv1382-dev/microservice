using Seller_Finance_Service.src._01_Domain.Core.ValueObjects;
using Seller_Finance_Service.src._02_Application.Interfaces;
using System.Text;
using System.Text.Json;

namespace Seller_Finance_Service.src._03_Infrastructure.Services.External
{
    public class FinanceServiceClient : IFinanceService
    {
        private readonly HttpClient _httpClient;

        public FinanceServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> RequestSettlementCreationAsync(Guid sellerId, BankAccountInfo bankInfo, Money amount)
        {
            var apiUrl = "https://localhost:7120/api/settlements";

            // ساخت Payload دقیقا طبق DTO جدید سرویس مالی
            var payload = new
            {
                SellerId = sellerId,
                TotalAmount = new { amount.Amount, Currency = "IRR" }, // ساختار Money باید دقیق باشد
                BankAccountInfo = JsonSerializer.Serialize(bankInfo), // یا ارسال رشته شبا
                DueDate = DateTime.UtcNow.AddDays(7)
            };

            // نکته: برای سریالایز شدن درست Money، بهتر است یک DTO مشترک داشته باشید 
            // یا ساختار anonymous object را دقیقا مطابق JSON مورد انتظار تنظیم کنید.
            // اینجا من فرض می‌کنم سمت گیرنده Money را به صورت Object می‌گیرد یا شما باید JsonSerializerOptions تنظیم کنید.

            // راه ساده‌تر برای سریالایز:
            var payloadRaw = new
            {
                SellerId = sellerId,
                TotalAmount = new { Amount = amount.Amount, Currency = "IRR" },
                DueDate = DateTime.UtcNow.AddDays(7)
            };

            var content = new StringContent(JsonSerializer.Serialize(payloadRaw), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(apiUrl, content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
