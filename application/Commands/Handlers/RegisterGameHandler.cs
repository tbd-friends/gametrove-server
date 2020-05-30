using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Application.ViewModels;
using GameTrove.Storage;
using GameTrove.Storage.Models;
using MediatR;

namespace GameTrove.Application.Commands.Handlers
{
    public class RegisterGameHandler : IRequestHandler<RegisterGame, GameViewModel>
    {
        private readonly GameTrackerContext _context;
        private readonly IMediator _mediator;

        public RegisterGameHandler(GameTrackerContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<GameViewModel> Handle(RegisterGame request, CancellationToken cancellationToken)
        {
            var exists = _context.Games.SingleOrDefault(g => g.Code == request.Code);

            if (exists == null)
            {
                var title = await _mediator.Send(
                    new RegisterTitle { Name = request.Name, Subtitle = request.Subtitle }, cancellationToken);

                var game = new Game()
                {
                    TitleId = title.Id,
                    PlatformId = request.Platform,
                    Code = request.Code
                };

                _context.Games.Add(game);

                await _context.SaveChangesAsync(cancellationToken);

                return new GameViewModel
                {
                    Id = game.Id,
                    Name = title.Name,
                    Description = title.Subtitle,
                    Platform = _context.Platforms.Single(p => p.Id == request.Platform).Name,
                    Registered = game.Registered,
                    Code = game.Code
                };
            }

            return null;
        }
    }
}