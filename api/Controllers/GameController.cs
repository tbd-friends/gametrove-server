using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Infrastructure;
using api.Models;
using GameTrove.Api.Models;
using GameTrove.Application.Commands;
using GameTrove.Application.Query;
using GameTrove.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        [HttpPost, Authorize(Roles = "Administrator,User")]
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

        [HttpGet("{id:guid}"), Authorize(Roles = "Administrator,User")]
        public async Task<ActionResult<GameViewModel>> GetGame(Guid id)
        {
            var result = await _mediator.Send(new RetrieveGameById() { Id = id });

            return result != null ? new ActionResult<GameViewModel>(result) : NotFound();
        }

        [HttpGet("{code}"), Authorize(Roles = "Administrator,User")]
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

        [HttpGet("{id}/title"), Authorize(Roles = "Administrator,User")]
        public async Task<ActionResult<TitleViewModel>> GetTitleForGame(Guid id)
        {
            var result = await _mediator.Send(new GetTitleForGame
            {
                GameId = id
            });

            return result != null ? new ActionResult<TitleViewModel>(result) : BadRequest();
        }

        [HttpPost("favorites"), Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> FavoriteGame(FavoriteModel model)
        {
            await _mediator.Send(new FavoriteGame
            {
                GameId = model.GameId
            });

            return Ok();
        }

        [HttpDelete("favorites"), Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> UnfavoriteGame(FavoriteModel model)
        {
            await _mediator.Send(new UnfavoriteGame
            {
                GameId = model.GameId
            });

            return Ok();
        }

        [HttpPost("{id}/images"), Authorize(Roles = "Administrator,User")]
        public async Task AddImageForGame(Guid id, [FromForm] IFormFile file)
        {
            await _mediator.Send(new AttachImageToGame
            {
                Id = id,
                Content = file.OpenReadStream(),
                FileName = file.FileName
            });
        }

        [HttpGet("{id}/images"), Authorize(Roles = "Administrator,User")]
        public async Task<IEnumerable<string>> GetImagesForGame(Guid id)
        {
            var paths = from x in await _mediator.Send(new GetImageIdentifiersForGame { Id = id })
                        select $"{HttpContext.Request.GetHost()}/images/{x}";

            return paths;
        }

        [HttpPost("{id}/copies"), Authorize(Roles = "Administrator,User")]
        public async Task<Guid> RegisterCopy(Guid id, RegisterCopyModel model)
        {
            return await _mediator.Send(new RegisterCopy
            {
                GameId = id,
                Tags = model.Tags,
                Cost = model.Cost,
                Purchased = model.Purchased
            });
        }

        [HttpGet("{id}/copies"), Authorize(Roles = "Administrator,User")]
        public async Task<IEnumerable<CopyViewModel>> GetCopies(Guid id)
        {
            return await _mediator.Send(new GetCopies
            {
                GameId = id
            });
        }

        [HttpPut("{id}/copies"), Authorize(Roles = "Administrator,User")]
        public async Task<CopyViewModel> UpdateCopy(Guid id, UpdateCopyModel model)
        {
            return await _mediator.Send(new UpdateCopy
            {
                Id = model.Id,
                Tags = model.Tags
            });
        }
    }
}