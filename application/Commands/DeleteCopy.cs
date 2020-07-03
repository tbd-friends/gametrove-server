using System;

namespace GameTrove.Application.Commands
{
    public class DeleteCopy : AuthenticatedRequest
    {
        public Guid CopyId { get; set; }
    }
}