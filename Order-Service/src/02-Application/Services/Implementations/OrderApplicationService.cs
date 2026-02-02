using AutoMapper;
using Order_Service.src._01_Domain.Core.Entities;
using Order_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Order_Service.src._01_Domain.Core.ValueObjects;
using Order_Service.src._01_Domain.Services.Interfaces;
using Order_Service.src._02_Application.DTOs.Requests;
using Order_Service.src._02_Application.DTOs.Responses;
using Order_Service.src._02_Application.Exceptions;
using Order_Service.src._02_Application.Services.Interfaces;
using Order_Service.src._03_Infrastructure.Services.External;

namespace Order_Service.src._02_Application.Services.Implementations
{
    public class OrderApplicationService : IOrderApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderDomainService _orderDomainService;
        private readonly IInventoryService _inventoryService;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderApplicationService> _logger;
        private readonly PaymentServiceClient _paymentServiceClient;
        private readonly CatalogServiceClient _catalogClient;
        private readonly SellerFinanceServiceClient _sellerFinanceClient;

        public OrderApplicationService(
            IUnitOfWork unitOfWork,
            IOrderDomainService orderDomainService,
            IInventoryService inventoryService,
            IMapper mapper,
            ILogger<OrderApplicationService> logger,
            PaymentServiceClient paymentServiceClient,
            CatalogServiceClient catalogClient,
            SellerFinanceServiceClient sellerFinanceClient)
        {
            _unitOfWork = unitOfWork;
            _orderDomainService = orderDomainService;
            _inventoryService = inventoryService;
            _mapper = mapper;
            _logger = logger;
            _paymentServiceClient = paymentServiceClient;
            _catalogClient = catalogClient;
            _sellerFinanceClient = sellerFinanceClient;
        }

