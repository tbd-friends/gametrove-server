using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Commands;
using api.Models;
using api.Query;
using api.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("games")]
    public class GameController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GameController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddNewGame(GameModel model)
        {
            var result = await _mediator.Send(new RegisterGame
            {
                Name = model.Name,
                Description = model.Description,
                Code = model.Code
            });

            return result != null ? (IActionResult)Created($"/games/codes/{result.Code}", result) : Conflict();
        }

        [HttpGet, Route("{id}")]
        public async Task<ActionResult<GameViewModel>> GetGame(Guid id)
        {
            var result = await _mediator.Send(new RetrieveGameById() { Id = id });

            return result != null ? new ActionResult<GameViewModel>(result) : NotFound();
        }

        [HttpGet, Route("last/{count}")]
        public async Task<IEnumerable<GameViewModel>> GetLastXGames(int count = 10)
        {
            return await _mediator.Send(new RetrieveRecentlyAddedGames() { Limit = count });
        }

        [HttpGet, Route("codes/{code}")]
        public async Task<ActionResult<GameViewModel>> GetGameByCode(string code)
        {
            var game = await _mediator.Send(new RetrieveGameByCode
            {
                Code = code
            });

            if (game != null)
            {
                return Ok(game);
            }

            return NotFound();
        }
    }
}