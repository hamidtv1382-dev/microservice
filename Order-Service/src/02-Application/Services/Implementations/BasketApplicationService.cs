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
    public class BasketApplicationService : IBasketApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDiscountService _discountService;
        private readonly IMapper _mapper;
        private readonly ILogger<BasketApplicationService> _logger;
        private readonly CatalogServiceClient _catalogClient;

        public BasketApplicationService(
            IUnitOfWork unitOfWork,
            IDiscountService discountService,
            IMapper mapper,
            ILogger<BasketApplicationService> logger,
            CatalogServiceClient catalogClient)
        {
            _unitOfWork = unitOfWork;
            _discountService = discountService;
            _mapper = mapper;
            _logger = logger;
            _catalogClient = catalogClient;
        }

        public async Task<BasketDetailResponseDto> GetBasketAsync(Guid buyerId)
        {
            var basket = await _unitOfWork.Baskets.GetByBuyerIdAsync(buyerId);

            if (basket == null)
            {
                return new BasketDetailResponseDto
                {
                    Id = Guid.Empty,
                    Items = new List<BasketDetailResponseDto.BasketItemResponseDto>(),
                    TotalAmount = 0,
                    DiscountAmount = 0,
                    FinalAmount = 0
                };
            }

            return await MapBasketToResponseDtoAsync(basket);
        }

        public async Task<BasketDetailResponseDto> AddItemAsync(Guid buyerId, AddItemToBasketRequestDto request)
        {
            var basket = await _unitOfWork.Baskets.GetByBuyerIdAsync(buyerId);

            if (basket == null)
            {
                basket = new Basket(Guid.NewGuid(), buyerId, TimeSpan.FromDays(7));
                await _unitOfWork.Baskets.AddAsync(basket);
            }

            var productInfo = await _catalogClient.GetProductByIdAsync(request.ProductId);

            if (productInfo == null)
            {
                throw new KeyNotFoundException($"Product with ID {request.ProductId} not found in Catalog Service.");
            }

            var productName = productInfo.Name;
            var unitPrice = new Money(productInfo.Price, "IRR");
            var imageUrl = productInfo.ImageUrl ?? "https://example.com/image.jpg";

            var item = new BasketItem(Guid.NewGuid(), basket.Id, request.ProductId, productName, imageUrl, unitPrice, request.Quantity);
            basket.AddItem(item);

            await _unitOfWork.SaveChangesAsync();

            return await MapBasketToResponseDtoAsync(basket);
        }

        public async Task<BasketDetailResponseDto> UpdateItemAsync(Guid buyerId, UpdateBasketItemRequestDto request)
        {
            var basket = await _unitOfWork.Baskets.GetByBuyerIdAsync(buyerId);
            if (basket == null)
                throw new BasketNotFoundException(buyerId.ToString());

            basket.UpdateItemQuantity(request.ProductId, request.Quantity);
            await _unitOfWork.SaveChangesAsync();

            return await MapBasketToResponseDtoAsync(basket);
        }

        public async Task<BasketDetailResponseDto> RemoveItemAsync(Guid buyerId, RemoveItemFromBasketRequestDto request)
        {
            var basket = await _unitOfWork.Baskets.GetByBuyerIdAsync(buyerId);
            if (basket == null)
                throw new BasketNotFoundException(buyerId.ToString());

            basket.RemoveItem(request.ProductId);
            await _unitOfWork.SaveChangesAsync();

            return await MapBasketToResponseDtoAsync(basket);
        }

        public async Task<BasketDetailResponseDto> ApplyDiscountAsync(Guid buyerId, ApplyDiscountRequestDto request)
        {
            var basket = await _unitOfWork.Baskets.GetByBuyerIdAsync(buyerId);
            if (basket == null)
                throw new BasketNotFoundException(buyerId.ToString());

            var discount = await _discountService.ValidateDiscountAsync(request.DiscountCode, basket.TotalAmount);
            if (discount == null)
                throw new InvalidDiscountCodeException(request.DiscountCode);

            basket.ApplyDiscount(discount.Id);

            await _unitOfWork.SaveChangesAsync();

            basket = await _unitOfWork.Baskets.GetByBuyerIdWithDiscountAsync(buyerId);

            return await MapBasketToResponseDtoAsync(basket);
        }

        public async Task ClearBasketAsync(Guid buyerId)
        {
            var basket = await _unitOfWork.Baskets.GetByBuyerIdAsync(buyerId);
            if (basket != null)
            {
                basket.Clear();
                await _unitOfWork.SaveChangesAsync();
            }
        }

        private async Task<BasketDetailResponseDto> MapBasketToResponseDtoAsync(Basket basket)
        {
            var dto = _mapper.Map<BasketDetailResponseDto>(basket);

            if (basket.AppliedDiscount != null)
            {
                var discountMoney = basket.AppliedDiscount.CalculateDiscountAmount(basket.TotalAmount);

                dto.DiscountAmount = discountMoney.Value;
                dto.FinalAmount = basket.TotalAmount.Value - discountMoney.Value;
                dto.AppliedDiscountCode = basket.AppliedDiscount.Code;
            }
            else
            {
                dto.DiscountAmount = 0;
                dto.FinalAmount = basket.TotalAmount.Value;
            }

            return dto;
        }
    }
}