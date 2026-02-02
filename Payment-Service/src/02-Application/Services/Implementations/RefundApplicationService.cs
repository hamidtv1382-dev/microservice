using Payment_Service.src._01_Domain.Core.Enums;
using Payment_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Payment_Service.src._01_Domain.Services.Interfaces;
using Payment_Service.src._02_Application.DTOs.Requests;
using Payment_Service.src._02_Application.DTOs.Responses;
using Payment_Service.src._02_Application.Exceptions;
using Payment_Service.src._02_Application.Interfaces;
using Payment_Service.src._02_Application.Mappings;
using Payment_Service.src._02_Application.Services.Interfaces;

namespace Payment_Service.src._02_Application.Services.Implementations
{
    public class RefundApplicationService : IRefundApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRefundDomainService _refundDomainService;
        private readonly IPaymentGateway _paymentGateway;
        private readonly PaymentMappingProfile _mapper;

        public RefundApplicationService(IUnitOfWork unitOfWork, IRefundDomainService refundDomainService, IPaymentGateway paymentGateway, PaymentMappingProfile mapper)
        {
            _unitOfWork = unitOfWork;
            _refundDomainService = refundDomainService;
            _paymentGateway = paymentGateway;
            _mapper = mapper;
        }

        public async Task<RefundResponseDto> RequestRefundAsync(RequestRefundRequestDto request)
        {
            var payment = await _unitOfWork.Payments.GetByIdAsync(request.PaymentId);
            if (payment == null) throw new RefundFailedException("Original payment not found.");

            if (payment.Status != PaymentStatus.Completed)
                throw new RefundFailedException("Cannot refund a payment that is not completed.");

            if (request.Amount.Amount > payment.Amount.Amount)
                throw new RefundFailedException("Refund amount cannot exceed original payment amount.");

            var refund = await _refundDomainService.InitiateRefundAsync(request.PaymentId, request.Amount, request.Reason);

            await _unitOfWork.Refunds.AddAsync(refund);

            var gatewayResult = await _paymentGateway.ProcessRefundAsync(refund);

            if (gatewayResult.Success)
            {
                _refundDomainService.ProcessRefund(refund, gatewayResult.TransactionId);
            }
            else
            {
                refund.MarkAsFailed(gatewayResult.Message);
            }

            await _unitOfWork.SaveChangesAsync();

            return _mapper.MapToRefundResponseDto(refund);
        }

        public async Task<RefundResponseDto> GetRefundStatusAsync(Guid refundId)
        {
            var refund = await _unitOfWork.Refunds.GetByIdAsync(refundId);
            if (refund == null) throw new RefundFailedException("Refund not found.");

            return _mapper.MapToRefundResponseDto(refund);
        }
    }
}

