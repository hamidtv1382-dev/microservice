using Payment_Service.src._01_Domain.Core.Entities;
using Payment_Service.src._01_Domain.Core.Enums;
using Payment_Service.src._01_Domain.Core.ValueObjects;
using Payment_Service.src._01_Domain.Services.Interfaces;

namespace Payment_Service.src._01_Domain.Services.Implementations
{
    public class PaymentDomainService : IPaymentDomainService
    {
        public async Task<Payment> InitiatePaymentAsync(Guid orderId, Money amount, string method)
        {
            if (orderId == Guid.Empty) throw new ArgumentException("Invalid Order ID");
            if (amount.Amount <= 0) throw new ArgumentException("Amount must be greater than zero");

            if (!Enum.TryParse<PaymentMethod>(method, true, out var paymentMethod))
            {
                throw new ArgumentException("Invalid payment method");
            }

            var transactionNumber = TransactionNumber.Generate();
            var payment = new Payment(orderId, amount, paymentMethod, transactionNumber, "System");

            return await Task.FromResult(payment);
        }

        public Task<bool> ValidatePaymentAsync(Payment payment)
        {
            if (payment == null) return Task.FromResult(false);
            if (payment.Amount.Amount <= 0) return Task.FromResult(false);
            if (payment.OrderId == Guid.Empty) return Task.FromResult(false);
            if (payment.Status != PaymentStatus.Pending) return Task.FromResult(false);

            return Task.FromResult(true);
        }

        public void CompletePayment(Payment payment, string externalTransactionId)
        {
            if (payment == null) throw new ArgumentNullException(nameof(payment));
            payment.CompletePayment(externalTransactionId);
        }

        public void FailPayment(Payment payment, string reason)
        {
            if (payment == null) throw new ArgumentNullException(nameof(payment));
            payment.FailPayment(reason);
        }
    }
}
