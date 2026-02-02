using Seller_Finance_Service.src._01_Domain.Core.Entities;
using Seller_Finance_Service.src._02_Application.DTOs.Responses;

namespace Seller_Finance_Service.src._02_Application.Mappings
{
    public class SellerFinanceMappingProfile
    {
        public SellerTransactionResponseDto MapToTransactionResponseDto(SellerTransaction transaction)
        {
            return new SellerTransactionResponseDto
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                Type = transaction.Type,
                Status = transaction.Status,
                ReferenceId = transaction.ReferenceId,
                Description = transaction.Description,
                TransactionDate = transaction.TransactionDate
            };
        }

        public SellerPayoutResponseDto MapToPayoutResponseDto(SellerPayout payout)
        {
            return new SellerPayoutResponseDto
            {
                Id = payout.Id,
                Amount = payout.Amount,
                Status = payout.Status,
                RequestedAt = payout.RequestedAt,
                ProcessedAt = payout.ProcessedAt,
                FailureReason = payout.FailureReason
            };
        }
    }
}
