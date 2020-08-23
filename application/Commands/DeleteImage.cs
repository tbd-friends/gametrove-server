using System;
using MediatR;

namespace GameTrove.Application.Commands
{
    public class DeleteImage : IRequest
    {
        public Guid Id { get; set; }
    }
}