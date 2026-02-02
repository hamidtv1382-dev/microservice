namespace Review_Rating_Service.src._01_Domain.Core.Events
{
    public class ReviewCreatedEvent
    {
        public Guid ReviewId { get; set; }
        public int ProductId { get; set; }
        public Guid UserId { get; set; }
        public DateTime OccurredOn { get; set; }

        public ReviewCreatedEvent(Guid reviewId, int productId, Guid userId)
        {
            ReviewId = reviewId;
            ProductId = productId;
            UserId = userId;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
