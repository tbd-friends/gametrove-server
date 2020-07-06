using System;

namespace GameTrove.Storage.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string Email { get; set; }
    }
}