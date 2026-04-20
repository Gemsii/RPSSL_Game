using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RPSSL.API.Features.Games.GetScoreboard;
using RPSSL.API.Features.Games.Play;
using RPSSL.API.Features.Games.ResetScoreboard;

namespace RPSSL.API.Features.Games
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly PlayGameHandler _playGameHandler;
        private readonly IValidator<PlayGameCommand> _validator;
        private readonly GetScoreboardHandler _getScoreboardHandler;
        private readonly ResetScoreboardHandler _resetScoreboardHandler;

        public GamesController(
            PlayGameHandler playGameHandler,
            IValidator<PlayGameCommand> validator,
            GetScoreboardHandler getScoreboardHandler,
            ResetScoreboardHandler resetScoreboardHandler)
        {
            _playGameHandler = playGameHandler;
            _validator = validator;
            _getScoreboardHandler = getScoreboardHandler;
            _resetScoreboardHandler = resetScoreboardHandler;
        }

        [HttpPost("play")]
        public async Task<IActionResult> Play([FromBody] PlayGameCommand command, CancellationToken ct)
        {
            var validation = await _validator.ValidateAsync(command, ct);
            if (!validation.IsValid)
                return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

            var result = await _playGameHandler.Handle(command, ct);
            return Ok(result);
        }

        [HttpGet("scoreboard/{playerId:guid}")]
        public async Task<IActionResult> GetScoreboard(Guid playerId, [FromQuery] int page = 1, CancellationToken ct = default)
        {
            var result = await _getScoreboardHandler.Handle(new GetScoreboardQuery { PlayerId = playerId, Page = page }, ct);
            return Ok(result);
        }

        [HttpDelete("scoreboard/{playerId:guid}")]
        public async Task<IActionResult> ResetScoreboard(Guid playerId, CancellationToken ct)
        {
            await _resetScoreboardHandler.Handle(new ResetScoreboardCommand { PlayerId = playerId }, ct);
            return Ok();
        }
    }
}
