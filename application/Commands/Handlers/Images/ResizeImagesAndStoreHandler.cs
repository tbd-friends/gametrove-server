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

            await using (MemoryStream stream = new MemoryStream(request.ImageContent.ResizeImage((int)ImageSize.Small)))
            {
                await _mediator.Send(new SendToAzureStorage
                {
                    Content = stream,
                    FileName = ImageFileName.GetGameImageFile(request.ImageId, request.GameId, ImageSize.Small)
                }, cancellationToken);
            }

            await using (MemoryStream stream = new MemoryStream(request.ImageContent.ResizeImage((int)ImageSize.Medium)))
            {
                await _mediator.Send(new SendToAzureStorage
                {
                    Content = stream,
                    FileName = ImageFileName.GetGameImageFile(request.ImageId, request.GameId, ImageSize.Medium)
                }, cancellationToken);
            }

            return Unit.Value;
        }

        private byte[] GetContentAsBytes(Stream content)
        {
            using MemoryStream memoryStream = new MemoryStream();

            content.CopyTo(memoryStream);

            return memoryStream.ToArray();
        }
    }
}