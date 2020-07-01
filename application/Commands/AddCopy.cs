using System;
using MediatR;

namespace GameTrove.Application.Commands
{
    public class AddCopy : AuthenticatedRequest<Guid?>
    {
        public Guid GameId { get; set; }
        public string[] Tags { get; set; }
        public decimal? Cost { get; set; }
        public DateTime? Purchased { get; set; }
        public bool IsWanted { get; set; }
    }
}