using System;
using GameTrove.Application.Infrastructure;
using GameTrove.Application.ViewModels;
using MediatR;

namespace GameTrove.Application.Commands
{
    public class RegisterGame : AuthenticatedRequest<GameViewModel>
    {
        public string Name { get; set; }
        public string Subtitle { get; set; }
        public string Code { get; set; }
        public Guid Platform { get; set; }
    }
}