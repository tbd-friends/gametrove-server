using System;
using System.Collections;
using System.Collections.Generic;
using GameTrove.Application.ViewModels;
using MediatR;

namespace GameTrove.Application.Query
{
    public class GetCopies : IRequest<IEnumerable<CopyViewModel>>
    {
        public Guid GameId { get; set; }
    }
}