using System.IO;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Application.Infrastructure;
using MediatR;

namespace GameTrove.Application.Commands.Handlers.Images
{
    public class ResizeImagesAndStoreHandler : IRequestHandler<ResizeImagesAndStore>
    {
        private readonly IMediator _mediator;

        public ResizeImagesAndStoreHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ResizeImagesAndStore request, CancellationToken cancellationToken)
        {
            await using (MemoryStream stream = new MemoryStream(request.ImageContent))
            {
                await _mediator.Send(new SendToAzureStorage
                {
                    Content = stream,
                    FileName = $"{request.GameId}_{request.ImageId}.jpg"
                }, cancellationToken);
            }

            await UploadImageBytesSized(request, ImageSize.Small, cancellationToken);
            await UploadImageBytesSized(request, ImageSize.Medium, cancellationToken);

            return Unit.Value;
        }

        private async Task UploadImageBytesSized(ResizeImagesAndStore request, ImageSize size, CancellationToken cancellationToken)
        {
            await using MemoryStream stream = new MemoryStream(request.ImageContent.ResizeImage((int)size));
            await _mediator.Send(new SendToAzureStorage
            {
                Content = stream,
                FileName = ImageFileName.GetGameImageFile(request.ImageId, request.GameId, size)
            }, cancellationToken);
        }
    }
}