using System;
using GameTrove.Application.ViewModels;
using MediatR;

namespace GameTrove.Application.Query
{
    public class RetrieveGameById : IRequest<GameViewModel>
    {
        public Guid Id { get; set; }
    }
}