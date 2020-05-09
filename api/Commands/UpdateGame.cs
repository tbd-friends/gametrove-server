using System;
using api.ViewModels;
using MediatR;

namespace api.Commands
{
    public class UpdateGame : IRequest<GameViewModel>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}