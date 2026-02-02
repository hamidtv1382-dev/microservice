namespace Payment_Service.src._02_Application.DTOs.Responses
{
    public class TransactionSummaryResponseDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime ProcessedAt { get; set; }
        public string Status { get; set; }
    }
}
