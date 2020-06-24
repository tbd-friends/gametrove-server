using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Storage;
using GameTrove.Storage.Models;
using MediatR;

namespace GameTrove.Application.Commands.Handlers
{
    public class RegisterUserHandler : IRequestHandler<RegisterUser, Guid>
    {
        private readonly GameTrackerContext _context;

        public RegisterUserHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(RegisterUser request, CancellationToken cancellationToken)
        {
            var existing = _context.Users.SingleOrDefault(u => u.Email == request.Email);

            if (existing != null)
            {
                return existing.Id;
            }

            var user = new User
            {
                Email = request.Email,
                Identifier = request.Identifier
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}