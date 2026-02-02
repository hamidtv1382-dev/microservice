namespace Review_Rating_Service.src._01_Domain.Core.Events
{
    public class ReviewDeletedEvent
    {
        public Guid ReviewId { get; set; }
        public DateTime OccurredOn { get; set; }

        public ReviewDeletedEvent(Guid reviewId)
        {
            ReviewId = reviewId;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
