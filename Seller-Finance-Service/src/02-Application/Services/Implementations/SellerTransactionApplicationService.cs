using Seller_Finance_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Seller_Finance_Service.src._02_Application.DTOs.Requests;
using Seller_Finance_Service.src._02_Application.DTOs.Responses;
using Seller_Finance_Service.src._02_Application.Exceptions;
using Seller_Finance_Service.src._02_Application.Mappings;
using Seller_Finance_Service.src._02_Application.Services.Interfaces;

namespace Seller_Finance_Service.src._02_Application.Services.Implementations
{
    public class SellerTransactionApplicationService : ISellerTransactionApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SellerFinanceMappingProfile _mapper;

        public SellerTransactionApplicationService(IUnitOfWork unitOfWork, SellerFinanceMappingProfile mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SellerTransactionResponseDto>> GetTransactionsAsync(GetSellerTransactionsRequestDto request)
        {
            var account = await _unitOfWork.SellerAccounts.GetBySellerIdAsync(request.SellerId);
            if (account == null) throw new SellerAccountNotFoundException("Seller account not found.");

            var transactions = await _unitOfWork.SellerTransactions.GetBySellerAccountIdAsync(account.Id);

            // Simple pagination logic
            var pagedTransactions = transactions
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return pagedTransactions.Select(t => _mapper.MapToTransactionResponseDto(t));
        }
    }
}
