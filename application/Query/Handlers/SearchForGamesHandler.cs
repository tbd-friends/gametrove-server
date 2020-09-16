using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Application.ViewModels;
using GameTrove.Storage;
using GameTrove.Storage.Models;
using MediatR;

namespace GameTrove.Application.Query.Handlers
{
    public class SearchForGamesHandler : IRequestHandler<SearchForGames, IEnumerable<GameSearchViewModel>>
    {
        private readonly GameTrackerContext _context;

        public SearchForGamesHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<GameSearchViewModel>> Handle(SearchForGames request, CancellationToken cancellationToken)
        {
            IQueryable<Title> query = _context.Titles;

            if (!string.IsNullOrEmpty(request.Text))
            {
                query = query.Where(t => t.Name.Contains(request.Text));
            }

            var results = from t in query
                          join pg in _context.Games on t.Id equals pg.TitleId
                          join p in _context.Platforms on pg.PlatformId equals p.Id
                          join gp in _context.GamePricing on pg.Id equals gp.Id
                              into gpx
                          from pricing in gpx.DefaultIfEmpty()
                          where (
                              from c in _context.Copies
                              where c.GameId == pg.Id && c.TenantId == request.TenantId
                              select c).Any()
                          select new
                          {
                              Id = pg.Id,
                              Name = t.Name,
                              Subtitle = t.Subtitle,
                              Platform = p.Name,
                              Genres = (from tg in _context.TitleGenres
                                        join g in _context.Genres on tg.GenreId equals g.Id
                                        where tg.TitleId == t.Id
                                        select g.Name).ToList()
                          };

            results = results.OrderBy(r => r.Name);

            return Task.FromResult(results.AsEnumerable().Select(r =>
                new GameSearchViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Subtitle = r.Subtitle,
                    Platform = r.Platform,
                }));
        }
    }
}