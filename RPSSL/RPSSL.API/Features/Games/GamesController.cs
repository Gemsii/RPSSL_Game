using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RPSSL.API.Features.Games.Play;

namespace RPSSL.API.Features.Games
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly PlayGameHandler _handler;
        private readonly IValidator<PlayGameCommand> _validator;

        public GamesController(PlayGameHandler handler, IValidator<PlayGameCommand> validator)
        {
            _handler = handler;
            _validator = validator;
        }

        [HttpPost("play")]
        public async Task<IActionResult> Play([FromBody] PlayGameCommand command, CancellationToken ct)
        {
            var validation = await _validator.ValidateAsync(command, ct);
            if (!validation.IsValid)
                return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

            var result = await _handler.Handle(command, ct);
            return Ok(result);
        }
    }
}
