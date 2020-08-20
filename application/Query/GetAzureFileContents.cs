using System;
using GameTrove.Application.Infrastructure;
using MediatR;

namespace GameTrove.Application.Query
{
    public class GetAzureFileContents : IRequest<byte[]>
    {
        public Guid GameId { get; set; }
        public Guid ImageId { get; set; }
        public ImageSize Size { get; set; }
    }
}