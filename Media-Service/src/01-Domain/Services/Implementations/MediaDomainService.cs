using Media_Service.src._01_Domain.Core.Enums;
using Media_Service.src._01_Domain.Services.Interfaces;

namespace Media_Service.src._01_Domain.Services.Implementations
{
    public class MediaDomainService : IMediaDomainService
    {
        private readonly ILoggingService _loggingService;

        public MediaDomainService(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public void ValidateFileExtension(string fileName, MediaFileType fileType)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();

            if (fileType == MediaFileType.Image)
            {
                if (!new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" }.Contains(extension))
                    throw new ArgumentException($"Invalid image extension: {extension}");
            }
            else if (fileType == MediaFileType.Video)
            {
                if (!new[] { ".mp4", ".mov", ".avi", ".webm" }.Contains(extension))
                    throw new ArgumentException($"Invalid video extension: {extension}");
            }
        }

        public void ValidateFileSize(long sizeBytes)
        {
            // Max size 50MB for example
            if (sizeBytes > 52428800)
            {
                throw new ArgumentException("File size exceeds the maximum limit (50MB).");
            }
        }
    }
}
