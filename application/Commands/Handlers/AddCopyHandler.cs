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
    public class AddCopyHandler : IRequestHandler<AddCopy, Guid?>
    {
        private readonly GameTrackerContext _context;
        
        public AddCopyHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public async Task<Guid?> Handle(AddCopy request, CancellationToken cancellationToken)
        {
            var copy = new Copy
            {
                GameId = request.GameId,
                Cost = request.Cost,
                TenantId = request.TenantId,
                Tags = JsonSerializer.Serialize(request.Tags),
                Purchased = request.Purchased,
                IsWanted = request.IsWanted
            };

            _context.Copies.Add(copy);

            await _context.SaveChangesAsync(cancellationToken);

            return copy.Id;
        }
    }
}