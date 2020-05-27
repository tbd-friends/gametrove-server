using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Application.ViewModels;
using GameTrove.Storage;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GameTrove.Application.Commands.Handlers
{
    public class UpdateGameHandler : IRequestHandler<UpdateGame, GameViewModel>
    {
        private readonly GameTrackerContext _context;

        public UpdateGameHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public async Task<GameViewModel> Handle(UpdateGame request, CancellationToken cancellationToken)
        {
            var result = await (from pg in _context.Games
                                join t in _context.Titles on pg.TitleId equals t.Id
                                where pg.Id == request.Id
                                select new
                                {
                                    Title = t,
                                    PlatformInfo = pg
                                }).SingleOrDefaultAsync(cancellationToken);

            if (result != null)
            {
                result.Title.Name = request.Name;
                result.Title.Subtitle = request.Description;

                _context.Update(result.Title);

                await _context.SaveChangesAsync(cancellationToken);

                return new GameViewModel
                {
                    Id = request.Id,
                    Name = result.Title.Name,
                    Description = result.Title.Subtitle,
                    Code = result.PlatformInfo.Code,
                    Registered = result.PlatformInfo.Registered
                };
            }

            return null;
        }
    }
}