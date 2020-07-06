using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GameTrove.Api.Infrastructure;
using GameTrove.Api.Models;
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

        public UsersController(IAuthenticatedMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("verification")]
        public async Task<IActionResult> Get()
        {
            if (await _mediator.Send(
                new VerifyUser
                {
                    Email = User.Claims.Single(c => c.Type == ClaimTypes.Email).Value
                }
            ))
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

        [HttpPut("invite")]
        public async Task<IActionResult> AcceptInvite(AcceptModel model)
        {
            var result = await _mediator.Send(new RegisterUser
            {
                Email = model.Email,
                Token = model.Token
            });

            return result != null ? (ActionResult)Ok() : Unauthorized();
        }
    }
}