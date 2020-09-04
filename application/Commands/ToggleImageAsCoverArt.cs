using System;
using MediatR;

namespace GameTrove.Application.Commands
{
    public class ToggleImageAsCoverArt : IRequest<Unit>
    {
        public Guid ImageId { get; set; }
    }
}