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

            var game = await RegisterGame(request, title);

            await _mediator.Send(new AddCopy
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

        private async Task<Game> RegisterGame(RegisterGame request, TitleViewModel title)
        {
            var game = _context.Games.SingleOrDefault(g =>
                (!string.IsNullOrEmpty(request.Code) && g.Code == request.Code) ||
                g.PlatformId == request.Platform && g.TitleId == title.Id);

            if (game != null) return game;

            game = new Game()
            {
                TitleId = title.Id,
                PlatformId = request.Platform,
                Registered = DateTime.UtcNow,
                Code = request.Code
            };

            _context.Games.Add(game);

            await _context.SaveChangesAsync();

            return game;
        }
    }
}