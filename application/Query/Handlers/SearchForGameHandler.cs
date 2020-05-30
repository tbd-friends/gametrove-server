using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Application.ViewModels;
using GameTrove.Storage;
using MediatR;

namespace GameTrove.Application.Query.Handlers
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
            var results = from t in _context.Titles
                          join pg in _context.Games on t.Id equals pg.TitleId
                          join p in _context.Platforms on pg.PlatformId equals p.Id
                          where t.Name.Contains(request.Text)
                          orderby t.Name
                          select new
                          {
                              Id = pg.Id,
                              Name = t.Name,
                              Platform = p.Name
                          };

            return Task.FromResult(results
                .Select(r => new SearchResultViewModel { Id = r.Id, Name = r.Name, Platform = r.Platform })
                .AsEnumerable());
        }
    }
}