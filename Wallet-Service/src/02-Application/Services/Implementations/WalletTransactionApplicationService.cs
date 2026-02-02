using Wallet_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Wallet_Service.src._02_Application.DTOs.Requests;
using Wallet_Service.src._02_Application.DTOs.Responses;
using Wallet_Service.src._02_Application.Exceptions;
using Wallet_Service.src._02_Application.Mappings;
using Wallet_Service.src._02_Application.Services.Interfaces;

namespace Wallet_Service.src._02_Application.Services.Implementations
{
    public class WalletTransactionApplicationService : IWalletTransactionApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly WalletMappingProfile _mapper;

        public WalletTransactionApplicationService(IUnitOfWork unitOfWork, WalletMappingProfile mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WalletTransactionResponseDto>> GetTransactionsAsync(GetWalletTransactionsRequestDto request)
        {
            var wallet = await _unitOfWork.Wallets.GetByIdAsync(request.WalletId);
            if (wallet == null) throw new WalletNotFoundException("Wallet not found.");

            var transactions = await _unitOfWork.WalletTransactions.GetByWalletIdAsync(request.WalletId);

            // Pagination logic could be added here
            return transactions.Select(t => _mapper.MapToWalletTransactionResponseDto(t));
        }
    }
}
