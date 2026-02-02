using Order_Service.src._01_Domain.Core.ValueObjects;
using Order_Service.src._02_Application.Interfaces;

namespace Order_Service.src._02_Application.Services.Implementations
{
    /// <summary>
    /// پیاده‌سازی درگاه پرداخت (شبیه‌سازی شده)
    /// این کلاس اینترفیس IPaymentGateway را پیاده‌سازی می‌کند و منطق اصلی پرداخت را شبیه‌سازی می‌کند.
    /// در محیط واقعی، متدهای این کلاس باید به درگاه‌های بانکی متصل شوند.
    /// </summary>
    public class PaymentGatewayService : IPaymentGateway
    {
        public Task<PaymentGatewayResult> ProcessPaymentAsync(Money amount, string cardNumber, string cvv2, string expMonth, string expYear, string pin)
        {
            // 1. بررسی اعتبار کارت (Validation)
            if (string.IsNullOrWhiteSpace(cardNumber) || cardNumber.Length < 16)
            {
                return Task.FromResult(new PaymentGatewayResult
                {
                    IsSuccessful = false,
                    Message = "شماره کارت نامعتبر است."
                });
            }

            if (string.IsNullOrWhiteSpace(cvv2) || cvv2.Length < 3 || cvv2.Length > 4)
            {
                return Task.FromResult(new PaymentGatewayResult
                {
                    IsSuccessful = false,
                    Message = "کد CVV2 نامعتبر است."
                });
            }

            if (string.IsNullOrWhiteSpace(expMonth) || string.IsNullOrWhiteSpace(expYear))
            {
                return Task.FromResult(new PaymentGatewayResult
                {
                    IsSuccessful = false,
                    Message = "تاریخ انقضای کارت نامعتبر است."
                });
            }

            if (string.IsNullOrWhiteSpace(pin))
            {
                return Task.FromResult(new PaymentGatewayResult
                {
                    IsSuccessful = false,
                    Message = "رمز دوم (پویا) الزامی است."
                });
            }

            // 2. شبیه‌سازی پرداخت موفق
            // فرض بر این است که اگر اطلاعات درست باشد، درگاه بانک تایید می‌کند.
            var transactionId = Guid.NewGuid().ToString("N");

            return Task.FromResult(new PaymentGatewayResult
            {
                IsSuccessful = true,
                TransactionId = transactionId,
                Message = "تراکنش با موفقیت انجام شد.",
                PaymentUrl = null
            });
        }

        public Task<RefundGatewayResult> ProcessRefundAsync(Guid transactionId, Money amount)
        {
            // 1. بررسی معتبر بودن شناسه تراکنش
            if (transactionId == Guid.Empty)
            {
                return Task.FromResult(new RefundGatewayResult
                {
                    IsSuccessful = false,
                    Message = "شناسه تراکنش یافت نشد."
                });
            }

            // 2. بررسی مبلغ استرداد
            // کلاس Money خودش چک می‌کند که مقدار منفی نباشد، اما اینجا هم چک می‌کنیم
            if (amount.Value <= 0)
            {
                return Task.FromResult(new RefundGatewayResult
                {
                    IsSuccessful = false,
                    Message = "مبلغ استرداد باید بیشتر از صفر باشد."
                });
            }

            // 3. شبیه‌سازی استرداد موفق
            var refundId = Guid.NewGuid().ToString("N");

            return Task.FromResult(new RefundGatewayResult
            {
                IsSuccessful = true,
                RefundId = refundId,
                Message = $"مبلغ {amount.Value} {amount.Currency} با موفقیت مسترد شد."
            });
        }
    }
}
