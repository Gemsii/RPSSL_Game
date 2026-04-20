using RPSSL.API.Domain.ValueObjects;

namespace RPSSL.Tests.Domain
{
    public class PositiveNumberTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(50)]
        [InlineData(100)]
        public void Create_Succeeds_WithPositiveValue(int value)
        {
            var number = PositiveNumber.Create(value);

            Assert.Equal(value, number.Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Create_Throws_WhenValueIsNotPositive(int value)
        {
            var ex = Assert.Throws<ArgumentException>(() => PositiveNumber.Create(value));

            Assert.Contains("greater than zero", ex.Message);
        }
    }
}
