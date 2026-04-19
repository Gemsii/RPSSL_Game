using FluentValidation;

namespace RPSSL.API.Features.Games.Play
{
    public class PlayGameCommandValidator : AbstractValidator<PlayGameCommand>
    {
        public PlayGameCommandValidator()
        {
            RuleFor(x => x.Choice)
                .InclusiveBetween(1, 5)
                .WithMessage("Choice must be between 1 and 5.");
        }
    }
}
