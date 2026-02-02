namespace Order_Service.src._02_Application.DTOs.Responses
{
    public class BasketDetailResponseDto
    {
        public Guid Id { get; set; }
        public List<BasketItemResponseDto> Items { get; set; }

        // 1. مبلغ کل آیتم‌ها (بدون تغییر)
        public decimal TotalAmount { get; set; }

        // 2. مبلغ تخفیف اعمال شده (جدید)
        public decimal DiscountAmount { get; set; }

        // 3. مبلغ نهایی قابل پرداخت (جدید)
        public decimal FinalAmount { get; set; }

        public DateTime? ExpiresAt { get; set; }
        public string? AppliedDiscountCode { get; set; }

        public class BasketItemResponseDto
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public string? ImageUrl { get; set; }
            public decimal UnitPrice { get; set; }
            public int Quantity { get; set; }
            public decimal TotalPrice { get; set; }
        }
    }
}
