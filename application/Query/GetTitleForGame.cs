using System;
using GameTrove.Application.ViewModels;
using MediatR;

namespace GameTrove.Application.Query
{
    public class GetTitleForGame : IRequest<TitleViewModel>
    {
        public Guid GameId { get; set; }
    }
}