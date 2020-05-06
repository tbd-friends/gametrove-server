using System.Collections.Generic;
using System.Threading.Tasks;
using api.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("search")]
    public class SearchController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SearchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Route("games")]
        public async Task<IEnumerable<string>> SearchGames(string text)
        {
            return await _mediator.Send<IEnumerable<string>>(new SearchForGame() { Text = text });
        }
    }
}