using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using api.Storage;
using MediatR;

namespace api.Query.Handlers
{
    public class GetImageIdentifiersForGameHandler : IRequestHandler<GetImageIdentifiersForGame, IEnumerable<Guid>>
    {
        private readonly GameTrackerContext _context;

        public GetImageIdentifiersForGameHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<Guid>> Handle(GetImageIdentifiersForGame request, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                _context.PlatformGameImages
                    .Where(pgi => pgi.PlatformGameId == request.Id)
                    .Select(pgi => pgi.Id)
                    .AsEnumerable());
        }
    }
}