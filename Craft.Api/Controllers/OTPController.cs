using Craft.Application.Operations.Commands.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Craft.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OTPController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OTPController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("RequestOTP")]
        public async Task<IActionResult> RequestOTP([FromBody] RequestOTPCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
