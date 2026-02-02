namespace Order_Service.src._02_Application.DTOs.Responses
{
    public class PaymentResponseDto
    {
        public Guid PaymentId { get; set; }
        public bool IsSuccessful { get; set; }
        public string? TransactionId { get; set; }
        public string Message { get; set; }
        public string? PaymentUrl { get; set; }
    }
}
