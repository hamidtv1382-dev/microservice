namespace Order_Service.src._02_Application.DTOs.Responses
{
    public class OrderSummaryResponseDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public string Status { get; set; }
        public decimal FinalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
