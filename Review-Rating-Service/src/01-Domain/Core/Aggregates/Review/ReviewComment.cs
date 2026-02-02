namespace Review_Rating_Service.src._01_Domain.Core.Aggregates.Review
{
    public class ReviewComment
    {
        public Guid Id { get; private set; }
        public string Content { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Guid AuthorId { get; private set; } // e.g., Admin or Seller

        public ReviewComment(Guid authorId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Comment content is required.");

            Id = Guid.NewGuid();
            AuthorId = authorId;
            Content = content;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Comment content is required.");
            Content = content;
        }
    }
}
