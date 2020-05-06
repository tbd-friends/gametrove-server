using System.Threading;
using System.Threading.Tasks;
using api.Storage;
using api.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace api.Query.Handlers
{
    public class RetrieveGameByCodeHandler : IRequestHandler<RetrieveGameByCode, GameViewModel>
    {
        private readonly GameTrackerContext _context;

        public RetrieveGameByCodeHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public async Task<GameViewModel> Handle(RetrieveGameByCode request, CancellationToken cancellationToken)
        {
            var game = await _context.Games.SingleOrDefaultAsync(g => g.Code == request.Code, cancellationToken);

            return game != null ? new GameViewModel
            {
                Code = game.Code,
                Description = game.Description,
                Name = game.Name,
                Registered = game.Registered
            } : null;
        }
    }
}