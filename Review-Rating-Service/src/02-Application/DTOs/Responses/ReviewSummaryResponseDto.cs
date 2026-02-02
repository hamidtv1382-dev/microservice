namespace Review_Rating_Service.src._02_Application.DTOs.Responses
{
    public class ReviewSummaryResponseDto
    {
        public int ProductId { get; set; }
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public Dictionary<int, int> RatingDistribution { get; set; } // e.g., {1: 5, 2: 2, 3: 0, 4: 10, 5: 50}
    }
}
