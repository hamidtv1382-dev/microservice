using Wallet_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Wallet_Service.src._01_Domain.Services.Interfaces;
using Wallet_Service.src._02_Application.DTOs.Requests;
using Wallet_Service.src._02_Application.DTOs.Responses;
using Wallet_Service.src._02_Application.Exceptions;
using Wallet_Service.src._02_Application.Interfaces;
using Wallet_Service.src._02_Application.Mappings;
using Wallet_Service.src._02_Application.Services.Interfaces;

namespace Wallet_Service.src._02_Application.Services.Implementations
{
    public class WalletApplicationService : IWalletApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWalletDomainService _walletDomainService;
        private readonly IExternalPaymentGateway _externalPaymentGateway;
        private readonly WalletMappingProfile _mapper;

        public WalletApplicationService(IUnitOfWork unitOfWork, IWalletDomainService walletDomainService, IExternalPaymentGateway externalPaymentGateway, WalletMappingProfile mapper)
        {
            _unitOfWork = unitOfWork;
            _walletDomainService = walletDomainService;
            _externalPaymentGateway = externalPaymentGateway;
            _mapper = mapper;
        }

        public async Task<WalletBalanceResponseDto> GetBalanceAsync(Guid ownerId)
        {
            var wallet = await _unitOfWork.Wallets.GetByOwnerIdAsync(ownerId);

            if (wallet == null)
            {
                // Auto-create wallet if not exists
                wallet = await _walletDomainService.CreateWalletAsync(ownerId);
            }

            return new WalletBalanceResponseDto
            {
                WalletId = wallet.Id,
                OwnerId = wallet.OwnerId,
                Balance = wallet.Balance,
                IsActive = wallet.IsActive
            };
        }

        public async Task<WalletTransactionResponseDto> AddFundsAsync(AddFundsRequestDto request)
        {
            var wallet = await _unitOfWork.Wallets.GetByIdAsync(request.WalletId);
            if (wallet == null) throw new WalletNotFoundException("Wallet not found.");

            // Simulate external payment gateway interaction for top-up
            var gatewayResult = await _externalPaymentGateway.ProcessTopUpAsync(request.Amount, request.PaymentMethod);

            if (!gatewayResult.Success)
            {
                throw new Exception("Payment gateway failed.");
            }

            var success = await _walletDomainService.CreditWalletAsync(wallet.Id, request.Amount, gatewayResult.TransactionId);

            if (!success) throw new Exception("Failed to credit wallet.");

            // Fetch newly created transaction
            var transaction = await _unitOfWork.WalletTransactions.GetByWalletIdAsync(wallet.Id);
            var latestTransaction = transaction.OrderByDescending(t => t.CreatedAt).FirstOrDefault();

            return _mapper.MapToWalletTransactionResponseDto(latestTransaction);
        }

        public async Task<WalletTransactionResponseDto> DeductFundsAsync(DeductFundsRequestDto request)
        {
            var wallet = await _unitOfWork.Wallets.GetByIdAsync(request.WalletId);
            if (wallet == null) throw new WalletNotFoundException("Wallet not found.");

            if (wallet.Balance.Amount < request.Amount.Amount)
                throw new InsufficientWalletBalanceException("Insufficient funds.");

            var referenceId = Guid.NewGuid().ToString();

            var success = await _walletDomainService.DebitWalletAsync(wallet.Id, request.Amount, referenceId, request.Description);

            if (!success) throw new Exception("Failed to debit wallet.");

            var transactions = await _unitOfWork.WalletTransactions.GetByWalletIdAsync(wallet.Id);
            var latestTransaction = transactions.FirstOrDefault();

            return _mapper.MapToWalletTransactionResponseDto(latestTransaction);
        }

        public async Task<WalletTransactionResponseDto> DeductFundsByUserIdAsync(DeductFundsByUserIdRequestDto request)
        {
            var wallet = await _unitOfWork.Wallets.GetByOwnerIdAsync(request.UserId);

            if (wallet == null)
            {
                // اگر ولت وجود نداشت، خودکار می‌سازیم تا خطا ندهد (یا می‌توان خطا داد)
                wallet = await _walletDomainService.CreateWalletAsync(request.UserId);
            }

            if (wallet.Balance.Amount < request.Amount.Amount)
                throw new InsufficientWalletBalanceException("Insufficient funds.");

            var referenceId = Guid.NewGuid().ToString();
            var description = request.Description ?? "Deduction via UserId";

            var success = await _walletDomainService.DebitWalletAsync(wallet.Id, request.Amount, referenceId, description);

            if (!success) throw new Exception("Failed to debit wallet.");

            var transactions = await _unitOfWork.WalletTransactions.GetByWalletIdAsync(wallet.Id);
            var latestTransaction = transactions.FirstOrDefault();

            return _mapper.MapToWalletTransactionResponseDto(latestTransaction);
        }
    }
}