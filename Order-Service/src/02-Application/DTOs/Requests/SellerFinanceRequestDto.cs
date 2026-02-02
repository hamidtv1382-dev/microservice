namespace Order_Service.src._02_Application.DTOs.Requests
{
    public class RecordSellerEarningRequestDto
    {
        public Guid SellerId { get; set; }
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
        public Guid TransactionId { get; set; }
    }
}
