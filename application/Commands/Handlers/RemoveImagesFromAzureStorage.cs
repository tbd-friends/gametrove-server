using System;
using MediatR;

namespace GameTrove.Application.Commands.Handlers
{
    public class RemoveImagesFromAzureStorage : IRequest<Unit>
    {
        public Guid GameId { get; set; }
        public Guid ImageId { get; set; }
    }
}