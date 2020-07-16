using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Application.ViewModels;
using GameTrove.Storage;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GameTrove.Application.Query.Handlers
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
            var game = await (from pg in _context.Games
                              join p in _context.Platforms on pg.PlatformId equals p.Id
                              join t in _context.Titles on pg.TitleId equals t.Id
                              join gp in _context.GamePricing on pg.Id equals gp.Id
                                  into gpx
                              from pricing in gpx.DefaultIfEmpty()
                              where pg.Code == request.Code
                              select new
                              {
                                  Id = pg.Id,
                                  Name = t.Name,
                                  Description = t.Subtitle,
                                  Registered = pg.Registered,
                                  Code = pg.Code,
                                  Platform = p.Name,
                                  IsFavorite = pg.IsFavorite,
                                  CompleteInBoxPrice = pricing.CompleteInBoxPrice,
                                  LoosePrice = pricing.LoosePrice,
                                  Genres = (from tg in _context.TitleGenres
                                            join g in _context.Genres on tg.GenreId equals g.Id
                                            where tg.TitleId == t.Id
                                            select g.Name).ToList()
                              }).SingleOrDefaultAsync(cancellationToken: cancellationToken);

            return game != null ? new GameViewModel
            {
                Id = game.Id,
                Code = game.Code,
                Subtitle = game.Description,
                Name = game.Name,
                Registered = game.Registered,
                Platform = game.Platform,
                IsFavorite = game.IsFavorite,
                Genres = game.Genres,
                CompleteInBoxPrice = game.CompleteInBoxPrice,
                LoosePrice = game.LoosePrice
            } : null;
        }
    }
}