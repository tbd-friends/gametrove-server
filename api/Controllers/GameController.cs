using System.Threading.Tasks;
using api.Commands;
using api.Models;
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
        public async Task AddNewGame(GameModel model)
        {
            await _mediator.Send(new RegisterGame
            {
                Name = model.Name,
                Description = model.Description,
                Code = model.Code
            });
        }
    }
}