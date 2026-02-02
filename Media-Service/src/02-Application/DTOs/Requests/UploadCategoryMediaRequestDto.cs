using Media_Service.src._01_Domain.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Media_Service.src._02_Application.DTOs.Requests
{
    public class UploadCategoryMediaRequestDto
    {
        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string FileContentBase64 { get; set; }

        [Required]
        public MediaFileType Type { get; set; }
    }
}
