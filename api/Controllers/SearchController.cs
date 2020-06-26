using System.Collections.Generic;
using System.Threading.Tasks;
using GameTrove.Api.Infrastructure;
using GameTrove.Api.Models;
using GameTrove.Application.Infrastructure;
using GameTrove.Application.Query;
using GameTrove.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("search")]
    public class SearchController : ControllerBase
    {
        private readonly IAuthenticatedMediator _mediator;

        public SearchController(IAuthenticatedMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("games"), Authorize(Roles = "Administrator,User")]
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
