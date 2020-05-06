using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using api.Storage;
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
            var result = (from g in _context.Games
                          where g.Id == request.Id
                          select new GameViewModel
                          {
                              Code = g.Code,
                              Description = g.Description,
                              Name = g.Name,
                              Registered = g.Registered
                          }).SingleOrDefault();

            return Task.FromResult(result);
        }
    }
}