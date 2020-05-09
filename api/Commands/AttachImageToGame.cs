using System;
using System.IO;
using MediatR;

namespace api.Commands
{
    public class AttachImageToGame : IRequest
    {
        public Guid Id { get; set; }
        public Stream Content { get; set; }
        public string FileName { get; set; }
    }
}