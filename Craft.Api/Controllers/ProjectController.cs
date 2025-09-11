using Craft.Application.Operations.Commands.Requests;
using Craft.Application.Operations.Queries.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Craft.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProjectController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Commands
        [HttpPost("CreateProject")]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        #endregion

        #region Queries
        #endregion
    }
}
