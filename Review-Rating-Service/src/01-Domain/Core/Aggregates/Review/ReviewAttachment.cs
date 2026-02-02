using Review_Rating_Service.src._01_Domain.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Review_Rating_Service.src._01_Domain.Core.Aggregates.Review
{
    public class ReviewAttachment
    {
        public Guid Id { get; private set; }
        public string Url { get; private set; }
        public AttachmentType Type { get; private set; }

        // EF Core Navigation
        public Guid ReviewId { get; set; }
        public Review Review { get; set; }

        public ReviewAttachment(string url, AttachmentType type)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("Attachment URL is required.");
            Id = Guid.NewGuid();
            Url = url;
            Type = type;
        }

        private ReviewAttachment() { } // EF Core
    }

}
