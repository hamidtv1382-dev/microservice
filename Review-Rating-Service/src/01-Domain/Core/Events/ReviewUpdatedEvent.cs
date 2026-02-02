namespace Review_Rating_Service.src._01_Domain.Core.Events
{
    public class ReviewUpdatedEvent
    {
        public Guid ReviewId { get; set; }
        public DateTime OccurredOn { get; set; }

        public ReviewUpdatedEvent(Guid reviewId)
        {
            ReviewId = reviewId;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