        public async Task<OrderDetailResponseDto> CreateOrderAsync(Guid buyerId, CreateOrderRequestDto request)
        {
            var basket = await _unitOfWork.Baskets.GetByBuyerIdAsync(buyerId);
            if (basket == null || !basket.Items.Any())
                throw new ArgumentException("Basket is empty.");

            var shippingAddress = new ShippingAddress(
                request.FirstName,
                request.LastName,
                request.PhoneNumber,
                request.Country,
                request.City,
                request.State,
                request.Street,
                request.ZipCode,
                request.BuildingNumber,
                request.ApartmentNumber
            );

            var orderItems = new List<OrderItem>();
            foreach (var basketItem in basket.Items)
            {
                var productInfo = await _catalogClient.GetProductByIdAsync(basketItem.ProductId);
                if (productInfo == null) throw new KeyNotFoundException($"Product {basketItem.ProductId} not found.");

                orderItems.Add(new OrderItem(
                    Guid.NewGuid(),
                    Guid.NewGuid(), // Placeholder, will be set by Order
                    basketItem.ProductId,
                    basketItem.ProductName,
                    basketItem.ImageUrl,
                    basketItem.UnitPrice,
                    basketItem.Quantity,
                    productInfo.SellerId
                ));
            }

            var order = await _orderDomainService.CreateOrderAsync(buyerId, shippingAddress, orderItems, request.DiscountCode);

            foreach (var item in order.Items)
            {
                var reserved = await _inventoryService.ReserveStockAsync(item.ProductId, item.Quantity);
                if (!reserved)
                {
                    foreach (var reservedItem in order.Items.Where(i => i.ProductId != item.ProductId))
                    {
                        await _inventoryService.ReleaseStockAsync(reservedItem.ProductId, reservedItem.Quantity);
                    }
                    throw new InsufficientStockException(item.ProductId, item.Quantity);
                }
            }

            await _unitOfWork.Orders.AddAsync(order);
            basket.Clear();
            _unitOfWork.Baskets.Update(basket);

            await _unitOfWork.SaveChangesAsync();

            try
            {
                var callbackUrl = $"https://localhost:5001/api/payments/verify/{order.Id}";

                var paymentRequest = new PaymentRequestDto(
                    OrderId: order.Id,
                    Amount: order.FinalAmount.Value,
                    CallbackUrl: callbackUrl
                );

                var paymentResult = await _paymentServiceClient.ProcessPaymentAsync(paymentRequest);

                if (paymentResult == null || !paymentResult.IsSuccessful)
                {
                    _orderDomainService.CancelOrder(order, "Payment Failed");
                    foreach (var item in order.Items) await _inventoryService.ReleaseStockAsync(item.ProductId, item.Quantity);
                    await _unitOfWork.SaveChangesAsync();
                }
                else
                {
                    // --- تغییر جدید: آپدیت وضعیت سفارش و ثبت درآمد ---
                    order.Confirm(); // تغییر وضعیت به Confirmed
                    await _unitOfWork.SaveChangesAsync(); // ذخیره تغییر وضعیت

                    await ProcessSellerEarningsAsync(order); // ثبت درآمد فروشنده
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Payment integration failed for Order {OrderId}", order.Id);
            }

            return _mapper.Map<OrderDetailResponseDto>(order);
        }

        private async Task ProcessSellerEarningsAsync(Order order)
        {
            try
            {
                foreach (var item in order.Items)
                {
                    var itemAmount = item.TotalPrice.Value;
                    var transactionId = Guid.NewGuid();

                    await _sellerFinanceClient.RecordSellerEarningAsync(
                        sellerId: Guid.Parse(item.SellerId),
                        orderId: order.Id,
                        amount: itemAmount,
                        transactionId: transactionId
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to record seller earnings for Order {OrderId}", order.Id);
            }
        }

        public async Task<OrderDetailResponseDto> GetOrderByIdAsync(Guid orderId)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null) throw new OrderNotFoundException(orderId);

            return _mapper.Map<OrderDetailResponseDto>(order);
        }

        public async Task<IEnumerable<OrderSummaryResponseDto>> GetOrdersByBuyerIdAsync(Guid buyerId)
        {
            var orders = await _unitOfWork.Orders.GetByBuyerIdAsync(buyerId);
            return _mapper.Map<IEnumerable<OrderSummaryResponseDto>>(orders);
        }

        public async Task<OrderDetailResponseDto> UpdateOrderAsync(UpdateOrderRequestDto request)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
            if (order == null) throw new OrderNotFoundException(request.OrderId);

            var newAddress = new ShippingAddress(
                request.FirstName,
                request.LastName,
                request.PhoneNumber,
                request.Country,
                request.City,
                request.State,
                request.Street,
                request.ZipCode,
                request.BuildingNumber,
                request.ApartmentNumber
            );

            order.UpdateShippingAddress(newAddress);

            _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OrderDetailResponseDto>(order);
        }

        public async Task<OrderDetailResponseDto> CancelOrderAsync(CancelOrderRequestDto request)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
            if (order == null) throw new OrderNotFoundException(request.OrderId);

            _orderDomainService.CancelOrder(order, request.Reason ?? "User requested cancellation");

            foreach (var item in order.Items)
            {
                await _inventoryService.ReleaseStockAsync(item.ProductId, item.Quantity);
            }

            _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OrderDetailResponseDto>(order);
        }

        public async Task<TrackOrderResponseDto> TrackOrderAsync(TrackOrderRequestDto request)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
            if (order == null) throw new OrderNotFoundException(request.OrderId);

            var response = new TrackOrderResponseDto
            {
                OrderId = order.Id,
                OrderNumber = order.OrderNumber.Value,
                CurrentStatus = order.Status.ToString(),
                EstimatedDeliveryDate = DateTime.UtcNow.AddDays(5),
                History = new List<TrackOrderResponseDto.OrderTimelineDto>
                {
                    new TrackOrderResponseDto.OrderTimelineDto { Status = "Pending", Date = order.CreatedAt },
                    new TrackOrderResponseDto.OrderTimelineDto { Status = order.Status.ToString(), Date = order.UpdatedAt ?? order.CreatedAt }
                }
            };

            return response;
        }
    }
}