using System;
using api.ViewModels;
using MediatR;

namespace api.Commands
{
    public class RegisterGame : IRequest<GameViewModel>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public Guid Platform { get; set; }
    }
}