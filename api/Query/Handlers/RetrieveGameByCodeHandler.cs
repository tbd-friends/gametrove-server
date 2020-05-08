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
            var game = await (from p in _context.PlatformGames
                              join g in _context.Games on p.GameId equals g.Id
                              where p.Code == request.Code
                              select new
                              {
                                  Name = g.Name,
                                  Description = g.Description,
                                  Registered = p.Registered,
                                  Code = p.Code
                              }).SingleOrDefaultAsync(cancellationToken: cancellationToken);

            return game != null ? new GameViewModel
            {
                Code = game.Code,
                Description = game.Description,
                Name = game.Name,
                Registered = game.Registered
            } : null;
        }
    }
}