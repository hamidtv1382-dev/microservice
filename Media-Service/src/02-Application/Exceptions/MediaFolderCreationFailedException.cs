namespace Media_Service.src._02_Application.Exceptions
{
    public class MediaFolderCreationFailedException : Exception
    {
        public MediaFolderCreationFailedException(string message) : base(message) { }
        public MediaFolderCreationFailedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
