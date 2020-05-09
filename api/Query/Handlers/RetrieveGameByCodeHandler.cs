using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using api.Storage;
using api.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace api.Query.Handlers
{
    public class RetrieveGameByCodeHandler : IRequestHandler<RetrieveGameByCode, GameViewModel>
    {
        private readonly GameTrackerContext _context;

        public RetrieveGameByCodeHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public async Task<GameViewModel> Handle(RetrieveGameByCode request, CancellationToken cancellationToken)
        {
            var game = await (from pg in _context.PlatformGames
                              join p in _context.Platforms on pg.PlatformId equals p.Id
                              join g in _context.Games on pg.GameId equals g.Id
                              where pg.Code == request.Code
                              select new
                              {
                                  Id = pg.Id,
                                  Name = g.Name,
                                  Description = g.Description,
                                  Registered = pg.Registered,
                                  Code = pg.Code,
                                  Platform = p.Name
                              }).SingleOrDefaultAsync(cancellationToken: cancellationToken);

            return game != null ? new GameViewModel
            {
                Id = game.Id,
                Code = game.Code,
                Description = game.Description,
                Name = game.Name,
                Registered = game.Registered,
                Platform = game.Platform
            } : null;
        }
    }
}