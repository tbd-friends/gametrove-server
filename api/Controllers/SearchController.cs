using System.Collections.Generic;
using System.Threading.Tasks;
using GameTrove.Api.Models;
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

        [HttpPost("games")]
        public async Task<IEnumerable<GameViewModel>> SearchGames(SearchModel model)
        {
            return await _mediator.Send(new SearchForGames
            {
                Text = model.Text,
                MostRecentlyAdded = model.MostRecentlyAdded
            });
        }
    }
}
