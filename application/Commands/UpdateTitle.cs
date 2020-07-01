using System;
using System.Collections.Generic;
using GameTrove.Application.ViewModels;
using MediatR;

namespace GameTrove.Application.Commands
{
    public class UpdateTitle : IRequest<TitleViewModel>
    {
        public Guid TitleId { get; set; }
        public string Name { get; set; }
        public string Subtitle { get; set; }
        public IEnumerable<string> Genres { get; set; }
    }
}