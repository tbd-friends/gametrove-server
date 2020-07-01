using System.Linq;
using System.Security.Claims;
using GameTrove.Api.Infrastructure;
using GameTrove.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameTrove.Api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly AuthenticationService _service;

        public UsersController(AuthenticationService service)
        {
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
    }
}