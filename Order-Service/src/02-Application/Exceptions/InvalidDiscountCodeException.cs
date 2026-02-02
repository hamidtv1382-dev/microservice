namespace Order_Service.src._02_Application.Exceptions
{
    public class InvalidDiscountCodeException : Exception
    {
        public string Code { get; }

        public InvalidDiscountCodeException(string code)
            : base($"The discount code '{code}' is invalid, expired, or does not meet the requirements.")
        {
            Code = code;
        }

        public InvalidDiscountCodeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}