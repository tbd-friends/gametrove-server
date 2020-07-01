using System;

namespace GameTrove.Storage.Models
{
    public class Copy
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public Guid TenantId { get; set; }
        public string Tags { get; set; }
        public decimal? Cost { get; set; }
        public DateTime? Purchased { get; set; }
        public bool IsWanted { get; set; }
    }
}