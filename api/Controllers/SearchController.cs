using System.Collections.Generic;
using System.Threading.Tasks;
using GameTrove.Application.Query;
using GameTrove.Application.ViewModels;
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
        public async Task<IEnumerable<SearchResultViewModel>> SearchGames(string text)
        {
            return await _mediator.Send(new SearchForGame() { Text = text });
        }
    }
}