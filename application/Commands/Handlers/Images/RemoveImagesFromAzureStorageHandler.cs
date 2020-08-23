using System.Threading;
using System.Threading.Tasks;
using api.Settings;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using GameTrove.Application.Infrastructure;
using MediatR;

namespace GameTrove.Application.Commands.Handlers.Images
{
    public class RemoveImagesFromAzureStorageHandler : IRequestHandler<RemoveImagesFromAzureStorage>
    {
        private readonly ImageSettings _settings;
        private readonly BlobServiceClient _client;

        public RemoveImagesFromAzureStorageHandler(ImageSettings settings, BlobServiceClient client)
        {
            _settings = settings;
            _client = client;
        }

        public async Task<Unit> Handle(RemoveImagesFromAzureStorage request, CancellationToken cancellationToken)
        {
            var container = _client.GetBlobContainerClient(_settings.Container);

            var filesToDelete = new[]
            {
                $"{request.GameId}_{request.ImageId}.jpg",
                ImageFileName.GetGameImageFile(request.ImageId, request.GameId, ImageSize.Small),
                ImageFileName.GetGameImageFile(request.ImageId, request.GameId, ImageSize.Medium)
            };

            foreach (var file in filesToDelete)
            {
                var blobClient = container.GetBlobClient(file);

                await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots,
                    new BlobRequestConditions(),
                    cancellationToken);
            }

            return Unit.Value;
        }
    }
}