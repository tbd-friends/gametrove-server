using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using api.Settings;
using MediatR;
using File = System.IO.File;

namespace GameTrove.Application.Query.Handlers.Images
{
    public class GetLocalFileContentsHandler : IRequestHandler<GetLocalFileContents, byte[]>
    {
        private readonly string _imagesPath;
        private readonly string _defaultImage;

        public GetLocalFileContentsHandler(ImageSettings settings)
        {
            _imagesPath = Path.Combine(AppContext.BaseDirectory, settings.Path);
            _defaultImage = Path.Combine(AppContext.BaseDirectory, settings.Default);
        }

        public Task<byte[]> Handle(GetLocalFileContents request, CancellationToken cancellationToken)
        {
            var filePath = Path.Combine(_imagesPath, request.Name);

            if (File.Exists(filePath))
            {
                return Task.FromResult(File.ReadAllBytes(filePath));
            }

            return Task.FromResult(File.ReadAllBytes(_defaultImage));
        }
    }
}