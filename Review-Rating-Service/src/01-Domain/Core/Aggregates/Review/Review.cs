using Review_Rating_Service.src._01_Domain.Core.Common;
using Review_Rating_Service.src._01_Domain.Core.Enums;
using Review_Rating_Service.src._01_Domain.Core.Events;
using Review_Rating_Service.src._01_Domain.Core.ValueObjects;

namespace Review_Rating_Service.src._01_Domain.Core.Aggregates.Review
{
    public class Review : AggregateRoot
    {
        public int ProductId { get; private set; }
        public Guid UserId { get; private set; }
        public ReviewerName ReviewerName { get; private set; }
        public ReviewDate ReviewDate { get; private set; }
        public ReviewStatus Status { get; private set; }

        private readonly List<Rating> _ratings = new();
        public IReadOnlyCollection<Rating> Ratings => _ratings.AsReadOnly();

        private readonly List<ReviewComment> _comments = new();
        public IReadOnlyCollection<ReviewComment> Comments => _comments.AsReadOnly();

        private readonly List<ReviewAttachment> _attachments = new();
        public IReadOnlyCollection<ReviewAttachment> Attachments => _attachments.AsReadOnly();

        private Review() { }

        public Review(int productId, Guid userId, ReviewerName reviewerName, ReviewText text)
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            UserId = userId;
            ReviewerName = reviewerName;
            ReviewDate = new ReviewDate(DateTime.UtcNow);
            Status = ReviewStatus.Pending;

            // Stored separately for querying simplicity, validation handled by VO in constructor
            Text = text.Value;

            AddDomainEvent(new ReviewCreatedEvent(Id, ProductId, UserId));
        }

        // Public property for EF Core mapping, logic handled by VO
        public string Text { get; private set; }

        public void UpdateText(ReviewText newText)
        {
            Text = newText.Value;
            SetUpdatedAt();
            AddDomainEvent(new ReviewUpdatedEvent(Id));
        }

        public void AddRating(Rating rating)
        {
            if (_ratings.Any(r => r.Type == rating.Type))
                throw new InvalidOperationException($"Rating of type {rating.Type} already exists for this review.");

            _ratings.Add(rating);
            // اصلاح شده: چون rating.Value از نوع int است، دیگر .Value ندارد
            AddDomainEvent(new RatingAddedEvent(Id, rating.Value));
        }

        public void AddComment(ReviewComment comment)
        {
            _comments.Add(comment);
        }

        public void AddAttachment(ReviewAttachment attachment)
        {
            if (_attachments.Count >= 5)
                throw new InvalidOperationException("Maximum of 5 attachments allowed.");
            _attachments.Add(attachment);
        }

        public void Approve()
        {
            if (Status != ReviewStatus.Pending)
                throw new InvalidOperationException("Only pending reviews can be approved.");

            Status = ReviewStatus.Approved;
            SetUpdatedAt();
        }

        public void Reject()
        {
            if (Status != ReviewStatus.Pending)
                throw new InvalidOperationException("Only pending reviews can be rejected.");

            Status = ReviewStatus.Rejected;
            SetUpdatedAt();
        }

        public void Archive()
        {
            if (Status != ReviewStatus.Approved)
                throw new InvalidOperationException("Only approved reviews can be archived.");

            Status = ReviewStatus.Archived;
            SetUpdatedAt();
        }
    }
}
