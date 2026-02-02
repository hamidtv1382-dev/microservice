using Order_Service.src._01_Domain.Core.Entities;
using Order_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Order_Service.src._01_Domain.Services.Interfaces;
using Order_Service.src._02_Application.DTOs.Requests;
using Order_Service.src._02_Application.DTOs.Responses;
using Order_Service.src._02_Application.Exceptions;
using Order_Service.src._02_Application.Interfaces;
using Order_Service.src._02_Application.Services.Interfaces;
using System;

namespace Order_Service.src._02_Application.Services.Implementations
{
    public class PaymentApplicationService : IPaymentApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentGateway _paymentGateway;
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<PaymentApplicationService> _logger;

        public PaymentApplicationService(
            IUnitOfWork unitOfWork,
            IPaymentGateway paymentGateway,
            IInventoryService inventoryService,
            ILogger<PaymentApplicationService> logger)
        {
            _unitOfWork = unitOfWork;
            _paymentGateway = paymentGateway;
            _inventoryService = inventoryService;
            _logger = logger;
        }

        public async Task<PaymentResponseDto> MakePaymentAsync(Guid buyerId, MakePaymentRequestDto request)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
            if (order == null)
                throw new OrderNotFoundException(request.OrderId);

            if (order.Status != _01_Domain.Core.Enums.OrderStatus.Pending)
                throw new InvalidOperationException("Order is not in a payable state.");

            // Create Payment Record
            var payment = new Payment(Guid.NewGuid(), order.Id, order.FinalAmount, "ShaparakGateway");
            await _unitOfWork.Payments.AddAsync(payment);

            try
            {
                // Call External Gateway
                var gatewayResult = await _paymentGateway.ProcessPaymentAsync(
                    order.FinalAmount,
                    request.CardNumber,
                    request.Cvv2,
                    request.ExpMonth,
                    request.ExpYear,
                    request.Pin);

                if (gatewayResult.IsSuccessful)
                {
                    payment.Complete(gatewayResult.TransactionId ?? Guid.NewGuid().ToString());
                    order.Confirm();

                    // Finalize Inventory Deduction (if reserved earlier) or trigger here
                    // Assuming stock was reserved at order creation

                    await _unitOfWork.SaveChangesAsync();

                    return new PaymentResponseDto
                    {
                        PaymentId = payment.Id,
                        IsSuccessful = true,
                        Message = "Payment Successful",
                        TransactionId = payment.TransactionId
                    };
                }
                else
                {
                    payment.Fail(gatewayResult.Message ?? "Gateway Error");

                    // Compensating: Release Reserved Stock
                    foreach (var item in order.Items)
                        await _inventoryService.ReleaseStockAsync(item.ProductId, item.Quantity);

                    await _unitOfWork.SaveChangesAsync();

                    throw new PaymentFailedException(gatewayResult.Message);
                }
            }
            catch (Exception ex)
            {
                payment.Fail(ex.Message);
                await _unitOfWork.SaveChangesAsync();
                _logger.LogError(ex, "Payment failed for Order {OrderId}", order.Id);
                throw;
            }
        }

        public async Task<PaymentResponseDto> GetPaymentStatusAsync(Guid paymentId)
        {
            var payment = await _unitOfWork.Payments.GetByIdAsync(paymentId);
            if (payment == null)
                throw new KeyNotFoundException("Payment not found.");

            return new PaymentResponseDto
            {
                PaymentId = payment.Id,
                IsSuccessful = payment.Status == _01_Domain.Core.Enums.PaymentStatus.Completed,
                Message = payment.Status.ToString(),
                TransactionId = payment.TransactionId
            };
        }
    }
}
