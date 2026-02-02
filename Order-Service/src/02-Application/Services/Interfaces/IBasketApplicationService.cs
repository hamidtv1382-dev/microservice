using Order_Service.src._02_Application.DTOs.Requests;
using Order_Service.src._02_Application.DTOs.Responses;

namespace Order_Service.src._02_Application.Services.Interfaces
{
    public interface IBasketApplicationService
    {
        Task<BasketDetailResponseDto> GetBasketAsync(Guid buyerId);
        Task<BasketDetailResponseDto> AddItemAsync(Guid buyerId, AddItemToBasketRequestDto request);
        Task<BasketDetailResponseDto> UpdateItemAsync(Guid buyerId, UpdateBasketItemRequestDto request);
        Task<BasketDetailResponseDto> RemoveItemAsync(Guid buyerId, RemoveItemFromBasketRequestDto request);
        Task<BasketDetailResponseDto> ApplyDiscountAsync(Guid buyerId, ApplyDiscountRequestDto request);
        Task ClearBasketAsync(Guid buyerId);
    }
}