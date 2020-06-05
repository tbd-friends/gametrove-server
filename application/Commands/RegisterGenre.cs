using System;
using MediatR;

namespace GameTrove.Application.Commands
{
    public class RegisterGenre : IRequest<Guid>
    {
        public string Name { get; set; }
    }
}