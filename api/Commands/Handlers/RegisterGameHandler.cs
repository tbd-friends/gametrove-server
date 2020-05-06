using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using api.Storage;
using api.Storage.Models;
using api.ViewModels;
using MediatR;

namespace api.Commands.Handlers
{
    public class RegisterGameHandler : IRequestHandler<RegisterGame, GameViewModel>
    {
        private readonly GameTrackerContext _context;

        public RegisterGameHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public async Task<GameViewModel> Handle(RegisterGame request, CancellationToken cancellationToken)
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

                return new GameViewModel
                {
                    Code = request.Code,
                    Description = request.Description,
                    Name = request.Name
                };
            }

            return null;
        }
    }
}