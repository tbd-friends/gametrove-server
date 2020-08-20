using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GameTrove.Application.Infrastructure
{
    public class AzureDownloadClient
    {
        private readonly HttpClient _client;

        public AzureDownloadClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<byte[]> DownloadFileAsync(string container, string filename, CancellationToken cancellationToken)
        {
            var response = await _client.GetAsync($"{container}/{filename}", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                await using var output = new MemoryStream();
                var stream = await response.Content.ReadAsStreamAsync();

                await stream.CopyToAsync(output, cancellationToken);

                return output.ToArray();
            }

            return null;
        }
    }
}