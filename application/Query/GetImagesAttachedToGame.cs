using System;
using System.Collections.Generic;
using GameTrove.Application.ViewModels;
using MediatR;

namespace GameTrove.Application.Query
{
    public class GetImagesAttachedToGame : IRequest<IEnumerable<GameImageViewModel>>
    {
        public Guid GameId { get; set; }
    }
}