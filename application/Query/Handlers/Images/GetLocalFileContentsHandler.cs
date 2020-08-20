using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using api.Settings;
using GameTrove.Storage;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GameTrove.Application.Query.Handlers.Images
{
    public class GetLocalFileContentsHandler : IRequestHandler<GetLocalFileContents, byte[]>
    {
        private readonly string _imagesPath;

        public GetLocalFileContentsHandler(ImageSettings settings)
        {
            _imagesPath = Path.Combine(AppContext.BaseDirectory, settings.Path);
        }

        public Task<byte[]> Handle(GetLocalFileContents request, CancellationToken cancellationToken)
        {
            return Task.FromResult(File.ReadAllBytes(Path.Combine(_imagesPath, request.Name)));
        }
    }
}