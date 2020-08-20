using System;
using System.IO;
using MediatR;

namespace GameTrove.Application.Commands
{
    public class AttachImageToGame : IRequest
    {
        public Guid GameId { get; set; }
        public byte[] Content { get; set; }
        public string FileName { get; set; }
    }
}