using System.IO;
using MediatR;

namespace GameTrove.Application.Commands
{
    public class SendToAzureStorage : IRequest
    {
        public Stream Content { get; set; }
        public string FileName { get; set; }
    }
}