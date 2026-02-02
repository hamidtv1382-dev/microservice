namespace Order_Service.src._02_Application.DTOs.Responses
{
    public class SellerOrdersResponseDto
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public string BuyerFullName { get; set; }
        public int ItemCount { get; set; }
    }
}
