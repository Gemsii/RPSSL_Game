using RPSSL.API.Features.Games.Play;

namespace RPSSL.Tests.Features
{
    public class PlayGameCommandValidatorTests
    {
        private readonly PlayGameCommandValidator _sut = new();

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        public async Task Validate_Passes_WhenChoiceIsWithinRange(int choice)
        {
            var result = await _sut.ValidateAsync(new PlayGameCommand { Choice = choice });

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(6)]
        [InlineData(-1)]
        [InlineData(100)]
        public async Task Validate_Fails_WhenChoiceIsOutOfRange(int choice)
        {
            var result = await _sut.ValidateAsync(new PlayGameCommand { Choice = choice });

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Choice must be between 1 and 5.");
        }
    }
}
