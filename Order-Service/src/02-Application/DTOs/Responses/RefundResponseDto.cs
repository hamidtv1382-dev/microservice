namespace Order_Service.src._02_Application.DTOs.Responses
{
    public class RefundResponseDto
    {
        public Guid RefundId { get; set; }
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public decimal RefundedAmount { get; set; }
    }
}
