using System.Threading;
using System.Threading.Tasks;
using GameTrove.Storage;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GameTrove.Application.Commands.Handlers
{
    public class UnfavoriteGameHandler : IRequestHandler<UnfavoriteGame>
    {
        private readonly GameTrackerContext _context;

        public UnfavoriteGameHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UnfavoriteGame request, CancellationToken cancellationToken)
        {
            var game = await _context.Games.SingleAsync(g => g.Id == request.GameId, cancellationToken);

            game.IsFavorite = false;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}