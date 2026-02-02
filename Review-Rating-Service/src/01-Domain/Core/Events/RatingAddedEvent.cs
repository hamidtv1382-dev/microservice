namespace Review_Rating_Service.src._01_Domain.Core.Events
{
    public class RatingAddedEvent
    {
        public Guid ReviewId { get; set; }
        public int Rating { get; set; }
        public DateTime OccurredOn { get; set; }

        public RatingAddedEvent(Guid reviewId, int rating)
        {
            ReviewId = reviewId;
            Rating = rating;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
