using System.ComponentModel.DataAnnotations;

namespace Wallet_Service.src._02_Application.DTOs.Requests
{
    public class GetWalletTransactionsRequestDto
    {
        [Required]
        public Guid WalletId { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
