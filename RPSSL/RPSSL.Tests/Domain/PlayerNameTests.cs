using RPSSL.API.Domain.ValueObjects;

namespace RPSSL.Tests.Domain
{
    public class PlayerNameTests
    {
        [Theory]
        [InlineData("Alice")]
        [InlineData("A")]
        [InlineData("Player 1")]
        public void Create_Succeeds_WithValidName(string name)
        {
            var playerName = PlayerName.Create(name);

            Assert.Equal(name, playerName.Value);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Create_Throws_WhenNameIsNullOrWhitespace(string? name)
        {
            var ex = Assert.Throws<ArgumentException>(() => PlayerName.Create(name!));

            Assert.Contains("required", ex.Message);
        }

        [Fact]
        public void Create_Throws_WhenNameExceeds100Characters()
        {
            var longName = new string('x', 101);

            var ex = Assert.Throws<ArgumentException>(() => PlayerName.Create(longName));

            Assert.Contains("too long", ex.Message);
        }

        [Fact]
        public void Create_Succeeds_AtExactly100Characters()
        {
            var name = new string('x', 100);

            var playerName = PlayerName.Create(name);

            Assert.Equal(100, playerName.Value.Length);
        }
    }
}
