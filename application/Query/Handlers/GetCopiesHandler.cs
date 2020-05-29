using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Application.ViewModels;
using GameTrove.Storage;
using MediatR;

namespace GameTrove.Application.Query.Handlers
{
    public class GetCopiesHandler : IRequestHandler<GetCopies, IEnumerable<CopyViewModel>>
    {
        private readonly GameTrackerContext _context;

        public GetCopiesHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<CopyViewModel>> Handle(GetCopies request, CancellationToken cancellationToken)
        {
            var copies = _context.Copies.Where(c => c.GameId == request.GameId).ToList();

            return Task.FromResult((from c in copies
                                    select new CopyViewModel
                                    {
                                        Id = c.Id,
                                        Cost = c.Cost,
                                        Tags = JsonSerializer.Deserialize<string[]>(c.Tags),
                                        Purchased = c.Purchased
                                    }).AsEnumerable());
        }
    }
}