﻿using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Storage;
using GameTrove.Storage.Models;
using MediatR;

namespace GameTrove.Application.Commands.Handlers
{
    public class RegisterCopyHandler : IRequestHandler<RegisterCopy, Guid?>
    {
        private readonly GameTrackerContext _context;
        private readonly IMediator _mediator;

        public RegisterCopyHandler(GameTrackerContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Guid?> Handle(RegisterCopy request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Email) ||
                string.IsNullOrEmpty(request.Identifier))
            {
                return null;
            }

            var userId = await _mediator.Send(new RegisterUser
            {
                Email = request.Email,
                Identifier = request.Identifier
            }, cancellationToken);

            var copy = new Copy
            {
                GameId = request.GameId,
                Cost = request.Cost,
                UserId = userId,
                Tags = JsonSerializer.Serialize(request.Tags),
                Purchased = request.Purchased
            };

            _context.Copies.Add(copy);

            await _context.SaveChangesAsync(cancellationToken);

            return copy.Id;
        }
    }
}