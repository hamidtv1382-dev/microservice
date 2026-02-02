using Seller_Finance_Service.src._01_Domain.Core.Entities;
using Seller_Finance_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Seller_Finance_Service.src._01_Domain.Core.ValueObjects;
using Seller_Finance_Service.src._01_Domain.Services.Interfaces;
using Seller_Finance_Service.src._02_Application.DTOs.Requests;
using Seller_Finance_Service.src._02_Application.DTOs.Responses;
using Seller_Finance_Service.src._02_Application.Exceptions;
using Seller_Finance_Service.src._02_Application.Services.Interfaces;

namespace Seller_Finance_Service.src._02_Application.Services.Implementations
{
    public class SellerAccountApplicationService : ISellerAccountApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        // اگر بخواهیم از Domain Service برای ساخت اکانت استفاده کنیم تزریق می‌کنیم
        // اما متد CreateAccount در Domain Service شما وجود دارد، پس از آن استفاده می‌کنیم
        private readonly ISellerFinanceDomainService _sellerFinanceDomainService;

        public SellerAccountApplicationService(IUnitOfWork unitOfWork, ISellerFinanceDomainService sellerFinanceDomainService)
        {
            _unitOfWork = unitOfWork;
            _sellerFinanceDomainService = sellerFinanceDomainService;
        }

        public async Task<SellerAccountResponseDto> CreateAccountAsync(CreateSellerAccountRequestDto request)
        {
            // بررسی تکراری نبودن اکانت
            var existingAccount = await _unitOfWork.SellerAccounts.GetBySellerIdAsync(request.SellerId);
            if (existingAccount != null)
                throw new InvalidOperationException("Seller account already exists for this SellerId.");

            // ساخت ValueObject
            var bankInfo = new BankAccountInfo(
                request.AccountNumber,
                request.BankName,
                request.ShebaNumber,
                request.AccountHolderName
            );

            // استفاده از Domain Service برای ساخت اکانت (منطق دامنه)
            var account = await _sellerFinanceDomainService.CreateAccountAsync(request.SellerId, bankInfo);

            return MapToResponseDto(account);
        }

        public async Task<SellerAccountResponseDto?> GetAccountBySellerIdAsync(Guid sellerId)
        {
            var account = await _unitOfWork.SellerAccounts.GetBySellerIdAsync(sellerId);
            if (account == null) return null;

            return MapToResponseDto(account);
        }

        public async Task<SellerAccountResponseDto> UpdateBankAccountAsync(UpdateSellerBankAccountRequestDto request)
        {
            var account = await _unitOfWork.SellerAccounts.GetBySellerIdAsync(request.SellerId);
            if (account == null)
                throw new SellerAccountNotFoundException("Account not found.");

            // ساخت ValueObject جدید
            var newBankInfo = new BankAccountInfo(
                request.AccountNumber,
                request.BankName,
                request.ShebaNumber,
                request.AccountHolderName
            );

            // استفاده از متد موجودیت (Behaviors of Entity)
            account.UpdateBankAccount(newBankInfo);

            await _unitOfWork.SellerAccounts.UpdateAsync(account);
            await _unitOfWork.SaveChangesAsync();

            return MapToResponseDto(account);
        }

        public async Task ActivateAccountAsync(Guid sellerId)
        {
            var account = await _unitOfWork.SellerAccounts.GetBySellerIdAsync(sellerId);
            if (account == null) throw new SellerAccountNotFoundException("Account not found.");

            account.Activate();
            await _unitOfWork.SellerAccounts.UpdateAsync(account);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeactivateAccountAsync(Guid sellerId)
        {
            var account = await _unitOfWork.SellerAccounts.GetBySellerIdAsync(sellerId);
            if (account == null) throw new SellerAccountNotFoundException("Account not found.");

            account.Deactivate();
            await _unitOfWork.SellerAccounts.UpdateAsync(account);
            await _unitOfWork.SaveChangesAsync();
        }

        // Private Mapper Method
        private SellerAccountResponseDto MapToResponseDto(SellerAccount account)
        {
            return new SellerAccountResponseDto
            {
                Id = account.Id,
                SellerId = account.SellerId,
                IsActive = account.IsActive,
                BankAccount = account.BankAccount,
                AvailableBalance = account.Balance.AvailableBalance,
                PendingBalance = account.Balance.PendingBalance,
                HoldBalance = account.Balance.HoldBalance,
                CreatedAt = account.CreatedAt
            };
        }
    }
}
