using System.IO;
using System.Threading;
using System.Threading.Tasks;
using api.Settings;
using MediatR;

namespace GameTrove.Application.Commands.Handlers.Images
{
    public class RemoveImageFromStorageHandler : IRequestHandler<RemoveImageFromStorage>
    {
        private readonly IMediator _mediator;
        private readonly ImageSettings _settings;

        public RemoveImageFromStorageHandler(IMediator mediator, ImageSettings settings)
        {
            _mediator = mediator;
            _settings = settings;
        }

        public async Task<Unit> Handle(RemoveImageFromStorage request, CancellationToken cancellationToken)
        {
            if (_settings.Local)
            {
                File.Delete($"{request.GameId}_{request.ImageId}.jpg");
            }
            else
            {
                await _mediator.Send(new RemoveImagesFromAzureStorage
                { GameId = request.GameId, ImageId = request.ImageId },
                    cancellationToken);
            }

            return Unit.Value;
        }
    }
}