namespace Order_Service.src._02_Application.Exceptions
{
    public class BasketNotFoundException : Exception
    {
        public Guid BasketId { get; }

        public BasketNotFoundException(Guid basketId)
            : base($"Basket with ID '{basketId}' was not found.")
        {
            BasketId = basketId;
        }

        public BasketNotFoundException(string message) : base(message)
        {
        }
    }
}
