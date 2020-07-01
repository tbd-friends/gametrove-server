using System;

namespace GameTrove.Storage.Models
{
    public class Title
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string Name { get; set; }
        public string Subtitle { get; set; }
    }
}