using System.ComponentModel.DataAnnotations;

namespace Order_Service.src._02_Application.DTOs.Requests
{
    public class UpdateBasketItemRequestDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
}
