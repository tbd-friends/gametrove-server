using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Storage;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GameTrove.Application.Commands.Handlers
{
    public class FavoriteGameHandler : IRequestHandler<FavoriteGame>
    {
        private readonly GameTrackerContext _context;

        public FavoriteGameHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(FavoriteGame request, CancellationToken cancellationToken)
        {
            var game = await _context.Games.SingleAsync(g => g.Id == request.GameId, cancellationToken);

            game.IsFavorite = true;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}