using System;
using GameTrove.Application.Infrastructure;

namespace GameTrove.Application.Commands
{
    public abstract class AuthenticatedRequest<T> : IRequestWithAuthentication<T>
    {
        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
    }
}