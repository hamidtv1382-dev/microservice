namespace Order_Service.src._02_Application.Exceptions
{
    public class InsufficientStockException : Exception
    {
        public int ProductId { get; }
        public int RequestedQuantity { get; }

        public InsufficientStockException(int productId, int requestedQuantity)
            : base($"Insufficient stock for product ID '{productId}'. Requested: {requestedQuantity}.")
        {
            ProductId = productId;
            RequestedQuantity = requestedQuantity;
        }

        public InsufficientStockException(string message) : base(message)
        {
        }
    }
}
