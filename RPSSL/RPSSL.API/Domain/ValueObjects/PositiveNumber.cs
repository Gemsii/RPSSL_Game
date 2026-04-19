using RPSSL.API.Domain.Common;

namespace RPSSL.API.Domain.ValueObjects
{
    public class PositiveNumber : ValueObject
    {
        public int Value { get; }

        private PositiveNumber(int value)
        {
            Value = value;
        }

        public static PositiveNumber Create(int value)
        {
            if (value <= 0)
                throw new ArgumentException("Value must be greater than zero.", nameof(value));

            return new PositiveNumber(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}

