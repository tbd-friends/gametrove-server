using System;
using MediatR;

namespace api.Query
{
    public class GetFilePathForImage : IRequest<string>
    {
        public Guid Id { get; set; }
    }
}