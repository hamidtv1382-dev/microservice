using Media_Service.src._01_Domain.Core.Common;
using Media_Service.src._01_Domain.Core.Enums;
using Media_Service.src._01_Domain.Core.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace Media_Service.src._01_Domain.Core.Entities
{
    [Table("MediaFiles")]
    public class MediaFile : BaseEntity
    {
        public string OriginalFileName { get; private set; }
        public string StoredFileName { get; private set; }
        public string Extension { get; private set; }
        public long SizeBytes { get; private set; }
        public string RelativePath { get; private set; }
        public string AbsoluteUrl { get; private set; }
        public MediaFileType Type { get; private set; }
        public MediaOwnerType OwnerType { get; private set; }
        public Guid OwnerId { get; private set; }
        public MediaVisibility Visibility { get; private set; }
        public AuditInfo Audit { get; private set; }

        // Parameterless constructor for EF Core
        private MediaFile() { }

        // سازنده اصلاح شده برای Application Service
        // نام پارامترها باید با پراپرتی‌ها مطابقت داشته باشد یا به صورت صریح مقداردهی شوند
        public MediaFile(string originalName, string storedName, string extension, long size, string relativePath, string absoluteUrl, MediaFileType type, MediaOwnerType ownerType, Guid ownerId, MediaVisibility visibility, AuditInfo audit)
        {
            OriginalFileName = originalName;
            StoredFileName = storedName;
            Extension = extension;
            SizeBytes = size;
            RelativePath = relativePath;
            AbsoluteUrl = absoluteUrl;
            Type = type;
            OwnerType = ownerType;
            OwnerId = ownerId;
            Visibility = visibility;
            Audit = audit;
        }

        public void UpdatePath(string newRelativePath, string newAbsoluteUrl)
        {
            RelativePath = newRelativePath;
            AbsoluteUrl = newAbsoluteUrl;
            Audit.UpdateAudit("System");
        }
    }
}
