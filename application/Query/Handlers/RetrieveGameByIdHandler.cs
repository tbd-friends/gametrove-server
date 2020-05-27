using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Application.ViewModels;
using GameTrove.Storage;
using MediatR;

namespace GameTrove.Application.Query.Handlers
{
    public class RetrieveGameByIdHandler : IRequestHandler<RetrieveGameById, GameViewModel>
    {
        private readonly GameTrackerContext _context;

        public RetrieveGameByIdHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public Task<GameViewModel> Handle(RetrieveGameById request, CancellationToken cancellationToken)
        {
            var result = (from pg in _context.PlatformGames
                          join p in _context.Platforms on pg.PlatformId equals p.Id
                          join g in _context.Games on pg.GameId equals g.Id
                          where pg.Id == request.Id
                          select new GameViewModel
                          {
                              Id = pg.Id,
                              Code = pg.Code,
                              Description = g.Description,
                              Name = g.Name,
                              Registered = pg.Registered,
                              Platform = p.Name
                          }).SingleOrDefault();

            return Task.FromResult(result);
        }
    }
}