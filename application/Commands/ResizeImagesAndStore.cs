using System;
using System.Collections.Generic;
using System.IO;
using MediatR;

namespace GameTrove.Application.Commands.Handlers
{
    public class ResizeImagesAndStore : IRequest
    {
        public Guid GameId { get; set; }
        public Guid ImageId { get; set; }
        public byte[] ImageContent { get; set; }
    }
}