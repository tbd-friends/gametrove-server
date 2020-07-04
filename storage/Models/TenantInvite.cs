using System;

namespace GameTrove.Storage.Models
{
    public class TenantInvite
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}