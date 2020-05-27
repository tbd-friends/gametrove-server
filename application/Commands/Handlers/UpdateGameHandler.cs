using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Application.ViewModels;
using GameTrove.Storage;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GameTrove.Application.Commands.Handlers
{
    public class UpdateGameHandler : IRequestHandler<UpdateGame, GameViewModel>
    {
        private readonly GameTrackerContext _context;

        public UpdateGameHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public async Task<GameViewModel> Handle(UpdateGame request, CancellationToken cancellationToken)
        {
            var result = await (from pg in _context.PlatformGames
                                join g in _context.Games on pg.GameId equals g.Id
                                where pg.Id == request.Id
                                select new
                                {
                                    Game = g,
                                    PlatformInfo = pg
                                }).SingleOrDefaultAsync(cancellationToken);

            if (result != null)
            {
                result.Game.Name = request.Name;
                result.Game.Description = request.Description;

                _context.Update(result.Game);

                await _context.SaveChangesAsync(cancellationToken);

                return new GameViewModel
                {
                    Id = request.Id,
                    Name = result.Game.Name,
                    Description = result.Game.Description,
                    Code = result.PlatformInfo.Code,
                    Registered = result.PlatformInfo.Registered
                };
            }

            return null;
        }
    }
}