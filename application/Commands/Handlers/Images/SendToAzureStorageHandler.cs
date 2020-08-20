using System;
using System.Threading;
using System.Threading.Tasks;
using api.Settings;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MediatR;

namespace GameTrove.Application.Commands.Handlers.Images
{
    public class SendToAzureStorageHandler : IRequestHandler<SendToAzureStorage>
    {
        private readonly ImageSettings _settings;
        private readonly BlobServiceClient _client;

        public SendToAzureStorageHandler(ImageSettings settings, BlobServiceClient client)
        {
            _settings = settings;
            _client = client;
        }

        public async Task<Unit> Handle(SendToAzureStorage request, CancellationToken cancellationToken)
        {
            var container = _client.GetBlobContainerClient(_settings.Container);

            if (!container.Exists(cancellationToken))
            {
                await container.CreateIfNotExistsAsync(PublicAccessType.Blob,
                    cancellationToken: cancellationToken);
            }

            var upload = container.GetBlobClient(request.FileName);

            await upload.UploadAsync(request.Content, 
                new BlobHttpHeaders { ContentType = "image/jpeg" },
                cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}