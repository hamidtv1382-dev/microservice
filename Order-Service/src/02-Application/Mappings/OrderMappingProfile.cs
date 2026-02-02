using Order_Service.src._01_Domain.Core.Entities;
using Order_Service.src._01_Domain.Core.ValueObjects;
using Order_Service.src._02_Application.DTOs.Requests;
using Order_Service.src._02_Application.DTOs.Responses;
using Order_Service.src._01_Domain.Core.Enums;

namespace Order_Service.src._02_Application.Mappings
{
    public class OrderMappingProfile : AutoMapper.Profile
    {
        public OrderMappingProfile()
        {
            // Entity to DTO Mapping
            CreateMap<Discount, DiscountDetailResponseDto>()
              .ForMember(dest => dest.MinimumOrderAmount, opt => opt.MapFrom(src => src.MinimumOrderAmount.Value));

            CreateMap<Discount, DiscountSummaryResponseDto>();
            CreateMap<Order, OrderDetailResponseDto>()
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddress))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount.Value))
                .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => src.DiscountAmount.Value))
                .ForMember(dest => dest.FinalAmount, opt => opt.MapFrom(src => src.FinalAmount.Value))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate ?? src.CreatedAt));

            CreateMap<Order, OrderSummaryResponseDto>()
                .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.OrderNumber.Value))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.FinalAmount, opt => opt.MapFrom(src => src.FinalAmount.Value));

            CreateMap<OrderItem, OrderDetailResponseDto.OrderItemResponseDto>()
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice.Value))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice.Value))
                .ForMember(dest => dest.SellerId, opt => opt.MapFrom(src => src.SellerId));

            CreateMap<Basket, BasketDetailResponseDto>()
              .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount.Value))
              .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<BasketItem, BasketDetailResponseDto.BasketItemResponseDto>()
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice.Value))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice.Value));

            CreateMap<Payment, PaymentResponseDto>()
                .ForMember(dest => dest.IsSuccessful, opt => opt.MapFrom(src => src.Status == _01_Domain.Core.Enums.PaymentStatus.Completed));

            CreateMap<Refund, RefundResponseDto>()
                .ForMember(dest => dest.IsSuccessful, opt => opt.MapFrom(src => src.Status == _01_Domain.Core.Enums.PaymentStatus.Completed))
                .ForMember(dest => dest.RefundedAmount, opt => opt.MapFrom(src => src.Amount.Value));

            // Value Objects to DTO Mapping (Nesting)
            CreateMap<ShippingAddress, OrderDetailResponseDto.ShippingAddressDto>();

            // Request DTO to Entity/ValueObject Mapping
            CreateMap<CreateOrderRequestDto, ShippingAddress>();

            CreateMap<AddItemToBasketRequestDto, BasketItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BasketId, opt => opt.Ignore());

            // Mapping for Report/Statistics
            CreateMap<Order, SellerOrdersResponseDto>()
                .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.OrderNumber.Value))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount.Value))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate ?? src.CreatedAt))
                .ForMember(dest => dest.BuyerFullName, opt => opt.MapFrom(src => $"{src.ShippingAddress.FirstName} {src.ShippingAddress.LastName}"))
                .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src => src.Items.Count));

            CreateMap<Order, OrderHistoryResponseDto>()
                .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.OrderNumber.Value))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.FinalAmount, opt => opt.MapFrom(src => src.FinalAmount.Value))
                .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(src => src.UpdatedAt));
        }
    }
}