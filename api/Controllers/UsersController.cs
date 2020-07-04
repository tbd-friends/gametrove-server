using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GameTrove.Api.Infrastructure;
using GameTrove.Application.Commands;
using GameTrove.Application.Infrastructure;
using GameTrove.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameTrove.Api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IAuthenticatedMediator _mediator;
        private readonly AuthenticationService _service;

        public UsersController(IAuthenticatedMediator mediator, AuthenticationService service)
        {
            _mediator = mediator;
            _service = service;
        }

        [HttpGet("verification")]
        public IActionResult Get()
        {
            if (_service.Verify(User.Claims.Single(c => c.Type == ClaimTypes.Email).Value))
            {
                return Ok();
            }

            return Unauthorized();
        }

        [HttpGet("invite")]
        public async Task<ActionResult<string>> GetInvite()
        {
            return await _mediator.Send(new GenerateInviteToken());
        }
    }
}