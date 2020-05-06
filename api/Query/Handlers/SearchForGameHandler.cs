using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using api.Storage;
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
                          where g.Name.Contains(request.Text)
                          select g;

            return Task.FromResult(results.Select(r => new SearchResultViewModel { Id = r.Id, Name = r.Name })
                .AsEnumerable());
        }
    }
}