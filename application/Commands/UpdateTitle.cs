using System;
using GameTrove.Application.ViewModels;
using MediatR;

namespace GameTrove.Application.Commands
{
    public class UpdateTitle : IRequest<TitleViewModel>
    {
        public Guid TitleId { get; set; }
        public string Name { get; set; }
        public string Subtitle { get; set; }
    }
}