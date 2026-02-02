using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Seller_Finance_Service.src._01_Domain.Core.ValueObjects;

namespace Seller_Finance_Service.src._03_Infrastructure.Data
{
    public class MoneyValueConverter : ValueConverter<Money, decimal>
    {
        public MoneyValueConverter()
            : base(
                v => v.Amount,                 // To Database: Take the decimal amount
                v => new Money(v, "IRR"))      // From Database: Reconstruct Money object with default currency
        {
        }
    }
}
