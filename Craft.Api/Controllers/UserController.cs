using Craft.Application.Operations.Queries.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Craft.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Queries
        [HttpGet("GetUserList")]
        public async Task<IActionResult> GetUserList([FromQuery] GetUserListQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
        #endregion

    }
}
