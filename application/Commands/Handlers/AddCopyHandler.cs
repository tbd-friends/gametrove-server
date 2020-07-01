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
        private readonly IMediator _mediator;

        public AddCopyHandler(GameTrackerContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Guid?> Handle(AddCopy request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Email) ||
                string.IsNullOrEmpty(request.Identifier))
            {
                return null;
            }

            var user = await _mediator.Send(new RegisterUser
            {
                Email = request.Email,
                Identifier = request.Identifier
            }, cancellationToken);

            var copy = new Copy
            {
                GameId = request.GameId,
                Cost = request.Cost,
                TenantId = user.TenantId,
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