namespace Order_Service.src._02_Application.DTOs.Responses
{
    public class OrderDetailResponseDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public string? Description { get; set; }

        public ShippingAddressDto ShippingAddress { get; set; }
        public List<OrderItemResponseDto> Items { get; set; }

        public class ShippingAddressDto
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string PhoneNumber { get; set; }
            public string Country { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Street { get; set; }
            public string ZipCode { get; set; }
        }

        public class OrderItemResponseDto
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public string? ImageUrl { get; set; }
            public decimal UnitPrice { get; set; }
            public int Quantity { get; set; }
            public decimal TotalPrice { get; set; }
            public string SellerId { get; set; }
        }
    }
}