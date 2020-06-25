using System;
using MediatR;

namespace GameTrove.Application.Commands
{
    public class RegisterUser : IRequest<Guid>
    {
        public string Email { get; set; }
        public string Identifier { get; set; }
    }
}