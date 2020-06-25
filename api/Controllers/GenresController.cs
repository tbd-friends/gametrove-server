using System.Collections.Generic;
using System.Threading.Tasks;
using GameTrove.Application.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("genres")]
    public class GenresController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GenresController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Authorize(Roles = "Administrator,User")]
        public async Task<IEnumerable<string>> GetGenreLabels()
        {
            return await _mediator.Send(new GetGenreLabels());
        }
    }
}