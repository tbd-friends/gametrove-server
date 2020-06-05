using System;
using System.Collections;
using System.Collections.Generic;
using MediatR;

namespace GameTrove.Application.Commands
{
    public class AssignGenresToTitle : IRequest
    {
        public Guid TitleId { get; set; }
        public IEnumerable<string> Genres { get; set; }
    }
}