using Finance_Service.src._01_Domain.Core.Entities;
using Finance_Service.src._02_Application.DTOs.Responses;

namespace Finance_Service.src._02_Application.Mappings
{
    public class FinanceMappingProfile
    {
        public FeeResponseDto MapToFeeResponseDto(Fee fee)
        {
            return new FeeResponseDto
            {
                Id = fee.Id,
                OrderId = fee.OrderId,
                Amount = fee.Amount,
                Type = fee.Type,
                Description = fee.Description,
                IsPaid = fee.IsPaid,
                PaidAt = fee.PaidAt
            };
        }

        public List<FeeResponseDto> MapToFeeResponseDtoList(IEnumerable<Fee> fees)
        {
            return fees.Select(f => MapToFeeResponseDto(f)).ToList();
        }

        public CommissionResponseDto MapToCommissionResponseDto(Commission commission)
        {
            return new CommissionResponseDto
            {
                Id = commission.Id,
                OrderId = commission.OrderId,
                SellerId = commission.SellerId,
                Amount = commission.Amount,
                Type = commission.Type,
                RatePercentage = commission.RatePercentage,
                IsSettled = commission.IsSettled,
                SettledAt = commission.SettledAt
            };
        }

        public List<CommissionResponseDto> MapToCommissionResponseDtoList(IEnumerable<Commission> commissions)
        {
            return commissions.Select(c => MapToCommissionResponseDto(c)).ToList();
        }

        public SettlementResponseDto MapToSettlementResponseDto(Settlement settlement)
        {
            return new SettlementResponseDto
            {
                Id = settlement.Id,
                SellerId = settlement.SellerId,
                TotalAmount = settlement.TotalAmount,
                Status = settlement.Status,
                SettledAt = settlement.SettledAt,
                DueDate = settlement.DueDate
            };
        }

        public List<SettlementResponseDto> MapToSettlementResponseDtoList(IEnumerable<Settlement> settlements)
        {
            return settlements.Select(s => MapToSettlementResponseDto(s)).ToList();
        }
    }
}
