using Microsoft.AspNetCore.Mvc;
using RPSSL.API.Features.Choices.GetChoices;
using RPSSL.API.Features.Choices.GetRandomChoice;

namespace RPSSL.API.Features.Choices
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChoicesController : ControllerBase
    {
        private readonly GetChoicesHandler _handler;
        private readonly GetRandomChoiceHandler _randomChoiceHandler;

        public ChoicesController(GetChoicesHandler handler, GetRandomChoiceHandler randomChoiceHandler)
        {
            _handler = handler;
            _randomChoiceHandler = randomChoiceHandler;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken ct)
        {
            var result = await _handler.Handle(new GetChoicesQuery(), ct);
            return Ok(result);
        }

        [HttpGet("random")]
        public async Task<IActionResult> GetRandom(CancellationToken ct)
        {
            var result = await _randomChoiceHandler.Handle(new GetRandomChoiceQuery(), ct);
            return Ok(result);
        }
    }
}

