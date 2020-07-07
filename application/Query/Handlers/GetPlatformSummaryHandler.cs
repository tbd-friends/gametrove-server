using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Application.ViewModels;
using GameTrove.Storage;
using MediatR;

namespace GameTrove.Application.Query.Handlers
{
    public class GetPlatformSummaryHandler : IRequestHandler<GetPlatformSummary, IEnumerable<PlatformSummaryViewModel>>
    {
        private readonly GameTrackerContext _context;

        public GetPlatformSummaryHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PlatformSummaryViewModel>> Handle(GetPlatformSummary request,
            CancellationToken cancellationToken)
        {
            var results = from x in
                    (from u in _context.Users
                     join t in _context.Titles on u.TenantId equals t.TenantId
                     join g in _context.Games on new { t.Id, t.TenantId } equals new { Id = g.TitleId, g.TenantId }
                     join p in _context.Platforms on g.PlatformId equals p.Id
                     where u.TenantId == request.TenantId
                     select new { Game = g, Platform = p, Title = t }
                    ).AsEnumerable()
                          group x by new { x.Platform.Id, x.Platform.Name } into byPlatform
                          where byPlatform.Any()
                          select new PlatformSummaryViewModel
                          {
                              Id = byPlatform.Key.Id,
                              Name = byPlatform.Key.Name,
                              NumberOfGames = byPlatform.Count()
                          };

            return await Task.FromResult(results);
        }
    }
}