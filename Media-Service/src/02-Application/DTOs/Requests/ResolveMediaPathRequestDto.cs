using Media_Service.src._01_Domain.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Media_Service.src._02_Application.DTOs.Requests
{
    public class ResolveMediaPathRequestDto
    {
        [Required]
        public MediaOwnerType OwnerType { get; set; }

        [Required]
        public Guid OwnerId { get; set; }

        public Guid? CategoryId { get; set; }
        public Guid? SubCategoryId { get; set; }
    }
}
