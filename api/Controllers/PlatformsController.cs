using System.Collections.Generic;
using System.Threading.Tasks;
using GameTrove.Application.Infrastructure;
using GameTrove.Application.Query;
using GameTrove.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameTrove.Api.Controllers
{
    [ApiController]
    [Route("platforms")]
    public class PlatformsController : ControllerBase
    {
        private readonly IAuthenticatedMediator _mediator;

        public PlatformsController(IAuthenticatedMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Authorize(Roles = "Administrator,User")]
        public async Task<IEnumerable<PlatformViewModel>> GetPlatforms()
        {
            return await _mediator.Send(new GetPlatforms());
        }

        [HttpGet("summary"), Authorize(Roles = "Administrator,User")]
        public async Task<IEnumerable<PlatformSummaryViewModel>> GetSummary()
        {
            return await _mediator.Send(new GetPlatformSummary());
        }
    }
}