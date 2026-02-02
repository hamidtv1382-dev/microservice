namespace Order_Service.src._02_Application.DTOs.Requests
{
    public class GetCompletedOrdersRequestDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public DateTime? FromDate { get; set; }
    }
}
