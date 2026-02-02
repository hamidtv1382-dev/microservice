namespace Order_Service.src._02_Application.DTOs.Responses
{
    public class TrackOrderResponseDto
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string CurrentStatus { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public List<OrderTimelineDto> History { get; set; }

        public class OrderTimelineDto
        {
            public string Status { get; set; }
            public DateTime Date { get; set; }
            public string? Description { get; set; }
        }
    }
}
