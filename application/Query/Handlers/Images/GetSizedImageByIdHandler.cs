using System;
using System.Threading;
using System.Threading.Tasks;
using api.Settings;
using GameTrove.Application.Infrastructure;
using GameTrove.Storage;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GameTrove.Application.Query.Handlers.Images
{
    public class GetSizedImageByIdHandler : IRequestHandler<GetSizedImageById, byte[]>
    {
        private readonly GameTrackerContext _context;
        private readonly ImageSettings _settings;
        private readonly IMediator _mediator;

        public GetSizedImageByIdHandler(GameTrackerContext context,
                                        ImageSettings settings, IMediator mediator)
        {
            _context = context;
            _settings = settings;
            _mediator = mediator;
        }

        public async Task<byte[]> Handle(GetSizedImageById request, CancellationToken cancellationToken)
        {
            byte[] result = null;

            var image = await _context.PlatformGameImages.SingleAsync(pgi => pgi.Id == request.Id, cancellationToken);

            if (_settings.Local)
            {
                result = await _mediator.Send(new GetLocalFileContents() { Name = image.FileName }, cancellationToken);

                result = result.ResizeImage((int)request.Size);
            }
            else
            {
                result = await _mediator.Send(new GetAzureFileContents
                {
                    ImageId = request.Id,
                    GameId = image.GameId,
                    Size = request.Size
                }, cancellationToken);
            }

            return result;
        }
    }
}