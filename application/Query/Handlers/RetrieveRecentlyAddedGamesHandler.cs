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
            var recentlyAddedGames = _context.Games.OrderByDescending(g => g.Registered);

            return Task.FromResult((from x in recentlyAddedGames.Take(request.Limit)
                                    join p in _context.Platforms on x.PlatformId equals p.Id
                                    join t in _context.Titles on x.TitleId equals t.Id
                                    select new GameViewModel
                                    {
                                        Id = x.Id,
                                        Name = t.Name,
                                        Code = x.Code,
                                        Subtitle = t.Subtitle,
                                        Registered = x.Registered,
                                        Platform = p.Name,
                                        IsFavorite = x.IsFavorite
                                    }).AsEnumerable());
        }
    }
}