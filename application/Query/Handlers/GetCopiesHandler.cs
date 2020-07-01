using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Application.ViewModels;
using GameTrove.Storage;
using GameTrove.Storage.Models;
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
            var copies = (from cp in _context.Copies
                          where cp.GameId == request.GameId && cp.TenantId == request.TenantId
                          select cp).ToList();

            return Task.FromResult((from c in copies
                                    select new CopyViewModel
                                    {
                                        Id = c.Id,
                                        Cost = c.Cost,
                                        Tags = JsonSerializer.Deserialize<string[]>(c.Tags),
                                        Purchased = c.Purchased,
                                        IsWanted = c.IsWanted
                                    }).AsEnumerable());
        }
    }
}