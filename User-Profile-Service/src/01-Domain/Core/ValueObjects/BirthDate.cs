using User_Profile_Service.src._01_Domain.Core.Common;

namespace User_Profile_Service.src._01_Domain.Core.ValueObjects
{
    public class BirthDate : ValueObject
    {
        public DateTime Date { get; private set; }

        public BirthDate(DateTime date)
        {
            if (date > DateTime.UtcNow.AddYears(-13)) // Assuming minimum age logic
                throw new ArgumentException("User must be at least 13 years old.", nameof(date));

            Date = date;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Date;
        }
    }
}
