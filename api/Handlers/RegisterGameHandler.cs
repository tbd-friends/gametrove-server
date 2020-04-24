using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using api.Commands;
using api.Storage;
using api.Storage.Models;
using MediatR;

namespace api.Handlers
{
    public class RegisterGameHandler : IRequestHandler<RegisterGame>
    {
        private readonly GameTrackerContext _context;

        public RegisterGameHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RegisterGame request, CancellationToken cancellationToken)
        {
            var exists = _context.Games.SingleOrDefault(g => g.Code == request.Code);

            if (exists == null)
            {
                _context.Add(new Game
                {
                    Name = request.Name,
                    Description = request.Description,
                    Code = request.Code
                });

                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}