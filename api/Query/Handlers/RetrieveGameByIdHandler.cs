using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using api.Storage;
using api.Storage.Models;
using api.ViewModels;
using MediatR;

namespace api.Query.Handlers
{
    public class RetrieveGameByIdHandler : IRequestHandler<RetrieveGameById, GameViewModel>
    {
        private readonly GameTrackerContext _context;

        public RetrieveGameByIdHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public Task<GameViewModel> Handle(RetrieveGameById request, CancellationToken cancellationToken)
        {
            var result = (from p in _context.PlatformGames
                          join g in _context.Games on p.GameId equals g.Id
                          where p.Id == request.Id
                          select new GameViewModel
                          {
                              Id = p.Id,
                              Code = p.Code,
                              Description = g.Description,
                              Name = g.Name,
                              Registered = p.Registered
                          }).SingleOrDefault();

            return Task.FromResult(result);
        }
    }
}