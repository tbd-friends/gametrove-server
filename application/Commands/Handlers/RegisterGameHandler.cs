using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Application.Infrastructure;
using GameTrove.Application.ViewModels;
using GameTrove.Storage;
using GameTrove.Storage.Models;
using MediatR;

namespace GameTrove.Application.Commands.Handlers
{
    public class RegisterGameHandler : IRequestHandler<RegisterGame, GameViewModel>
    {
        private readonly GameTrackerContext _context;
        private readonly IAuthenticatedMediator _mediator;

        public RegisterGameHandler(GameTrackerContext context, IAuthenticatedMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<GameViewModel> Handle(RegisterGame request, CancellationToken cancellationToken)
        {
            var title = await _mediator.Send(
                new RegisterTitle { Name = request.Name, Subtitle = request.Subtitle }, cancellationToken);

            var exists = _context.Games.SingleOrDefault(g =>
                (!string.IsNullOrEmpty(request.Code) && g.Code == request.Code) ||
                (g.PlatformId == request.Platform && g.TitleId == title.Id)
            );

            if (exists != null)
            {
                await _mediator.Send(new RegisterCopy
                {
                    GameId = exists.Id
                }, cancellationToken);

                return new GameViewModel
                {
                    Id = exists.Id,
                    Name = title.Name,
                    Subtitle = title.Subtitle,
                    Platform = _context.Platforms.Single(p => p.Id == request.Platform).Name,
                    Registered = exists.Registered,
                    Code = exists.Code
                };
            }

            var game = new Game()
            {
                TitleId = title.Id,
                PlatformId = request.Platform,
                Registered = DateTime.UtcNow,
                Code = request.Code
            };

            _context.Games.Add(game);

            await _context.SaveChangesAsync(cancellationToken);

            await _mediator.Send(new RegisterCopy
            {
                GameId = game.Id
            }, cancellationToken);

            return new GameViewModel
            {
                Id = game.Id,
                Name = title.Name,
                Subtitle = title.Subtitle,
                Platform = _context.Platforms.Single(p => p.Id == request.Platform).Name,
                Registered = game.Registered,
                Code = game.Code
            };
        }
    }
}