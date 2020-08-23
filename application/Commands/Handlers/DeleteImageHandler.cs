using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Storage;
using MediatR;

namespace GameTrove.Application.Commands.Handlers
{
    public class DeleteImageHandler : IRequestHandler<DeleteImage>
    {
        private readonly IMediator _mediator;
        private readonly GameTrackerContext _context;

        public DeleteImageHandler(IMediator mediator, GameTrackerContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        public async Task<Unit> Handle(DeleteImage request, CancellationToken cancellationToken)
        {
            var image = _context.PlatformGameImages.Single(pgi => pgi.Id == request.Id);

            _context.PlatformGameImages.Remove(image);

            _context.SaveChanges();

            await _mediator.Send(new RemoveImageFromStorage { GameId = image.GameId, ImageId = request.Id },
                cancellationToken);

            return Unit.Value;
        }
    }
}