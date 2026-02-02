using Review_Rating_Service.src._01_Domain.Core.Enums;
using Review_Rating_Service.src._01_Domain.Core.ValueObjects;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Review_Rating_Service.src._01_Domain.Core.Aggregates.Review
{
    public class Rating
    {
        public Guid Id { get; private set; }
        public RatingType Type { get; private set; }
        public int Value { get; private set; }

        // EF Core Navigation
        public Guid ReviewId { get; set; }
        public Review Review { get; set; }

        public Rating(RatingType type, RatingValue value)
        {
            Id = Guid.NewGuid();
            Type = type;
            Value = value.Value;
        }

        private Rating() { } // EF Core
    }

}
