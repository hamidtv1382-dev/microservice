using Media_Service.src._01_Domain.Core.Enums;

namespace Media_Service.src._02_Application.DTOs.Responses
{
    public class MediaFolderResponseDto
    {
        public Guid FolderId { get; set; }
        public string FolderName { get; set; }
        public string FullPhysicalPath { get; set; }
        public MediaOwnerType OwnerType { get; set; }
        public Guid? OwnerId { get; set; }
    }
}
