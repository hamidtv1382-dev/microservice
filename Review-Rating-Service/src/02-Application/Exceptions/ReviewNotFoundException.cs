namespace Review_Rating_Service.src._02_Application.Exceptions
{
    public class ReviewNotFoundException : Exception
    {
        public ReviewNotFoundException(string message) : base(message) { }
        public ReviewNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
