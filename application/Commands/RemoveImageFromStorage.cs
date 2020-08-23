using System;
using MediatR;

namespace GameTrove.Application.Commands
{
    public class RemoveImageFromStorage : IRequest
    {
        public Guid GameId { get; set; }
        public Guid ImageId { get; set; }
    }
}