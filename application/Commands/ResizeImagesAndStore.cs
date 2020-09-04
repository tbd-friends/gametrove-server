using System;
using MediatR;

namespace GameTrove.Application.Commands
{
    public class ResizeImagesAndStore : IRequest
    {
        public Guid GameId { get; set; }
        public Guid ImageId { get; set; }
        public byte[] ImageContent { get; set; }
    }
}