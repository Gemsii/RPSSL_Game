using RPSSL.API.Domain.Common;

namespace RPSSL.API.Domain.ValueObjects
{
    public class PlayerName : ValueObject
    {
        public string Value { get; }

        private PlayerName(string value)
        {
            Value = value;
        }

        public static PlayerName Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Player name is required");

            if (value.Length > 100)
                throw new ArgumentException("Player name is too long");

            return new PlayerName(value);
        }

        public override string ToString() => Value;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
