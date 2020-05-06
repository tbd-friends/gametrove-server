using System;
using api.ViewModels;
using MediatR;

namespace api.Query
{
    public class RetrieveGameById : IRequest<GameViewModel>
    {
        public Guid Id { get; set; }
    }
}