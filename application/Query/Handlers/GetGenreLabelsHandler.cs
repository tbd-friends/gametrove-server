using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Storage;
using MediatR;

namespace GameTrove.Application.Query.Handlers
{
    public class GetGenreLabelsHandler : IRequestHandler<GetGenreLabels, IEnumerable<string>>
    {
        private readonly GameTrackerContext _context;

        public GetGenreLabelsHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<string>> Handle(GetGenreLabels request, CancellationToken cancellationToken)
        {
            return Task.FromResult((from g in _context.Genres
                                    orderby g.Name
                                    select g.Name).AsEnumerable());
        }
    }
}