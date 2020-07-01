using System;
using System.Collections.Generic;
using GameTrove.Application.Commands;
using GameTrove.Application.Infrastructure;
using GameTrove.Application.ViewModels;

namespace GameTrove.Application.Query
{
    public class GetCopies : AuthenticatedRequest<IEnumerable<CopyViewModel>>
    {
        public Guid GameId { get; set; }
    }
}