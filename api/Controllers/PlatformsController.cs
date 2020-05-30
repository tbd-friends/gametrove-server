using System.Collections.Generic;
using System.Threading.Tasks;
using GameTrove.Application.Query;
using GameTrove.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("platforms")]
    public class PlatformsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PlatformsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<PlatformViewModel>> GetPlatforms()
        {
            return await _mediator.Send(new GetPlatforms());
        }
    }
}