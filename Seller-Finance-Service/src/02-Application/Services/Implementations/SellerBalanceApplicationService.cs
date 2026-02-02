using Seller_Finance_Service.src._01_Domain.Core.Enums;
using Seller_Finance_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Seller_Finance_Service.src._01_Domain.Core.ValueObjects;
using Seller_Finance_Service.src._01_Domain.Services.Interfaces;
using Seller_Finance_Service.src._02_Application.DTOs.Requests;
using Seller_Finance_Service.src._02_Application.DTOs.Responses;
using Seller_Finance_Service.src._02_Application.Exceptions;
using Seller_Finance_Service.src._02_Application.Mappings;
using Seller_Finance_Service.src._02_Application.Services.Interfaces;

namespace Seller_Finance_Service.src._02_Application.Services.Implementations
{
    public class SellerBalanceApplicationService : ISellerBalanceApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISellerFinanceDomainService _domainService;
        private readonly SellerFinanceMappingProfile _mapper;

        public SellerBalanceApplicationService(IUnitOfWork unitOfWork, ISellerFinanceDomainService domainService, SellerFinanceMappingProfile mapper)
        {
            _unitOfWork = unitOfWork;
            _domainService = domainService;
            _mapper = mapper;
        }

        public async Task<SellerBalanceResponseDto> GetBalanceAsync(Guid sellerId)
        {
            var account = await _unitOfWork.SellerAccounts.GetBySellerIdAsync(sellerId);
            if (account == null) throw new SellerAccountNotFoundException("Seller account not found.");

            return new SellerBalanceResponseDto
            {
                SellerId = account.SellerId,
                AvailableBalance = account.Balance.AvailableBalance,
                PendingBalance = account.Balance.PendingBalance,
                HoldBalance = account.Balance.HoldBalance
            };
        }

        public async Task<SellerTransactionResponseDto> RecordEarningAsync(RecordSellerEarningRequestDto request)
        {
            var success = await _domainService.RecordEarningAsync(request.SellerId, request.OrderId, new Money(request.Amount, "IRR"));
            if (!success) throw new Exception("Failed to record earning.");

            // Fetch the latest transaction
            var txs = await _unitOfWork.SellerTransactions.GetByReferenceIdAsync(request.OrderId.ToString());
            var latestTx = txs.OrderByDescending(t => t.TransactionDate).FirstOrDefault();

            return _mapper.MapToTransactionResponseDto(latestTx);
        }

        public async Task<SellerTransactionResponseDto> ReleaseBalanceAsync(ReleaseSellerBalanceRequestDto request)
        {
            var success = await _domainService.ReleaseBalanceAsync(request.SellerId, request.OrderId);
            if (!success) throw new SellerBalanceHoldException("Failed to release balance. Earning not found or already released.");

            var txs = await _unitOfWork.SellerTransactions.GetByReferenceIdAsync(request.OrderId.ToString());
            var latestTx = txs.Where(t => t.Type == SellerTransactionType.BalanceRelease).OrderByDescending(t => t.TransactionDate).FirstOrDefault();

            return _mapper.MapToTransactionResponseDto(latestTx);
        }

        public async Task<bool> HoldBalanceAsync(Guid sellerId, decimal amount, string reason, string description)
        {
            // Determine HoldReasonType from string (simplified)
            var holdReason = HoldReasonType.ManualAdmin;
            return await _domainService.HoldBalanceAsync(sellerId, new Money(amount, "IRR"), holdReason, description);
        }
    }
}
