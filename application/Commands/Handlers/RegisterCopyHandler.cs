using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Storage;
using GameTrove.Storage.Models;
using MediatR;

namespace GameTrove.Application.Commands.Handlers
{
    public class RegisterCopyHandler : IRequestHandler<RegisterCopy, Guid>
    {
        private readonly GameTrackerContext _context;

        public RegisterCopyHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(RegisterCopy request, CancellationToken cancellationToken)
        {
            var copy = new Copy
            {
                GameId = request.GameId,
                Cost = request.Cost,
                Tags = JsonSerializer.Serialize(request.Tags),
                Purchased = request.Purchased
            };

            _context.Copies.Add(copy);

            await _context.SaveChangesAsync(cancellationToken);

            return copy.Id;
        }
    }
}