using Microsoft.AspNetCore.Mvc;
using RPSSL.API.Features.Choices.GetChoices;

namespace RPSSL.API.Features.Choices
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChoicesController : ControllerBase
    {
        private readonly GetChoicesHandler _handler;

        public ChoicesController(GetChoicesHandler handler)
        {
            _handler = handler;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken ct)
        {
            var result = await _handler.Handle(new GetChoicesQuery(), ct);
            return Ok(result);
        }
    }
}
