using System;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using api.Settings;
using GameTrove.Storage;
using GameTrove.Storage.Models;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace GameTrove.Application.Commands.Handlers.Images
{
    public class AttachImageToGameHandler : IRequestHandler<AttachImageToGame>
    {
        private readonly IMediator _mediator;
        private readonly GameTrackerContext _context;
        private readonly ImageSettings _settings;
        private readonly string _imagePath;

        public AttachImageToGameHandler(IConfiguration configuration,
                                        IMediator mediator,
                                        GameTrackerContext context,
                                        ImageSettings settings)
        {
            _mediator = mediator;
            _context = context;
            _settings = settings;
            _imagePath = Path.Combine(AppContext.BaseDirectory, configuration["images:path"]);
        }

        public async Task<Unit> Handle(AttachImageToGame request, CancellationToken cancellationToken)
        {  
            if (request.Content.Length > 0)
            {
                var gameImage = new GameImage
                {
                    Id = Guid.NewGuid(),
                    FileName = request.FileName,
                    GameId = request.GameId
                };

                if (_settings.Local)
                {
                    if (!Directory.Exists(_imagePath))
                    {
                        Directory.CreateDirectory(_imagePath);
                    }

                    File.WriteAllBytes(Path.Combine(_imagePath, $"{request.GameId}_{gameImage.Id}.jpg"),
                        request.Content);
                }
                else
                {
                    await _mediator.Send(new ResizeImagesAndStore
                    { ImageContent = request.Content, GameId = request.GameId, ImageId = gameImage.Id },
                        cancellationToken);
                }

                _context.Add(gameImage);

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
                .Replace("/", "_")
                .Replace("?", "_");
        }
    }
}

