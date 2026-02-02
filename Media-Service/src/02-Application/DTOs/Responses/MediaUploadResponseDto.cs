using Media_Service.src._01_Domain.Core.Enums;

namespace Media_Service.src._02_Application.DTOs.Responses
{
    public class MediaUploadResponseDto
    {
        public Guid MediaFileId { get; set; }
        public string FileName { get; set; }
        public string FullPath { get; set; }
        public string AbsoluteUrl { get; set; }
        public MediaFileType Type { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
