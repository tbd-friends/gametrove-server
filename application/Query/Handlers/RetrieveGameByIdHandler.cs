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
            var result = (from pg in _context.Games
                          join p in _context.Platforms on pg.PlatformId equals p.Id
                          join t in _context.Titles on pg.TitleId equals t.Id
                          where pg.Id == request.Id
                          select new GameViewModel
                          {
                              Id = pg.Id,
                              Code = pg.Code,
                              Subtitle = t.Subtitle,
                              Name = t.Name,
                              Registered = pg.Registered,
                              Platform = p.Name,
                              IsFavorite = pg.IsFavorite
                          }).SingleOrDefault();

            return Task.FromResult(result);
        }
    }
}