namespace Order_Service.src._02_Application.DTOs.Responses
{
    public class OrderHistoryResponseDto
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string Status { get; set; }
        public decimal FinalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool CanCancel { get; set; }
        public bool CanRefund { get; set; }
    }
}
