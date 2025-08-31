using Craft.Application.Operations.Commands.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Craft.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PasswordController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Commands
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        #endregion

        #region Queries
        #endregion
    }
}
