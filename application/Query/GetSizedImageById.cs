using System;
using GameTrove.Application.Infrastructure;
using MediatR;

namespace GameTrove.Application.Query
{
    public class GetSizedImageById : IRequest<byte[]>
    {
        public Guid Id { get; set; }
        public ImageSize Size { get; set; }
    }
}