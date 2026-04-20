using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RPSSL.API.Features.Players.Create;

namespace RPSSL.API.Features.Players
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly CreatePlayerHandler _handler;
        private readonly IValidator<CreatePlayerCommand> _validator;

        public PlayersController(CreatePlayerHandler handler, IValidator<CreatePlayerCommand> validator)
        {
            _handler = handler;
            _validator = validator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePlayerCommand command, CancellationToken ct)
        {
            var validation = await _validator.ValidateAsync(command, ct);
            if (!validation.IsValid)
                return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

            var result = await _handler.Handle(command, ct);
            return Ok(result);
        }
    }
}
