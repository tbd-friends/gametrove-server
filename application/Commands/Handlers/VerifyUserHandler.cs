using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Application.Infrastructure;
using GameTrove.Storage;
using MediatR;

namespace GameTrove.Application.Commands.Handlers
{
    public class VerifyUserHandler : IRequestHandler<VerifyUser, bool>
    {
        private readonly GameTrackerContext _context;
        private readonly IAuthenticatedMediator _mediator;

        public VerifyUserHandler(GameTrackerContext context, IAuthenticatedMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<bool> Handle(VerifyUser request, CancellationToken cancellationToken)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == request.Email);

            if (user == null)
            {
                var registration = await _mediator.Send(new RegisterUser
                {
                    Email = request.Email
                }, cancellationToken);

                if (registration == null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}