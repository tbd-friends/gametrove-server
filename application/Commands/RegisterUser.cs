using System;
using GameTrove.Application.ViewModels;
using MediatR;

namespace GameTrove.Application.Commands
{
    public class RegisterUser : IRequest<RegisterUserResult>
    {
        public string Email { get; set; }
        public string Identifier { get; set; }
    }
}