using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Storage;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GameTrove.Application.Query.Handlers
{
    public class GetFilePathForImageHandler : IRequestHandler<GetFilePathForImage, string>
    {
        private readonly GameTrackerContext _context;
        private string _imagesPath;

        public GetFilePathForImageHandler(GameTrackerContext context, IConfiguration configuration)
        {
            _context = context;

            _imagesPath = Path.Combine(AppContext.BaseDirectory, configuration["images:path"]);
        }

        public async Task<string> Handle(GetFilePathForImage request, CancellationToken cancellationToken)
        {
            var image = await _context.PlatformGameImages.SingleAsync(pgi => pgi.Id == request.Id, cancellationToken);

            return Path.Combine(_imagesPath, image.FileName);
        }
    }
}