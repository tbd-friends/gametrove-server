using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Storage;
using MediatR;

namespace GameTrove.Application.Commands.Handlers
{
    public class ToggleImageAsCoverArtHandler : IRequestHandler<ToggleImageAsCoverArt>
    {
        private readonly GameTrackerContext _context;

        public ToggleImageAsCoverArtHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public Task<Unit> Handle(ToggleImageAsCoverArt request, CancellationToken cancellationToken)
        {
            var image = _context.PlatformGameImages.Single(pgi => pgi.Id == request.ImageId);
            var currentCoverArtImage =
                _context.PlatformGameImages.SingleOrDefault(pgi => pgi.Id != request.ImageId &&
                                                                   pgi.GameId == image.GameId &&
                                                                   pgi.IsCoverArt);

            image.IsCoverArt = !image.IsCoverArt;

            if (currentCoverArtImage != null)
            {
                currentCoverArtImage.IsCoverArt = false;
            }

            _context.SaveChanges();

            return Unit.Task;
        }
    }
}