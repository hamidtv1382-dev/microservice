namespace Order_Service.src._02_Application.DTOs.Responses
{
    public class ApplyDiscountResponseDto
    {
        public bool Success { get; set; }
        public decimal NewTotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public string Message { get; set; }
    }
}
