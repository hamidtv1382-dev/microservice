using Media_Service.src._01_Domain.Core.Common;
using Media_Service.src._01_Domain.Core.Enums;
using Media_Service.src._01_Domain.Core.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace Media_Service.src._01_Domain.Core.Entities
{
    [Table("MediaFolders")]
    public class MediaFolder : BaseEntity
    {
        public string FolderName { get; private set; }
        public string FullPhysicalPath { get; private set; }
        public MediaOwnerType OwnerType { get; private set; }
        public Guid? OwnerId { get; private set; }
        public Guid? ParentFolderId { get; private set; }
        public AuditInfo Audit { get; private set; }

        // Parameterless constructor for EF Core
        private MediaFolder() { }

        // سازنده اصلاح شده
        // نام پارامترها با نام پراپرتی‌ها یکی شد
        public MediaFolder(string folderName, string fullPhysicalPath, MediaOwnerType ownerType, Guid? ownerId = null, Guid? parentFolderId = null, AuditInfo audit = null)
        {
            FolderName = folderName;
            FullPhysicalPath = fullPhysicalPath;
            OwnerType = ownerType;
            OwnerId = ownerId;
            ParentFolderId = parentFolderId;
            Audit = audit ?? new AuditInfo("System");
        }
    }
}
