using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Application.ViewModels;
using GameTrove.Storage;
using MediatR;

namespace GameTrove.Application.Query.Handlers.Images
{
    public class GetImagesAttachedToGameHandler : IRequestHandler<GetImagesAttachedToGame, IEnumerable<GameImageViewModel>>
    {
        private readonly GameTrackerContext _context;

        public GetImagesAttachedToGameHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<GameImageViewModel>> Handle(GetImagesAttachedToGame request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(
                _context.PlatformGameImages
                    .Where(pgi => pgi.GameId == request.GameId)
                    .Select(pgi => new GameImageViewModel
                    {
                        Id = pgi.Id,
                        IsCoverArt = pgi.IsCoverArt
                    })
                    .AsEnumerable());
        }
    }
}