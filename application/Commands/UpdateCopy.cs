using System;
using System.Collections.Generic;
using GameTrove.Application.ViewModels;
using MediatR;

namespace GameTrove.Application.Commands
{
    public class UpdateCopy : IRequest<CopyViewModel>
    {
        public Guid Id { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public bool IsWanted { get; set; }
        public DateTime? Purchased { get; set; }
        public decimal? Cost { get; set; }
    }
}