using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using api.Storage;
using api.Storage.Models;
using api.ViewModels;
using MediatR;

namespace api.Query.Handlers
{
    public class SearchForGameHandler : IRequestHandler<SearchForGame, IEnumerable<SearchResultViewModel>>
    {
        private readonly GameTrackerContext _context;

        public SearchForGameHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<SearchResultViewModel>> Handle(SearchForGame request, CancellationToken cancellationToken)
        {
            var results = from g in _context.Games
                          join pg in _context.PlatformGames on g.Id equals pg.GameId
                          join p in _context.Platforms on pg.PlatformId equals p.Id
                          where g.Name.Contains(request.Text)
                          orderby g.Name
                          select new
                          {
                              Id = pg.Id,
                              Name = $"{g.Name}({p.Name})"
                          };

            return Task.FromResult(results
                .Select(r => new SearchResultViewModel { Id = r.Id, Name = r.Name })
                .AsEnumerable());
        }
    }
}