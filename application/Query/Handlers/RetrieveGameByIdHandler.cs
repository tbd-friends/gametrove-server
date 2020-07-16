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
                          join gp in _context.GamePricing on pg.Id equals gp.Id 
                                into gpx
                          from pricing in gpx.DefaultIfEmpty()
                          where pg.Id == request.Id
                          select new GameViewModel
                          {
                              Id = pg.Id,
                              Code = pg.Code,
                              Subtitle = t.Subtitle,
                              Name = t.Name,
                              Registered = pg.Registered,
                              Platform = p.Name,
                              IsFavorite = pg.IsFavorite,
                              CompleteInBoxPrice = pricing.CompleteInBoxPrice, 
                              LoosePrice = pricing.LoosePrice,
                              Genres = (from tg in _context.TitleGenres
                                  join g in _context.Genres on tg.GenreId equals g.Id
                                  where tg.TitleId == t.Id
                                  select g.Name)
                          }).SingleOrDefault();

            return Task.FromResult(result);
        }
    }
}