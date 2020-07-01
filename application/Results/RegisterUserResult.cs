using System;

namespace GameTrove.Application.ViewModels
{
    public class RegisterUserResult
    {
        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
    }
}