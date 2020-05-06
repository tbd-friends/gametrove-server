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
                                    select new GameViewModel
                                    {
                                        Name = x.Name,
                                        Code = x.Code,
                                        Description = x.Description,
                                        Registered = x.Registered
                                    }).AsEnumerable());
        }
    }
}