namespace Order_Service.src._02_Application.DTOs.Responses
{
    public class CompletedOrdersResponseDto
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public decimal FinalAmount { get; set; }
        public DateTime DeliveredDate { get; set; }
    }
}
