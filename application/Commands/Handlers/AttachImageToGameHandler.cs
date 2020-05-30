using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Storage;
using GameTrove.Storage.Models;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace GameTrove.Application.Commands.Handlers
{
    public class AttachImageToGameHandler : IRequestHandler<AttachImageToGame>
    {
        private readonly GameTrackerContext _context;
        private readonly string _imagePath;

        public AttachImageToGameHandler(IConfiguration configuration, GameTrackerContext context)
        {
            _context = context;
            _imagePath = Path.Combine(AppContext.BaseDirectory, configuration["images:path"]);
        }

        public async Task<Unit> Handle(AttachImageToGame request, CancellationToken cancellationToken)
        {
            if (!Directory.Exists(_imagePath))
            {
                Directory.CreateDirectory(_imagePath);
            }

            if (request.Content.Length > 0)
            {
                await using var fileStream = new FileStream(Path.Combine(_imagePath, request.FileName.PrepareFileName()), FileMode.Create);

                await request.Content.CopyToAsync(fileStream, cancellationToken);

                _context.Add(new GameImage
                {
                    FileName = request.FileName.PrepareFileName(),
                    GameId = request.Id
                });

                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }

    public static class StringExtensions
    {
        public static string PrepareFileName(this string fileName)
        {
            return fileName.Replace(":", "_")
                .Replace("\\", "_")
                .Replace("/", "_");
        }
    }
}