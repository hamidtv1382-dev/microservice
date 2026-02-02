namespace Order_Service.src._02_Application.DTOs.Requests
{
    public class GetSellerOrdersRequestDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
