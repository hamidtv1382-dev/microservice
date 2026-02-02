using Order_Service.src._01_Domain.Core.Entities;
using Order_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Order_Service.src._02_Application.DTOs.Requests;
using Order_Service.src._02_Application.DTOs.Responses;
using Order_Service.src._02_Application.Exceptions;
using Order_Service.src._02_Application.Interfaces;
using Order_Service.src._02_Application.Services.Interfaces;
using System;

namespace Order_Service.src._02_Application.Services.Implementations
{
    public class RefundApplicationService : IRefundApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentGateway _paymentGateway;
        private readonly ILogger<RefundApplicationService> _logger;

        public RefundApplicationService(
            IUnitOfWork unitOfWork,
            IPaymentGateway paymentGateway,
            ILogger<RefundApplicationService> logger)
        {
            _unitOfWork = unitOfWork;
            _paymentGateway = paymentGateway;
            _logger = logger;
        }

        public async Task<RefundResponseDto> RequestRefundAsync(Guid buyerId, RequestRefundRequestDto request)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
            if (order == null)
                throw new OrderNotFoundException(request.OrderId);

            if (order.BuyerId != buyerId)
                throw new UnauthorizedAccessException("You are not the owner of this order.");

            if (order.Status != _01_Domain.Core.Enums.OrderStatus.Delivered)
                throw new InvalidOperationException("Refunds are only available for delivered orders.");

            var existingPayment = await _unitOfWork.Payments.GetByOrderIdAsync(order.Id);
            if (existingPayment == null || existingPayment.Status != _01_Domain.Core.Enums.PaymentStatus.Completed)
                throw new InvalidOperationException("No valid payment found for this order.");

            // Create Refund Entity
            var refund = new Refund(Guid.NewGuid(), order.Id, existingPayment.Id, order.FinalAmount, request.Reason);
            await _unitOfWork.Refunds.AddAsync(refund);

            try
            {
                // Call Gateway
                var gatewayResult = await _paymentGateway.ProcessRefundAsync(existingPayment.Id, order.FinalAmount);

                if (gatewayResult.IsSuccessful)
                {
                    refund.Process(gatewayResult.RefundId ?? Guid.NewGuid().ToString());
                    order.MarkAsRefunded();  // ✅ به جای دسترسی مستقیم
                }

                else
                {
                    refund.Fail(gatewayResult.Message ?? "Gateway Refund Failed");
                    throw new RefundFailedException(gatewayResult.Message);
                }

                await _unitOfWork.SaveChangesAsync();

                return new RefundResponseDto
                {
                    RefundId = refund.Id,
                    IsSuccessful = true,
                    Message = "Refund processed successfully.",
                    RefundedAmount = refund.Amount.Value
                };
            }
            catch (Exception ex)
            {
                refund.Fail(ex.Message);
                await _unitOfWork.SaveChangesAsync();
                _logger.LogError(ex, "Refund failed for Order {OrderId}", order.Id);
                throw;
            }
        }

        public async Task<RefundResponseDto> GetRefundStatusAsync(Guid refundId)
        {
            var refund = await _unitOfWork.Refunds.GetByIdAsync(refundId);
            if (refund == null)
                throw new KeyNotFoundException("Refund not found.");

            return new RefundResponseDto
            {
                RefundId = refund.Id,
                IsSuccessful = refund.Status == _01_Domain.Core.Enums.PaymentStatus.Completed,
                Message = refund.Status.ToString(),
                RefundedAmount = refund.Amount.Value
            };
        }
    }
}
