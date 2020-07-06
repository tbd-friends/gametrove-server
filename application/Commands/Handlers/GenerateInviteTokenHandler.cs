using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Application.Infrastructure;
using GameTrove.Storage;
using GameTrove.Storage.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GameTrove.Application.Commands.Handlers
{
    public class GenerateInviteTokenHandler : IRequestHandler<GenerateInviteToken, string>
    {
        private readonly GameTrackerContext _context;
        private readonly ITokenService _tokenService;

        public GenerateInviteTokenHandler(GameTrackerContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<string> Handle(GenerateInviteToken request, CancellationToken cancellationToken)
        {
            var pendingInvite = await _context.TenantInvites.SingleOrDefaultAsync(ti => ti.TenantId == request.TenantId,
                cancellationToken: cancellationToken);

            if (pendingInvite != null)
            {
                if (pendingInvite.Expiration > DateTime.UtcNow)
                {
                    return pendingInvite.Token;
                }

                _context.TenantInvites.Remove(pendingInvite);

                await _context.SaveChangesAsync(cancellationToken);
            }

            var result = _tokenService.TokenFromGuid(request.TenantId);

            _context.TenantInvites.Add(new TenantInvite
            {
                TenantId = request.TenantId,
                Token = result,
                Expiration = DateTime.UtcNow.AddHours(4)
            });

            await _context.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}