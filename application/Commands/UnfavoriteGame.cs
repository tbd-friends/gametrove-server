using System;
using MediatR;

namespace GameTrove.Application.Commands
{
    public class UnfavoriteGame : IRequest
    {
        public Guid GameId { get; set; }
    }
}