namespace Payment_Service.src._02_Application.DTOs.Requests
{
    public class GetTransactionsRequestDto
    {
        public Guid? PaymentId { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
