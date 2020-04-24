using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("games")]
    public class GameController : ControllerBase
    {
        [HttpPost]
        public void AddNewGame(GameModel model)
        {

        }
    }
}