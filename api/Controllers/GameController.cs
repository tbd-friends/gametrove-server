using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Infrastructure;
using api.Models;
using GameTrove.Application.Commands;
using GameTrove.Application.Query;
using GameTrove.Application.ViewModels;
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
                Subtitle = model.Subtitle,
                Code = model.Code,
                Platform = model.Platform
            });

            return result != null ? (IActionResult)Created($"/games/codes/{result.Code}", result) : Conflict();
        }

        [HttpGet, Route("{id}")]
        public async Task<ActionResult<GameViewModel>> GetGame(Guid id)
        {
            var result = await _mediator.Send(new RetrieveGameById() { Id = id });

            return result != null ? new ActionResult<GameViewModel>(result) : NotFound();
        }

        [HttpGet("{id}/title")]
        public async Task<ActionResult<TitleViewModel>> GetTitleForGame(Guid id)
        {
            var result = await _mediator.Send(new GetTitleForGame
            {
                GameId = id
            });

            return result != null ? new ActionResult<TitleViewModel>(result) : BadRequest();
        }

        [HttpPost("favorite/{id}")]
        public async Task<IActionResult> FavoriteGame(Guid id)
        {
            await _mediator.Send(new FavoriteGame
            {
                GameId = id
            });

            return Ok();
        }

        [HttpDelete("favorite/{id}")]
        public async Task<IActionResult> UnfavoriteGame(Guid id)
        {
            await _mediator.Send(new UnfavoriteGame
            {
                GameId = id
            });

            return Ok();
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

        [HttpGet, Route("images/{id}")]
        public async Task<IEnumerable<string>> GetImagesForGame(Guid id)
        {
            var paths = from x in await _mediator.Send(new GetImageIdentifiersForGame { Id = id })
                        select $"{HttpContext.Request.GetHost()}/images/{x}";

            return paths;
        }
    }
}