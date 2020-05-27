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
            var exists = _context.PlatformGames.SingleOrDefault(g => g.Code == request.Code);

            if (exists == null)
            {
                var title = RegisterTitle(request);

                var game = RegisterTitleWithPlatform(request, title);

                await _context.SaveChangesAsync(cancellationToken);

                return new GameViewModel
                {
                    Id = game.Id,
                    Name = title.Name,
                    Description = title.Description,
                    Code = request.Code,
                    Registered = game.Registered,
                    Platform = _context.Platforms.Single(p => p.Id == request.Platform).Name
                };
            }

            return null;
        }

        private PlatformGame RegisterTitleWithPlatform(RegisterGame request, Game game)
        {
            var existing =
                _context.PlatformGames.SingleOrDefault(pg => pg.GameId == game.Id && pg.PlatformId == request.Platform);

            if (existing == null)
            {
                var platformGame = new PlatformGame
                {
                    GameId = game.Id,
                    Code = request.Code,
                    PlatformId = request.Platform,
                    Registered = DateTime.UtcNow
                };

                _context.Add(platformGame);

                return platformGame;
            }

            return existing;
        }

        private Game RegisterTitle(RegisterGame request)
        {
            var existing = _context.Games.SingleOrDefault(g => g.Name == request.Name.Trim());

            if (existing == null)
            {
                var game = new Game
                {
                    Name = request.Name.Trim(),
                    Description = request.Description?.Trim()
                };

                _context.Add(game);

                return game;
            }

            return existing;
        }
    }
}