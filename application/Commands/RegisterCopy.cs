using System;
using MediatR;

namespace GameTrove.Application.Commands
{
    public class RegisterCopy : IRequest
    {
        public Guid GameId { get; set; }
        public string[] Tags { get; set; }
        public decimal? Cost { get; set; }
    }
}