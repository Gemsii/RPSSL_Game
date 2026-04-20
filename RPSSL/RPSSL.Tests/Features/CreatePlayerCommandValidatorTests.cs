using RPSSL.API.Features.Players.Create;

namespace RPSSL.Tests.Features
{
    public class CreatePlayerCommandValidatorTests
    {
        private readonly CreatePlayerCommandValidator _sut = new();

        [Theory]
        [InlineData("Alice")]
        [InlineData("A")]
        [InlineData("Bob 123")]
        public async Task Validate_Passes_WhenNameIsValid(string name)
        {
            var result = await _sut.ValidateAsync(new CreatePlayerCommand { Name = name });

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Validate_Fails_WhenNameIsEmpty(string name)
        {
            var result = await _sut.ValidateAsync(new CreatePlayerCommand { Name = name });

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Name is required.");
        }

        [Fact]
        public async Task Validate_Fails_WhenNameExceeds100Characters()
        {
            var longName = new string('x', 101);

            var result = await _sut.ValidateAsync(new CreatePlayerCommand { Name = longName });

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Name must not exceed 100 characters.");
        }

        [Fact]
        public async Task Validate_Passes_WhenNameIsExactly100Characters()
        {
            var name = new string('x', 100);

            var result = await _sut.ValidateAsync(new CreatePlayerCommand { Name = name });

            Assert.True(result.IsValid);
        }
    }
}
