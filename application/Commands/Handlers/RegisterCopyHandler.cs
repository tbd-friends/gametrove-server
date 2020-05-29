using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Storage;
using GameTrove.Storage.Models;
using MediatR;

namespace GameTrove.Application.Commands.Handlers
{
    public class RegisterCopyHandler : IRequestHandler<RegisterCopy>
    {
        private readonly GameTrackerContext _context;

        public RegisterCopyHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RegisterCopy request, CancellationToken cancellationToken)
        {
            _context.Copies.Add(new Copy
            {
                GameId = request.GameId,
                Cost = request.Cost,
                Tags = JsonSerializer.Serialize(request.Tags)
            });

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}