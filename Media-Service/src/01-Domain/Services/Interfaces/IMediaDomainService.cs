using Media_Service.src._01_Domain.Core.Enums;

namespace Media_Service.src._01_Domain.Services.Interfaces
{
    public interface IMediaDomainService
    {
        void ValidateFileExtension(string fileName, MediaFileType fileType);
        void ValidateFileSize(long sizeBytes);
    }
}
