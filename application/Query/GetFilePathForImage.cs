using System;
using MediatR;

namespace GameTrove.Application.Query
{
    public class GetFilePathForImage : IRequest<string>
    {
        public Guid Id { get; set; }
    }
}