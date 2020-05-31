using System;
using MediatR;

namespace GameTrove.Application.Commands
{
    public class FavoriteGame : IRequest
    {
        public Guid GameId { get; set; }
    }
}