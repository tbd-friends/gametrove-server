using System.Threading;
using System.Threading.Tasks;
using api.Settings;
using GameTrove.Application.Infrastructure;
using MediatR;

namespace GameTrove.Application.Query.Handlers.Images
{
    public class GetAzureFileContentsHandler : IRequestHandler<GetAzureFileContents, byte[]>
    {
        private readonly ImageSettings _settings;
        private readonly AzureDownloadClient _client;

        public GetAzureFileContentsHandler(ImageSettings settings,
                                            AzureDownloadClient client)
        {
            _settings = settings;
            _client = client;
        }

        public async Task<byte[]> Handle(GetAzureFileContents request, CancellationToken cancellationToken)
        {
            string filename = ImageFileName.GetGameImageFile(request.ImageId, request.GameId, request.Size);

            return await _client.DownloadFileAsync(_settings.Container, filename, cancellationToken);
        }
    }
}