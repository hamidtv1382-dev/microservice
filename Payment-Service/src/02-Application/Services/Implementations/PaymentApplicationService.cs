using Payment_Service.src._01_Domain.Core.Enums;
using Payment_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Payment_Service.src._01_Domain.Services.Interfaces;
using Payment_Service.src._02_Application.DTOs.Requests;
using Payment_Service.src._02_Application.DTOs.Responses;
using Payment_Service.src._02_Application.Exceptions;
using Payment_Service.src._02_Application.Interfaces;
using Payment_Service.src._02_Application.Mappings;
using Payment_Service.src._02_Application.Services.Interfaces;
using Payment_Service.src._03_Infrastructure.Services.External;

namespace Payment_Service.src._02_Application.Services.Implementations
{
    public class PaymentApplicationService : IPaymentApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentDomainService _paymentDomainService;
        private readonly IPaymentGateway _paymentGateway;
        private readonly IWalletServiceClient _walletServiceClient;
        private readonly IOrderServiceClient _orderServiceClient;
        private readonly PaymentMappingProfile _mapper;
        private readonly ILogger<PaymentApplicationService> _logger;

        public PaymentApplicationService(
            IUnitOfWork unitOfWork,
            IPaymentDomainService paymentDomainService,
            IPaymentGateway paymentGateway,
            IWalletServiceClient walletServiceClient,
            IOrderServiceClient orderServiceClient,
            PaymentMappingProfile mapper,
            ILogger<PaymentApplicationService> logger)
        {
            _unitOfWork = unitOfWork;
            _paymentDomainService = paymentDomainService;
            _paymentGateway = paymentGateway;
            _walletServiceClient = walletServiceClient;
            _orderServiceClient = orderServiceClient;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PaymentResponseDto> MakePaymentAsync(MakePaymentRequestDto request)
        {
            // 1. دریافت اطلاعات واقعی سفارش از OrderService (UserId و Amount)
            var orderInfo = await _orderServiceClient.GetOrderInfoAsync(request.OrderId);
            if (orderInfo == null)
            {
                throw new PaymentFailedException("Order not found.");
            }

            Guid userId = orderInfo.BuyerId;
            decimal amount = orderInfo.FinalAmount;

            // 2. کسر موجودی از کیف پول (Wallet Service)
            var walletDeducted = await _walletServiceClient.DeductFundsAsync(userId, amount);
            if (!walletDeducted)
            {
                throw new PaymentFailedException("Insufficient balance in wallet.");
            }

            // 3. ثبت درخواست پرداخت در دیتابیس خودمان
            var transactionNumber = new Payment_Service.src._01_Domain.Core.ValueObjects.TransactionNumber($"TRX-{Guid.NewGuid():N}");
            var payment = new Payment_Service.src._01_Domain.Core.Entities.Payment(
                request.OrderId,
                new Payment_Service.src._01_Domain.Core.ValueObjects.Money(amount),
                PaymentMethod.Wallet,
                transactionNumber,
                "System");

            await _unitOfWork.Payments.AddAsync(payment);

            // 4. پردازش از طریق درگاه بانکی (یا شبیه‌سازی آن)
            var gatewayResult = await _paymentGateway.ProcessPaymentAsync(payment);

            if (gatewayResult.Success)
            {
                _paymentDomainService.CompletePayment(payment, gatewayResult.TransactionId);
            }
            else
            {
                _paymentDomainService.FailPayment(payment, gatewayResult.Message);

                // Compensating: برگرداندن پول به کیف پول چون پرداخت بانکی شکست خورد
                await _walletServiceClient.RefundFundsAsync(userId, amount);
                _logger.LogError($"Payment failed for Order {request.OrderId}. Wallet refunded.");
            }

            await _unitOfWork.SaveChangesAsync();

            return _mapper.MapToPaymentResponseDto(payment);
        }

        public async Task<PaymentResponseDto> GetPaymentStatusAsync(Guid paymentId)
        {
            var payment = await _unitOfWork.Payments.GetByIdAsync(paymentId);
            if (payment == null) throw new PaymentFailedException("Payment not found.");

            return _mapper.MapToPaymentResponseDto(payment);
        }

        public async Task<PaymentResponseDto> VerifyPaymentAsync(Guid paymentId)
        {
            var payment = await _unitOfWork.Payments.GetByIdAsync(paymentId);
            if (payment == null) throw new PaymentFailedException("Payment not found.");

            var verificationResult = await _paymentGateway.VerifyPaymentAsync(payment.TransactionNumber.Value);

            if (verificationResult.IsVerified && payment.Status == PaymentStatus.Pending)
            {
                _paymentDomainService.CompletePayment(payment, verificationResult.TransactionId);
                await _unitOfWork.SaveChangesAsync();
            }
            else if (!verificationResult.IsVerified && payment.Status == PaymentStatus.Pending)
            {
                _paymentDomainService.FailPayment(payment, "Verification failed");
                // در اینجا هم باید پول را برگردانیم (Compensating Transaction)
                // نیازمند ذخیره کردن UserId در جدول Payments یا دریافت مجدد از OrderService است.
                await _unitOfWork.SaveChangesAsync();
            }

            return _mapper.MapToPaymentResponseDto(payment);
        }
    }
}