using System;
using GameTrove.Application.Infrastructure;
using MediatR;

namespace GameTrove.Application.Commands
{
    public class AddCopy : IRequestWithAuthentication<Guid?>
    {
        public Guid GameId { get; set; }
        public string[] Tags { get; set; }
        public decimal? Cost { get; set; }
        public DateTime? Purchased { get; set; }
        public string Email { get; set; }
        public string Identifier { get; set; }
        public bool IsWanted { get; set; }
    }
}