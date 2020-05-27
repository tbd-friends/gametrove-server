using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Application.ViewModels;
using GameTrove.Storage;
using MediatR;

namespace GameTrove.Application.Query.Handlers
{
    public class RetrieveRecentlyAddedGamesHandler : IRequestHandler<RetrieveRecentlyAddedGames, IEnumerable<GameViewModel>>
    {
        private readonly GameTrackerContext _context;

        public RetrieveRecentlyAddedGamesHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<GameViewModel>> Handle(RetrieveRecentlyAddedGames request, CancellationToken cancellationToken)
        {
            var recentlyAddedGames = _context.PlatformGames.OrderByDescending(g => g.Registered);

            return Task.FromResult((from x in recentlyAddedGames.Take(request.Limit)
                                    join p in _context.Platforms on x.PlatformId equals p.Id
                                    join g in _context.Games on x.GameId equals g.Id
                                    select new GameViewModel
                                    {
                                        Id = x.Id,
                                        Name = g.Name,
                                        Code = x.Code,
                                        Description = g.Description,
                                        Registered = x.Registered,
                                        Platform = p.Name
                                    }).AsEnumerable());
        }
    }
}