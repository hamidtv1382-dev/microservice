using Order_Service.src._01_Domain.Core.Common;

namespace Order_Service.src._01_Domain.Core.ValueObjects
{
    public class Money : ValueObject
    {
        public decimal Value { get; private set; }
        public string Currency { get; private set; }

        protected Money()
        {
            Currency = "IRR";
        }

        public Money(decimal value, string currency = "IRR")
        {
            if (value < 0)
                throw new ArgumentException("Money value cannot be negative.", nameof(value));

            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Currency cannot be empty.", nameof(currency));

            Value = decimal.Round(value, 0);
            Currency = currency;
        }

        public static Money Zero() => new Money(0);

        public static Money operator +(Money left, Money right)
        {
            if (left.Currency != right.Currency)
                throw new InvalidOperationException("Cannot add money with different currencies.");

            return new Money(left.Value + right.Value, left.Currency);
        }

        public static Money operator -(Money left, Money right)
        {
            if (left.Currency != right.Currency)
                throw new InvalidOperationException("Cannot subtract money with different currencies.");

            return new Money(left.Value - right.Value, left.Currency);
        }

        public static Money operator *(Money money, int multiplier)
        {
            return new Money(money.Value * multiplier, money.Currency);
        }

        public static Money operator *(int multiplier, Money money)
        {
            return money * multiplier;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
            yield return Currency;
        }
    }
}