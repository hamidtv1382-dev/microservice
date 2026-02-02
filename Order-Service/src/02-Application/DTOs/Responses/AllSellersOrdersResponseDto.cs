namespace Order_Service.src._02_Application.DTOs.Responses
{
    public class AllSellersOrdersResponseDto
    {
        public List<SellerOrdersResponseDto> Orders { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
    }
}
