using System;
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

        public RegisterGameHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public async Task<GameViewModel> Handle(RegisterGame request, CancellationToken cancellationToken)
        {
            var exists = _context.Games.SingleOrDefault(g => g.Code == request.Code);

            if (exists == null)
            {
                var title = RegisterTitle(request);

                var game = RegisterTitleWithPlatform(request, title);

                await _context.SaveChangesAsync(cancellationToken);

                return new GameViewModel
                {
                    Id = game.Id,
                    Name = title.Name,
                    Description = title.Subtitle,
                    Code = request.Code,
                    Registered = game.Registered,
                    Platform = _context.Platforms.Single(p => p.Id == request.Platform).Name
                };
            }

            return null;
        }

        private Game RegisterTitleWithPlatform(RegisterGame request, Title title)
        {
            var existing =
                _context.Games.SingleOrDefault(pg => pg.TitleId == title.Id && pg.PlatformId == request.Platform);

            if (existing == null)
            {
                var platformGame = new Game
                {
                    TitleId = title.Id,
                    Code = request.Code,
                    PlatformId = request.Platform,
                    Registered = DateTime.UtcNow
                };

                _context.Add(platformGame);

                return platformGame;
            }

            return existing;
        }

        private Title RegisterTitle(RegisterGame request)
        {
            var existing = _context.Titles.SingleOrDefault(g => g.Name == request.Name.Trim());

            if (existing == null)
            {
                var game = new Title
                {
                    Name = request.Name.Trim(),
                    Subtitle = request.Description?.Trim()
                };

                _context.Add(game);

                return game;
            }

            return existing;
        }
    }
}