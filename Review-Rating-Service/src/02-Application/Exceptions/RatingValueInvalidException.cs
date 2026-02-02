namespace Review_Rating_Service.src._02_Application.Exceptions
{
    public class RatingValueInvalidException : Exception
    {
        public RatingValueInvalidException(string message) : base(message) { }
        public RatingValueInvalidException(string message, Exception innerException) : base(message, innerException) { }
    }
}
