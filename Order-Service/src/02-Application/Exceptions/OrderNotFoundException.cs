namespace Order_Service.src._02_Application.Exceptions
{
    public class OrderNotFoundException : Exception
    {
        public Guid OrderId { get; }

        public OrderNotFoundException(Guid orderId)
            : base($"Order with ID '{orderId}' was not found.")
        {
            OrderId = orderId;
        }

        public OrderNotFoundException(string message) : base(message)
        {
        }
    }
}
