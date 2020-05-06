using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using api.Storage;
using MediatR;

namespace api.Query.Handlers
{
    public class SearchForGameHandler : IRequestHandler<SearchForGame, IEnumerable<string>>
    {
        private readonly GameTrackerContext _context;

        public SearchForGameHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<string>> Handle(SearchForGame request, CancellationToken cancellationToken)
        {
            var results = from g in _context.Games
                          where g.Name.Contains(request.Text)
                          select g;

            return Task.FromResult(results.Select(r => r.Name).AsEnumerable());
        }
    }
}