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
    public class SearchForGamesHandler : IRequestHandler<SearchForGames, IEnumerable<GameViewModel>>
    {
        private readonly GameTrackerContext _context;

        public SearchForGamesHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<GameViewModel>> Handle(SearchForGames request, CancellationToken cancellationToken)
        {
            IQueryable<Title> query = _context.Titles;

            if (!string.IsNullOrEmpty(request.Text))
            {
                query = query.Where(t => t.Name.Contains(request.Text));
            }

            var results = from t in query
                          join pg in _context.Games on t.Id equals pg.TitleId
                          join p in _context.Platforms on pg.PlatformId equals p.Id
                          where (
                                  from c in _context.Copies
                                  where c.GameId == pg.Id && c.TenantId == request.TenantId
                                  select c).Any()
                          select new
                          {
                              Id = pg.Id,
                              Name = t.Name,
                              Subtitle = t.Subtitle,
                              Code = pg.Code,
                              Registered = pg.Registered,
                              Platform = p.Name,
                              IsFavorite = pg.IsFavorite,
                              Genres = (from tg in _context.TitleGenres
                                        join g in _context.Genres on tg.GenreId equals g.Id
                                        where tg.TitleId == t.Id
                                        select g.Name).ToList()
                          };

            if (request.MostRecentlyAdded > 0)
            {
                results = results.OrderByDescending(r => r.Registered).Take(request.MostRecentlyAdded.Value);
            }
            else
            {
                results = results.OrderBy(r => r.Name);
            }

            return Task.FromResult(results.AsEnumerable().Select(r =>
                new GameViewModel
                {
                    Id = r.Id,
                    Code = r.Code,
                    Name = r.Name,
                    Subtitle = r.Subtitle,
                    Registered = r.Registered,
                    Platform = r.Platform,
                    IsFavorite = r.IsFavorite,
                    Genres = r.Genres
                }));
        }
    }
}