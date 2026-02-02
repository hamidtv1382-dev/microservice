using AutoMapper;
using Order_Service.src._01_Domain.Core.Entities;
using Order_Service.src._01_Domain.Core.Interfaces.Repositories;
using Order_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Order_Service.src._01_Domain.Core.ValueObjects;
using Order_Service.src._02_Application.DTOs.Requests;
using Order_Service.src._02_Application.DTOs.Responses;
using Order_Service.src._02_Application.Services.Interfaces;

namespace Order_Service.src._02_Application.Services.Implementations
{
    public class AdminDiscountApplicationService : IAdminDiscountApplicationService
    {
        private readonly IAdminDiscountRepository _adminDiscountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AdminDiscountApplicationService(
            IAdminDiscountRepository adminDiscountRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _adminDiscountRepository = adminDiscountRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DiscountDetailResponseDto> CreateDiscountAsync(CreateDiscountRequestDto request)
        {
            // بررسی تکراری نبودن کد
            var existingDiscount = await _adminDiscountRepository.GetByCodeAsync(request.Code);
            if (existingDiscount != null)
                throw new InvalidOperationException("A discount with this code already exists.");

            var minimumOrderAmount = new Money(request.MinimumOrderAmount, "IRR");

            var discount = new Discount(
                Guid.NewGuid(),
                request.Code,
                request.Description,
                request.Type,
                request.Value,
                minimumOrderAmount,
                request.StartDate,
                request.EndDate,
                request.UsageLimit
            );

            await _adminDiscountRepository.AddAsync(discount);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<DiscountDetailResponseDto>(discount);
        }

        public async Task<DiscountDetailResponseDto?> GetDiscountByIdAsync(Guid id)
        {
            var discount = await _adminDiscountRepository.GetByIdAsync(id);
            return _mapper.Map<DiscountDetailResponseDto>(discount);
        }

        public async Task<IEnumerable<DiscountSummaryResponseDto>> GetAllDiscountsAsync()
        {
            var discounts = await _adminDiscountRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<DiscountSummaryResponseDto>>(discounts);
        }

        public async Task<DiscountDetailResponseDto?> UpdateDiscountAsync(Guid id, UpdateDiscountRequestDto request)
        {
            var discount = await _adminDiscountRepository.GetByIdAsync(id);
            if (discount == null)
                throw new KeyNotFoundException($"Discount with ID {id} not found.");

            // استفاده از متد داخلی UpdateDetails برای تغییر پراپرتی‌های Private
            Money? minAmount = request.MinimumOrderAmount.HasValue
                ? new Money(request.MinimumOrderAmount.Value, "IRR")
                : null;

            discount.UpdateDetails(
                request.Description,
                request.Type,
                request.Value,
                minAmount,
                request.StartDate,
                request.EndDate,
                request.UsageLimit
            );

            // مدیریت وضعیت فعال/غیرفعال
            if (request.IsActive.HasValue)
            {
                if (request.IsActive.Value)
                    discount.Activate();
                else
                    discount.Deactivate();
            }

            _adminDiscountRepository.Update(discount);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<DiscountDetailResponseDto>(discount);
        }

        public async Task DeleteDiscountAsync(Guid id)
        {
            var discount = await _adminDiscountRepository.GetByIdAsync(id);
            if (discount == null)
                throw new KeyNotFoundException($"Discount with ID {id} not found.");

            _adminDiscountRepository.Delete(discount);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
